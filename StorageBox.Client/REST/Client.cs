using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Client.REST
{
    #region internal Classes
    public class AuthenticationResponse
    {
        public string SessionId { get; set; }
        public bool AuthenticationSuccesful { get; set; }
        public bool OperationSuccesful { get; set; }
        public string ErrorMessage { get; set; }
        public string ExceptionDetails { get; set; }
    }

    public class SubmitFileRequest
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }

    public class SubmitFileResponse
    {
        public string FileId { get; set; }

        public bool OperationSuccesful { get; set; }
        public string ErrorMessage { get; set; }
        public string ExceptionDetails { get; set; }
    }
    #endregion

    public class Client
    {
        #region Properties
        public StorageConnectionSettings ServiceConnectionSettings { get; set; }
        public string SessionId { get; set; }
        #endregion


        public bool IsAuthenticated()
        {
            return false;
        }

        #region ctors
        public Client()
        {
            this.ServiceConnectionSettings = StorageConnectionSettings.LoadSettingsFromConfigFile();
        }

        public Client(StorageConnectionSettings settings)
        {
            this.ServiceConnectionSettings = settings;
        }
        #endregion

        public void Authenticate()
        {
            string authenticationUri = string.Format("{0}{1}", this.ServiceConnectionSettings.Url, "api/Authentication/StartSession");
            var web = new WebMethod(authenticationUri);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthenticationResponse>(web.SubmitAndRetrieveJson(this.ServiceConnectionSettings.ToString()));

            if (!response.AuthenticationSuccesful)
            {
                throw new AuthenticationException("Unable to authenticate to storage service. See server response for more details.", response);
            }

            this.SessionId = response.SessionId;
        }

        public string SubmitFile(SubmitFileRequest request)
        {
            if (!this.IsAuthenticated())
            {
                this.Authenticate();
            }

            //Set uri for calling service:
            string authenticationUri = string.Format("{0}{1}", this.ServiceConnectionSettings.Url, "api/Storage/SubmitFile");
            var web = new WebMethod(authenticationUri);

            //Add authentication headers:
            web.RequestHeaders.Add("_sessionID", this.SessionId);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<SubmitFileResponse>(web.SubmitAndRetrieveJson(request.ToString()));

            if (!response.OperationSuccesful)
            {
                throw new FileUploadException(response.ErrorMessage, response);
            }

            return response.FileId;
        }
    }
}
