// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.DimensionUsageStore
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using CommandCenter.Metering;

    /// <summary>
    /// Interface for storing triggered dimension usage events.
    /// </summary>
    public interface IDimensionUsageStore
    {
        /// <summary>
        /// Get all dimension usage events.
        /// </summary>
        /// <param name="subscriptionId">Subscription.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of dimension records.</returns>
        Task<IEnumerable<DimensionUsageRecord>> GetAllDimensionRecordsAsync(Guid subscriptionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Record dimension usage.
        /// </summary>
        /// <param name="subscriptionId">Related subscription.</param>
        /// <param name="result">result.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Void.</returns>
        Task RecordAsync(Guid subscriptionId, DimensionUsageRecord result, CancellationToken cancellationToken = default);
    }
}
