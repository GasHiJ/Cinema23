using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.BLL.DTOs
{
    public class HallDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rows { get; set; }
        public int SeatsPerRow { get; set; }
    }

}
