﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Core.DTOs.User
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First Name is Required")]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(20)]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        [MaxLength(100, ErrorMessage = "Max Length is 100")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone is Required")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
