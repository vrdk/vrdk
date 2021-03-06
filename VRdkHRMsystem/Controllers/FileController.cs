﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadUserPhoto(IFormFile photo, string id)
        {    
            if(photo != null && id != null)
            {
                await _fileManagmentService.UploadUserPhoto(photo, "photos", id);
                return Json(true);
            }
            else
            {
                return Json(false);
            }         
        }

        public async Task<FileResult> DownloadFile(string fileName,string containerName)
        {
                byte[] file = await _fileManagmentService.DownloadFileAsync(fileName, containerName);
                return File(file, "application/x-msdownload", fileName);
        }
    }
}