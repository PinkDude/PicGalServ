using BLL.Interface;
using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    class PersonInfoService
    {
        private readonly IHelperService _helper;

        public PersonInfoService(IHelperService h)
        {
            _helper = h;
        }

        public async Task<IEnumerable<PersonInfo>> GetAllPersonInfo(string Name)
        {
            List<string> list = new List<string>()
            {
                "FirstName",
                "MiddleName",
                "LastName"
            };

            for (int i = 0; i < list.Count; i++)
                list[i] += $" LIKE N'%{Name}%'";

            var res = await _helper.Get<PersonInfo>("PersonInfo", "*", false, list);
            return res;
        }
    }
}
