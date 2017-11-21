﻿namespace MockTestExample.Website.ViewModels
{
    using System.Collections.Generic;

    using Microsoft.AspNet.Identity;

    /// <summary>
    /// The index view model.
    /// </summary>
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }
}