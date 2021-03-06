﻿using Domain;
using Core.Pager;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using Models.Role;
using Models.User;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core;
using Core.Data;
using Domain.Common;
using System.ComponentModel;

namespace Services
{
    public class AppUserService : IAppUserService
    {
        private readonly AppUserManager _userManager;
        private readonly AppRoleManager _appRoleManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        private readonly IRepository<GenericAttribute> _rpGenericAttribute;
        private readonly IRepository<WZHumanMaster> _rpWZHumanMaster;
        private readonly IRepository<CreateAccountCode> _rpCreateAccountCode;
        public AppUserService(IRepository<CreateAccountCode> rpCreateAccountCode, IRepository<WZHumanMaster> rpWZHumanMaster, ILoggerService loggerService, AppUserManager userManager, AppRoleManager appRoleManager,
            IAuthenticationManager authenticationManager, IRepository<GenericAttribute> rpGenericAttribute)
        {
            _rpWZHumanMaster = rpWZHumanMaster;
            _userManager = userManager;
            _appRoleManager = appRoleManager;
            _loggerService = loggerService;
            _authenticationManager = authenticationManager;
            _rpGenericAttribute = rpGenericAttribute;
            _rpCreateAccountCode = rpCreateAccountCode;
        }


        public Task<IdentityResult> AddToRoleAsync(string uid, string roleName)
        {
            try
            {
                var user = _userManager.AddToRoleAsync(uid, roleName);
                return user;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:AddToRoleAsync");
                return null;
            }
        }
        public bool DeleteBeforeRoleAndNew(string uid, string roleName)
        {
            try
            {
                var roles = _userManager.GetRoles(uid);
                var isRolesRemoved = _userManager.RemoveFromRoles(uid, roles.ToArray());
                if (!isRolesRemoved.Succeeded)
                {
                    return false;
                }
                var result = _userManager.AddToRole(uid, roleName);
                if (result.Succeeded)
                {
                    if (roleName == "WZHumanCustomer" || roleName == "WZHumanAssistant")//吴中人力资源要角色在WZHumanMaster表中新增一条记录
                    {
                        var user = _userManager.FindById(uid);

                        var item = _rpWZHumanMaster.Table.SingleOrDefault(w => w.Account == user.UserName);
                        if (item != null)//已存在
                        {
                            item.Account = user.UserName;
                            item.CompanyName = user.CompanyName;
                            _rpWZHumanMaster.Update(item);
                            return true;
                        }
                        else
                        {
                            var id = _rpWZHumanMaster.InsertGetId(new WZHumanMaster() { Account = user.UserName, CompanyName = user.CompanyName, Author = "Admin" });
                            return id > 0;
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, string name, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Error, "AppUserService:CreateAsync");
                throw e;
            }
        }
        public Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string appCookie)
        {
            try
            {
                var result = _userManager.CreateIdentityAsync(user, appCookie);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:CreateIdentityAsync");
                return null;
            }
        }
        public ClaimsIdentity CreateIdentity(AppUser user, string appCookie)
        {
            try
            {
                var result = _userManager.CreateIdentity(user, appCookie);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:CreateIdentity");
                return null;
            }
        }
        public bool ChangePassword(string id, string oldPassword, string password)
        {
            try
            {
                var result = _userManager.ChangePassword(id, oldPassword, password);
                if (result.Succeeded) return true;
                else
                {
                    throw new Exception(result.Errors.First());
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public Task<IdentityResult> DeleteAsync(AppUser user)
        {
            try
            {
                var result = _userManager.DeleteAsync(user);
                return result;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:DeleteAsync");
                return null;
            }
        }

        public AppUser FindByEmail(string email)
        {

            try
            {
                var result = _userManager.FindByEmail(email);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:FindByEmail");
                return null;
            }

        }
        public AppUser FindByName(string name)
        {
            try
            {
                var result = _userManager.FindByName(name);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:FindByName");
                return null;
            }
        }
        public AppUser FindById(string id)
        {

            try
            {
                var result = _userManager.FindById(id);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:FindById");
                return null;
            }

        }
        public AppUser Find(string userName, string password)
        {
            try
            {
                var result = _userManager.Find(userName, password);
                return result;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:Find");
                return null;
            }
        }
        public AppUser Find(UserLoginInfo info)
        {
            try
            {
                var result = _userManager.Find(info);
                return result;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:Find");
                return null;
            }
        }

        public IPagedList<UserModel> GetUserList(int pageIndex, int pageSize, string userName, string email, string role, string roleId, string createUserId)
        {
            try
            {
                var model = new List<UserModel>();
                var user = _userManager.Users.Where(u =>
                (createUserId == "-1" || u.CreaterId == createUserId)

                ).ToList();
                if (!string.IsNullOrEmpty(userName))
                {
                    user = user.Where(s => s.UserName.Contains(userName)).ToList();
                }
                if (!string.IsNullOrEmpty(role))
                {
                    var rId = _appRoleManager.FindByName(role).Id;
                    user = user.Where(s => s.Roles.Any(r => r.RoleId == rId)).ToList();
                }
                if (!string.IsNullOrEmpty(roleId))
                {
                    user = user.Where(s => s.Roles.Any(r => r.RoleId == roleId)).ToList();
                }
                if (!string.IsNullOrEmpty(email))
                {
                    user = user.Where(s => s.Email == email).ToList();
                }
                if (user.Count > 0)
                {
                    model = user.Select(u => new UserModel
                    {
                        CompanyName = u.CompanyName,
                        Id = u.Id,
                        Name = u.UserName,
                        Email = u.Email,
                        Phone = u.PhoneNumber,
                        LinkMan = u.LinkMan,
                        FanBao = u.FanBao,
                        TiYong = u.TiYong,
                        RoleIds = u.Roles.Any() ? u.Roles.Select(r => r.RoleId).ToList() : null,
                        RoleName = u.Roles.Any() ? _appRoleManager.FindById(u.Roles.First().RoleId).Name : "",
                        CreateTime = u.CreateTime,
                        CreaterId = u.CreaterId
                    }).OrderBy(c => c.CreateTime).ToList();
                }
                return new PagedList<UserModel>(model, pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:GetUserList");
            }
            return null;
        }
        public List<UserRoleModel> GetUserRoles()
        {
            try
            {
                var roles = FindByName(_authenticationManager.User.Identity.Name).Roles;
                if (roles.Any())
                {
                    var list = new List<UserRoleModel>();
                    foreach (var s in roles)
                    {
                        var item = new UserRoleModel()
                        {
                            RoleId = s.RoleId,
                            UserId = s.UserId,
                            RoleName = _appRoleManager.FindById(s.RoleId).Name
                        };
                        list.Add(item);
                    }
                    return list;
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:GetUserRoles");
            }
            return new List<UserRoleModel>();
        }
        public AppUser GetCurrentUser()
        {
            try
            {
                var name = _authenticationManager.User.Identity.Name;
                return FindByName(name);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:GetUserRoles");
            }
            return null;
        }
        public RegisterModel Get_RegisterModel_ById(string id)
        {

            try
            {
                var result = _userManager.FindById(id);
                if (result != null)
                {
                    var role = result.Roles;
                    var roles = "";
                    if (role.Any()) roles = _appRoleManager.FindById(role.First().RoleId).Name;
                    string[] nulklStr = new string[0];
                    return new RegisterModel
                    {
                        Id = result.Id,
                        CompanyName = result.CompanyName,
                        UserName = result.UserName,
                        PhoneNumber = result.PhoneNumber,
                        Linkman = result.LinkMan,
                        Email = result.Email,
                        TiYong = result.TiYong,
                        FanBao = result.FanBao,
                        BankName = result.BankName,
                        BankNumber = result.BankNumber,
                        Rebate = result.Rebate,
                        CommissionMethod = result.CommissionMethod,
                        AccountName = result.AccountName,
                        IsDelete = result.IsDelete,
                        Roles = roles,
                        ProdSeries = result.ProdSeries == null ? nulklStr : result.ProdSeries.Split(';'),
                        ProdInsurances = result.ProdInsurance == null ? nulklStr : result.ProdInsurance.Split(';'),
                        Memo = result.Memo

                    };
                }
                else
                {
                    throw new Exception("未找到");
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public string GetRoleByUserId(string uId)
        {
            try
            {
                var role = _userManager.GetRoles(uId).FirstOrDefault();
                if (role == null) throw new Exception("尚未给该用户分配角色");
                return role;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public List<string> GetRolesByUserId(string uId)
        {
            try
            {
                var roles = _userManager.GetRoles(uId);
                if (!roles.Any()) throw new Exception("尚未给该用户分配角色");
                return roles.ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public SelectList GetRolesManagerPermissionByUserId(string uId, string valueField, string sellectedValue)
        {
            try
            {
                var allRoles = _appRoleManager.Roles.ToList();
                var userRoles = GetRolesByUserId(uId);
                if (!userRoles.Any())
                {
                    throw new Exception("尚未给该用户分配角色");
                }
                if (userRoles.Contains("Admin"))
                {
                }
                else
                {
                    allRoles.RemoveAll(r => r.Name.Equals("Admin"));

                    allRoles.RemoveAll(r => r.Name.Equals("InscooFinance"));
                    allRoles.RemoveAll(r => r.Name.Equals("InsuranceCompany"));
                    allRoles.RemoveAll(r => r.Name.Equals("InscooOperator"));

                    allRoles.RemoveAll(r => r.Name.Equals("CarInscuranceCompany"));
                    allRoles.RemoveAll(r => r.Name.Equals("CarInscuranceCustomer"));

                    allRoles.RemoveAll(r => r.Name.Equals("WZHumanAssistant"));
                    allRoles.RemoveAll(r => r.Name.Equals("WZHumanCustomer"));

                    allRoles.RemoveAll(r => r.Name.Equals("HealthCheck"));

                }

                if (userRoles.Contains("BusinessDeveloper"))
                {
                    allRoles =
                        _appRoleManager.Roles.Where(r => r.Name.Equals("PartnerChannel") || r.Name.Equals("CompanyHR"))
                            .ToList();
                }

                if (userRoles.Contains("PartnerChannel"))
                {
                    allRoles.RemoveAll(r => !r.Name.Equals("CompanyHR"));
                }

                if (userRoles.Contains("CompanyHR"))
                {
                    allRoles.RemoveAll(r => r.Name.Equals("BusinessDeveloper"));
                    allRoles.RemoveAll(r => r.Name.Equals("PartnerChannel"));
                }

                if (!allRoles.Any())
                {
                    throw new Exception("尚未给该用户分配角色");
                }

                SelectList result;
                if (!allRoles.Any()) allRoles = new List<AppRole>();
                if (valueField.Equals("Name", StringComparison.CurrentCultureIgnoreCase))
                {
                    result = new SelectList(allRoles, "Name", "Description", sellectedValue);
                }
                else if (valueField.Equals("Id", StringComparison.CurrentCultureIgnoreCase))
                    result = new SelectList(allRoles, "Id", "Description", sellectedValue);
                else
                {
                    throw new Exception("valueField 只能是Name或Id");
                }
                if (!result.Any())
                {
                    throw new Exception("尚未给该用户分配角色");
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public SelectList GetProdInsurances(string uId)
        {
            var isAdmin = _userManager.GetRoles(uId).Contains("Admin");
            var list =
                   _rpGenericAttribute.TableNoTracking.Where(s => s.KeyGroup == "InsuranceCompany")
                       .Select(s => new { s.Key, s.Value })
                       .ToList();
            if (!isAdmin)
            {
                var items = _userManager.FindByIdAsync(uId).Result.ProdInsurance.Split(';');
                list = list.Where(l => items.Contains(l.Value)).ToList();
            }
            return new SelectList(list, "Value", "Key");
        }
        public SelectList GetProdSeries(string uId)
        {
            var isAdmin = _userManager.GetRoles(uId).Contains("Admin");
            var list =
                   _rpGenericAttribute.TableNoTracking.Where(s => s.KeyGroup == "ProductSeries")
                       .Select(s => new { s.Key, s.Value })
                       .ToList();
            if (!isAdmin)
            {
                var u = _userManager.FindById(uId);
                if (!string.IsNullOrEmpty(u.ProdSeries))
                {
                    var items = u.ProdSeries.Split(';');
                    list = list.Where(l => items.Contains(l.Value)).ToList();
                }
                else
                {
                    list.Clear();
                    //list.Add(new { Key = "没有", Value = "nothing" });
                }
            }
            return new SelectList(list, "Value", "Key");
        }

        public bool IsUserExist(string key)
        {
            try
            {
                var role = _userManager.FindByEmail(key) != null;
                if (role) return true;

                role = _userManager.FindByName(key) != null;

                if (role) return true;

                return false;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void SignIn(AppUser user, bool isPersistent)
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            _authenticationManager.SignIn(new AuthenticationProperties() { ExpiresUtc = DateTime.UtcNow.AddHours(6), IsPersistent = isPersistent }, identity);//默认6小时内不需要再次登陆
        }
        public void SignOut()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }


        public Task<IdentityResult> UpdateAsync(AppUser user)
        {
            try
            {
                user.Changer = _authenticationManager.User.Identity.Name;
                user.ModifyTime = DateTime.Now;
                var result = _userManager.UpdateAsync(user);
                return result;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:UpdateAsync");
                return null;
            }
        }
        public bool Update(AppUser user)
        {
            try
            {
                user.Changer = _authenticationManager.User.Identity.Name;
                user.ModifyTime = DateTime.Now;
                var result = _userManager.Update(user);

                if (result.Succeeded) return true;
                else throw new Exception(result.Errors.First());
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:Update");
                throw e;
            }
        }
        public Task<IdentityResult> UpdateSecurityStampAsync(AppUser user)
        {
            try
            {
                user.Changer = _authenticationManager.User.Identity.Name;
                user.ModifyTime = DateTime.Now;
                var result = _userManager.UpdateSecurityStampAsync(user.Id);
                return result;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:UpdateSecurityStampAsync");
                return null;
            }
        }

        #region CreateAccountCode
        public bool AddCreateAccountCode(CreateAccountCode model)
        {
            try
            {
                model.AccountEncryCode = GetAccountEncryCode();
                return _rpCreateAccountCode.InsertGetId(model) > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> CreateUserByEncry(EncryInfoModel encry)
        {
            try
            {
                var model = GetAccountEncryByCode(encry.AccountEncryCode.Trim());
                if (model == null) throw new WarningException("您的邀请码错误");
                //if (model.EncryBeginDate.Value.Date > DateTime.Now.Date || model.IsUsed)
                if (model.EncryBeginDate.Value.Date > DateTime.Now.Date)
                {
                    throw new WarningException("您的邀请码不可用");
                }
                if (model.EncryEndDate.HasValue)
                {
                    if (model.EncryEndDate < DateTime.Now)
                    {
                        throw new WarningException("您的邀请码已过期");
                    }
                }
                var user = new AppUser()
                {
                    //BankName = model.BankName,
                    //BankNumber = model.BankNumber,
                    UserName = encry.Email,
                    Email = encry.Email,
                    PhoneNumber = encry.Phone,
                    LinkMan = encry.LinkMan,
                    CompanyName = encry.CompanyName,

                    TiYong = model.EncryTiYong,
                    FanBao = model.EncryFanBao,
                    CreaterId = model.EncryCreateID,
                    Changer = model.EncryCreateID,
                    CommissionMethod = model.EncryCommissionMethod,
                    Rebate = model.EncryRebate,
                    ProdSeries = model.EncrySeries,
                    ProdInsurance = model.EncryInsurance,
                    Memo = model.AccountEncryCode

                };
                var result = await CreateAsync(user, user.UserName, encry.Password);
                if (result.Succeeded)
                {
                    var isDddRole = DeleteBeforeRoleAndNew(user.Id, model.EncryRoleName);
                    if (isDddRole)
                    {
                        model.IsUsed = true;
                        _rpCreateAccountCode.Update(model);
                        var mailContent = string.Format("<p><b>{0}</b>,您好：</p><div style=\"text-indent:4em;\"><p>已为您开通保酷平台的用户权限，请登录使用，详情如下：</p><p>            登录网站：<b>www.inscoo.com</b></p><p>            登录账号：<b>{1}</b></p><p>            密码：<b>inscoo</b></p><p>请您在首次登录后立即修改密码，谢谢！</p><br><p>如果有任何疑问，请随时拨打400-612-6750咨询！</p><p>欢迎加入保酷大家庭，祝您工作愉快，顺祝商祺！</p><br></div><p><b>保酷网 www.inscoo.com</b></p><p style=\"overflow:hidden\"><img src=\"http://www.inscoo.com/Content/img/InscooLogo.png\"alt=\"\"style=\"float: left;\" /><img src=\"http://www.inscoo.com/Content/img/InscooWeChat.png\" alt=\"\" style=\"float: left;\" /></p><p>上海皓为商务咨询有限公司</p>", user.UserName, user.Email);
                        MailService.SendMail(new MailQueue()
                        {
                            MQTYPE = "保酷账号",
                            MQSUBJECT = "保酷账号",
                            MQMAILCONTENT = mailContent,
                            MQMAILFRM = "service@inscoo.com",
                            MQMAILTO = user.Email,
                            //MQFILE = AppDomain.CurrentDomain.BaseDirectory + @"Archive\Template\caozuozhinan.docx"

                        });
                        return true;
                    }

                }

                return false;
            }
            catch (DbEntityValidationException e)
            {
                _loggerService.insert(e, LogLevel.Error, "AppUserService:CreateUserByEncry");
                throw e;
            }
        }


        public string GetAccountEncryCode()
        {
            try
            {
                var dt = DateTime.Now.Ticks.ToString("X").Substring(7, 7);
                var isExist = _rpCreateAccountCode.TableNoTracking.Where(c => c.AccountEncryCode == dt).Any();
                while (isExist)
                {
                    dt = DateTime.Now.Ticks.ToString().Substring(9);
                    isExist = _rpCreateAccountCode.TableNoTracking.Where(c => c.AccountEncryCode == dt).Any();
                }
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public CreateAccountCode GetCreateAccountCode(int id)
        {
            try
            {
                var item = _rpCreateAccountCode.GetById(id);
                return item;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public bool DeleteAccountCode(int id)
        {
            try
            {
                _rpCreateAccountCode.DeleteById(id);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool UpdateAccountCode(CreateAccountCode model)
        {
            try
            {
                _rpCreateAccountCode.Update(model);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public CreateAccountCode GetAccountEncryByCode(string code)
        {
            try
            {
                var item = _rpCreateAccountCode.Table.FirstOrDefault(c => c.AccountEncryCode == code);

                return item;
            }
            catch (Exception e)
            {
                throw null;
            }

        }
        public TRegisterModel GetTRegisterModelById(int id)
        {
            try
            {
                var ss = _rpCreateAccountCode.TableNoTracking.Where(i => i.Id == id).ToList();
                var item = ss.Select(i => new TRegisterModel
                {
                    CommissionMethod = i.EncryCommissionMethod,
                    Roles = i.EncryRoleName,
                    CompanyName = i.EncryCompanyName,
                    ProdSeries = i.EncrySeries.Split(';'),
                    FanBao = i.EncryFanBao,
                    TiYong = i.EncryTiYong,
                    EncryBeginDate = i.EncryBeginDate,
                    EncryEndDate = i.EncryEndDate,
                    Id = i.Id,
                    Memo = i.EncryMemo,
                    ProdInsurances = i.EncryInsurance.Split(';'),
                    Rebate = i.EncryRebate
                });

                return item.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw null;
            }

        }

        public IPagedList<CreateAccountCode> GetCreateAccountList(int pageIndex, int pageSize, string company, string roleId)
        {
            try
            {
                var model = new List<UserModel>();
                var user = _rpCreateAccountCode.TableNoTracking.ToList();

                if (!string.IsNullOrEmpty(roleId))
                {
                    var rolename = _appRoleManager.FindById(roleId).Name;
                    user = user.Where(s => s.EncryRoleName == rolename).ToList();
                }
                if (!string.IsNullOrEmpty(company))
                {
                    user = user.Where(s => s.EncryCompanyName == company).ToList();
                }


                return new PagedList<CreateAccountCode>(user.OrderByDescending(i => i.Id).ToList(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Information, "AppUserService:GetCreateAccountList");
            }
            return null;
        }
        #endregion
    }
}
