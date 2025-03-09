using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yanyana.BackEnd.Core.Entities
{
    public class PlaceCategory
    {
        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
