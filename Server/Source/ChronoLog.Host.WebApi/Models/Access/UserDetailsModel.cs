using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChronoLog.Host.WebApi.Models.Access
{
    /// <summary>
    /// Represents the user details model.
    /// </summary>
    public class UserDetailsModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }
    }
}