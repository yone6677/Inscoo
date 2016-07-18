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
        /// <param name="id">��Ʒid</param>
        /// <param name="uId">�û�id</param>
        /// <returns></returns>
        vCheckProductDetail GetHealthProductById(int id, string uId);
    }
}