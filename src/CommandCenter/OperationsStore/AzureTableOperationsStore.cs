// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.OperationsStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Store for operations on Azure Table service.
    /// </summary>
    public class AzureTableOperationsStore : IOperationsStore
    {
        private const string TableName = "marketplaceoperations";
        private readonly CloudTableClient tableClient;
        private bool tableInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableOperationsStore"/> class.
        /// </summary>
        /// <param name="storageAccountConnectionString">Storage account connection string.</param>
        public AzureTableOperationsStore(string storageAccountConnectionString)
        {
            this.tableClient = CloudStorageAccount.Parse(storageAccountConnectionString).CreateCloudTableClient();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<OperationRecord>> GetAllSubscriptionRecordsAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
        {
            var table = this.tableClient.GetTableReference(TableName);
            var result = new List<OperationRecord>();

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
                    (key, rowKey, timestamp, properties, etag) => new OperationRecord(key, rowKey),
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
            Guid operationId,
            CancellationToken cancellationToken = default)
        {
            var table = this.tableClient.GetTableReference(TableName);

            await this.InitTable(cancellationToken);

            var entity = new OperationRecord(subscriptionId.ToString(), operationId.ToString());

            var tableOperation = TableOperation.InsertOrMerge(entity);

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