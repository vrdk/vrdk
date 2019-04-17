using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapHelper _mapHelper;

        public PostService(IPostRepository postRepository,
                           IMapHelper mapHelper)
        {
            _postRepository = postRepository;
            _mapHelper = mapHelper;
        }
        public async Task<PostDTO[]> GetPostsByOrganisationIdAsync(string id)
        {
            var posts = await _postRepository.GetAsync(post => post.OrganisationId.Equals(id));
            return _mapHelper.MapCollection<Post, PostDTO>(posts);
        }
    }
}
