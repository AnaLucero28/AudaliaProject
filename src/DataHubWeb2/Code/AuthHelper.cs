using Audalia.DataHUBCommon;
using Audalia.DataHUBServer.Audit;
using System.Web;

namespace DataHubWeb2.Model {
    public class ApplicationUser {
        public EmployeeBase EmployeeBase { get; set; }
        public string UserName { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
    }

    public static class AuthHelper 
    {
        static EmployeeBase employee;
        public static EmployeeBase Employee
        {
            get
            {
                return employee;
            }
        }

        public static bool SignIn(string userName, string password) 
        {                       
            var windowsUser = @"audalia\" + userName.ToLowerInvariant();
            var employees = AuditData.GetEmployees();
            employee = employees.FindLast(p => p.WindowsUser.ToLowerInvariant() == windowsUser);
            if (employee != null)
            {

                HttpContext.Current.Session["User"] = new ApplicationUser
                {
                    EmployeeBase = employee,
                    UserName = windowsUser,
                    FirstName = employee.Name.Split(' ')[0],
                    LastName = employee.Name.Split(' ')[1],
                    Email = "",                    
                    AvatarUrl = "~/Content/Photo/User.png"
                };
                return true;
            }
            else
                return false;
            
            /*
            HttpContext.Current.Session["User"] = CreateDefualtUser();  // Mock user data
            return true;
            */
            
        }
        public static void SignOut() {
            HttpContext.Current.Session["User"] = null;
        }
        public static bool IsAuthenticated() {
            return GetLoggedInUserInfo() != null;
        }

        public static ApplicationUser GetLoggedInUserInfo() {
            return HttpContext.Current.Session["User"] as ApplicationUser;
        }
        private static ApplicationUser CreateDefualtUser() {
            return new ApplicationUser {
                UserName = "JBell",
                FirstName = "Julia",
                LastName = "Bell",
                Email = "julia.bell@example.com",
                //AvatarUrl = "~/Content/Photo/Julia_Bell.jpg"
                AvatarUrl = "~/Content/Photo/User.png"
            };
        }

    }
}