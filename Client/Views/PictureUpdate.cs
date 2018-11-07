using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Views
{
    public class PictureUpdate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AutorId { get; set; }

        public string Description { get; set; }

        public DateTime? Date { get; set; }

        public int GenreId { get; set; }

        public string PicturePath { get; set; }

        public bool Status { get; set; }

        public int? ParentId { get; set; }
    }
}
