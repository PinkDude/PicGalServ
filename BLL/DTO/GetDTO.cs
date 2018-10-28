using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class GetDTO
    {
        public int Take { get; set; } = 10;

        public int Skip { get; set; } = 0;
    }
}
