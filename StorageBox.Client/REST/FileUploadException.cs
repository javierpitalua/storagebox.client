using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Client.REST
{
    public class FileUploadException : Exception
    {
        public FileUploadException(string errorMessage, SubmitFileResponse response) : base (errorMessage)
        {
            this.Response = response;
        }

        public SubmitFileResponse Response { get; set; }
    }
}
