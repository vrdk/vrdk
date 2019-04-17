using System;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.Profile
{
    public class ProfileViewModel
    {
        public string EmployeeId { get; set; }
        [Display(Name ="Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Должность")]
        public string Post { get; set; }
        [Display(Name = "Статус")]
        public bool State { get; set; }
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата найма")]
        public DateTime HireDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата увольнения")]
        public DateTime? DismissalDate { get; set; }
        [Display(Name = "Персональная почта")]
        public string PersonalEmail { get; set; }
        [Display(Name = "Рабочая почта")]
        public string WorkEmail { get; set; }
        [Display(Name = "Команда")]
        public string Team { get; set; }
        [Display(Name = "Руководитель")]
        public string Teamlead { get; set; }
    }
}
