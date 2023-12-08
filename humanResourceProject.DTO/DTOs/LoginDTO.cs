﻿using humanResourceProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace humanResourceProject.DTO.DTOs
{
    public class LoginDTO
    {

        public string Email { get; set; }

        public string Password { get; set; }

        public string? ReturnUrl { get; set; }

        public Status Status { get; set; }
    }
}
