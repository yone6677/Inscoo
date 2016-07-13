using Core.Data;
using Domain;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using Core.Pager;
using Models;

namespace Services
{
    public class ArchiveService : IArchiveService
    {
        private readonly IRepository<Archive> _archiveRepository;
        private readonly IRepository<BusinessLicense> _rpBusinessLicense;
        private readonly IRepository<CarInsuranceFile> _rpCarInsuranceFile;
        private readonly IRepository<CarInsurance> _rpCarInsurance;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        private readonly IFileService _fileService;
        public ArchiveService(IRepository<CarInsurance> rpCarInsurance, IRepository<Archive> archiveRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService, IFileService fileService, IRepository<BusinessLicense> rpBusinessLicense, IRepository<CarInsuranceFile> rpCarInsuranceFile)
        {
            _rpCarInsuranceFile = rpCarInsuranceFile;
            _rpCarInsurance = rpCarInsurance;
            _archiveRepository = archiveRepository;
            _authenticationManager = authenticationManager;
            _loggerService = loggerService;
            _fileService = fileService;
            _rpBusinessLicense = rpBusinessLicense;
        }

        public void DeleteCarInsuranceExcel(string excelId)
        {
            try
            {

                var item = _rpCarInsurance.GetById(Convert.ToInt32(excelId));
                _rpCarInsurance.Delete(item);
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
        public bool Delete(Archive item, bool disable)
        {
            try
            {
                _archiveRepository.Delete(item, false, disable);
                if (disable)//从硬盘中删除文件
                {
                    _fileService.DeleteFile(item.Url);
                }
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：Delete");
            }
            return false;
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
                    var item = new CarInsuranceFile()
                    {

                        CarInsurance = new CarInsurance() { AppUserId = userId },
                        Author = userName,
                        Name = model.Name + model.Postfix,
                        Path = model.Path,
                        Url = model.Path + model.Name + model.Postfix,
                        EditTime = DateTime.Now
                    };
                    _rpCarInsuranceFile.InsertGetId(item);
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
        public string InsertCarInsuranceEinsurance(HttpPostedFileBase file, string userName, int insuranceId)
        {
            try
            {
                var model = _fileService.SaveCarInsuranceExcel(file);
                if (model != null)
                {
                    var item = new CarInsuranceFile()
                    {
                        Type = 2,
                        CarInsuranceId = insuranceId,
                        Author = userName,
                        Name = model.Name + model.Postfix,
                        Path = model.Path,
                        Url = model.Path + model.Name + model.Postfix,
                        EditTime = DateTime.Now
                    };

                    //var sql =
                    //    string.Format(
                    //        "insert into CarInsuranceExcel (CarInsuranceId,CarInsurance_Id,Author,Name,Path,Url,EditTime) values({0},{1},{2},{3},{4},{5}) ",
                    //        insuranceId, insuranceId, userName, model.Name + model.Postfix, model.Path,
                    //        model.Path + model.Name + model.Postfix, DateTime.Now);
                    _rpCarInsuranceFile.Insert(item);
                    //_rpCarInsuranceEinsurance.DatabaseContext.Database.ExecuteSqlCommand(sql);
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



        public IPagedList<vCarInsuranceList> GetCarInsuranceExcel(int pageIndex, int pageSize, string createrId = "-1")
        {
            try
            {
                //var s = _rpCarInsurance.Table.FirstOrDefault().CarInsuranceEinsurances;
                var listOri =
                    _rpCarInsurance.Entities.Include(c => c.CarInsuranceFiles)
                        .Where(c => !c.IsDeleted && (c.AppUserId == createrId || createrId == "-1"))
                       .AsNoTracking().ToList();
                var list = (from c in listOri
                            let excel = c.CarInsuranceFiles.Where(f => f.Type == 1).OrderByDescending(e => e.Id).FirstOrDefault()
                            let ei = c.CarInsuranceFiles.Where(f => f.Type == 2).OrderByDescending(e => e.Id).FirstOrDefault()
                            select new vCarInsuranceList()
                            {
                                ExcelId = excel.Id,
                                InsuranceId = c.Id,
                                ExcelName = excel.Name,
                                ExceUrl = excel.Url,
                                ExceCreateTime = excel.CreateTime,
                                ExceCreateUser = c.AppUser.UserName,
                                EinsuranceName = ei == null ? "" : ei.Name,
                                EinsuranceUrl = ei == null ? "" : ei.Url
                            }).ToList();

                return new PagedList<vCarInsuranceList>(list.ToList(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CarInsuranceExcel：GetList");
                return new PagedList<vCarInsuranceList>(new List<vCarInsuranceList>(), pageIndex, pageSize);
            }
        }

        public string UpdateCarInsuranceExcel(HttpPostedFileBase file, string excelId)
        {
            try
            {
                var model = _fileService.SaveCarInsuranceExcel(file);
                if (model != null)
                {

                    var item = _rpCarInsuranceFile.GetById(Convert.ToInt32(excelId));
                    var oldUrl = item.Url;
                    item.Url = model.Path + model.Name + model.Postfix;
                    item.Name = model.Name;
                    item.Path = model.Path;
                    item.CreateTime = DateTime.Now;
                    _rpCarInsuranceFile.Update(item);
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
