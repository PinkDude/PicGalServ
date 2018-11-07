using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entity
{
    public class ApplicationUsers : CommonEntity
    {
        public string Email { get; set; }

        public string EmailNorm { get; set; }

        public string PasswordHash { get; set; }

        public int PersonInfoId { get; set; }

        public int RoleId { get; set; }

        public string Role { get; set; }

        public override string GetNameOfTable()
        {
            return "ApplicationUsers";
        }

        public override IEnumerable<string> GetListOfFields()
        {
            var list = new List<string>
            {
                "Email",
                "EmailNorm",
                "PasswordHash",
                "PersonInfoId",
                "RoleId"
            };
            return list;
        }

        public override IEnumerable<string> GetFields()
        {
            var list = new List<string>
            {
                Email,
                EmailNorm,
                PasswordHash,
                PersonInfoId.ToString(),
                RoleId.ToString()
            };
            return list;
        }
    }
}
