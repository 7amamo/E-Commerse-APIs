using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductSpecParams : SpecParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

    }
}
