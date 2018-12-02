using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using VRdkHRMsysBLL.DTOs.Post;
using VRdkHRMsysBLL.DTOs.Role;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsystem.Interfaces;

namespace VRdkHRMsystem.Services
{
    public class ViewListMapper : IViewListMapper
    {
        public SelectListItem[] CreateVacationTypesList (VacationTypeDTO[] types)
        {
            return types.Select(type => new SelectListItem() { Text = type.Name, Value = type.VacationTypeId }).ToArray();
        }

        public SelectListItem[] CreateOrganisationPostsList(PostDTO[] posts)
        {
            return posts.Select(post => new SelectListItem() { Text = post.Name, Value = post.PostId }).ToArray();
        }

        public SelectListItem[] CreateRolesList(RoleDTO[] roles)
        {
            return roles.Select(role => new SelectListItem() { Text = role.Name, Value = role.Name }).ToArray();
        }
    }
}
