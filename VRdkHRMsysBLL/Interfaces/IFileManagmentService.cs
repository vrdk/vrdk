using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IFileManagmentService
    {
        Task UploadSickLeaveFilesAsync(IFormFile[] files, string containerName);
        Task<string[]> GetSickLeaveFilesAsync(string id);
        Task<byte[]> DownloadFileAsync(string fileName, string containerName);
    }
}