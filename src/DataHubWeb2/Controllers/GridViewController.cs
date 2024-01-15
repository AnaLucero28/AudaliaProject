using Audalia.DataHUBCommon;
using Audalia.DataHUBServer;
using Audalia.DataHUBServer.Audit;
using DataHubWeb2.Model;
using System;
using System.IO;
using System.Web.Mvc;

namespace DataHubWeb2.Controllers {
    public class GridViewController : BaseController {
        // GET: GridView
        public ActionResult Index()
        {
            return View(GridViewHelper.GetIssues());
        }
        public ActionResult GridViewDetailsPage(long id) {
            ViewBag.ShowBackButton = true;
            return View(DataProvider.GetIssues().Find(i => i.DBID == id.ToString()));
        }
        public ActionResult GridViewPartial() {
            return PartialView("GridViewPartial", GridViewHelper.GetIssues());
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewCustomActionPartial(string customAction) {
            if(customAction == "delete")
                SafeExecute(() => PerformDelete());
            return GridViewPartial();
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewAddNewPartial(ReportRow issue) {
            return UpdateModelWithDataValidation(issue, GridViewHelper.AddNewRecord);
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewUpdatePartial(ReportRow issue) {
            return UpdateModelWithDataValidation(issue, GridViewHelper.UpdateRecord);
        }

        //[ValidateAntiForgeryToken]
        public ActionResult Download(string reportId) //FileResult
        {                                    
            var report = AuditData.GetReport(reportId);
            if (report.CreatorUserName == AuthHelper.Employee.WindowsUser)
            {
                MemoryStream ms = new MemoryStream();
                DataHUBReports.DownloadReport(report).CopyTo(ms);
                byte[] fileBytes = ms.ToArray();

                string fileName = report.ReportName + @".xlsx";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
                return RedirectToAction("Index", "Home");
        }


        private ActionResult UpdateModelWithDataValidation(ReportRow issue, Action<ReportRow> updateMethod) {
            if(ModelState.IsValid)
                SafeExecute(() => updateMethod(issue));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return GridViewPartial();
        }
        private void PerformDelete() {
            if(!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                GridViewHelper.DeleteRecords(Request.Params["SelectedRows"]);
        }
    }
}