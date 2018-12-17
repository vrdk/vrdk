using Microsoft.AspNetCore.Mvc.Rendering;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Post;

namespace VRdkHRMsystem.Interfaces
{
    public interface IViewListMapper
    {
        SelectListItem[] CreateVacationTypesList();
        SelectListItem[] CreateOrganisationPostsList(PostDTO[] posts);
        SelectListItem[] CreateRolesList();
        SelectListItem[] CreateEmployeesList(EmployeeDTO[] employees);
        MultiSelectList CreateMultiEmployeesList(EmployeeDTO[] employees);
    }
}