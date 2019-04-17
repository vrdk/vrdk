using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VRdkHRMsysBLL.Interfaces;

namespace VRdkHRMsystem.Controllers
{
    [Authorize]
    public class RemoteValidationController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public RemoteValidationController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<JsonResult> ValidateEmail(string WorkEmail, string EmployeeID)
        {
            var employee = await _employeeService.GetByEmailAsync(WorkEmail);
            if (EmployeeID == "undefined")
            {               
                if (employee != null)
                {
                    return Json(" уже используется");
                }

                return Json(true);
            }

            if(employee != null && employee.EmployeeId != EmployeeID)
            {
                return Json(" уже используется");
            }

            return Json(true);
        }
    }
}