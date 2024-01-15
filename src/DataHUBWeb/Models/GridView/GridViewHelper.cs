using DevExpress.Web;
using DevExpress.Web.Mvc;
using DataHUBWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace DataHUBWeb.Model {
    public static class GridViewHelper {
        public static List<Report> GetReports()
        {
            return DataProvider.GetReports();
        }
        
        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }
        public static void AddNewRecord(Report Report)
        {
            DataProvider.AddNewReport(Report);
        }

        public static void UpdateRecord(Report Report)
        {
            DataProvider.UpdateReport(Report);
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            List<long> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => long.Parse(id));
            DataProvider.DeleteReports(selectedIds);
        }
    }
}