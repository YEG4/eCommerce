using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.API.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

    }
}