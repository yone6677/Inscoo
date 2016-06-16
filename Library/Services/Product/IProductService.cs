using Domain.Products;

namespace Services.Product
{
   public interface IProductService
    {
        bool Insert(ProductItem item);
        bool Update(ProductItem item);
        /// <summary>
        /// 默认真实删除
        /// </summary>
        /// <param name="item"></param>
        /// <param name="disable"></param>
        /// <returns></returns>
        bool Delete(int id, bool disable = true);
        ProductItem GetById(int id);       
    }
}
