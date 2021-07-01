// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Models
{
    using System;
    using System.Collections.Generic;
    using CommandCenter.Marketplace;
    using Microsoft.Marketplace.SaaS.Models;

    /// <summary>
    /// Subscription view model.
    /// </summary>
    public class SubscriptionViewModel : MarketplaceSubscription
    {
        /// <summary>
        ///  Gets next actions.
        /// </summary>
        public IEnumerable<ActionsEnum> NextActions
        {
            get
            {
                if (this.State == SubscriptionStatusEnum.PendingFulfillmentStart)
                {
                    return new List<ActionsEnum>();
                }

                if (this.State == SubscriptionStatusEnum.Subscribed)
                {
                    return new List<ActionsEnum> { ActionsEnum.Update, ActionsEnum.Unsubscribe };
                }

                return new List<ActionsEnum>();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether there are pending operations.
        /// </summary>
        public bool PendingOperations { get; set; }

        /// <summary>
        ///  Gets or sets purchaser tenant ID.
        /// </summary>
        public Guid BeneficiaryTenantId { get; set; }

        /// <summary>
        /// Gets or sets purchaser email.
        /// </summary>
        public string BeneficiaryEmail { get; set; }

        /// <summary>
        /// Gets a value indicating whether there are existing operations.
        /// </summary>
        public bool ExistingOperations { get; internal set; }

        /// <summary>
        /// Gets operation count.
        /// </summary>
        public int OperationCount { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether gets or sets ParticipatesInDimension.
        /// </summary>
        public bool IsDimensionEnabled { get; internal set; }

        /// <summary>
        /// Creates a subscription model from a subscription.
        /// </summary>
        /// <param name="marketplaceSubscription">subscription.</param>
        /// <returns>Subscription view model.</returns>
        public static SubscriptionViewModel FromSubscription(Subscription marketplaceSubscription)
        {
            if (marketplaceSubscription == null)
            {
                throw new ArgumentNullException(nameof(marketplaceSubscription));
            }

            return new SubscriptionViewModel
            {
                PlanId = marketplaceSubscription.PlanId,
                Quantity = marketplaceSubscription.Quantity ?? 0,
                SubscriptionId = marketplaceSubscription.Id ?? Guid.Empty,
                OfferId = marketplaceSubscription.OfferId,
                State = marketplaceSubscription.SaasSubscriptionStatus ?? SubscriptionStatusEnum.NotStarted,
                SubscriptionName = marketplaceSubscription.Name,
                BeneficiaryEmail = marketplaceSubscription.Beneficiary.EmailId,
                BeneficiaryTenantId = marketplaceSubscription.Beneficiary.TenantId ?? Guid.Empty,
            };
        }
    }
}