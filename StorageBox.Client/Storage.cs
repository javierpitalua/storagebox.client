using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Client
{
    public class UploadFileException : Exception
    {
        public UploadFileException(string exceptionMessage) : base(exceptionMessage)
        {

        }
    }

    public class Storage
    {
        public REST.Client Service { get; set; }

        public Storage()
        {
            this.Service = new REST.Client();
        }

        public Storage(StorageConnectionSettings settings)
        {
            this.Service = new REST.Client(settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="fileId"></param>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        public string RetrieveFile(string sessionId, string fileId, string outputDir)
        {
            var fileRecord = this.Service.RetrieveFile(new REST.RetrieveFileRequest() { FileId = fileId }, sessionId);
            var fileName = System.IO.Path.Combine(outputDir, string.Format("{0}{1}", fileRecord.FileID, fileRecord.Extension));
            System.IO.File.WriteAllBytes(fileName, Convert.FromBase64String(fileRecord.FileContent));
            return fileName; 
        }

        /// <summary>
        /// Submits credentials to server an returns a session id
        /// </summary>
        /// <returns>Returns session Id when storage has been succesful</returns>
        public string Authenticate()
        {
            return this.Service.Authenticate();
        }

        /// <summary>
        /// Uploads a file to the storage service, then returns the file Id
        /// </summary>
        /// <param name="pathToFile">The full path to the file name.</param>
        /// <returns></returns>
        public string SubmitFile(string sessionId, string pathToFile, bool deleteAfterSubmit)
        {
            if (!System.IO.File.Exists(pathToFile))
            {
                throw new UploadFileException(string.Format("File [{0}] is not present.", pathToFile));
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            var fileContent = Convert.ToBase64String(bytes);

            var fileId = this.Service.SubmitFile(new REST.SubmitFileRequest()
            {
                FileName = pathToFile,
                FileContent = fileContent
            }, sessionId);

            if (deleteAfterSubmit)
            {
                System.IO.File.Delete(pathToFile);
            }

            return fileId;
        }

        /// <summary>
        /// Uploads a file to the storage service, then returns the file Id
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="deleteAfterSubmit"></param>
        /// <returns></returns>
        public static string UploadFile(string sessionId, string pathToFile, bool deleteAfterSubmit)
        {
            var storage = new Storage();
            return storage.SubmitFile(sessionId, pathToFile, deleteAfterSubmit);
        }

        /// <summary>
        /// Gets a file from the service
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="fileId"></param>
        /// <param name="outputDir"></param>
        /// <returns></returns>
        public static string DownloadFile(string sessionId, string fileId, string outputDir)
        {
            var storage = new Storage();
            return storage.RetrieveFile(sessionId, fileId, outputDir);
        }

        /// <summary>
        /// Submits credentials to server an returns a session id
        /// </summary>
        /// <returns>Returns session Id when storage has been succesful</returns>
        public static string GetSessionId()
        {
            var storage = new Storage();
            return storage.Authenticate();
        }
    }
}
