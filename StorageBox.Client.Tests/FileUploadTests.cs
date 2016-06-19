using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StorageBox.Client.Tests
{
    [TestClass]
    public class FileUploadTests
    {
        [TestMethod]
        public void UploadFile()
        {
            string fileId = StorageBox.Client.Storage.UploadFile(@"c:\Tools\tweakslogon.zip", false);
            Assert.IsTrue(!string.IsNullOrEmpty(fileId));
        }
    }
}

