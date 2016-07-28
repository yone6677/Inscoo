using Domain.Products;
using Models.Insurance;
using Models;
using System.Collections.Generic;
using System.Web.Mvc;

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
        /// <summary>
        /// 根据产品唯一料号查询
        /// </summary>
        /// <param name="itemNo"></param>
        /// <returns></returns>
        Product GetByItemNo(string itemNo);
        List<Product> GetList(string company = null, string SafeguardCode = null, string CoverageSum = null, string PayoutRatio = null, string InsuredWho = "主被保险人");
        SelectList GetInsuredComs(string selectedValue);
        List<vProvisionPDF> GetProvisionPdfByInsuredCom(string insuredCom);
        SelectList GetSafeguardNameByInsuredCom(string insuredCom);
        /// <summary>
        /// 保险酷自定义产品展示
        /// </summary>
        /// <param name="company"></param>
        /// <param name="productType"></param>
        /// <param name="staffNum"></param>
        /// <param name="avarage"></param>
        /// <returns></returns>
        List<ProductListModel> GetProductListForInscoo(string company = null, string productType = null, int staffsNum = 0, string InsuredWho = "主被保险人");
        vProvisionPDF GetProvisionPdfByInsuredComAndSafeguardName(string insuredCom, string safeguardName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cid">product_ID</param>
        /// <param name="payrat">赔付比例</param>
        /// <param name="staffsNumber">员工人数</param>
        /// <param name="avarage">平均年龄</param>
        /// <returns></returns>
        ProductModel GetProductPrice(int cid = 0, string payrat = null, int staffsNumber = 0, int avarage = 0);

        int UpdateProvisionPath(string insuredCom, string safeguardName, string path);
    }
}
