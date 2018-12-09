using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.Interfaces;

namespace VRdkHRMsystem.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly IFileManagmentService _fileManagmentService;
        private readonly IEmployeeService _employeeService;

        public FileController(IFileManagmentService fileManagmentService,
                              IEmployeeService employeeService)
        {
            _fileManagmentService = fileManagmentService;
            _employeeService = employeeService;
        }

        public async Task<FileResult> DownloadFile(string fileName,string containerName,string code)
        {
            var caller = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if(caller != null && caller.OrganisationId.Equals(code))
            {
                byte[] file = await _fileManagmentService.DownloadFileAsync(fileName, containerName);
                return File(file, "application/x-msdownload", fileName);
            }

            return File(new byte[] { }, "", fileName);
        }
    }
}