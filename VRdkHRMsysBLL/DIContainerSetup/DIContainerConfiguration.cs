﻿using SimpleInjector;
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

            container.Register<IEmailSender, EmailSender>();
            container.Register<IMapHelper, MapHelper>();
            container.Register<IEmployeeService, EmployeeService>();
            container.Register<IPostService, PostService>();
            container.Register<IVacationService, VacationService>();
            container.Register<ISickLeaveService, SickLeaveService>();
            container.Register<IFileManagmentService,FileManagmentService>();
            container.Register<ITransactionService, TransactionService>();
            container.Register<IResidualsService, ResidualsService>();
            return container;
        }
    }
}
