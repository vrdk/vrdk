﻿namespace VRdkHRMsystem.Models.Profile.Absences
{
    public class ProfileAbsencesListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public ProfileAbsencesViewModel[] Absences { get; set; }
    }
}
