
using Domain.Products;

namespace Services.Products
{
   public interface IMixProductItemService
    {
        bool Insert(MixProductItem item);
        bool Update(MixProductItem item);
        bool Delete(MixProductItem item);
        MixProductItem GetById(int id);
    }
}
