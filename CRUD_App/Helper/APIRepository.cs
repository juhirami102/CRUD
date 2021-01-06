using Go2Share.Entity.Entity;
using Go2Share.General.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Go2Share.Web.Helper
{
    public class APICommunicationResponseModel<T>
    {
        public HttpStatusCode statusCode { get; set; }
        public T data { get; set; }
        public String Message { get; set; }
        public Boolean Success { get; set; }
        public string NotificationType { get; set; }
        public string ReturnToUrl { get; set; }
        List<ErrorMessage> ErrorMessages { get; set; }
    }
    public class APIRepository
    {
        public static string baseURL;
        private IConfiguration Configuration;
        public APIRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            baseURL = Configuration.GetSection("Apiconfig").GetSection("baseurl").Value;
        }

        #region APICommunication - Common Method for API calling
        public APICommunicationResponseModel<string> APICommunication(string URL, HttpMethod invokeType, ByteArrayContent body, string token)
        {
            APICommunicationResponseModel<string> response = new APICommunicationResponseModel<string>();
            response.statusCode = HttpStatusCode.BadRequest;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    HttpResponseMessage oHttpResponseMessage = new HttpResponseMessage();

                    if (invokeType.Method == HttpMethod.Get.ToString())
                    {
                        var responseTask = client.GetAsync(URL);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    else if (invokeType.Method == HttpMethod.Post.ToString())
                    {
                        if (body != null)
                            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                        var responseTask = client.PostAsync(URL, body);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    else if (invokeType.Method == HttpMethod.Put.ToString())
                    {
                        if (body != null)
                            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                        var responseTask = client.PostAsync(URL, body);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    else if (invokeType.Method == HttpMethod.Delete.ToString())
                    {
                        if (body != null)
                            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                        var responseTask = client.DeleteAsync(URL);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    response.statusCode = oHttpResponseMessage.StatusCode;

                    if (oHttpResponseMessage.IsSuccessStatusCode)
                        response.data = oHttpResponseMessage.Content.ReadAsStringAsync().Result;
                    else
                        response.data = string.Empty;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public APICommunicationResponseModel<string> APICommunicationImage(string URL, HttpMethod invokeType, MultipartFormDataContent body, string token)
        {
            APICommunicationResponseModel<string> response = new APICommunicationResponseModel<string>();
            response.statusCode = HttpStatusCode.BadRequest;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    HttpResponseMessage oHttpResponseMessage = new HttpResponseMessage();

                    if (invokeType.Method == HttpMethod.Get.ToString())
                    {
                        var responseTask = client.GetAsync(URL);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    else if (invokeType.Method == HttpMethod.Post.ToString())
                    {
                        if (body != null)
                            body.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                        var responseTask = client.PostAsync(URL, body);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    else if (invokeType.Method == HttpMethod.Put.ToString())
                    {
                        if (body != null)
                            body.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                        var responseTask = client.PostAsync(URL, body);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    else if (invokeType.Method == HttpMethod.Delete.ToString())
                    {
                        if (body != null)
                            body.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                        var responseTask = client.DeleteAsync(URL);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    response.statusCode = oHttpResponseMessage.StatusCode;

                    if (oHttpResponseMessage.IsSuccessStatusCode)
                        response.data = oHttpResponseMessage.Content.ReadAsStringAsync().Result;
                    else
                        response.data = string.Empty;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }
        #endregion

        #region Common Method for GetToken
        public AccessTokenModel GetToken(LoginModel objuser,string apiurl)
        {
            AccessTokenModel tokenData = new AccessTokenModel();

            try
            {
                HttpResponseMessage oHttpResponseMessage = new HttpResponseMessage();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    var content = new StringContent(JsonConvert.SerializeObject(objuser), Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync(apiurl, content);//"/api/LoginAuth/login"
                    responseTask.Wait();

                    oHttpResponseMessage = responseTask.Result;
                }

                if (oHttpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResult = oHttpResponseMessage.Content.ReadAsStringAsync().Result;

                    tokenData = JsonConvert.DeserializeObject<AccessTokenModel>(jsonResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tokenData;
        }
        #endregion

        
    }
}
