// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Quickstart.UI
{
    public class LoginInputModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Username { get; set; }
        
        [Required]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembre-me meu login")]
        public bool RememberLogin { get; set; }
        
        public string ReturnUrl { get; set; }
    }
}