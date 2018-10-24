using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entity
{
    public class Pictures : CommonEntity
    {
        public string Name { get; set; }

        public int AutorId { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int GenreId { get; set; }

        public string PicturePath { get; set; }
    }
}
