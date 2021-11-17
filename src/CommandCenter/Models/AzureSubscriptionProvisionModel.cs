// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Marketplace.SaaS.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Model for subscription view.
    /// </summary>
    [BindProperties]
    public class AzureSubscriptionProvisionModel
    {
        /// <summary>
        ///  Gets or sets available plans.
        /// </summary>
        [Display(Name = "Available plans")]
        public IList<Plan> AvailablePlans { get; set; }

        /// <summary>
        /// Gets or sets business contact email.
        /// </summary>
        [Display(Name = "Business unit contact email")]
        public string BusinessUnitContactEmail { get; set; }

        /// <summary>
        /// Gets or sets business contact phone number.
        /// </summary>
        [Display(Name = "Business unit contact phone number")]
        [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10}$", ErrorMessage ="Enter valid phone number")]
        public string BusinessUnitContactPhone { get; set; }

        /// <summary>
        ///  Gets or sets email.
        /// </summary>
        [Display(Name = "Preferred user name")]
        public string EmailFromClaims { get; set; }

        /// <summary>
        /// Gets or sets full name.
        /// </summary>
        [Display(Name = "Name")]
        public string NameFromOpenIdConnect { get; set; }

        /// <summary>
        /// Gets or sets new plan ID.
        /// </summary>
        [BindProperty]
        public string NewPlanId { get; set; }

        /// <summary>
        /// Gets or sets the offer ID.
        /// </summary>
        [Display(Name = "Offer ID")]
        public string OfferId { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether there are pending operations.
        /// </summary>
        public bool PendingOperations { get; set; }

        /// <summary>
        /// Gets or sets the plan.
        /// </summary>
        [Display(Name = "Current plan")]
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public TargetContosoRegionEnum Region { get; set; }

        /// <summary>
        /// Gets or sets the subscription ID.
        /// </summary>
        [Display(Name = "SaaS Subscription Id")]
        public Guid SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the subscription name.
        /// </summary>
        [Display(Name = "Subscription name")]
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Gets or sets the subscription status.
        /// </summary>
        public SubscriptionStatusEnum SubscriptionStatus { get; set; }

        /// <summary>
        /// Gets or sets the purchaser email.
        /// </summary>
        [Display(Name = "Purchaser user principal name from marketplace API")]
        public string PurchaserUPN { get; set; }

        /// <summary>
        /// Gets or sets the purchaser tenant ID.
        /// </summary>
        [Display(Name = "Purchaser AAD TenantId")]
        public Guid PurchaserTenantId { get; set; }

        /// <summary>
        /// Gets or sets the beneficiary email.
        /// </summary>
        [Display(Name = "Beneficiary user principal name from marketplace API")]
        public string BeneficiaryUPN { get; set; }

        /// <summary>
        /// Gets or sets the beneficiary tenant ID.
        /// </summary>
        [Display(Name = "Beneficiary AAD TenantId")]
        public Guid BeneficiaryTenantId { get; set; }

        /// <summary>
        /// Gets or sets the email address received from the Graph API.
        /// </summary>
        [Display(Name = "Purchaser email")]
        public string EmailFromGraph { get;  set; }

        /// <summary>
        /// Gets or sets the purchaser name received from the Graph API.
        /// </summary>
        [Display(Name = "Purchaser name")]
        public string NameFromGraph { get;  set; }

        /// <summary>
        /// Gets or sets the user principal name received from the Graph API.
        /// </summary>
        [Display(Name = "User Principal Name")]
        public string UserPrincipalName { get;  set; }

        /// <summary>
        /// Gets or sets the purchaser email.
        /// </summary>
        [Display(Name = "Purchaser Email")]
        public string PurchaserEmail { get; set; }
    }
}