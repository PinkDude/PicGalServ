using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Views
{
    public class AuAndPage
    {
        public int Count { get; set; }

        public IEnumerable<PersonInfo> Persons { get; set; }
    }
}
