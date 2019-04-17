using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IPostService
    {
        Task<PostDTO[]> GetPostsByOrganisationIdAsync(string id);
    }
}