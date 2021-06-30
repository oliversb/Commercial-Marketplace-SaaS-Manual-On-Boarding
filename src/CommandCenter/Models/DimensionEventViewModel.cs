// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using CommandCenter.Metering;

    /// <summary>
    /// Dimension view model.
    /// </summary>
    public class DimensionEventViewModel
    {
        /// <summary>
        /// Gets or sets the SubscriptionId.
        /// </summary>
        [Display(Name = "Subscription Id")]
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the SubscriptionName.
        /// </summary>
        [Display(Name = "Subscription Name")]
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Gets or sets the OfferId.
        /// </summary>
        [Display(Name = "Offer Id")]
        public string OfferId { get; set; }

        /// <summary>
        /// Gets or sets the PlanId.
        /// </summary>
        [Display(Name = "Plan Id")]
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets the SubscriptionDimensions.
        /// </summary>
        [Display(Name = "Subscription Dimensions")]
        public IEnumerable<string> SubscriptionDimensions { get; set; }

        /// <summary>
        /// Gets or sets the SelectedDimension.
        /// </summary>
        [Required]
        [Display(Name = "Selected dimension")]
        public string SelectedDimension { get; set; }

        /// <summary>
        /// Gets or sets the Quantity.
        /// </summary>
        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the EventTime.
        /// </summary>
        [Required]
        [DisplayName("Event Time In UTC")]
        public DateTime EventTime { get; set; }

        /// <summary>
        /// Gets or sets the PastUsageEvents.
        /// </summary>
        [Display(Name = "Past usage events")]
        public IEnumerable<DimensionUsageRecord> PastUsageEvents { get; set; }
    }
}
