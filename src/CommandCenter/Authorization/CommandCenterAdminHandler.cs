// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Authorization
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using CommandCenter.Utilities;
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// Custom authorization policy handler.
    /// </summary>
    public class CommandCenterAdminHandler : AuthorizationHandler<CommandCenterAdminRequirement>
    {
        /// <summary>
        /// Handle authorization requirement.
        /// </summary>
        /// <param name="context">Authorization context.</param>
        /// <param name="requirement">Requirement.</param>
        /// <returns>void.</returns>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CommandCenterAdminRequirement requirement)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (requirement == null)
            {
                throw new ArgumentNullException(nameof(requirement));
            }

            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            if (context.User.Identity.GetUserEmail().GetDomainNameFromEmail()
                == requirement.AdminName.GetDomainNameFromEmail())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}