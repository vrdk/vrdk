﻿namespace VRdkHRMsystem.Models.Profile
{
    public class ProfileVacationListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public ProfileVacationsViewModel[] Vacations { get; set; }
    }
}
