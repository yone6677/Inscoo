using Microsoft.AspNet.Identity;
using Models.Cart;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class CartController : BaseController
    {
        private readonly IWebHelper _webHelper;
        private readonly IHealthService _svHealth;
        public CartController(IWebHelper webHelper, IHealthService svHealth)
        {
            _webHelper = webHelper;
            _svHealth = svHealth;
        }
        // GET: Cart
        public ViewResult Index()
        {
            return View();
        }
        public PartialViewResult List()
        {
            var cartCookie = Request.Cookies["InscooCart"];
            var cartList = new List<CartModel>();
            if (cartCookie != null)
            {
                var decryptJson = _webHelper.DecryptCookie(cartCookie.Value);
                cartList = JsonConvert.DeserializeObject<List<CartModel>>(decryptJson);
                if (cartList.Any())
                {
                    cartList = cartList.Where(c => c.UserId == User.Identity.GetUserId()).ToList();
                }
            }
            return PartialView(cartList);
        }
        public ActionResult Add(int id, int num)
        {
            var cartCookie = Request.Cookies["InscooCart"];
            var uId = User.Identity.GetUserId();
            var prod = _svHealth.GetHealthProductById(id, uId);
            if (cartCookie == null)
            {
                var list = new List<CartModel>();
                if (prod != null)
                {
                    var item = new CartModel()
                    {
                        CreateTime = DateTime.Now,
                        PName = prod.ProductName,
                        Total = num,
                        UserId = uId,
                        Id = id,
                        Amount = prod.PrivilegePrice * num,
                        Price = prod.PrivilegePrice,
                        CompanyName = prod.CompanyName
                    };
                    list.Add(item);
                    var cartJson = JsonConvert.SerializeObject(list);
                    var encryStr = _webHelper.EncryptCookie(cartJson);
                    var cookie = new HttpCookie("InscooCart") { };
                    cookie.Expires = DateTime.Now.AddDays(7);
                    cookie.Value = encryStr;
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                }
            }
            else
            {
                var decryptJson = _webHelper.DecryptCookie(cartCookie.Value);
                var cartList = JsonConvert.DeserializeObject<List<CartModel>>(decryptJson);
                if (cartList.Count > 0)
                {
                    var originalProduct = cartList.Where(s => s.Id == id);
                    var item = new CartModel();
                    if (originalProduct.Any())
                    {
                        item = originalProduct.FirstOrDefault();
                        cartList.Remove(item);
                    }
                    item.Id = id;
                    item.PName = prod.ProductName;
                    item.Price = prod.PrivilegePrice;
                    item.Total += num;
                    item.Amount = item.Total * prod.PrivilegePrice;
                    item.CreateTime = DateTime.Now;
                    item.CompanyName = prod.CompanyName;
                    cartList.Add(item);
                    var cartJson = JsonConvert.SerializeObject(cartList);
                    var encryStr = _webHelper.EncryptCookie(cartJson);
                    var cookie = new HttpCookie("InscooCart");
                    cookie.Expires = DateTime.Now.AddDays(7);
                    cookie.Value = encryStr;
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                var cartCookie = Request.Cookies["InscooCart"];
                var cartList = new List<CartModel>();
                if (cartCookie != null)
                {
                    var decryptJson = _webHelper.DecryptCookie(cartCookie.Value);
                    cartList = JsonConvert.DeserializeObject<List<CartModel>>(decryptJson);
                    cartList.Remove(cartList.Where(s => s.Id == id).FirstOrDefault());
                    var cookie = new HttpCookie("InscooCart");
                    if (cartList.Any())
                    {
                        cookie.Expires = DateTime.Now.AddDays(7);
                    }
                    else
                    {
                        cookie.Expires = DateTime.Now.AddDays(-1);
                    }
                    var cartJson = JsonConvert.SerializeObject(cartList);
                    var encryStr = _webHelper.EncryptCookie(cartJson);
                    cookie.Value = encryStr;
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                }
            }
            return RedirectToAction("Index");
        }
        //[HttpPost]
        public ActionResult Buy(string shoppingCart)
        {
            try
            {
                TempData["cartTmpData"] = shoppingCart;
                return RedirectToAction("MakeSure", "Health", null);
                //var cartList = JsonConvert.DeserializeObject<List<CartBuyModel>>(shoppingCart);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }

}