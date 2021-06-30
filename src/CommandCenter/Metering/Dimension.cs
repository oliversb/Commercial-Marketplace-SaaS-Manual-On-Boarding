// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Metering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Model for Dimensions.
    /// </summary>
    public class Dimension
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string DimensionId { get; set; }

        /// <summary>
        /// Gets or sets the PlanIds.
        /// </summary>
        public List<string> PlanIds { get; set; }

        /// <summary>
        /// Gets or sets the OfferIds.
        /// </summary>
        public List<string> OfferIds { get; set; }
    }
}
