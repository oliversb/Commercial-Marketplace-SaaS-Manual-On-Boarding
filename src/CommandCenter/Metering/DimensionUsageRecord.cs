// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Metering
{
    using System;
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Dimension record entity.
    /// </summary>
    public class DimensionUsageRecord : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionUsageRecord"/> class.
        /// </summary>
        /// <param name="subscriptionId">Subscription ID as the partition key.</param>
        /// <param name="sentDateTime">Sent Datetime as the row key.</param>
        public DimensionUsageRecord(string subscriptionId, string sentDateTime)
        {
            PartitionKey = subscriptionId;
            RowKey = sentDateTime;
        }

        /// <summary>
        /// Gets or sets unique identifier associated with the usage event.
        /// </summary>
        public Guid? UsageEventId { get; set; }

        /// <summary>
        /// Gets or sets status of the operation. Possible values include:
        /// 'Accepted', 'Expired', 'Duplicate', 'Error', 'ResourceNotFound',
        /// 'ResourceNotAuthorized', 'InvalidDimension|BadArgument'.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets number of units consumed.
        /// </summary>
        public double? Quantity { get; set; }

        /// <summary>
        /// Gets or sets dimension identifier.
        /// </summary>
        public string Dimension { get; set; }

        /// <summary>
        /// Gets or sets time in UTC when the usage event occurred.
        /// </summary>
        public DateTimeOffset? EffectiveStartTime { get; set; }

        /// <summary>
        /// Gets or sets plan associated with the purchased offer.
        /// </summary>
        public string PlanId { get; set; }
    }
}
