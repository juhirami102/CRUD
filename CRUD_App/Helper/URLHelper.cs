using Microsoft.AspNetCore.Hosting;

namespace CRUD_App.Web.Helper
{
    public class URLHelper
    {
        public static string WebRootPath = string.Empty;

        public static class Common
        {
            public static string AlertURL = WebRootPath + "Admin/Base/GetAlert";
            public static string AlertPartialURL = "~/Views/Shared/_Alert.cshtml";
            public static string DeletePartialURL = "~/Views/Shared/_Delete.cshtml";
            public static string InsertUpdatePartialURL = "_InsertUpdate";
        }

        
 
        public static class UserManagement
        {
            public static string UserTableId = "UserTable";
            public static string UserModalId = "UserModal";
            public static string UserListUrl = "LoadUserList";
            public static string UserDeleteURL = "Admin/UserManagement/DeleteUser/";
        }
        
    }
}
