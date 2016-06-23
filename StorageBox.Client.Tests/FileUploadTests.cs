using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StorageBox.Client.Tests
{
    [TestClass]
    public class FileUploadTests
    {
        private string sessionId;
        public string SessionId
        {
            get
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    sessionId = Storage.GetSessionId();
                };
                return sessionId;
            }
        }

        public FileUploadTests()
        {

        }

        [TestMethod]
        public void UploadFile()
        {
            string fileId = StorageBox.Client.Storage.UploadFile(this.SessionId, @"c:\Tools\tweakslogon.zip", false);

            Assert.IsTrue(!string.IsNullOrEmpty(fileId));

            string outputFile = StorageBox.Client.Storage.DownloadFile(this.SessionId, fileId, @"C:\Temp\Files\");

            Assert.IsTrue(System.IO.File.Exists(outputFile));
        }


    }
}

