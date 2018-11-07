using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DAL.Entity
{
    public class PersonInfo : CommonEntity
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Photo { get; set; }

        public string Description { get; set; }

        public bool Autor { get; set; }

        public DateTime? Birthday { get; set; }

        public override IEnumerable<string> GetListOfFields()
        {
            List<string> ListOfFields = new List<string>
            {
                "FirstName",
                "MiddleName",
                "LastName",
                "Phone",
                "Photo",
                "Description",
                "Autor",
                "Birthday"
            };

            return ListOfFields;
        }

        public override IEnumerable<string> GetFields()
        {
            List<string> list = new List<string>
            {
                FirstName,
                MiddleName,
                LastName,
                Phone,
                Photo,
                Description,
                Autor ? "1" : "0",
                Birthday == null ? "" : $"{this.Birthday.Value.Year}-{this.Birthday.Value.Month}-{this.Birthday.Value.Day}"
            };


            return list;
        }

        public override IEnumerable<string> GetFieldsForSearch()
        {
            List<string> list = new List<string>
            {
                "FirstName",
                "MiddleName",
                "LastName"
            };

            return list;
        }

        public override string GetNameOfTable()
        {
            return "PersonInfo";
        }
    }
}


