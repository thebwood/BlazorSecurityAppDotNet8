﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSecurityApp.Core.DTOs
{
    public class RefreshUserTokenResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
