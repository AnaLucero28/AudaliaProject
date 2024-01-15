using DevExpress.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.Web.Internal;
using DevExpress.Web.Mvc;
using DevExpress.XtraScheduler;
using DataHUBWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace DataHUBWeb.Model {
    public static class SchedulerHelper {
        public static SchedulerSettings Settings {
            get {
                SchedulerSettings settings = new SchedulerSettings();
                settings.Name = "scheduler";
                
                settings.CallbackRouteValues = new { Controller = "Scheduler", Action = "SchedulerPartial" };
                settings.EditAppointmentRouteValues = new { Controller = "Scheduler", Action = "SchedulerPartialEditAppointment" };

                settings.ClientSideEvents.Init = "onSchedulerInit";
                settings.ClientSideEvents.EndCallback = "onSchedulerEndCallback";
                settings.ClientSideEvents.BeginCallback = "onSchedulerBeginCallback";
                
                settings.Start = DateTime.Now;
                settings.EnablePagingGestures = AutoBoolean.False;
                settings.Width = Unit.Percentage(100);
                settings.ControlStyle.CssClass = "scheduler";

                // DXCOMMENT: Setting ViewType: a compact view (Day) for mobile devices, a large view (WorkWeek) for desktops
                //settings.ActiveViewType = RenderUtils.Browser.Platform.IsMobileUI ? SchedulerViewType.Day : SchedulerViewType.WorkWeek;
                settings.ActiveViewType = SchedulerViewType.Month;
                settings.Views.MonthView.CellAutoHeightOptions.MinHeight = 64;
                settings.Views.MonthView.CellAutoHeightOptions.Mode = DevExpress.Web.ASPxScheduler.AutoHeightMode.FitToContent; //None; //LimitHeight; //DevExpress.Web.ASPxScheduler.AutoHeightMode.FitToContent;
                settings.Views.MonthView.WeekCount = 5;
                settings.Views.MonthView.ResourcesPerPage = 1;

                settings.OptionsView.VerticalScrollBarMode = ScrollBarMode.Auto;
                settings.OptionsAdaptivity.Enabled = true;

                settings.OptionsBehavior.RecurrentAppointmentEditAction = RecurrentAppointmentAction.Ask;
                settings.OptionsBehavior.ShowViewVisibleInterval = true;
                settings.OptionsBehavior.ShowViewNavigator = true;

                settings.OptionsCustomization.AllowAppointmentResize = UsedAppointmentType.None;
                settings.OptionsCustomization.AllowAppointmentEdit = UsedAppointmentType.None;
                settings.OptionsCustomization.AllowAppointmentDelete = UsedAppointmentType.None;
                settings.OptionsCustomization.AllowAppointmentCreate = UsedAppointmentType.None;
                settings.OptionsCustomization.AllowAppointmentCopy = UsedAppointmentType.None;

                settings.Storage.EnableReminders = true;
                settings.Storage.Appointments.AutoRetrieveId = true;

                // DXCOMMENT: Configure appointment mappings
                settings.Storage.Appointments.Mappings.AppointmentId = "Id";
                settings.Storage.Appointments.Mappings.Type = "EventType";
                settings.Storage.Appointments.Mappings.Start = "StartDate";
                settings.Storage.Appointments.Mappings.End = "EndDate";
                settings.Storage.Appointments.Mappings.AllDay = "AllDay";
                settings.Storage.Appointments.Mappings.Subject = "Subject";
                settings.Storage.Appointments.Mappings.Location = "Location";
                settings.Storage.Appointments.Mappings.Description = "Description";
                settings.Storage.Appointments.Mappings.Status = "Status";
                settings.Storage.Appointments.Mappings.Label = "LabelId";
                settings.Storage.Appointments.Mappings.ResourceId = "ResourceId";
                settings.Storage.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo";

                // DXCOMMENT: Map labels by their ids
                settings.Storage.Appointments.Labels.Clear();
                foreach(SchedulerLabel label in SchedulerLabelsHelper.GetItems())
                    settings.Storage.Appointments.Labels.Add(label.Id, label.Name, label.Name, label.Color);

                // DXCOMMENT: Configure resource mappings
                settings.Storage.Resources.Mappings.ResourceId = "Id";
                settings.Storage.Resources.Mappings.Caption = "Name";

                settings.Views.AgendaView.DayCount = 30;
                settings.Views.DayView.VisibleTime.Start = new TimeSpan(7, 0, 0);
                settings.Views.DayView.VisibleTime.End = new TimeSpan(22, 0, 0);
                settings.Views.WeekView.Enabled = false;
                settings.Views.FullWeekView.Enabled = true;
                settings.Views.MonthView.CompressWeekend = false;
                settings.Views.TimelineView.Enabled = false;                

                // DXCOMMENT: Scroll to actual time
                var currentTime = new TimeSpan(DateTime.Now.Hour - 1, 0, 0);

                settings.Views.DayView.TopRowTime = currentTime;
                settings.Views.WorkWeekView.TopRowTime = currentTime;
                settings.Views.FullWeekView.TopRowTime = currentTime;

                // DXCOMMENT: Configure DateNavigator for Scheduler
                settings.DateNavigatorExtensionSettings.Name = "dateNavigator";
                settings.DateNavigatorExtensionSettings.SchedulerName = "Scheduler";
                settings.DateNavigatorExtensionSettings.Width = Unit.Percentage(100);
                settings.DateNavigatorExtensionSettings.Properties.ShowTodayButton = true;
                settings.DateNavigatorExtensionSettings.Properties.EnableChangeVisibleDateGestures = DevExpress.Utils.DefaultBoolean.True;
                settings.DateNavigatorExtensionSettings.Properties.EnableLargePeriodNavigation = false;
                settings.DateNavigatorExtensionSettings.Properties.AppointmentDatesHighlightMode = AppointmentDatesHighlightMode.Labels;
                settings.DateNavigatorExtensionSettings.Properties.Style.CssClass = "date-navigator";
                settings.DateNavigatorExtensionSettings.Properties.HeaderStyle.CssClass = "date-navigator-header";
                settings.DateNavigatorExtensionSettings.ClientSideEvents.SelectionChanged = "onDateNavigatorSelectionChanged";

                return settings;
            }
        }

        public static SchedulerDataSource GetSchedulerDataSource(List<long> selectedResourceIds) {
            return new SchedulerDataSource(selectedResourceIds);
        }

        public static void UpdateSchedulerDataSource() {
            SchedulerDataSource schedulerDataSource = new SchedulerDataSource(null);
            InsertAppointments(schedulerDataSource);
            UpdateAppointments(schedulerDataSource);
            RemoveAppointments(schedulerDataSource);
        }
        static void InsertAppointments(SchedulerDataSource schedulerDataSource) {
            SchedulerAppointment[] appointmentsToInsert = SchedulerExtension.GetAppointmentsToInsert<SchedulerAppointment>("scheduler",
                schedulerDataSource.Appointments, schedulerDataSource.Resources,
                Settings.Storage.Appointments, Settings.Storage.Resources);
            schedulerDataSource.InsertSchedulerAppointments(appointmentsToInsert.ToList());
        }
        static void UpdateAppointments(SchedulerDataSource schedulerDataSource) {
            SchedulerAppointment[] appointmentsToUpdate = SchedulerExtension.GetAppointmentsToUpdate<SchedulerAppointment>("scheduler",
                schedulerDataSource.Appointments, schedulerDataSource.Resources,
                Settings.Storage.Appointments, Settings.Storage.Resources);
            schedulerDataSource.UpdateSchedulerAppointments(appointmentsToUpdate.ToList());
        }
        static void RemoveAppointments(SchedulerDataSource schedulerDataSource) {
            SchedulerAppointment[] appointmentsToRemove = SchedulerExtension.GetAppointmentsToRemove<SchedulerAppointment>("scheduler",
                schedulerDataSource.Appointments, schedulerDataSource.Resources,
                Settings.Storage.Appointments, Settings.Storage.Resources);
            schedulerDataSource.RemoveSchedulerAppointments(appointmentsToRemove.ToList());
        }
    }
}