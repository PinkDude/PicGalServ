using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Views
{
    public class Picture
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AutorId { get; set; }

        public string Description { get; set; }

        public DateTime? Date { get; set; }

        public string Genre { get; set; }

        public string PicturePath { get; set; }

        public AutorDTO Autor { get; set; }

        public int GenreId { get; set; }

        public bool Status { get; set; }
    }
}
