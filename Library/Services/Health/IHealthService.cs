using System.Collections.Generic;
using Models;

namespace Services
{
    public interface IHealthService
    {
        List<vCheckProductList> GetHealthProducts(string uId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">产品id</param>
        /// <param name="uId">用户id</param>
        /// <returns></returns>
        vCheckProductDetail GetHealthProductById(int id, string uId);
    }
}