// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Utilities
{
    using System.Net.Mail;

    /// <summary>
    ///  Extensions for string.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Get domain name from an email string.
        /// </summary>
        /// <param name="emailString">string containing the email.</param>
        /// <returns>string.</returns>
        public static string GetDomainNameFromEmail(this string emailString)
        {
            try
            {
                var email = new MailAddress(emailString);
                return email.Host;
            }
            catch
            {
                return "InvalidCustomer";
            }
        }

        /// <summary>
        /// Check whether a string is a valid email.
        /// </summary>
        /// <param name="email">email.</param>
        /// <returns>bool.</returns>
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}