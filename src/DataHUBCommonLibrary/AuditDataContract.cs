using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Audalia.DataHUBCommon
{
    [DataContract(Name = "ObjectAction", Namespace = "Audalia.DataHUBCommon")]
    public class ObjectActionMessage
    {
        [DataMember] public ObjectMessageActionType ActionType { get; set; }
        [DataMember] public ObjectMessageObjectType ObjectType { get; set; }
        [DataMember] public string ObjectId { get; set; }        
    }

    [DataContract(Name = "DataHubSessionBase", Namespace = "Audalia.DataHUBCommon")]
    public class DataHubSessionBase
    {
        [DataMember] public string SessionId { get; set; }
        [DataMember] public string ApplicationName { get; set; }
        [DataMember] public string ComputerName { get; set; }
        [DataMember] public DateTime ConnectionDateTime { get; set; }
        [DataMember] public bool Faulted { get; set; }
        [DataMember] public string Function { get; set; }
        [DataMember] public DateTime LastPongDateTime { get; set; }
        [DataMember] public string ProcessId { get; set; }
        [DataMember] public string UserName { get; set; }
    }

    [DataContract(Name = "HolidayBase", Namespace = "Audalia.DataHUBCommon")]
    public class HolidayBase
    {
        [DataMember] public virtual string DBID { get; set; }
        [DataMember] public virtual string Name { get; set; }
        [DataMember] public virtual int Year { get; set; }
        [DataMember] public virtual int Month { get; set; }
        [DataMember] public virtual int Day { get; set; }
    }


    [DataContract(Name = "ProjectBase", Namespace = "Audalia.DataHUBCommon")]
    public class ProjectBase
    {
        [DataMember] public virtual string DBID { get; set; }
        [DataMember] public virtual string Name { get; set; }
        [DataMember] public virtual string BranchOffice { get; set; }        
        [DataMember] public virtual string TaxYear { get; set; }
        [DataMember] public virtual string IdGextor { get; set; }

        [DataMember] public virtual DateTime StartDate1 { get; set; }
        [DataMember] public virtual DateTime FinishDate1 { get; set; }

        [DataMember] public virtual DateTime? StartDate2 { get; set; }
        [DataMember] public virtual DateTime? FinishDate2 { get; set; }

        [DataMember] public virtual DateTime? StartDate3 { get; set; }
        [DataMember] public virtual DateTime? FinishDate3 { get; set; }

        [DataMember] public virtual DateTime BaselineStartDate { get; set; }
        [DataMember] public virtual DateTime BaselineFinishDate { get; set; }
        [DataMember] public virtual string Color { get; set; }
        [DataMember] public virtual string Comments { get; set; }
    }

    [DataContract(Name = "BudgetBase", Namespace = "Audalia.DataHUBCommon")]
    public class BudgetBase
    {
        [DataMember] public virtual string DBID { get; set; }
        [DataMember] public virtual string ProjectId { get; set; }
        [DataMember] public virtual string Category { get; set; }
        [DataMember] public virtual int Quantity { get; set; }
    }

    [DataContract(Name = "EmployeeBase", Namespace = "Audalia.DataHUBCommon")]
    public class EmployeeBase
    {
        [DataMember] public virtual string DBID { get; set; }
        [DataMember] public virtual string Name { get; set; }
        [DataMember] public virtual string BranchOffice { get; set; }
        [DataMember] public virtual string Category { get; set; }
        [DataMember] public virtual string WindowsUser { get; set; }
        //[DataMember] public virtual string Status { get; set; }
        [DataMember] public virtual string Permissions { get; set; }
        [DataMember] public virtual DateTime? LeavingDate { get; set; }
    }

    [DataContract(Name = "ProjectEmployeeAssocBase", Namespace = "Audalia.DataHUBCommon")]
    public class ProjectEmployeeAssocBase
    {
        [DataMember] public virtual string DBID { get; set; }
        [DataMember] public virtual string ProjectId { get; set; }
        [DataMember] public virtual string EmployeeId { get; set; }
        [DataMember] public virtual string Role { get; set; }
    }

    [DataContract(Name = "ProjectEmployeeAllocBase", Namespace = "Audalia.DataHUBCommon")]
    public class ProjectEmployeeAllocBase
    {
        [DataMember] public virtual string DBID { get; set; }
        [DataMember] public virtual string ProjectId { get; set; }
        [DataMember] public virtual string EmployeeId { get; set; }
        [DataMember] public virtual DateTime AllocDate { get; set; }
        [DataMember] public virtual int Quantity { get; set; }
    }

    public enum ReportTemplate { Projects, Hours, BalanceAnalitico, BalanceAnaliticoBeta, FacturacionPorSocio, Imputaciones, Realizacion };

    [DataContract(Name = "ReportBase", Namespace = "Audalia.DataHUBCommon")]
    public class ReportBase
    {
        [DataMember] public virtual string DBID { get; set; }
        [DataMember] public virtual ReportTemplate ReportTemplate { get; set; }
        [DataMember] public virtual string ReportName { get; set; }
        [DataMember] public virtual string CreatorUserName { get; set; }
        [DataMember] public virtual DateTime CreationDate { get; set; }
        [DataMember] public virtual string Description { get; set; }
        [DataMember] public virtual Dictionary<string, string> Params { get; set; }
        [DataMember] public virtual string FilePath { get; set; }

    }



}


