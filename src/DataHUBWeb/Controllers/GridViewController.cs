using DataHUBWeb.Model;
using System;
using System.Web.Mvc;

namespace DataHUBWeb.Controllers
{
    public class GridViewController : BaseController
    {
        // GET: GridView
        public ActionResult Index()
        {
            return View(GridViewHelper.GetReports());
        }
        public ActionResult GridViewDetailsPage(long id)
        {
            ViewBag.ShowBackButton = true;
            return View(DataProvider.GetReports().Find(i => i.Id == id));
        }
        public ActionResult GridViewPartial()
        {
            return PartialView("GridViewPartial", GridViewHelper.GetReports());
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return GridViewPartial();
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewAddNewPartial(Report Report)
        {
            return UpdateModelWithDataValidation(Report, GridViewHelper.AddNewRecord);
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewUpdatePartial(Report Report)
        {
            return UpdateModelWithDataValidation(Report, GridViewHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(Report Report, Action<Report> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(Report));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return GridViewPartial();
        }
        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                GridViewHelper.DeleteRecords(Request.Params["SelectedRows"]);
        }
    }
}