// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Resolving redirect after signout.
    /// </summary>
    [AllowAnonymous]
    public class AccountController : Controller
    {
        /// <summary>
        /// Redirects to home page after sign out.
        /// </summary>
        /// <param name="page">Page.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult SignOut(string page)
        {
            return this.RedirectToAction("Index", "Subscriptions");
        }
    }
}
