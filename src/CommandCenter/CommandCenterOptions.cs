// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter
{
    using System;
    using System.Collections.Generic;
    using CommandCenter.Metering;

    /// <summary>
    /// Options for the command center.
    /// </summary>
    public class CommandCenterOptions
    {
        /// <summary>
        /// Gets or sets base URL of the Command center.
        /// </summary>
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets command center admin.
        /// </summary>
        public string CommandCenterAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether unsubcribed subscriptions are shown.
        /// </summary>
        public bool ShowUnsubscribed { get; set; }

        /// <summary>
        /// Gets or sets the connection string for the operations store.
        /// </summary>
        public string OperationsStoreConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the Azure Queue notifiction options.
        /// </summary>
        public AzureQueueOptions AzureQueue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether meter reporting is enabled.
        /// </summary>
        public bool EnableDimensionMeterReporting { get; set; }

        /// <summary>
        /// Gets or sets the Dimensions.
        /// </summary>
        public List<Dimension> Dimensions { get; set; }
    }
}