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
        /// Uploads a file to the storage service, then returns the file Id
        /// </summary>
        /// <param name="pathToFile">The full path to the file name.</param>
        /// <returns></returns>
        public string SubmitFile(string pathToFile, bool deleteAfterSubmit)
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
            });

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
        public static string UploadFile(string pathToFile, bool deleteAfterSubmit)
        {
            var storage = new Storage();
            return storage.SubmitFile(pathToFile, deleteAfterSubmit);
        }
    }
}
