using Core.Data;
using Domain;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Core.Pager;

namespace Services
{
    public class ArchiveService : IArchiveService
    {
        private readonly IRepository<Archive> _archiveRepository;
        private readonly IRepository<BusinessLicense> _rpBusinessLicense;
        private readonly IRepository<CarInsuranceExcel> _rpCarInsuranceExcel;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        private readonly IFileService _fileService;
        public ArchiveService(IRepository<Archive> archiveRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService, IFileService fileService, IRepository<BusinessLicense> rpBusinessLicense, IRepository<CarInsuranceExcel> rpCarInsuranceExcel)
        {
            _archiveRepository = archiveRepository;
            _authenticationManager = authenticationManager;
            _loggerService = loggerService;
            _fileService = fileService;
            _rpBusinessLicense = rpBusinessLicense;
            _rpCarInsuranceExcel = rpCarInsuranceExcel;
        }

        public void DeleteCarInsuranceExcel(string excelId)
        {
            try
            {

                var item = _rpCarInsuranceExcel.GetById(Convert.ToInt32(excelId));
                _rpCarInsuranceExcel.Delete(item);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CarInsuranceExcel：Delete");
                throw e;
            }
        }

        public Archive GetById(int id)
        {
            try
            {
                return _archiveRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：GetById");
            }
            return null;
        }

        public int InsertByUrl(List<string> fileInfo, string type, int pid, string memo)
        {
            try
            {
                var item = new Archive()
                {
                    pId = pid,
                    Memo = memo,
                    Author = _authenticationManager.User.Identity.Name,
                    Name = fileInfo[3],
                    Type = type,
                    Path = fileInfo[2],
                    Url = fileInfo[1]
                };
                return _archiveRepository.InsertGetId(item);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：InsertByUrl");
            }
            return 0;
        }
        public int Insert(HttpPostedFileBase file, string type, int pid, string memo)
        {
            try
            {
                var model = _fileService.SaveFile(file);
                if (model != null)
                {
                    var item = new Archive()
                    {
                        Author = _authenticationManager.User.Identity.Name,
                        pId = pid,
                        Memo = memo,
                        Type = type,
                        Name = model.Name + model.Postfix,
                        Path = model.Path,
                        Url = model.Path + model.Name + model.Postfix
                    };
                    return _archiveRepository.InsertGetId(item);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：Insert");
            }
            return 0;
        }
        public int InsertBusinessLicense(HttpPostedFileBase file, string userName, int companyId)
        {
            try
            {
                var model = _fileService.SaveFile(file);
                if (model != null)
                {
                    var item = new BusinessLicense()
                    {
                        CompanyId = companyId,
                        Author = userName,
                        Name = model.Name + model.Postfix,
                        Path = model.Path,
                        Url = model.Path + model.Name + model.Postfix
                    };
                    return _rpBusinessLicense.InsertGetId(item);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "BusinessLicense：Insert");
            }
            return 0;
        }
        public string InsertCarInsuranceExcel(HttpPostedFileBase file, string userId, string userName)
        {
            try
            {
                var model = _fileService.SaveCarInsuranceExcel(file);
                if (model != null)
                {
                    var item = new CarInsuranceExcel()
                    {
                        AppUserId = userId,
                        Author = userName,
                        Name = model.Name + model.Postfix,
                        Path = model.Path,
                        Url = model.Path + model.Name + model.Postfix,
                        EinsuranceEditTime = DateTime.Now
                    };
                    _rpCarInsuranceExcel.InsertGetId(item);
                    return item.Url;
                }
                throw new Exception("操作失败");
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "BusinessLicense：Insert");
                throw e;
            }
        }
        public string InsertCarEinsurance(HttpPostedFileBase file, string excelId)
        {
            try
            {
                var model = _fileService.SaveCarInsuranceExcel(file);
                if (model != null)
                {

                    var item = _rpCarInsuranceExcel.GetById(Convert.ToInt32(excelId));
                    var oldUrl = item.EinsuranceUrl;
                    item.EinsuranceUrl = model.Path + model.Name + model.Postfix;
                    item.EinsuranceName = model.Name;
                    item.EinsurancePath = model.Path;
                    item.EinsuranceEditTime=DateTime.Now;
                    _rpCarInsuranceExcel.Update(item);
                    if (!string.IsNullOrEmpty(oldUrl))
                        _fileService.DeleteFile(oldUrl);
                    return item.EinsuranceUrl;
                }
                else
                {
                    throw new Exception("保存文件失败");
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "BusinessLicense：Insert");
                throw e;
            }
            return "";
        }
        public string InsertUserPortrait(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 200 * 1024) throw new Exception("文件太大");
                var model = _fileService.SaveFile(file);
                if (model != null)
                {
                    return model.Path + model.Name + model.Postfix;
                }
                throw new Exception("修改失败");
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public IPagedList<CarInsuranceExcel> GetCarInsuranceExcel(int pageIndex, int pageSize, string createrId = "-1")
        {
            try
            {
                var list = _rpCarInsuranceExcel.Table.Where(c => c.AppUserId == createrId || createrId == "-1");
                return new PagedList<CarInsuranceExcel>(list.ToList(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CarInsuranceExcel：GetList");
                return new PagedList<CarInsuranceExcel>(new List<CarInsuranceExcel>(), pageIndex, pageSize);
            }
        }

        public string UpdateCarInsuranceExcel(HttpPostedFileBase file, string excelId)
        {
            try
            {
                var model = _fileService.SaveCarInsuranceExcel(file);
                if (model != null)
                {

                    var item = _rpCarInsuranceExcel.GetById(Convert.ToInt32(excelId));
                    var oldUrl = item.Url;
                    item.Url = model.Path + model.Name + model.Postfix;
                    item.Name = model.Name;
                    item.Path = model.Path;
                    item.CreateTime = DateTime.Now;
                    _rpCarInsuranceExcel.Update(item);
                    _fileService.DeleteFile(oldUrl);
                    return item.Url;
                }

                throw new Exception("保存文件失败");
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CarInsuranceExcel：Update");
                throw e;
            }
        }
    }
}
