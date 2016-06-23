using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Client.REST
{
    #region internal Classes
    public class RetrieveFileRequest
    {
        public string FileId { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }

    public class RetrieveFileResponse
    {
        /// <summary>
        /// The id of the original file
        /// </summary>
        public string FileID { get; set; }

        /// <summary>
        /// The original name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The encoded array of bytes of the file as BASE64
        /// </summary>
        public string FileContent { get; set; }

        /// <summary>
        /// The suggested mime type (as in the service database).
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// The extension of the file
        /// </summary>
        public string Extension { get; set; }


        public bool OperationSuccesful { get; set; }
    }

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
        #endregion

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

        /// <summary>
        /// Submits credentials to server an returns a session id
        /// </summary>
        /// <returns>Returns session Id when storage has been succesful</returns>
        public string Authenticate()
        {
            string authenticationUri = string.Format("{0}{1}", this.ServiceConnectionSettings.Url, "api/Authentication/StartSession");
            var web = new WebMethod(authenticationUri);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthenticationResponse>(web.SubmitAndRetrieveJson(this.ServiceConnectionSettings.ToString()));
            if (!response.AuthenticationSuccesful)
            {
                throw new AuthenticationException("Unable to authenticate to storage service. See server response for more details.", response);
            }
            return response.SessionId;
        }

        public string SubmitFile(SubmitFileRequest request, string sessionId)
        {
            //Set uri for calling service:
            string uri = string.Format("{0}{1}", this.ServiceConnectionSettings.Url, "api/Storage/SubmitFile");
            var web = new WebMethod(uri);

            //Add authentication headers:
            web.RequestHeaders.Add("_sessionID", sessionId);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<SubmitFileResponse>(web.SubmitAndRetrieveJson(request.ToString()));

            if (!response.OperationSuccesful)
            {
                throw new FileUploadException(response.ErrorMessage, response);
            }

            return response.FileId;
        }

        public RetrieveFileResponse RetrieveFile(RetrieveFileRequest request, string sessionId)
        {
            //Set uri for calling service:
            string uri = string.Format("{0}{1}", this.ServiceConnectionSettings.Url, "api/Storage/RetrieveFile");
            var web = new WebMethod(uri);
            //Add authentication headers:
            web.RequestHeaders.Add("_sessionID", sessionId);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<RetrieveFileResponse>(web.SubmitAndRetrieveJson(request.ToString()));
            return response;
        }
    }
}
