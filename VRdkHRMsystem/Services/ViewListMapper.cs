using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using VRdkHRMsysBLL.DTOs.Post;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models.ViewListsModels;

namespace VRdkHRMsystem.Services
{
    public class ViewListMapper : IViewListMapper
    {
        public SelectListItem[] CreateVacationTypesList ()
        {
            var types = new VacationType[] {
                new VacationType
                {
                    DisplayName = "Paid vacation",
                    Name = VacationTypeEnum.Paid_vacation.ToString()
                },
                new VacationType
                {
                      DisplayName = "Unpaid vacation",
                    Name = VacationTypeEnum.Unpaid_vacation.ToString()
                }
            };
            return types.Select(type => new SelectListItem() { Text = type.DisplayName, Value = type.Name }).ToArray();
        }

        public SelectListItem[] CreateOrganisationPostsList(PostDTO[] posts)
        {
            return posts.Select(post => new SelectListItem() { Text = post.Name, Value = post.PostId }).ToArray();
        }

        public SelectListItem[] CreateRolesList()
        {
            var roles = new Role[] {
                new Role
                {
                    Name=RoleEnum.Administrator.ToString()
                },
                new Role
                {
                    Name=RoleEnum.Teamlead.ToString()
                },
                new Role
                {
                    Name=RoleEnum.Employee.ToString()
                }
            };
            return roles.Select(role => new SelectListItem() { Text = role.Name, Value = role.Name }).ToArray();
        }
    }
}
