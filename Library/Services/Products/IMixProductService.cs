using Domain.Products;
using System.Collections.Generic;

namespace Services.Products
{
   public interface IMixProductService
    {
        bool Insert(MixProduct item);
        bool Update(MixProduct item);
        bool Delete(MixProduct item);
        MixProduct GetById(int id);
        List<MixProduct> GetAll();
    }
}
