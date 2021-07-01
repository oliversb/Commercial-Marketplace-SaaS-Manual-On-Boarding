// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Webhook
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CommandCenter.Marketplace;
    using Microsoft.Marketplace.SaaS;
    using Microsoft.Marketplace.SaaS.Models;

    /// <summary>
    /// Webhook handler.
    /// </summary>
    public class ContosoWebhookHandler : IWebhookHandler
    {
        private readonly IMarketplaceSaaSClient marketplaceClient;
        private readonly IMarketplaceNotificationHandler notificationHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContosoWebhookHandler"/> class.
        /// </summary>
        /// <param name="notificationHelper">Custom handler for notifications.</param>
        /// <param name="marketplaceClient">Marketplace client.</param>
        public ContosoWebhookHandler(
            IMarketplaceNotificationHandler notificationHelper,
            IMarketplaceSaaSClient marketplaceClient)
        {
            this.notificationHelper = notificationHelper;
            this.marketplaceClient = marketplaceClient;
        }

        /// <inheritdoc/>
        public async Task ChangePlanAsync(WebhookPayload payload)
        {
            await this.NotifyAndAck(
                payload,
                new UpdateOperation { PlanId = payload.PlanId, Status = UpdateOperationStatusEnum.Success },
                this.notificationHelper.NotifyChangePlanAsync);
        }

        /// <inheritdoc/>
        public async Task ChangeQuantityAsync(WebhookPayload payload)
        {
            await this.NotifyAndAck(
                payload,
                new UpdateOperation { Quantity = payload.Quantity, Status = UpdateOperationStatusEnum.Success },
                this.notificationHelper.NotifyChangeQuantityAsync);
        }

        /// <inheritdoc/>
        public async Task ReinstatedAsync(WebhookPayload payload)
        {
            await this.NotifyAndAck(
                payload,
                new UpdateOperation { Status = UpdateOperationStatusEnum.Success },
                this.notificationHelper.NotifyReinstatedAsync);
        }

        /// <inheritdoc/>
        public async Task SuspendedAsync(WebhookPayload payload)
        {
            await this.NotifyAndAck(
                payload,
                new UpdateOperation { Status = UpdateOperationStatusEnum.Success },
                this.notificationHelper.NotifySuspendedAsync);
        }

        /// <inheritdoc/>
        public async Task UnsubscribedAsync(WebhookPayload payload)
        {
            await this.NotifyAndAck(
                payload,
                new UpdateOperation { Status = UpdateOperationStatusEnum.Success },
                this.notificationHelper.NotifyUnsubscribedAsync);
        }

        private async Task NotifyAndAck(WebhookPayload payload, UpdateOperation updateOperation, Func<WebhookPayload, CancellationToken, Task> notify)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (payload.Status == OperationStatusEnum.Succeeded)
            {
                await notify(payload, CancellationToken.None);
                await this.marketplaceClient.Operations.UpdateOperationStatusAsync(
                        payload.SubscriptionId,
                        payload.OperationId,
                        updateOperation);
            }
            else if (payload.Status == OperationStatusEnum.Conflict || payload.Status == OperationStatusEnum.Failed)
            {
                await this.notificationHelper.ProcessOperationFailOrConflictAsync(payload);
            }
        }
    }
}
