﻿using Core.Data;
using Domain;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Core.Pager;
using Models;
using Models.Api.Archive;
using Newtonsoft.Json;

namespace Services
{
    public class ArchiveService : IArchiveService
    {
        private readonly IRepository<Archive> _archiveRepository;
        private readonly IRepository<BusinessLicense> _rpBusinessLicense;
        private readonly IRepository<CarInsurance> _rpCarInsurance;
        private readonly IRepository<MemberInsurance> _rpMemberInsurance;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IRepository<Domain.FileInfo> _rpFileInfo;
        private readonly ILoggerService _loggerService;
        private readonly IFileService _fileService;
        private readonly IWebHelper _webHelper;
        private readonly IResourceService _resourceService;
        public ArchiveService(IRepository<Domain.FileInfo> rpFileInfo, IRepository<CarInsurance> rpCarInsurance, IRepository<Archive> archiveRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService, IFileService fileService,
            IRepository<BusinessLicense> rpBusinessLicense, IWebHelper webHelper, IResourceService resourceService)
        {
            _webHelper = webHelper;
            _rpCarInsurance = rpCarInsurance;
            _archiveRepository = archiveRepository;
            _authenticationManager = authenticationManager;
            _loggerService = loggerService;
            _fileService = fileService;
            _rpBusinessLicense = rpBusinessLicense;
            _rpFileInfo = rpFileInfo;
            _resourceService = resourceService;
        }

        public void DeleteCarInsuranceExcel(int insuranceId)
        {
            try
            {
                _rpCarInsurance.DeleteById(insuranceId);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CarInsuranceExcel：Delete");
                throw e;
            }
        }
        public void DeleteMemberInsuranceExcel(int insuranceId)
        {
            try
            {
                _rpMemberInsurance.DeleteById(insuranceId);
            }
            catch (Exception e)
            {
                throw e;
            }
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
        public bool DeleteById(int id, bool disable, string author)
        {
            try
            {
                var item = _archiveRepository.GetById(id);
                if (item.Author != author) return false;
                if (disable)//从硬盘中删除文件
                {
                    _fileService.DeleteFile(item.Url);
                }
                _archiveRepository.Delete(item, false, disable);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：Delete");
            }
            return false;
        }
        public async Task DeleteFileInfo(Domain.FileInfo fileInfo)
        {
            try
            {
                await Task.Run(() =>
                {
                    _rpFileInfo.DeleteById(fileInfo.Id, true);
                    _fileService.DeleteFile(fileInfo.Url);
                });

            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "FileInfo：Delete");
            }
        }
        public async Task DeleteFileInfo(string url)
        {
            try
            {
                await Task.Run(() =>
                {
                    var item = _rpFileInfo.Table.First(f => f.Url.Equals(url, StringComparison.CurrentCultureIgnoreCase));
                    _rpFileInfo.DeleteById(item.Id, true);
                    _fileService.DeleteFile(url);
                });
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "FileInfo：Delete");
            }
        }
        public async Task DeleteFileBuUrl(string url)
        {
            try
            {
                await Task.Run(() =>
                {
                    var path = AppDomain.CurrentDomain.BaseDirectory + url.Substring(1);
                    if (!File.Exists(path)) return;
                    File.Delete(path);
                });
            }
            //catch(conta)
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "FileInfo：Delete");
            }
        }

        public CarInsurance GetCarEInsuranceUrl(int insuranceId, string uKey)
        {
            try
            {
                var item = _rpCarInsurance.Table.Include(c => c.Einsurance).AsEnumerable().FirstOrDefault(c => c.Id == insuranceId);
                if (item.UniqueKey != uKey) throw new WarningException("无权操作！");
                return item;
            }
            catch (WarningException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public MemberInsurance GetMemberEInsuranceUrl(int insuranceId, string uKey)
        {
            try
            {
                var item = _rpMemberInsurance.Table.Include(c => c.Einsurance).AsEnumerable().FirstOrDefault(c => c.Id == insuranceId);
                if (item.UniqueKey != uKey) throw new WarningException("无权操作！");
                return item;
            }
            catch (WarningException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                return null;
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
        public IQueryable<Archive> GetByTypeAndPId(int pId, string type)
        {
            try
            {
                var list = _archiveRepository.TableNoTracking.Where(a => a.Type == type && a.pId == pId).OrderByDescending(f => f.Id);
                return list;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：GetByTypeAndPId");
            }
            return null;
        }
        public IPagedList<vCarInsuranceList> GetCarInsuranceExcel(int fileType, int pageIndex, int pageSize, string createrId = "-1")
        {
            try
            {
                //var s = _rpCarInsurance.Table.FirstOrDefault().CarInsuranceEinsurances;
                var listOri =
                    _rpCarInsurance.Entities.Include(c => c.Einsurance).Include(c => c.Excel)
                        .Where(c => !c.IsDeleted && c.FileType == fileType && (c.AppUserId == createrId || createrId == "-1")).OrderByDescending(o => o.Id)
                       .AsNoTracking().ToList();
                var list = (from c in listOri
                            let excel = c.Excel
                            let ei = c.Einsurance
                            select new vCarInsuranceList()
                            {
                                ExcelId = excel.Id,
                                InsuranceId = c.Id,
                                ExcelName = excel.Name,
                                ExceUrl = excel.Url,
                                ExceCreateTime = excel.CreateTime,
                                ExceCreateUser = c.AppUser.UserName,
                                EinsuranceName = ei == null ? "" : ei.Name,
                                EinsuranceUrl = ei == null ? "" : ei.Url,
                                Status = c.Status ?? "",
                                EOrderCode = c.EOrderCode ?? "",
                                UniqueKey = c.UniqueKey ?? "",
                                PdfFileName = c.PdfFileName ?? ""
                            }).ToList();

                return new PagedList<vCarInsuranceList>(list.ToList(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CarInsuranceExcel：GetList");
                return new PagedList<vCarInsuranceList>(new List<vCarInsuranceList>(), pageIndex, pageSize);
            }
        }
        public IPagedList<vMemberInsuranceList> GetMemberInsuranceExcel(int fileType, int pageIndex, int pageSize, string author = "-1")
        {
            try
            {
                //var s = _rpCarInsurance.Table.FirstOrDefault().CarInsuranceEinsurances;
                var listOri =
                    _rpMemberInsurance.Entities.Include(c => c.Einsurance).Include(c => c.Excel)
                        .Where(c => !c.IsDeleted && c.FileType == fileType && (c.Author == author || author == "-1")).OrderByDescending(o => o.Id)
                       .AsNoTracking().ToList();
                var list = (from c in listOri
                            let excel = c.Excel
                            let ei = c.Einsurance
                            select new vMemberInsuranceList()
                            {
                                ExcelId = excel.Id,
                                InsuranceId = c.Id,
                                ExcelName = excel.Name,
                                ExceUrl = excel.Url,
                                ExceCreateTime = excel.CreateTime,
                                Author = c.Author,
                                EinsuranceName = ei == null ? "" : ei.Name,
                                EinsuranceUrl = ei == null ? "" : ei.Url,
                                Status = c.Status ?? "",
                                EOrderCode = c.EOrderCode ?? "",
                                UniqueKey = c.UniqueKey ?? "",
                                PdfFileName = c.PdfFileName ?? "",
                                FileType = c.FileType,
                                FileTypeName = c.FileTypeName
                            }).ToList();

                return new PagedList<vMemberInsuranceList>(list.ToList(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：GetMemberInsuranceExcel");
                return null;
            }
        }
        public IPagedList<Archive> GetPagelistByTypeAndPId(int pageIndex, int pageSize, int pId, string type)
        {
            try
            {
                var list = GetByTypeAndPId(pId, type).AsNoTracking(); ;

                return new PagedList<Archive>(list.ToList(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CarInsuranceExcel：GetList");
                return new PagedList<Archive>(new List<Archive>(), pageIndex, pageSize);
            }
        }
        public IPagedList<Archive> GetWZFileDataModels(WZFileSearchModel model, int pageIndex, int pageSize)
        {
            try
            {
                var list = GetByTypeAndPId(model.MasterId, "WZHuman").AsNoTracking();

                if (!string.IsNullOrEmpty(model.Author))
                {
                    list = list.Where(f => f.Author.Contains(model.Author));
                }
                if (!string.IsNullOrEmpty(model.Name))
                {
                    list = list.Where(f => f.Name.Contains(model.Name));
                }
                if (model.CreateTime.HasValue)
                {
                    var date = model.CreateTime.Value.AddDays(1).Date;
                    list = list.Where(f => f.CreateTime <= date);
                }

                return new PagedList<Archive>(list.ToList(), pageIndex, pageSize);
                var type = new System.Diagnostics.StackTrace().GetFrame(0).GetType();
                var method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "ArchiveService：GetWZFileDataModels");
                return new PagedList<Archive>(new List<Archive>(), pageIndex, pageSize);
            }
        }

        public int InsertBySaveResult(SaveResultModel model, string type, int pid, string memo = null)
        {
            try
            {
                var item = new Archive()
                {
                    pId = pid,
                    Memo = memo,
                    Author = _authenticationManager.User.Identity.Name,
                    Name = model.Name + "." + model.Postfix,
                    Type = type,
                    Path = model.Path,
                    Url = model.Path + model.Name + "." + model.Postfix
                };
                return _archiveRepository.InsertGetId(item);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：InsertBySaveResult");
            }
            return 0;
        }
        public Archive InsertByUrl(string url, string type, int pid, string memo = null)
        {
            try
            {
                var item = new Archive()
                {
                    pId = pid,
                    Memo = memo,
                    Author = _authenticationManager.User.Identity.Name,
                    Name = "",
                    Type = type,
                    Path = "",
                    Url = url
                };
                _archiveRepository.Insert(item);
                return item;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：InsertByUrl");
            }
            return null;
        }
        public int InsertByWechat(DownLoadWechatFileApi model)
        {
            try
            {
                var url = _resourceService.GetFileSystem() + "/api/file";
                var postData = JsonConvert.SerializeObject(model);
                var fid = _webHelper.PostData(url, postData, "post", "json");
                if (string.IsNullOrEmpty(fid))
                {
                    return 0;
                }
                return int.Parse(fid);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：InsertByWechat");
            }
            return 0;
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
        public int inert(Archive item)
        {
            try
            {
                return _archiveRepository.InsertGetId(item);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：Insert");
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
        public string InsertWZInsurants(HttpPostedFileBase file, string author, int masterId, string memo, string type)
        {
            try
            {
                var model = _fileService.SaveFile(file, true);
                if (model != null)
                {
                    var item = new Archive()
                    {
                        Author = author,
                        pId = masterId,
                        Memo = memo,
                        Type = "WZHuman",
                        SubType = type,
                        Name = model.Name + model.Postfix,
                        Path = model.Path,
                        Url = model.Path + model.Name + model.Postfix
                    };
                    _archiveRepository.Insert(item);
                    return item.Url;
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：InsertWZInsurants");
            }
            return null;
        }
        public Domain.FileInfo InsertFileInfo(HttpPostedFileBase file, string author, string memo = "")
        {
            try
            {
                var model = _fileService.SaveFile(file);
                if (model != null)
                {
                    var item = new Domain.FileInfo()
                    {
                        Author = author,
                        Memo = memo,
                        Name = model.Name + model.Postfix,
                        Path = model.Path,
                        Url = model.Path + model.Name + model.Postfix,
                        EditTime = DateTime.Now
                    };
                    _rpFileInfo.InsertGetId(item);
                    return item;
                }
                else
                {
                    throw new WarningException("上传失败");
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Archive：Insert");
                throw new WarningException("上传失败");
            }
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
        public string InsertCarInsuranceExcel(HttpPostedFileBase file, string userId, string userName, int fileType)
        {
            try
            {
                var model = _fileService.SaveInsuranceExcel(file, "CarInsuranceExcel");

                if (model != null)
                {
                    var item = new CarInsurance()
                    {
                        Excel = new Domain.FileInfo()
                        {
                            Author = userName,
                            Name = model.Name + model.Postfix,
                            Path = model.Path,
                            Url = model.Path + model.Name + model.Postfix,
                            EditTime = DateTime.Now,
                        },
                        AppUserId = userId,
                        FileType = fileType
                    };
                    item.Status = "A";
                    item.UniqueKey = DateTime.Now.Ticks.ToString();
                    _rpCarInsurance.InsertGetId(item);
                    return item.Excel.Url;
                }
                throw new Exception("操作失败");
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "InsertCarInsuranceEinsurance：Insert");
                throw e;
            }
        }
        public string InsertMemberInsuranceExcel(HttpPostedFileBase file, string userName, string fileTypeName, int fileType)
        {
            try
            {
                var model = _fileService.SaveInsuranceExcel(file, "CarInsuranceExcel");

                if (model != null)
                {
                    var item = new MemberInsurance()
                    {

                        Excel = new Domain.FileInfo()
                        {
                            Author = userName,
                            Name = model.Name + model.Postfix,
                            Path = model.Path,
                            Url = model.Path + model.Name + model.Postfix,
                            EditTime = DateTime.Now,
                        },
                        Author = userName,
                        FileTypeName = fileTypeName,
                        FileType = fileType,
                    };
                    item.Status = "A";
                    item.UniqueKey = DateTime.Now.Ticks.ToString();
                    _rpMemberInsurance.InsertGetId(item);
                    return item.Excel.Url;
                }
                throw new Exception("操作失败");
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "InsertMemberInsuranceExcel：Insert");
                throw e;
            }
        }
        public string InsertCarInsuranceEinsurance(HttpPostedFileBase file, string userName, int insuranceId, string uKey, string code)
        {
            try
            {
                var item = _rpCarInsurance.GetById(insuranceId);
                if (item.UniqueKey != uKey) throw new WarningException("无权操作！");

                if (file != null)
                {
                    var model = _fileService.SaveInsuranceExcel(file, "CarInsuranceExcel");
                    if (model != null)
                    {
                        item.Einsurance =
                       new Domain.FileInfo()
                       {
                           Author = userName,
                           Name = model.Name + model.Postfix,
                           Path = model.Path,
                           Url = model.Path + model.Name + model.Postfix,
                           EditTime = DateTime.Now,
                       };
                    }
                }
                item.EOrderCode = code;

                _rpCarInsurance.Update(item);
                if (item.EinsuranceId.HasValue && !string.IsNullOrEmpty(item.EOrderCode))
                {
                    item.Status = "C";
                    _rpCarInsurance.Update(item);
                }

                _rpCarInsurance.Update(item);
                if (item.Einsurance != null)
                    return item.Einsurance.Url;
                else return null;
            }
            catch (WarningException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "InsertCarInsuranceEinsurance：Insert");
                throw new WarningException("操作失败");
            }
        }
        public string InsertMemberInsuranceEinsurance(HttpPostedFileBase file, string userName, int insuranceId, string uKey, string code)
        {
            try
            {
                var item = _rpMemberInsurance.GetById(insuranceId);
                if (item.UniqueKey != uKey) throw new WarningException("无权操作！");

                if (file != null)
                {
                    var model = _fileService.SaveInsuranceExcel(file, "MemberInsuranceExcel");
                    if (model != null)
                    {
                        item.Einsurance =
                       new Domain.FileInfo()
                       {
                           Author = userName,
                           Name = model.Name + model.Postfix,
                           Path = model.Path,
                           Url = model.Path + model.Name + model.Postfix,
                           EditTime = DateTime.Now,
                       };
                    }
                }
                item.EOrderCode = code;

                _rpMemberInsurance.Update(item);
                if (item.EinsuranceId.HasValue && !string.IsNullOrEmpty(item.EOrderCode))
                {
                    item.Status = "C";
                    _rpMemberInsurance.Update(item);
                }

                _rpMemberInsurance.Update(item);
                if (item.Einsurance != null)
                    return item.Einsurance.Url;
                else return null;
            }
            catch (WarningException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "InsertCarInsuranceEinsurance：Insert");
                throw new WarningException("操作失败");
            }
        }

        public void UploadCarInsuranceEOrderCode(string code, int insuranceId, string uKey)
        {
            try
            {
                var item = _rpCarInsurance.GetById(insuranceId);
                if (item.UniqueKey != uKey) throw new WarningException("无权操作！");
                item.EOrderCode = code;
                item.Status = "C";

                _rpCarInsurance.Update(item);
            }
            catch (WarningException e)
            {
                throw e;
            }
            catch (Exception)
            {
                throw new WarningException("操作失败！");
            }
        }
        public void UploadMemberInsuranceEOrderCode(string code, int insuranceId, string uKey)
        {
            try
            {
                var item = _rpMemberInsurance.GetById(insuranceId);
                if (item.UniqueKey != uKey) throw new WarningException("无权操作！");
                item.EOrderCode = code;
                item.Status = "C";

                _rpMemberInsurance.Update(item);
            }
            catch (WarningException e)
            {
                throw e;
            }
            catch (Exception)
            {
                throw new WarningException("操作失败！");
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





        public string UpdateCarInsuranceExcel(HttpPostedFileBase file, string excelId, string author)
        {
            try
            {
                var item = _rpFileInfo.GetById(Convert.ToInt32(excelId));
                var dir = "";

                if (item.Path.Contains("CarInsuranceExcel")) dir = "CarInsuranceExcel";
                else if (item.Path.Contains("MemberInsuranceExcel")) dir = "MemberInsuranceExcel";

                if (item.Author != author) throw new WarningException("操作失败");
                var model = _fileService.SaveInsuranceExcel(file, dir);
                if (model != null)
                {

                    var oldUrl = item.Url;
                    item.Url = model.Path + model.Name + model.Postfix;
                    item.Name = model.Name;
                    item.Path = model.Path;
                    item.CreateTime = DateTime.Now;
                    _rpFileInfo.Update(item);
                    _fileService.DeleteFile(oldUrl);
                    return item.Url;
                }

                throw new WarningException("保存文件失败");
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, $"excelId：{excelId}");
                throw e;
            }
        }
    }
}
