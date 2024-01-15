using Audalia.DataHUBClient;
using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Audalia.DataHUBCommon;

namespace WindowsApplication
{
    public partial class SchedulerUserControl : UserControl
    {
        public SchedulerUserControl()
        {
            InitializeComponent();

            //


            //AuditData.ReadDB();
            Init();
        }


        //string filtroAuditor = "Araceli Catalán";

        private void Init()
        {
            ISchedulerStorage storage = schedulerDataStorage;

            storage.BeginUpdate();
            try
            {
                storage.Resources.Mappings.Id = "DBID";
                storage.Resources.Mappings.Caption = "Name";
                storage.Resources.DataSource = Data.Audit.Employees.Values.Where(p => p.Category == "Socio" /*&& p.Name == filtroAuditor*/);

                storage.Appointments.Mappings.AllDay = "AllDay";
                storage.Appointments.Mappings.Description = "Description";
                storage.Appointments.Mappings.End = "EndTime";
                storage.Appointments.Mappings.Label = "Label";
                storage.Appointments.Mappings.Location = "Location";
                storage.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo";
                storage.Appointments.Mappings.ReminderInfo = "ReminderInfo";
                storage.Appointments.Mappings.ResourceId = "ResourceId";
                storage.Appointments.Mappings.Start = "StartTime";
                storage.Appointments.Mappings.Status = "Status";
                storage.Appointments.Mappings.Subject = "Subject";
                storage.Appointments.Mappings.Type = "EventType";
                storage.Appointments.DataSource = CreateAppointments();
            }
            finally
            {
                storage.EndUpdate();
            }


            schedulerControl.Start = DateTime.Today;
        }

        public BindingList<SchedulerAppointment> CreateAppointments()
        {

            

            var appointments = new BindingList<SchedulerAppointment>();

            string reccurenceFormat = @"<RecurrenceInfo Start=""{0}"" End=""{1}"" WeekOfMonth=""{2}"" WeekDays=""{3}"" Month=""{4}"" OccurrenceCount=""{5}"" Range=""{6}"" Type=""{7}"" Id=""{8}""/>";
            string changedOccurrenceFormat = @"<RecurrenceInfo Id=""{0}"" Index=""{1}""/>";

            /*
            foreach (var task in AuditData.Tasks.Values)
            {
                foreach (var employee in task.Employees)
                {
                    var startDate = task.StartDate;
                    var endDate = task.FinishDate;
                    //if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        appointments.Add(new SchedulerAppointment()
                        {
                            Id = task.DBID + employee.Employee.DBID,
                            ResourceId = employee.Employee.DBID,
                            //AllDay = true,
                            Modified = false,
                            EventType = (int)AppointmentType.Normal,
                            StartTime = startDate,
                            EndTime = endDate,
                            Description = $"{task.Project.Name} - {task.Name} ({employee.Role})",
                            Label = 1,
                            Location = "Oficina",
                            Status = 1,
                            Subject = $"{task.Project.Name} - {task.Name} ({employee.Role})",
                            Price = 0
                        });
                    }
                }            
            }
            */
            /*
            Random random = new Random();
            foreach (var task in AuditData.Tasks.Values)
            {
                var startDate = task.StartDate;
                var endDate = task.FinishDate;

                string resources = "";
                foreach (var employee in task.Employees)
                {
                    if (employee.Role == "SOCIO")
                    {
                        resources = employee.Employee.DBID;
                    }
                    //resources += (resources == "" ? "" : ",") + employee.Employee.DBID;
                }

                int label = 0;                
                label = random.Next(1, 11);

                appointments.Add(new SchedulerAppointment()
                {
                    Id = task.DBID,
                    ResourceId = resources,
                    //AllDay = true,
                    Modified = false,
                    EventType = (int)AppointmentType.Normal,
                    StartTime = startDate,
                    EndTime = endDate,
                    Description = task.Project.Name,                    
                    Location = "Oficina",
                    Status = 1,
                    Subject = $"{task.Project.Name} - {task.Name}",
                    Label = label,
                    Price = 0
                });
            }
            */
            //Por dias o semanas -->


            Random random = new Random();

            foreach (var employee in Data.Audit.Employees.Values.Where(p => p.DBID == "12"))
                foreach (var task in Data.Audit.ProjectEmployeeAssocs.Values.Where(p=> p.EmployeeId == employee.DBID))
                //foreach (var task in Data.Audit.Projects.Values)
                {
                /*
                if (!task.Employees.Exists(p => p.Employee.Name == filtroAuditor))
                { continue; }
                */

                
                string resources = "";
                    /*
                    foreach (var resource in task.Employees)
                    {
                        if (resource.Role == "SOCIO")
                        {
                            resources = resource.Employee.DBID;
                        }
                        //resources += (resources == "" ? "" : ",") + employee.Employee.DBID;
                    }
                    */
                    resources = employee.DBID;

                    int label = random.Next(1, 11);
                var taskStartDate = task.Project.StartDate.TruncStartDate();
                var taskFinishDate = task.Project.FinishDate.TruncFinishDate().AddDays(-0.5);

                int weeks = 1 + taskFinishDate.GetIso8601WeekOfYear() - taskStartDate.GetIso8601WeekOfYear();
                var firstMonday = taskStartDate.AddDays(-(int)taskStartDate.DayOfWeek + (int)DayOfWeek.Monday);

                for (var weekIx = 0; weekIx < weeks; weekIx++)
                {
                    var description = task.Project.Name;
                    if (weeks > 1)
                        description += $" - Semana {weekIx + 1}";

                    var startDate = firstMonday.AddDays(7 * weekIx);
                    var finishDate = startDate.AddDays(5);

                    startDate = new DateTime(Math.Max(taskStartDate.Ticks, startDate.Ticks));
                    finishDate = new DateTime(Math.Min(taskFinishDate.Ticks, finishDate.Ticks));

                    appointments.Add(new SchedulerAppointment()
                    {
                        Id = task.DBID,
                        ResourceId = resources,
                        AllDay = true,
                        Modified = false,
                        EventType = (int)AppointmentType.Normal,
                        StartTime = startDate,
                        EndTime = finishDate,
                        Description = $"{description} - {task.Project.Name}",
                        Location = "Oficina",
                        Status = 2,
                        Subject = description,
                        Label = label,
                        Price = 0
                    });
                }
            }
           
            //<--

            return appointments;
        }

        public BindingList<SchedulerAppointment> CreateAppointments1()
        {
            var appointments = new BindingList<SchedulerAppointment>();

            string reccurenceFormat = @"<RecurrenceInfo Start=""{0}"" End=""{1}"" WeekOfMonth=""{2}"" WeekDays=""{3}"" Month=""{4}"" OccurrenceCount=""{5}"" Range=""{6}"" Type=""{7}"" Id=""{8}""/>";
            string changedOccurrenceFormat = @"<RecurrenceInfo Id=""{0}"" Index=""{1}""/>";

            foreach (var task in Data.Audit.Projects.Values)
            {
                foreach (var employee in task.Employees)
                {
                    var days = (task.FinishDate - task.StartDate).Days;
                    for (int day = 0; day < days; day++)
                    {
                        var startDate = task.StartDate.Date.AddDays(day).AddHours(8);
                        var endDate = startDate.AddHours(10);
                        if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday)
                        {
                            appointments.Add(new SchedulerAppointment()
                            {
                                Id = task.DBID + employee.Employee.DBID,
                                ResourceId = employee.Employee.DBID,
                                //AllDay = true,
                                Modified = false,
                                EventType = (int)AppointmentType.Normal,
                                StartTime = startDate,
                                EndTime = endDate,
                                Description = $"{task.Name} - {task.Name} ({employee.Role})",
                                Label = 1,
                                Location = "Oficina",
                                Status = 1,
                                Subject = $"{task.Name} - {task.Name} ({employee.Role})",
                                Price = 0
                            });
                        }
                    }                    
                }
            }

            return appointments;
        }
    }

    public class SchedulerAppointment
    {
        [Key]
        public string Id { get; set; }
        public string ResourceId { get; set; }
        public bool Modified { get; set; }
        public bool AllDay { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int Label { get; set; }
        public int EventType { get; set; }
        public string Location { get; set; }
        public string Subject { get; set; }
        public string RecurrenceInfo { get; set; }
        public string ReminderInfo { get; set; }        
        public double Price { get; set; }
    }

}
