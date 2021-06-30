// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Marketplace
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Marketplace.SaaS;
    using Microsoft.Marketplace.SaaS.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Implements the protocol aspect of the fulfillment API.
    /// </summary>
    public class MarketplaceProcessor : IMarketplaceProcessor
    {
        private readonly ILogger<MarketplaceProcessor> logger;
        private readonly IMarketplaceSaaSClient marketplaceClient;
        private readonly IWebhookHandler webhookHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketplaceProcessor"/> class.
        /// </summary>
        /// <param name="marketplaceClient">Marketplace API client.</param>
        /// <param name="webhookHandler">Webhook handler.</param>
        /// <param name="logger">Logger.</param>
        public MarketplaceProcessor(
            IMarketplaceSaaSClient marketplaceClient,
            IWebhookHandler webhookHandler,
            ILogger<MarketplaceProcessor> logger)
        {
            this.logger = logger;
            this.marketplaceClient = marketplaceClient;
            this.webhookHandler = webhookHandler;
            this.marketplaceClient = marketplaceClient;
        }

        /// <inheritdoc/>
        public async Task ActivateSubscriptionAsync(Guid subscriptionId, string planId, CancellationToken cancellationToken)
        {
            await this.marketplaceClient.Fulfillment.ActivateSubscriptionAsync(
                subscriptionId,
                new SubscriberPlan { PlanId = planId },
                null,
                null,
                cancellationToken);

            this.logger.LogInformation($"Activated subscription {subscriptionId} with plan {planId}");
        }

        /// <inheritdoc/>
        public async Task<ResolvedSubscription> GetSubscriptionFromPurchaseIdentificationTokenAsync(string token, CancellationToken cancellationToken)
        {
            if (token == default)
            {
                throw new ApplicationException("marketplace purchase identification token is empty");
            }

            try
            {
                var resolvedSubscription = await this.marketplaceClient.Fulfillment.ResolveAsync(token, null, null, cancellationToken);

                if (resolvedSubscription != default)
                {
                    this.logger.LogInformation($"Resolved subscription {resolvedSubscription.Value.Id} with plan {resolvedSubscription.Value.PlanId}");
                }

                return resolvedSubscription;
            }
            catch (Azure.RequestFailedException e)
            {
                this.logger.LogError($"Cannot resolve the token {token} for a subscription. Details are {e.ToString()}");
                return default(ResolvedSubscription);
            }
        }

        /// <inheritdoc/>
        public async Task OperationAckAsync(Guid subscriptionId, Guid operationId, string planId, int quantity, CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"Ackonwledging operation {operationId} for subscription {subscriptionId}");

            await this.marketplaceClient.Operations.UpdateOperationStatusAsync(
                    subscriptionId,
                    operationId,
                    new UpdateOperation { PlanId = planId, Quantity = quantity, Status = UpdateOperationStatusEnum.Success },
                    null,
                    null,
                    cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ProcessWebhookNotificationAsync(WebhookPayload payload, CancellationToken cancellationToken)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            // Always query the fulfillment API for the received Operation for security reasons. Webhook endpoint is not authenticated.
            var operationDetails = await this.marketplaceClient.Operations.GetOperationStatusAsync(
                payload.SubscriptionId,
                payload.OperationId,
                null,
                null,
                cancellationToken);

            if (operationDetails == null)
            {
                this.logger.LogError(
                    $"Operation query returned {JsonConvert.SerializeObject(operationDetails)} for subscription {payload.SubscriptionId} operation {payload.OperationId}");
                return;
            }

            this.logger.LogInformation($"Received webhook notification, operation {payload.OperationId}, for subscription {payload.SubscriptionId}, details are {JsonConvert.SerializeObject(payload)}");

            switch (payload.Action)
            {
                case WebhookAction.Unsubscribe:
                    await this.webhookHandler.UnsubscribedAsync(payload);
                    break;

                case WebhookAction.ChangePlan:
                    await this.webhookHandler.ChangePlanAsync(payload);
                    break;

                case WebhookAction.ChangeQuantity:
                    await this.webhookHandler.ChangeQuantityAsync(payload);
                    break;

                case WebhookAction.Suspend:
                    await this.webhookHandler.SuspendedAsync(payload);
                    break;

                case WebhookAction.Reinstate:
                    await this.webhookHandler.ReinstatedAsync(payload);
                    break;

                case WebhookAction.Transfer:
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
