using Services;
using System.Web.Mvc;

namespace Inscoo.Controllers
{
    public class FileController : BaseController
    {
        private readonly IResourceService _resourceService;
        private readonly IFileService _fileService;
        public FileController(IResourceService resourceService, IFileService fileService)
        {
            _resourceService = resourceService;
            _fileService = fileService;
        }
        public void DownloadEmpInfo()
        {
            var url = _resourceService.GetEmployeeInfoTemp();
            _fileService.DownloadFile(url, "人员信息.xlsx");
        }
        public void DownloadInsurancePolicy()
        {
            var url = _resourceService.GetEmployeeInfoTemp();
            _fileService.DownloadFile(url, "投保单(盖章).xlsx");
        }
    }
}