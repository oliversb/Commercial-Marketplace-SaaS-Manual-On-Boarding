// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.AzureQueues
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Storage.Queues;
    using CommandCenter.Marketplace;
    using CommandCenter.Models;
    using CommandCenter.Utilities;
    using Microsoft.Extensions.Options;
    using Microsoft.Marketplace.SaaS;
    using Newtonsoft.Json;

    /// <summary>
    /// Implementation for sending messages to a queue.
    /// </summary>
    public class AzureQueueNotificationHandler : IMarketplaceNotificationHandler
    {
        private const string MailLinkControllerName = "MailLink";

        private readonly QueueClient queueClient;
        private IMarketplaceSaaSClient marketplaceClient;
        private CommandCenterOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureQueueNotificationHandler"/> class.
        /// </summary>
        /// <param name="optionsMonitor">Adapter options.</param>
        /// <param name="marketplaceClient">Marketplace API client.</param>
        public AzureQueueNotificationHandler(IOptionsMonitor<CommandCenterOptions> optionsMonitor, IMarketplaceSaaSClient marketplaceClient)
        {
            if (optionsMonitor == null)
            {
                throw new ArgumentNullException(nameof(optionsMonitor));
            }

            this.marketplaceClient = marketplaceClient;
            this.options = optionsMonitor.CurrentValue;

            if (this.options.AzureQueue == null)
            {
                throw new ApplicationException("AzureQueue options are needed.");
            }

            if (string.IsNullOrEmpty(this.options.AzureQueue.QueueName) || string.IsNullOrEmpty(this.options.AzureQueue.StorageConnectionString))
            {
                throw new ApplicationException("Queue name or connection string is empty for the Azure Queue options.");
            }

            this.queueClient = new QueueClient(this.options.AzureQueue.StorageConnectionString, this.options.AzureQueue.QueueName);

            this.queueClient.CreateIfNotExists();
        }

        /// <inheritdoc/>
        public async Task NotifyChangePlanAsync(WebhookPayload payload, CancellationToken cancellationToken = default)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            await this.SendWebhookNotificationMessageAsync(
                payload,
                cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ProcessChangePlanAsync(AzureSubscriptionProvisionModel provisionModel, CancellationToken cancellationToken = default)
        {
            if (provisionModel == null)
            {
                throw new ArgumentNullException(nameof(provisionModel));
            }

            await this.SendLandingPageMessageAsync(provisionModel, MailLinkControllerName, "Update", cancellationToken);
        }

        /// <inheritdoc/>
        public async Task NotifyChangeQuantityAsync(WebhookPayload payload, CancellationToken cancellationToken = default)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            await this.SendWebhookNotificationMessageAsync(
                payload,
                cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ProcessNewSubscriptionAsyc(AzureSubscriptionProvisionModel provisionModel, CancellationToken cancellationToken = default)
        {
            if (provisionModel == null)
            {
                throw new ArgumentNullException(nameof(provisionModel));
            }

            await this.SendLandingPageMessageAsync(provisionModel, MailLinkControllerName, "Activate", cancellationToken);
        }

        /// <inheritdoc/>
        public async Task ProcessOperationFailOrConflictAsync(WebhookPayload payload, CancellationToken cancellationToken = default)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            await this.SendWebhookNotificationMessageAsync(
                payload,
                cancellationToken);
        }

        /// <inheritdoc/>
        public async Task NotifyReinstatedAsync(WebhookPayload payload, CancellationToken cancellationToken = default)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            await this.SendWebhookNotificationMessageAsync(
                payload,
                cancellationToken);
        }

        /// <inheritdoc/>
        public async Task NotifySuspendedAsync(WebhookPayload payload, CancellationToken cancellationToken = default)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            await this.SendWebhookNotificationMessageAsync(
                payload,
                cancellationToken);
        }

        /// <inheritdoc/>
        public async Task NotifyUnsubscribedAsync(WebhookPayload payload, CancellationToken cancellationToken = default)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            await this.SendWebhookNotificationMessageAsync(
                payload,
                cancellationToken);
        }

        private async Task SendLandingPageMessageAsync(AzureSubscriptionProvisionModel provisionModel, string controllerName, string actionName, CancellationToken cancellationToken)
        {
            if (provisionModel == null)
            {
                throw new ArgumentNullException(nameof(provisionModel));
            }

            var queryParams = new List<Tuple<string, string>>
            {
                new Tuple<string, string>(
                    "subscriptionId",
                    provisionModel.SubscriptionId.ToString()),
                new Tuple<string, string>("planId", provisionModel.NewPlanId == null ? provisionModel.PlanId : provisionModel.NewPlanId),
            };

            var message = $"{{\"ActionLink\": \"{this.BuildLink(queryParams, controllerName, actionName)}\", \"Payload\" : {JsonConvert.SerializeObject(provisionModel)} }}";

            await this.queueClient.SendMessageAsync(message, cancellationToken);
        }

        private async Task SendWebhookNotificationMessageAsync(WebhookPayload message, CancellationToken cancellationToken)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            await this.queueClient.SendMessageAsync(JsonConvert.SerializeObject(message), cancellationToken);
        }

        private string BuildLink(List<Tuple<string, string>> queryParams, string controllerName, string controllerAction)
        {
            var uriStart = FluentUriBuilder.Start(this.options.BaseUrl).AddPath(controllerName).AddPath(controllerAction);

            foreach (var (item1, item2) in queryParams) uriStart.AddQuery(item1, item2);

            return uriStart.Uri.ToString();
        }
    }
}
