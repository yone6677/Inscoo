﻿using Core.Data;
using Domain;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Web;

namespace Services
{
    public class ArchiveService : IArchiveService
    {
        private readonly IRepository<Archive> _archiveRepository;
        private readonly IRepository<BusinessLicense> _rpBusinessLicense;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        private readonly IFileService _fileService;
        public ArchiveService(IRepository<Archive> archiveRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService, IFileService fileService, IRepository<BusinessLicense> rpBusinessLicense)
        {
            _archiveRepository = archiveRepository;
            _authenticationManager = authenticationManager;
            _loggerService = loggerService;
            _fileService = fileService;
            _rpBusinessLicense = rpBusinessLicense;
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
    }
}
