using SimpleInjector;
using SimpleInjector.Lifestyles;
using VRdkHRMsysBLL.Helpers;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysBLL.Services;
using VRdkHRMsysDAL.Contexts;
using VRdkHRMsysDAL.Interfaces;
using VRdkHRMsysDAL.Repositories;

namespace VRdkHRMsysBLL.DIContainerSetup
{
    public class DIContainerConfiguration
    {
        private Container container;
        public Container RegistrateDependencies()
        {
            container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            container.Register<HRMSystemDbContext, HRMSystemDbContext>(Lifestyle.Scoped);
            container.Register<IEmployeeRepository, EmployeeRepository>();
            container.Register<IPostRepository, PostRepository>();
            container.Register<IVacationRequestRepository, VacationRequestRepository>();
            container.Register<ISickLeaveRequestRepository, SickLeaveRequestRepository>();
            container.Register<ITransactionRepository, TransactionRepository>();
            container.Register<IEmployeeBalanceResidualsRepository, EmployeeBalanceResidualsRepository>();
            container.Register<ITeamRepository, TeamRepository>();
            container.Register<IAssignmentRepository, AssignmentRepository>();
            container.Register<INotificationRepository, NotificationRepository>();
            container.Register<IAbsenceRepository, AbsenceRepository>();
            container.Register<IDayOffRepository, DayOffRepository>();
            container.Register<IWorkDayRepository, WorkDayRepository>();

            container.Register<IEmailSender, EmailSender>();
            container.Register<IMapHelper, MapHelper>();
            container.Register<IEmployeeService, EmployeeService>();
            container.Register<IPostService, PostService>();
            container.Register<IVacationService, VacationService>();
            container.Register<ISickLeaveService, SickLeaveService>();
            container.Register<IFileManagmentService,FileManagmentService>();
            container.Register<ITransactionService, TransactionService>();
            container.Register<IResidualsService, ResidualsService>();
            container.Register<ITeamService, TeamService>();
            container.Register<IAssignmentService, AssignmentService>();
            container.Register<INotificationService, NotificationService>();
            container.Register<IAbsenceService, AbsenceService>();
            container.Register<IDayOffService, DayOffService>();
            container.Register<IWorkDayService, WorkDayService>();
            return container;
        }
    }
}
