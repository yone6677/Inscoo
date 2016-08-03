using System;
using System.Linq;
using Core.Data;
using Domain;
using Models;
using Core.Pager;
using System.Data.Entity;
using System.Collections.Generic;

namespace Services
{
    public class ClaimService : IClaimService
    {
        private readonly IRepository<Company> _repCompany;
        private readonly IRepository<ClaimManagementDetail> _repClaimManagementDetail;
        private readonly IAppUserService _appUserService;
        public ClaimService(IRepository<Company> repCompany, IRepository<ClaimManagementDetail> repClaimManagementDetail, IAppUserService appUserService)
        {
            _repCompany = repCompany;
            _repClaimManagementDetail = repClaimManagementDetail;
            _appUserService = appUserService;
        }
        public IPagedList<vClaimManagementDetailList> GetClaimsDetailList(int pageIndex, int pageSize, vClaimManagementDetailListSearch model)
        {
            try
            {
                var user = _appUserService.GetCurrentUser();
                var role = _appUserService.GetRoleByUserId(user.Id);
                if (role == "Admin")
                {
                    if (model.ClaimAccdtDateBegin == DateTime.MinValue)
                    {
                        model.ClaimAccdtDateBegin = DateTime.Now.AddMonths(-1);
                        model.ClaimAccdtDateEnd = DateTime.Now;
                    }

                    var list = _repClaimManagementDetail.Entities.AsNoTracking()
                        .Where(c =>
                    (string.IsNullOrEmpty(model.InsuranceGroupName) || c.InsuranceGroupName.Contains(model.InsuranceGroupName))
                    && (string.IsNullOrEmpty(model.InsuranceName) || c.InsuranceName.Contains(model.InsuranceName))
                    && (string.IsNullOrEmpty(model.InsuranceNo) || c.InsuranceNo.Contains(model.InsuranceNo))
                    && c.ClaimAccdtDate >= model.ClaimAccdtDateBegin
                     && c.ClaimAccdtDate <= model.ClaimAccdtDateEnd
                    )
                    .Select(c => new vClaimManagementDetailList
                    {
                        Id = c.Id,
                        ClaimBatch = c.ClaimBatch,
                        InsuranceGroupName = c.InsuranceGroupName,
                        InsuranceNo = c.InsuranceNo,
                        InsuranceName = c.InsuranceName,
                        ClaimAccdtDate = c.ClaimAccdtDate,
                        ClaimAmt = c.ClaimAmt,
                        ClaimPayDate = c.ClaimPayDate,

                        ExpTotal = c.ExpTotal

                    }).OrderBy(c => c.Id).ToList();
                    return new PagedList<vClaimManagementDetailList>(list, pageIndex, pageSize);
                }
                return new PagedList<vClaimManagementDetailList>(new List<vClaimManagementDetailList>(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public vCompanyEdit GetClaimsDetailById(int id)
        {
            try
            {
                return _repCompany.Table.Include(c => c.BusinessLicenses).AsNoTracking().Where(c => c.Id == id).Select(c => new vCompanyEdit
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    Phone = c.Phone,
                    Code = c.Code,
                    BusinessLicenseFilePath = c.BusinessLicenses.OrderByDescending(b => b.Id).FirstOrDefault().Url,
                    Email = c.Email,
                    LinkMan = c.LinkMan,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
