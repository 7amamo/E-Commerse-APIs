using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : Specifications<Product>
    {
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams parameters) : base(p=>
        (string.IsNullOrEmpty(parameters.Search) || p.Name.ToLower().Contains(parameters.Search) )
        &&
        (!parameters.TypeId.HasValue || p.ProductTypeId == parameters.TypeId)
        &&
        (!parameters.BrandId.HasValue || p.ProductBrandId == parameters.BrandId)
        )
        {
            if (! string.IsNullOrEmpty(parameters.Sort))
            {
                switch (parameters.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDescinding(p => p.Price);
                        break;

                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }

            ApplyPagination(parameters.PageIndex, parameters.PageSize);

        }

        public ProductWithBrandAndTypeSpecifications(int id ) : base(p=>p.Id == id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }
    }
}
