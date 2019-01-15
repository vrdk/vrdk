using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using VRdkHRMsysBLL.DTOs.Employee;
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
                    DisplayName = "Оплачиваемый отпуск",
                    Name = VacationTypeEnum.Paid_vacation.ToString()
                },
                new VacationType
                {
                      DisplayName = "Неоплачиваемый vacation",
                    Name = VacationTypeEnum.Unpaid_vacation.ToString()
                }
            };
            return types.Select(type => new SelectListItem() { Text = type.DisplayName, Value = type.Name }).ToArray();
        }

        public SelectListItem[] CreateOrganisationPostsList(PostDTO[] posts, string editUserPost = null)
        {
            return posts.Select(post => new SelectListItem() { Text = post.Name, Value = post.PostId, Selected = post.PostId == editUserPost }).ToArray();
        }

        public SelectListItem[] CreateRolesList(string editUserRole = null)
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
            return roles.Select(role => new SelectListItem() { Text = role.Name, Value = role.Name, Selected = role.Name == editUserRole }).ToArray();
        }

        public SelectListItem[] CreateEmployeesList(EmployeeDTO[] employees)
        {
            return employees.Select(emp => new SelectListItem() { Text = $"{emp.FirstName} {emp.LastName}", Value = emp.EmployeeId }).ToArray();
        }

        public SelectListItem[] CreateSelectedEmployeesList(EmployeeDTO[] employees, string[] selectedEmployees)
        {
            return employees.Select(emp => new SelectListItem() { Text = $"{emp.FirstName} {emp.LastName}", Value = emp.EmployeeId, Selected =  selectedEmployees.Contains(emp.EmployeeId)}).ToArray();
        }

        public SelectListItem[] CreateStateList(string userState = null)
        {
            return new SelectListItem[]
            {               
                new SelectListItem
                {
                    Text = "Уволен",
                    Value = false.ToString(),
                    Selected = false.ToString() == userState?.ToString()
                },new SelectListItem
                {
                    Text = "Работает",
                    Value = true.ToString(),
                    Selected = userState == null ? true : true.ToString() == userState.ToString()
                }
            };
        }
    }
}
