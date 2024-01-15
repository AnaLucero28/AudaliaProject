using Audalia.DataHUBCommon;
using Audalia.DataHUBServer.Audit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Audalia.DataHUBServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = true)]
    //[GlobalErrorBehavior(typeof(GlobalErrorHandler))]
    public partial class DataHUBServiceContract : IDataHUBServiceContract
    {

        #region System

        private static System.Timers.Timer _pingTimer = null;
        private static readonly Dictionary<string, DataHubSession> _sessionList = new Dictionary<string, DataHubSession>();
        private static ReaderWriterLock _sessionListLock = new ReaderWriterLock();
        private static List<string> _faultedSessions = new List<string>();
        private static SynchronizationContext _syncContext;

        public DataHUBServiceContract()
        {
            _syncContext = SynchronizationContext.Current;

            /*
            //Deshabilitado por el momento

            if (_pingTimer == null)
            {
                _pingTimer = new System.Timers.Timer();
                _pingTimer.Interval = 10000;
                _pingTimer.Elapsed += _pingTimer_Elapsed;
                _pingTimer.Start();
            }
            */
        }

        void IDataHUBServiceContract.Initiate()
        {

        }

        string IDataHUBServiceContract.Register(string processId, string applicationName, string computerName,
            string userName)
        {
            /*
            lock (_sessionList)
            {
                foreach (var session in _sessionList.Values)
                    if (session.ComputerName == computerName && session.ProcessId == processId)
                        return OperationContext.Current.SessionId;
            }
            */
            _sessionListLock.AcquireReaderLock(30);
            try
            {
                foreach (var session in _sessionList.Values)
                    if (session.ComputerName == computerName && session.ProcessId == processId)
                        return OperationContext.Current.SessionId;
            }
            finally
            {
                _sessionListLock.ReleaseReaderLock();
            }

            /*
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint =
                prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            */

            var newSesion = new DataHubSession()
            {
                ProcessId = processId,
                SessionId = OperationContext.Current.SessionId,
                ApplicationName = applicationName,
                ComputerName = computerName,
                UserName = userName,
                ConnectionDateTime = DateTime.Now.ToUniversalTime(),
                LastPongDateTime = DateTime.Now.ToUniversalTime(),
                Faulted = false,
                //
                OperationContext = OperationContext.Current,
                RemoteEndpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty
        };

            AddSessionToList(newSesion);
            CallbackActivityMessage($"Registered {OperationContext.Current.SessionId}");
            DataHubLog.WriteLine($"DataHUBServiceContract.Register {OperationContext.Current.SessionId}");

            return OperationContext.Current.SessionId;
        }

        void IDataHUBServiceContract.Terminate()
        {
            //throw new NotImplementedException();
        }
                               
        void AddSessionToList(DataHubSession session)
        {            
            lock (_sessionList)
            {
                _sessionList.Add(session.SessionId, session);
            }
            

            /*
            _sessionListLock.AcquireWriterLock(30);
            try
            {
                _sessionList.Add(session.SessionId, session);
            }
            finally
            {
                _sessionListLock.ReleaseWriterLock();
            }
            */
        }

        public static DataHubSession GetSession(string sessionId)
        {
            return _sessionList[sessionId];
        }

        private static void _pingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {            
            _pingTimer.Stop();

            try
            {
                CallbackAction(x => x.Ping());

                foreach (var sessionId in _faultedSessions)
                {
                    var session = _sessionList[sessionId];
                    session.Faulted = true;
                    CallbackActivityMessage("DataHUB-Session Faulted: " + sessionId);

                    TimeSpan ts = DateTime.UtcNow - session.LastPongDateTime;
                    if (ts.TotalMinutes > 1)
                    {
                        _sessionList.Remove(sessionId);
                        CallbackActivityMessage("DataHUB-Session Removed: " + sessionId);
                    }
                }
            }
            finally
            {
                _pingTimer.Start();
            }
        }

        void IDataHUBServiceContract.Pong()
        {
            lock (_sessionList)
            {
                if (_sessionList.ContainsKey(OperationContext.Current.SessionId))
                {
                    var session = _sessionList[OperationContext.Current.SessionId];
                    session.LastPongDateTime = DateTime.Now.ToUniversalTime();
                    session.Faulted = false;
                    //CallbackActivityMessage(DataHUBCallbackMessageType.System, "Pong - " + OperationContext.Current.SessionId);
                }
            }

            /*
            _sessionListLock.AcquireWriterLock(30);
            try
            {
                if (_sessionList.ContainsKey(OperationContext.Current.SessionId))
                {
                    var session = _sessionList[OperationContext.Current.SessionId];
                    session.LastPongDateTime = DateTime.Now.ToUniversalTime();
                    session.Faulted = false;                    
                }
            }
            finally
            {
                _sessionListLock.ReleaseWriterLock();
            }
            */

            //DataHubLog.WriteLine("DataHUBServiceContract.Pong");            
        }

        public static void CallbackActivityMessage(string message)
        {
            //Deshabilitado por el momento
            return;

            DataHubLog.WriteLine("DataHUBServiceContract.CallbackActivityMessage - " + message);
            Task.Factory.StartNew(() => CallbackAction(x => x.ActivityMessage(message)));
        }
       
        public static void CallbackObjectMessage(ObjectMessageActionType objectMessageActionType, ObjectMessageObjectType objectMessageObjectType, string objectId)
        {
            //Deshabilitado temporalmente
            //Task.Factory.StartNew(() => CallbackAction(x => x.ObjectMessage(objectMessageActionType, objectMessageObjectType, objectId)));
        }

        public static void CallbackObjectActionMessageList(List<ObjectActionMessage> objectActionMessageList)
        {
            //Deshabilitado temporalmente
            //Task.Factory.StartNew(() => CallbackAction(x => x.ObjectActionMessageList(objectActionMessageList)));
        }

        private static void CallbackAction(Action<IDataHubServiceCallbackContract> action)
        {

            _sessionListLock.AcquireReaderLock(30);
            try
            {
                Parallel.ForEach(_sessionList.Values, (session) =>
                {
                    try
                    {
                        IDataHubServiceCallbackContract callback =
                            session.OperationContext.GetCallbackChannel<IDataHubServiceCallbackContract>();
                        /*
                        if (log)
                            DataHubLog.WriteLine("DataHUBServiceContract.CallbackAction - SessionId: " + session.SessionId, " - " + action.Method.Name);
                        */
                        //System.Threading.ThreadPool.QueueUserWorkItem(delegate { action(callback); }, null);

                        action(callback);
                    }
                    catch
                    {
                        _faultedSessions.Add(session.SessionId);
                    }
                });
            }
            finally
            {
                _sessionListLock.ReleaseReaderLock();
            }
        }

        #endregion

        #region IDataHUBServiceContract - AuditData

        List<ProjectBase> IDataHUBServiceContract.GetProjects()
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetProjects");
            return AuditData.GetProjects();
        }

        ProjectBase IDataHUBServiceContract.GetProject(string projectId)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetProject(" + projectId + ")" );
            return AuditData.GetProject(projectId);
        }

        List<BudgetBase> IDataHUBServiceContract.GetBudget(string projectId)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetBudget(" + projectId + ")");
            return AuditData.GetBudget(projectId);
        }

        List<EmployeeBase> IDataHUBServiceContract.GetEmployees()
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetEmployees");
            return AuditData.GetEmployees();
        }

        EmployeeBase IDataHUBServiceContract.GetEmployee(string employeeId)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetEmployee("+ employeeId + ")");
            return AuditData.GetEmployee(employeeId);
        }

        List<ProjectEmployeeAssocBase> IDataHUBServiceContract.GetProjectEmployeeAssocs()
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetProjectEmployeeAssocs");
            return AuditData.GetProjectEmployeeAssocs();
        }

        ProjectEmployeeAssocBase IDataHUBServiceContract.GetProjectEmployeeAssoc(string projectEmployeeAssocId)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetProjectEmployeeAssoc(" + projectEmployeeAssocId + ")");
            return AuditData.GetProjectEmployeeAssoc(projectEmployeeAssocId);
        }

        List<ProjectEmployeeAllocBase> IDataHUBServiceContract.GetProjectEmployeeAllocs()
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetProjectEmployeeAllocs");
            return AuditData.GetProjectEmployeeAllocs();
        }

        ProjectEmployeeAllocBase IDataHUBServiceContract.GetProjectEmployeeAlloc(string projectEmployeeAllocId)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetProjectEmployeeAlloc(" + projectEmployeeAllocId + ")");
            return AuditData.GetProjectEmployeeAlloc(projectEmployeeAllocId);
        }

        List<ReportBase> IDataHUBServiceContract.GetReports()
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetReports()");
            return AuditData.GetReports();
        }        

        ProjectBase IDataHUBServiceContract.PostProject(ProjectBase projectBase)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.PostProject(" + projectBase.Name + ")");
            var result = AuditData.PostProject(projectBase);
            CallbackObjectMessage(ObjectMessageActionType.Post, ObjectMessageObjectType.Project, result.DBID);
            return result;
        }

        BudgetBase IDataHUBServiceContract.PostBudget(BudgetBase budget)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.PostBudget(" + budget.DBID + ")");
            var result = AuditData.PostBudget(budget);
            CallbackObjectMessage(ObjectMessageActionType.Post, ObjectMessageObjectType.Budget, result.DBID);
            return result;
        }

        EmployeeBase IDataHUBServiceContract.PostEmployee(EmployeeBase employeeBase)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.PostEmployee(" + employeeBase.Name + ")");
            var result = AuditData.PostEmployee(employeeBase);
            CallbackObjectMessage(ObjectMessageActionType.Post, ObjectMessageObjectType.Employee, result.DBID);
            return result;
        }

        ProjectEmployeeAssocBase IDataHUBServiceContract.PostProjectEmployeeAssoc(ProjectEmployeeAssocBase projectEmployeeAssocBase)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.PostProjectEmployeeAssoc(" + projectEmployeeAssocBase.ProjectId + "," + projectEmployeeAssocBase.EmployeeId + ")");
            var result = AuditData.PostProjectEmployeeAssoc(projectEmployeeAssocBase);
            CallbackObjectMessage(ObjectMessageActionType.Post, ObjectMessageObjectType.ProjectEmployeeAssoc, result.DBID);
            return result;

        }

        ProjectEmployeeAllocBase IDataHUBServiceContract.PostProjectEmployeeAlloc(ProjectEmployeeAllocBase projectEmployeeAllocBase)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.PostProjectEmployeeAlloc(" + projectEmployeeAllocBase.ProjectId + "," + projectEmployeeAllocBase.EmployeeId + "," + projectEmployeeAllocBase.AllocDate + ")");
            var result = AuditData.PostProjectEmployeeAlloc(projectEmployeeAllocBase);
            CallbackObjectMessage(ObjectMessageActionType.Post, ObjectMessageObjectType.ProjectEmployeeAlloc, result.DBID);
            return result;
        }

        List<ProjectEmployeeAllocBase> IDataHUBServiceContract.PostProjectEmployeeAllocList(List<ProjectEmployeeAllocBase> projectEmployeeAllocBaseList)
        {
            List<ProjectEmployeeAllocBase> result = new List<ProjectEmployeeAllocBase>();
            List<ObjectActionMessage> objectActionMessageList = new List<ObjectActionMessage>();

            foreach (var projectEmployeeAllocBase in projectEmployeeAllocBaseList)
            {
                DataHubLog.WriteLine("DataHUBServiceContract.PostProjectEmployeeAlloc(" + projectEmployeeAllocBase.ProjectId + "," + projectEmployeeAllocBase.EmployeeId + "," + projectEmployeeAllocBase.AllocDate + ")");
                var alloc = AuditData.PostProjectEmployeeAlloc(projectEmployeeAllocBase);
                result.Add(alloc);
                objectActionMessageList.Add(new ObjectActionMessage() { ActionType = ObjectMessageActionType.Post, ObjectType = ObjectMessageObjectType.ProjectEmployeeAlloc, ObjectId = alloc.DBID });
            }

            CallbackObjectActionMessageList(objectActionMessageList);
            return result;
        }

        ProjectBase IDataHUBServiceContract.PutProject(ProjectBase projectBase)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.PutProject(" + projectBase.DBID + ")");
            var result = AuditData.PutProject(projectBase);
            CallbackObjectMessage(ObjectMessageActionType.Put, ObjectMessageObjectType.Project, result.DBID);
            return result;
        }

        BudgetBase IDataHUBServiceContract.PutBudget(BudgetBase budget)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.PutBudget(" + budget.DBID + ")");
            var result = AuditData.PutBudget(budget);
            CallbackObjectMessage(ObjectMessageActionType.Put, ObjectMessageObjectType.Budget, result.DBID);
            return result;
        }

        EmployeeBase IDataHUBServiceContract.PutEmployee(EmployeeBase employeeBase)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.PutEmployee");
            var result = AuditData.PutEmployee(employeeBase);
            CallbackObjectMessage(ObjectMessageActionType.Put, ObjectMessageObjectType.Employee, result.DBID);
            return result;
        }

        ProjectEmployeeAssocBase IDataHUBServiceContract.PutProjectEmployeeAssoc(ProjectEmployeeAssocBase projectEmployeeAssocBase)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.PutProjectEmployeeAssoc(" + projectEmployeeAssocBase.DBID + ")");
            var result = AuditData.PutProjectEmployeeAssoc(projectEmployeeAssocBase);
            CallbackObjectMessage(ObjectMessageActionType.Put, ObjectMessageObjectType.ProjectEmployeeAssoc, result.DBID);
            return result;
        }

        ProjectEmployeeAllocBase IDataHUBServiceContract.PutProjectEmployeeAlloc(ProjectEmployeeAllocBase projectEmployeeAllocBase)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.PutProjectEmployeeAlloc(" + projectEmployeeAllocBase.DBID + ")");
            var result = AuditData.PutProjectEmployeeAlloc(projectEmployeeAllocBase);
            CallbackObjectMessage(ObjectMessageActionType.Put, ObjectMessageObjectType.ProjectEmployeeAlloc, result.DBID);
            return result;
        }


        BudgetBase IDataHUBServiceContract.DeleteBudget(BudgetBase budget)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.DeleteBudget(" + budget.DBID + ")");
            var result = AuditData.DeleteBudget(budget);
            CallbackObjectMessage(ObjectMessageActionType.Delete, ObjectMessageObjectType.Budget, result.DBID);
            return result;
        }

        ProjectEmployeeAssocBase IDataHUBServiceContract.DeleteProjectEmployeeAssoc(ProjectEmployeeAssocBase projectEmployeeAssocBase)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.DeleteProjectEmployeeAssoc(" + projectEmployeeAssocBase.DBID + ")");
            var result = AuditData.DeleteProjectEmployeeAssoc(projectEmployeeAssocBase);
            CallbackObjectMessage(ObjectMessageActionType.Delete, ObjectMessageObjectType.ProjectEmployeeAssoc, result.DBID);
            return result;
        }

        ProjectEmployeeAllocBase IDataHUBServiceContract.DeleteProjectEmployeeAlloc(ProjectEmployeeAllocBase projectEmployeeAllocBase)
        {            
            DataHubLog.WriteLine("DataHUBServiceContract.DeleteProjectEmployeeAlloc(" + projectEmployeeAllocBase.DBID + ")");
            var result = AuditData.DeleteProjectEmployeeAlloc(projectEmployeeAllocBase);
            CallbackObjectMessage(ObjectMessageActionType.Delete, ObjectMessageObjectType.ProjectEmployeeAlloc, result.DBID);
            return result;
        }

        List<ProjectEmployeeAllocBase> IDataHUBServiceContract.DeleteProjectEmployeeAllocList(List<ProjectEmployeeAllocBase> projectEmployeeAllocBaseList)
        {
            List<ProjectEmployeeAllocBase> result = new List<ProjectEmployeeAllocBase>();
            List<ObjectActionMessage> objectActionMessageList = new List<ObjectActionMessage>();

            foreach (var projectEmployeeAllocBase in projectEmployeeAllocBaseList)
            {
                DataHubLog.WriteLine("DataHUBServiceContract.DeleteProjectEmployeeAlloc(" + projectEmployeeAllocBase.DBID + ")");
                var alloc = AuditData.DeleteProjectEmployeeAlloc(projectEmployeeAllocBase);
                result.Add(alloc);
                objectActionMessageList.Add(new ObjectActionMessage() { ActionType = ObjectMessageActionType.Delete, ObjectType = ObjectMessageObjectType.ProjectEmployeeAlloc, ObjectId = projectEmployeeAllocBase.DBID });
            }

            CallbackObjectActionMessageList(objectActionMessageList);
            return result;
        }

        List<HolidayBase> IDataHUBServiceContract.GetHolidays(int year)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetHolidays");
            return AuditData.GetHolidays(year);
        }

        string IDataHUBServiceContract.GetSetting(string param)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetSetting(" + param + ")");
            return AuditData.GetSetting(param);
        }

        string IDataHUBServiceContract.PostSetting(string param, string value)
        {
            DataHubLog.WriteLine("DataHUBServiceContract.PostSetting(" + param + "," + value + ")");
            return AuditData.PostSetting(param, value);
        }

        string IDataHUBServiceContract.GetPermissions()
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetPermissions");
            return AuditData.GetPermissions();
        }

        Dictionary<string, string> IDataHUBServiceContract.GetAppUserPermissions()
        {
            DataHubLog.WriteLine("DataHUBServiceContract.GetAppUserPermissions");
            return AuditData.GetAppUserPermissions();
        }

        ReportBase IDataHUBServiceContract.CreateReport(ReportBase report)
        {
            ReportBase result = null;
            try
            {
                DataHubLog.WriteLine("DataHUBServiceContract.CreateReport(" + report.Description + ")");
                return DataHUBReports.CreateReport(report);
            }
            catch (Exception e)
            {

                
            }
            return result;
        }

        Stream IDataHUBServiceContract.DownloadReport(ReportBase report)
        {
            try
            {
                DataHubLog.WriteLine("DataHUBServiceContract.DownloadReport(" + report.DBID + ")");
                return DataHUBReports.DownloadReport(report);
            }
            catch (Exception e)
            {

                return null;
            }
        }


        void IDataHUBServiceContract.DeleteReport(ReportBase report)
        {
            try
            {
                DataHubLog.WriteLine("DataHUBServiceContract.DeleteReport(" + report.DBID + ")");
                DataHUBReports.DeleteReport(report);
            }
            catch (Exception e)
            {
                
            }
        }
        

        #endregion

        /*
        List<ObjectActionMessage> IDataHUBServiceContract.Test(List<ObjectActionMessage> objectActionMessageList)
        {
            CallbackObjectActionMessageList(objectActionMessageList);
            return objectActionMessageList;
        }
        */

    }
}
