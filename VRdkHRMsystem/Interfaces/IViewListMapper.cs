using Microsoft.AspNetCore.Mvc.Rendering;
using VRdkHRMsysBLL.DTOs.Post;

namespace VRdkHRMsystem.Interfaces
{
    public interface IViewListMapper
    {
        SelectListItem[] CreateVacationTypesList();
        SelectListItem[] CreateOrganisationPostsList(PostDTO[] posts);
        SelectListItem[] CreateRolesList();
    }
}