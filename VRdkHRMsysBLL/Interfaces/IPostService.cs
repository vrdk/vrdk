using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Post;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IPostService
    {
        Task<PostDTO[]> GetPostsByOrganisationIdAsync(string id);
    }
}