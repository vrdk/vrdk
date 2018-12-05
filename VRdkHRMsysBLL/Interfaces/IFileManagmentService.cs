using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IFileManagmentService
    {
        Task UploadSickLeaveFiles(IFormFile[] files, string id, string containerName);
    }
}