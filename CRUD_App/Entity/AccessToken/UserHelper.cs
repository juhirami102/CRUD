using CRUD_App.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CRUD_App.Entity.AccessToken
{
    public class UserHelper
    {
        public static string LoggedInUserEmailAddress = "LoggedInUserEmailAddress";
        public static string LoggedInUserName = "LoggedInUserName";
        public static string LoggedInUserId = "LoggedInUserId";


        private string _apiBaseURI = "https://localhost:5001";
        public HttpClient InitializeClient()
        {
            var client = new HttpClient();
            //Passing service base url  
            client.BaseAddress = new Uri(_apiBaseURI);

            client.DefaultRequestHeaders.Clear();
            //Define request data format  
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;

        }

        public static UserLoggedInModel GetLoggedInUser(string token)
        {


            var handler = new JwtSecurityTokenHandler();

            var PolicyList = handler.ReadJwtToken(token);

            UserLoggedInModel oUserLoggedInModel = new UserLoggedInModel();

            foreach (var item in PolicyList.Claims)
            {

                foreach (var prop in oUserLoggedInModel.GetType().GetProperties())
                {
                    if (prop.Name.ToLower() == item.Type.ToLower())
                    {
                        if (prop.PropertyType == typeof(System.Int32))
                        {
                            prop.SetValue(oUserLoggedInModel, (!string.IsNullOrEmpty(item.Value) ? Convert.ToInt32(item.Value) : 0));
                        }
                        else if (prop.PropertyType == typeof(System.String))
                        {
                            prop.SetValue(oUserLoggedInModel, item.Value);
                        }
                    }
                }
            }



            return oUserLoggedInModel;
        }

        public static int GetLoggedInUserId(string authorizationToken)
        {
            int UserId = 0;///string.Empty;
            if (!string.IsNullOrEmpty(authorizationToken))
            {
                authorizationToken = authorizationToken.Replace("Bearer", "").Trim();
                var loggedInUser = GetLoggedInUser(authorizationToken);
                UserId = loggedInUser.UserId;//.ToString();
            }
            return UserId;
        }

        public static int GetLoggedInRoleId(string authorizationToken)
        {
            int RoleId = 0;
            if (!string.IsNullOrEmpty(authorizationToken))
            {
                authorizationToken = authorizationToken.Replace("Bearer", "").Trim();
                var loggedInUser = GetLoggedInUser(authorizationToken);
                RoleId = loggedInUser.RoleId;
            }
            return RoleId;
        }

    }
}
