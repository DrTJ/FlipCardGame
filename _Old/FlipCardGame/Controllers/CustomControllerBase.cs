using RoyaMVC_EN.Attributes;
using FlipCardGame.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlipCardGame.Controllers
{

    [RoyaErrorHandler()]
    [FlipCardGame.Filters.InitializeSimpleMembership()]
    public class CustomControllerBase : Controller
    {
        protected GeneralRepository repoGeneral;

        public CustomControllerBase() {
            this.repoGeneral = new GeneralRepository();
            if (ViewBag.LogonData == null) ViewBag.LogonData = new RoyaMVC_EN.AccountManagement.LoginModel();
        }

        public byte[] GetNoImageBytes(Size ImageSize) {
            var mem = new MemoryStream();
            RoyaMVC_EN.Properties.Resources.NoPhotoIcon.GetThumbnailImage(ImageSize.Width, ImageSize.Height, null, IntPtr.Zero).Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
            return mem.ToArray();
        }

        public void AddMessage(RoyaMVC_EN.Models.AppMessage message) {
            AddMessage(RoyaMVC_EN.Models.AppMessage.Add(message));
        }

        public void AddMessage(List<RoyaMVC_EN.Models.AppMessage> messages) {
            var resMessages = new List<RoyaMVC_EN.Models.AppMessage>();

            if (ViewBag.Messages != null) {
                resMessages.AddRange(ViewBag.Messages as List<RoyaMVC_EN.Models.AppMessage>);
            }

            resMessages.AddRange(messages);
            ViewBag.Messages = resMessages;
        }

        public byte[] UploadSetImage(string fileInputID, System.Drawing.Size desiredSize, Func<bool> hasImage, Func<byte[]> GetImage) {
            if ((Request.Files.Count > 0) && (Request.Files.AllKeys.Contains(fileInputID))) {
                var thumbnailImage = this.repoGeneral.UploadData(Request.Files[fileInputID]);
                return thumbnailImage.Content;
            }
            else if (hasImage()) {
                return GetImage();
            }
            else
                return this.GetNoImageBytes(new Size(desiredSize.Width, desiredSize.Height));
        }

        public string UploadSaveImage(string fileInputID, string desiredFileName, Size imageSize, System.Drawing.Imaging.ImageFormat format = null) {
            var res = "";
            if ((Request.Files.Count > 0) && (Request.Files.AllKeys.Contains(fileInputID))) {
                var fileUploader = Request.Files[fileInputID];
                var thumbnailImage = this.repoGeneral.UploadData(Request.Files[fileInputID]);
                var thumbnailImageBytes = thumbnailImage.Content;
                var resPath = "/Content/Products/";
                var savingPath = Server.MapPath(resPath);

                if (format == null) {
                    var extension = RoyaMVC_EN.MIMEAssister.GetExtensionFromMIMEType(fileUploader.ContentType);
                    format = GetImageFormatFromExtension(extension);
                    desiredFileName += "." + extension;
                }

                if ((fileUploader.ContentType == "application/octet-stream") && (fileUploader.ContentLength == 0)) {
                    return resPath + desiredFileName;
                }

                if (Directory.Exists(savingPath) == false)
                    Directory.CreateDirectory(savingPath);

                var imgBytes = RoyaMVC_EN.HelperClass.ResizeImageBytes(thumbnailImageBytes, imageSize.Width, imageSize.Height, format);
                res = savingPath + desiredFileName;
                System.IO.File.WriteAllBytes(res, imgBytes);
                res = resPath + desiredFileName;
            }

            return res;
        }

        private System.Drawing.Imaging.ImageFormat GetImageFormatFromExtension(string extension) {
            ImageFormat res = ImageFormat.Png;

            switch (extension.ToLower()) {
                case "jpg":
                case "jpeg":
                    res = ImageFormat.Jpeg; break;

                case "png":
                    res = ImageFormat.Png; break;

                case "bmp":
                    res = ImageFormat.Bmp; break;

                case "exif":
                    res = ImageFormat.Exif; break;

                case "gif":
                    res = ImageFormat.Gif; break;

                case "icon":
                case "ico":
                    res = ImageFormat.Icon; break;

                case "tiff":
                    res = ImageFormat.Tiff; break;

                case "wmf":
                    res = ImageFormat.Wmf; break;
            }


            return res;
        }

        public string UploadSaveFile(string fileInputID, string desiredFileName) {
            if ((Request.Files.Count > 0) && (Request.Files.AllKeys.Contains(fileInputID))) {
                var fileUploader = Request.Files[fileInputID];
                var resUpload = this.repoGeneral.UploadData(fileUploader);

                var resourcesPath = "/Resources/";
                var relativeFileName = resourcesPath + desiredFileName + GetExtension(fileUploader.FileName);

                if ((fileUploader.ContentType == "application/octet-stream") && (fileUploader.ContentLength == 0)) {
                    return relativeFileName;
                }

                var directoryPath = Server.MapPath(resourcesPath);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var localFileName = Server.MapPath(relativeFileName);
                System.IO.File.WriteAllBytes(localFileName, resUpload.Content);

                return relativeFileName;
            }
            else
                return "";
        }

        private string GetExtension(string fileName) {
            return new System.IO.FileInfo(fileName).Extension;
        }

        public RoyaMVC_EN.UploadFileData UploadFile(string fileInputID) {
            if ((Request.Files.Count > 0) && (Request.Files.AllKeys.Contains(fileInputID))) {
                var res = this.repoGeneral.UploadData(Request.Files[fileInputID]);
                return res;
            }
            else
                return RoyaMVC_EN.UploadFileData.NoFile;
        }
    }
}