// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Models
{
    using System.ComponentModel.DataAnnotations;
    using CommandCenter.Marketplace;

    /// <summary>
    /// Operation update view model.
    /// </summary>
    public class OperationUpdateViewModel
    {
        /// <summary>
        /// Gets or sets operation type.
        /// </summary>
        [Display(Name = "Operation type")]
        public string OperationType { get; set; }

        /// <summary>
        /// Gets or sets webhook payload.
        /// </summary>
        [Display(Name = "Payload")]
        public WebhookPayload Payload { get; set; }
    }
}