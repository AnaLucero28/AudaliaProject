using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Audalia.DataHUBCommon
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IDataHubServiceCallbackContract))]
    public interface IDataHUBServiceContract
    {

        /////////////

        [OperationContract(IsInitiating = true, IsTerminating = false, IsOneWay = true)]
        void Initiate();

        [OperationContract]
        string Register(string processId, string applicationName, string computerName, string userName);

        [OperationContract(IsInitiating = false, IsTerminating = true, IsOneWay = true)]
        void Terminate();

        [OperationContract(IsOneWay = true)]
        void Pong();

        /////////////

        [OperationContract]
        List<HolidayBase> GetHolidays(int year);

        [OperationContract]
        string GetSetting(string param);

        [OperationContract]
        string PostSetting(string param, string value);

        [OperationContract]
        string GetPermissions();

        [OperationContract]
        Dictionary<string, string> GetAppUserPermissions();

        ///////////////        

        [OperationContract]
        List<ProjectBase> GetProjects();

        [OperationContract]
        ProjectBase GetProject(string projectId);

        [OperationContract]
        List<BudgetBase> GetBudget(string projectId);

        [OperationContract]
        List<EmployeeBase> GetEmployees();

        [OperationContract]
        EmployeeBase GetEmployee(string employeeId);

        [OperationContract]
        List<ProjectEmployeeAssocBase> GetProjectEmployeeAssocs();

        [OperationContract]
        ProjectEmployeeAssocBase GetProjectEmployeeAssoc(string projectEmployeeAssocId);

        [OperationContract]
        List<ProjectEmployeeAllocBase> GetProjectEmployeeAllocs();

        [OperationContract]
        ProjectEmployeeAllocBase GetProjectEmployeeAlloc(string projectEmployeeAllocId);

        [OperationContract]
        List<ReportBase> GetReports();

        /////////////

        [OperationContract]
        ProjectBase PostProject(ProjectBase projectBase);

        [OperationContract]
        BudgetBase PostBudget(BudgetBase budget);

        [OperationContract]
        EmployeeBase PostEmployee(EmployeeBase employeeBase);

        [OperationContract]
        ProjectEmployeeAssocBase PostProjectEmployeeAssoc(ProjectEmployeeAssocBase projectEmployeeAssocBase);

        [OperationContract]
        ProjectEmployeeAllocBase PostProjectEmployeeAlloc(ProjectEmployeeAllocBase projectEmployeeAllocBase);

        [OperationContract]
        List<ProjectEmployeeAllocBase> PostProjectEmployeeAllocList(List<ProjectEmployeeAllocBase> projectEmployeeAllocBaseList);

        /////////////

        [OperationContract]
        ProjectBase PutProject(ProjectBase projectBase);

        [OperationContract]
        BudgetBase PutBudget(BudgetBase budget);

        [OperationContract]
        EmployeeBase PutEmployee(EmployeeBase employeeBase);

        [OperationContract]
        ProjectEmployeeAssocBase PutProjectEmployeeAssoc(ProjectEmployeeAssocBase projectEmployeeAssocBase);

        [OperationContract]
        ProjectEmployeeAllocBase PutProjectEmployeeAlloc(ProjectEmployeeAllocBase projectEmployeeAllocBase);

        
        /////////////

        [OperationContract]
        BudgetBase DeleteBudget(BudgetBase budget);

        [OperationContract]
        ProjectEmployeeAssocBase DeleteProjectEmployeeAssoc(ProjectEmployeeAssocBase projectEmployeeAssocBase);

        [OperationContract]
        ProjectEmployeeAllocBase DeleteProjectEmployeeAlloc(ProjectEmployeeAllocBase projectEmployeeAllocBase);

        [OperationContract]
        List<ProjectEmployeeAllocBase> DeleteProjectEmployeeAllocList(List<ProjectEmployeeAllocBase> projectEmployeeAllocBaseList);

        /////////////

        [OperationContract]        
        ReportBase CreateReport(ReportBase report);

        [OperationContract]
        Stream DownloadReport(ReportBase report);

        [OperationContract]
        void DeleteReport(ReportBase report);


        /////////////

        /*
        [OperationContract]
        List<ObjectActionMessage> Test(List<ObjectActionMessage> objectActionMessages);
        */
    }
}

