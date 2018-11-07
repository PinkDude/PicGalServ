using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DAL.Entity
{
    public class Pictures : CommonEntity
    {
        public string Name { get; set; }

        public int AutorId { get; set; }

        public string Description { get; set; }

        public DateTime? Date { get; set; }

        public int GenreId { get; set; }

        public string PicturePath { get; set; }

        public bool Status { get; set; }

        public int? ParentId { get; set; }

        public override IEnumerable<string> GetListOfFields()
        {
            List<string> ListOfFields = new List<string>
            {
                "Name",
                "AutorId",
                "Description",
                "Date",
                "GenreId",
                "PicturePath",
                "Status",
                "ParentId"
            };

            return ListOfFields;
        }

        public override IEnumerable<string> GetFields()
        {
            List<string> list = new List<string>
            {
                Name,
                AutorId.ToString(),
                Description,
                Date?.Date.ToString(),
                GenreId.ToString(),
                PicturePath,
                Status ? "1" : "0",
                ParentId.ToString()
            };

            return list;
        }

        public override IEnumerable<string> GetFieldsForSearch()
        {
            List<string> list = new List<string>
            {
                "Name"
            };

            return list;
        }

        public override string GetNameOfTable()
        {
            return "Pictures";
        }
    }
}
