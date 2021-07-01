// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Activate action view model.
    /// </summary>
    public class ActivateActionViewModel
    {
        /// <summary>
        /// Gets or sets plan ID.
        /// </summary>
        [Display(Name = "Plan ID")]
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets subscription ID.
        /// </summary>
        [Display(Name = "Subscription ID")]
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Gets message.
        /// </summary>
        [Display(Name = "Message")]
        public string Message { get; internal set; }
    }
}