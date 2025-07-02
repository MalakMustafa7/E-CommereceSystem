using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
     public class ProductWithBrandAndTypeSpecification : BaseSpecifications<Product>
    {
        public ProductWithBrandAndTypeSpecification(ProductSpecParams Params) : 
            base(P=>
            (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))
             &&
             (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
             &&
             (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)
             )  
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);

            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(p =>p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p=>p.Name);
                        break;
                }
            }

            ApplyPagination(Params.PageSize*(Params.PageIndex-1), Params.PageSize);
        }
        public ProductWithBrandAndTypeSpecification(int id) : base(p=>p.Id == id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }

    }
}
