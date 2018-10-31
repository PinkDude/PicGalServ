using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Views
{
    public class PicAndPage
    {
        public int Count { get; set; }

        public IEnumerable<Picture> Pictures { get; set; }
    }
}
