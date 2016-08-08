using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FlipCardGame.Models.Repositories
{
    public class RepositoryBase : IRepositoryBase<DAL.FlipCardGameEntities>, IDisposable
    {
        public DAL.FlipCardGameEntities Context { get; set; }

        public RepositoryBase() { this.Context = new DAL.FlipCardGameEntities(); }

        public static RoyaMVC_EN.UploadFileData UploadFile(HttpFileCollectionBase requestFiles, string fileUploadID) {
            if (requestFiles.AllKeys.Contains(fileUploadID)) {
                HttpPostedFileBase file = requestFiles[fileUploadID];
                return UploadFile(file);
            }
            else {
                return RoyaMVC_EN.UploadFileData.NoFile;
            }
        }

        public static RoyaMVC_EN.UploadFileData UploadFile(HttpPostedFileBase file) {
            int fileLength = file.ContentLength;
            var fileName = file.FileName;
            var mimeType = file.ContentType;

            byte[] theContent = new byte[fileLength];
            file.InputStream.Read(theContent, 0, fileLength);
            file.InputStream.Seek(0, System.IO.SeekOrigin.Begin);

            return new RoyaMVC_EN.UploadFileData() { FileName = fileName, Content = theContent, MimeType = mimeType };
        }

        public static string Alert(string Message) {
            return string.Format("<script>alert('{0}');</script>", Message);
        }

        public void Dispose() {
            this.Context.Dispose();
        }


    }
}
