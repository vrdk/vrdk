using Microsoft.AspNetCore.Mvc.Rendering;
using VRdkHRMsysBLL.DTOs.Post;
using VRdkHRMsysBLL.DTOs.Role;
using VRdkHRMsysBLL.DTOs.Vacation;

namespace VRdkHRMsystem.Interfaces
{
    public interface IViewListMapper
    {
        SelectListItem[] CreateVacationTypesList(VacationTypeDTO[] types);
        SelectListItem[] CreateOrganisationPostsList(PostDTO[] posts);
        SelectListItem[] CreateRolesList(RoleDTO[] roles);
    }
}