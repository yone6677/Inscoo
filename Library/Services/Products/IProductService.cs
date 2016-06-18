using Domain.Products;

namespace Services.Products
{
   public interface IProductService
    {
        bool Insert(Product item);
        bool Update(Product item);
        /// <summary>
        /// 默认真实删除
        /// </summary>
        /// <param name="item"></param>
        /// <param name="disable"></param>
        /// <returns></returns>
        bool Delete(int id, bool disable = true);
        Product GetById(int id);       
    }
}
