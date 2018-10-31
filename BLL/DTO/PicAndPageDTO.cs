using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class PicAndPageDTO
    {
        public int Count { get; set; }

        public IEnumerable<PictureDTO> Pictures { get; set; }
    }
}
