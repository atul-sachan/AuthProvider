using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthProvider.Authentication.Models
{
    public class RegisterModel
    {
        public RegisterModel()
        {
            Roles = new List<RoleCheckbox>();
        }
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public List<RoleCheckbox> Roles { get; set; }
    }

    public class RoleCheckbox
    {
        public RoleCheckbox()
        {

        }

        public RoleCheckbox(string name)
        {
            this.Name = name;
        }
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string NormalizeName
        {
            get
            {
                return this.Name.ToUpper();
            }
        }
        public bool Selected { get; set; } = false;
    }
}
