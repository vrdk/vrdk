using Microsoft.EntityFrameworkCore;
using System.Configuration;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Contexts
{
    public partial class HRMSystemDbContext : DbContext
    {
        public virtual DbSet<Absence> Absence { get; set; }
        public virtual DbSet<Assignment> Assignment { get; set; }
        public virtual DbSet<AssignmentEmployee> AssignmentEmployee { get; set; }
        public virtual DbSet<DayOff> DayOff { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeBalanceResiduals> EmployeeBalanceResiduals { get; set; }
        public virtual DbSet<Organisation> Organisation { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<SickLeaveRequest> SickLeaveRequest { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<TimeManagementRecord> TimeManagementRecord { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }      
        public virtual DbSet<VacationRequest> VacationRequest { get; set; }
        public virtual DbSet<WorkDay> WorkDay { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=LAPTOP-6R7SBS7T;Database=VRdkHRMsystemDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Absence>(entity =>
            {
                entity.ToTable("absence");

                entity.Property(e => e.AbsenceId)
                    .HasColumnName("absence_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AbsenceDate)
                    .HasColumnName("absence_date")
                    .HasColumnType("date");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Absences)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__absence__employe__5812160E");
            });

            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.ToTable("assignment");

                entity.Property(e => e.AssignmentId)
                    .HasColumnName("assignment_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BeginDate)
                    .HasColumnName("begin_date")
                    .HasColumnType("date");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(512);

                entity.Property(e => e.OrganisationId)
                    .HasColumnName("organisation_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Organisation)
                    .WithMany(p => p.Assignment)
                    .HasForeignKey(d => d.OrganisationId)
                    .HasConstraintName("FK__assignmen__organ__607251E5");
            });

            modelBuilder.Entity<AssignmentEmployee>(entity =>
            {
                entity.HasKey(e => e.RowId)
                    .HasName("PK__assignme__6965AB577132819A");

                entity.ToTable("assignment_employee");

                entity.Property(e => e.RowId)
                    .HasColumnName("row_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssignmentId)
                    .HasColumnName("assignment_id")
                    .HasMaxLength(450);

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentEmployee)
                    .HasForeignKey(d => d.AssignmentId)
                    .HasConstraintName("FK__assignmen__assig__5BE2A6F2");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__assignmen__emplo__5AEE82B9");
            });

            modelBuilder.Entity<DayOff>(entity =>
            {
                entity.ToTable("day_off");

                entity.Property(e => e.DayOffId)
                    .HasColumnName("day_off_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DayOffDate)
                    .HasColumnName("day_off_date")
                    .HasColumnType("date");

                entity.Property(e => e.DayOffImportance)
                    .HasColumnName("day_off_importance")
                    .HasMaxLength(512);

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasMaxLength(100);

                entity.Property(e => e.DayOffState)
                    .IsRequired()
                    .HasColumnName("day_off_state")
                    .HasMaxLength(512);

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasMaxLength(450);

                entity.Property(e => e.ProcessDate)
                    .HasColumnName("process_date")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.DayOffs)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__day_off__employe__68487DD7");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee");

                entity.HasIndex(e => e.WorkEmail)
                    .HasName("UQ__employee__0DD4ED7945AA3853")
                    .IsUnique();

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BirthDate)
                    .HasColumnName("birth_date")
                    .HasColumnType("date");

                entity.Property(e => e.DismissalDate)
                    .HasColumnName("dismissal_date")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(512);

                entity.Property(e => e.HireDate)
                    .HasColumnName("hire_date")
                    .HasColumnType("date");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(512);

                entity.Property(e => e.OrganisationId)
                    .IsRequired()
                    .HasColumnName("organisation_id")
                    .HasMaxLength(450);

                entity.Property(e => e.PersonalEmail)
                    .IsRequired()
                    .HasColumnName("personal_email")
                    .HasMaxLength(512);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasColumnName("phone_number")
                    .HasMaxLength(128)
                    .HasDefaultValueSql("('0970301467')");

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasColumnName("post_id")
                    .HasMaxLength(450);

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id")
                    .HasMaxLength(450);

                entity.Property(e => e.WorkEmail)
                    .IsRequired()
                    .HasColumnName("work_email")
                    .HasMaxLength(512);

                entity.HasOne(d => d.Organisation)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.OrganisationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__employee__organi__403A8C7D");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__employee__post_i__412EB0B6");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK__employee__team_i__151B244E");
            });

            modelBuilder.Entity<EmployeeBalanceResiduals>(entity =>
            {
                entity.HasKey(e => e.ResidualId)
                    .HasName("PK__employee__A916765B3A700CC8");

                entity.ToTable("employee_balance_residuals");

                entity.Property(e => e.ResidualId)
                    .HasColumnName("residual_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasMaxLength(450);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(512);

                entity.Property(e => e.ResidualBalance).HasColumnName("residual_balance");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeBalanceResiduals)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__employee___emplo__60A75C0F");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notification");

                entity.Property(e => e.NotificationId)
                    .HasColumnName("notification_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(512);

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasMaxLength(450);

                entity.Property(e => e.IsChecked).HasColumnName("isChecked");

                entity.Property(e => e.NotificationDate)
                    .HasColumnName("notification_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NotificationRange)
                    .IsRequired()
                    .HasColumnName("notification_range")
                    .HasMaxLength(512)
                    .HasDefaultValueSql("('user')");

                entity.Property(e => e.NotificationType)
                    .IsRequired()
                    .HasColumnName("notification_type")
                    .HasMaxLength(512);

                entity.Property(e => e.OrganisationId)
                    .IsRequired()
                    .HasColumnName("organisation_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__notificat__emplo__681373AD");

                entity.HasOne(d => d.Organisation)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.OrganisationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__notificat__organ__690797E6");
            });

            modelBuilder.Entity<Organisation>(entity =>
            {
                entity.ToTable("organisation");

                entity.Property(e => e.OrganisationId)
                    .HasColumnName("organisation_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(512);

                entity.Property(e => e.RegistrationDate)
                    .HasColumnName("registration_date")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.Property(e => e.PostId)
                    .HasColumnName("post_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(512);

                entity.Property(e => e.OrganisationId)
                    .IsRequired()
                    .HasColumnName("organisation_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Organisation)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.OrganisationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__post__organisati__398D8EEE");
            });

            modelBuilder.Entity<SickLeaveRequest>(entity =>
            {
                entity.HasKey(e => e.SickLeaveId)
                    .HasName("PK__sick_lea__BC1B4FECC4FCD277");

                entity.ToTable("sick_leave_request");

                entity.Property(e => e.SickLeaveId)
                    .HasColumnName("sick_leave_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CloseDate)
                    .HasColumnName("close_date")
                    .HasColumnType("date");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasMaxLength(512);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("date");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasColumnName("employee_id")
                    .HasMaxLength(450);

                entity.Property(e => e.ProccessedbyId)
                    .HasColumnName("proccessedby_id")
                    .HasMaxLength(450);

                entity.Property(e => e.RequestStatus)
                    .IsRequired()
                    .HasColumnName("request_status")
                    .HasMaxLength(512);

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.SickLeaves)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__sick_leav__emplo__52593CB8");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("team");

                entity.Property(e => e.TeamId)
                    .HasColumnName("team_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(512);

                entity.Property(e => e.OrganisationId)
                    .IsRequired()
                    .HasColumnName("organisation_id")
                    .HasMaxLength(450);

                entity.Property(e => e.TeamleadId)
                    .IsRequired()
                    .HasColumnName("teamlead_id")
                    .HasMaxLength(450)
                    .HasDefaultValueSql("('1e60d24b-57f1-4beb-b5d9-4888d154ad50')");

                entity.HasOne(d => d.Organisation)
                    .WithMany(p => p.Team)
                    .HasForeignKey(d => d.OrganisationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__team__organisati__3C69FB99");

                entity.HasOne(d => d.Teamlead)
                    .WithMany(p => p.TeamNavigation)
                    .HasForeignKey(d => d.TeamleadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__team__teamlead_i__214BF109");
            });

            modelBuilder.Entity<TimeManagementRecord>(entity =>
            {
                entity.HasKey(e => e.TimeManagementRecordId)
                    .HasName("PK__time_man__F5CD42EA8952EF95");

                entity.ToTable("time_managment_record");

                entity.Property(e => e.TimeManagementRecordId)
                    .HasColumnName("time_management_record_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(100);

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasMaxLength(450);

                entity.Property(e => e.ProccessDate)
                    .HasColumnName("proccess_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.RecordDate)
                    .HasColumnName("record_date")
                    .HasColumnType("date");

                entity.Property(e => e.TimeFrom).HasColumnName("time_from");

                entity.Property(e => e.TimeTo).HasColumnName("time_to");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TimeManagmentRecords)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__time_mana__emplo__160F4887");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Change).HasColumnName("change");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(100);

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasColumnName("employee_id")
                    .HasMaxLength(450);

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transaction_date")
                    .HasColumnType("date");

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasColumnName("transaction_type")
                    .HasMaxLength(512);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__transacti__emplo__47DBAE45");
            });

            modelBuilder.Entity<VacationRequest>(entity =>
            {
                entity.HasKey(e => e.VacationId)
                    .HasName("PK__vacation__F558A74CDAEC33E6");

                entity.ToTable("vacation_request");

                entity.Property(e => e.VacationId)
                    .HasColumnName("vacation_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BeginDate)
                    .HasColumnName("begin_date")
                    .HasColumnType("date");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasMaxLength(512);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("date");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasColumnName("employee_id")
                    .HasMaxLength(450);

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.ProccessDate)
                    .HasColumnName("proccess_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProccessedbyId)
                    .HasColumnName("proccessedby_id")
                    .HasMaxLength(450);

                entity.Property(e => e.RequestStatus)
                    .IsRequired()
                    .HasColumnName("request_status")
                    .HasMaxLength(512);

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(450);

                entity.Property(e => e.VacationType)
                    .IsRequired()
                    .HasColumnName("vacation_type")
                    .HasMaxLength(512);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Vacations)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__vacation___emplo__4D94879B");
            });

            modelBuilder.Entity<WorkDay>(entity =>
            {
                entity.ToTable("work_day");

                entity.Property(e => e.WorkDayId)
                    .HasColumnName("work_day_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasMaxLength(450);

                entity.Property(e => e.ProcessDate)
                    .HasColumnName("process_date")
                    .HasColumnType("date");

                entity.Property(e => e.TimeFrom).HasColumnName("time_from");

                entity.Property(e => e.TimeTo).HasColumnName("time_to");

                entity.Property(e => e.WorkDayDate)
                    .HasColumnName("work_day_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.WorkDays)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__work_day__employ__6D0D32F4");
            });
        }
    }
}
