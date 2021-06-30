// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Marketplace.SaaS.Models;

    /// <summary>
    ///  Operations view modek.
    /// </summary>
    public class OperationsViewModel
    {
        /// <summary>
        ///  Gets or sets operations.
        /// </summary>
        [Display(Name = "Operations")]
        public List<Operation> Operations { get; set; }

        /// <summary>
        /// Gets or sets subscription name.
        /// </summary>
        [Display(Name = "Subscription name")]
        public string SubscriptionName { get; set; }
    }
}