using System;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.TeamleadViewModels.Calendar
{
    public class EditCalendarDayViewModel
    {
        public string CalendarDayId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamId { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = " заполните")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public TimeSpan TimeFrom { get; set; }
        [Required(ErrorMessage = " заполните")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public TimeSpan TimeTo { get; set; }
        public string Result { get; set; }
        public DateTime ProcessDate { get; set; }
    }
}
