﻿using Domain.Products;
using Models.Insurance;
using System.Collections.Generic;

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
        List<Product> GetList(string company = null, string SafeguardCode = null, string CoverageSum = null, string PayoutRatio = null, string InsuredWho = "主被保险人");
        /// <summary>
        /// 保险酷自定义产品展示
        /// </summary>
        /// <param name="company"></param>
        /// <param name="productType"></param>
        /// <param name="staffNum"></param>
        /// <param name="avarage"></param>
        /// <returns></returns>
        List<ProductListModel> GetProductListForInscoo(string company = null, string productType = null, int staffsNum = 0, string InsuredWho = "主被保险人");
    }
}
