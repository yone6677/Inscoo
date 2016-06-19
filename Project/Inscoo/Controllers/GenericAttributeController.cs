using Domain.Common;
using Innscoo.Infrastructure;
using Models.Common;
using Services.Common;
using System.Net;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class GenericAttributeController : BaseController
    {
        private readonly IGenericAttributeService _genericAttributeService;
        public GenericAttributeController(IGenericAttributeService genericAttributeService)
        {
            _genericAttributeService = genericAttributeService;
        }
        // GET: GenericAttribute
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var model = _genericAttributeService.GetListOfPager(1, 15);
            var command = new PageCommand()
            {
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                TotalCount = model.TotalCount,
                TotalPages = model.TotalPages
            };
            ViewBag.pageCommand = command;
            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult List(int PageIndex = 1, int PageSize = 15, string keyGroup = null)
        {
            var model = _genericAttributeService.GetListOfPager(PageIndex, PageSize, keyGroup);
            var command = new PageCommand()
            {
                PageIndex = model.PageIndex,
                PageSize = model.PageSize,
                TotalCount = model.TotalCount,
                TotalPages = model.TotalPages
            };
            ViewBag.pageCommand = command;
            return PartialView(model);
        }
        public ActionResult Create()
        {
            var model = new GenericAttributeModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GenericAttributeModel model)
        {
            if (ModelState.IsValid)
            {
                var item = new GenericAttribute()
                {
                    Description = model.Description,
                    Key = model.Key,
                    KeyGroup = model.KeyGroup,
                    Value = model.Value
                };
                if (_genericAttributeService.Insert(item))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);

        }
        public ActionResult Edit(int id = 0)
        {
            if (id > 0)
            {
                var item = _genericAttributeService.GetById(id);
                if (item != null)
                {
                    var model = new GenericAttributeModel()
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Key = item.Key,
                        KeyGroup = item.KeyGroup,
                        Value = item.Value
                    };
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GenericAttributeModel model)
        {
            if (ModelState.IsValid)
            {
                var item = _genericAttributeService.GetById(model.Id);
                if (item != null)
                {
                    item.Description = model.Description;
                    item.Key = model.Key;
                    item.KeyGroup = model.KeyGroup;
                    item.Value = model.Value;
                    if (_genericAttributeService.Update(item))
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);
        }
        public ActionResult Details(int id = 0)
        {
            if (id > 0)
            {
                var item = _genericAttributeService.GetById(id);
                if (item != null)
                {
                    var model = new GenericAttributeModel()
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Key = item.Key,
                        KeyGroup = item.KeyGroup,
                        Value = item.Value
                    };
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                var item = _genericAttributeService.GetById(id);
                if (item != null)
                {
                    var model = new GenericAttributeModel()
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Key = item.Key,
                        KeyGroup = item.KeyGroup,
                        Value = item.Value,
                        Author = item.Author,
                        CreateTime = item.CreateTime
                    };
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!_genericAttributeService.Delete(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Index");
        }
    }
}