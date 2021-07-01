// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Metering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CommandCenter.DimensionUsageStore;
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Store for dimensions on Azure Table service.
    /// </summary>
    public class AzureTableDimensionUsageStore : IDimensionUsageStore
    {
        private const string TableName = "marketplacedimensions";
        private readonly CloudTableClient tableClient;
        private bool tableInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableDimensionUsageStore"/> class.
        /// </summary>
        /// <param name="storageAccountConnectionString">Storage account connection string.</param>
        public AzureTableDimensionUsageStore(string storageAccountConnectionString)
        {
            tableClient = CloudStorageAccount.Parse(storageAccountConnectionString).CreateCloudTableClient();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DimensionUsageRecord>> GetAllDimensionRecordsAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
        {
            var table = this.tableClient.GetTableReference(TableName);
            var result = new List<DimensionUsageRecord>();

            if (!table.Exists())
            {
                return result;
            }

            var query = new TableQuery().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, subscriptionId.ToString()));

            TableContinuationToken token = null;

            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(
                    query,
                    (key, rowKey, timestamp, properties, etag) => new DimensionUsageRecord(key, rowKey)
                    {
                        PlanId = properties["PlanId"]?.ToString(),
                        Dimension = properties["Dimension"]?.ToString(),
                        Quantity = Convert.ToInt64(properties["Quantity"]?.ToString()),
                        Status = properties.ContainsKey("Status") ? properties["Status"]?.ToString() : string.Empty,
                        EffectiveStartTime = DateTime.Parse(properties["EffectiveStartTime"]?.ToString()),
                    },
                    token,
                    cancellationToken);

                result.AddRange(segment.Results.Select(r => r));

                token = segment.ContinuationToken;
            }
            while (token != default);

            return result;
        }

        /// <inheritdoc/>
        public async Task RecordAsync(
            Guid subscriptionId,
            DimensionUsageRecord result,
            CancellationToken cancellationToken = default)
        {
            var table = this.tableClient.GetTableReference(TableName);

            await this.InitTable(cancellationToken);

            var tableOperation = TableOperation.InsertOrMerge(result);

            await table.ExecuteAsync(tableOperation, cancellationToken);
        }

        private async Task InitTable(CancellationToken cancellationToken = default)
        {
            if (!this.tableInitialized)
            {
                var table = this.tableClient.GetTableReference(TableName);

                await table.CreateIfNotExistsAsync(cancellationToken);
                this.tableInitialized = true;
            }
        }
    }
}
