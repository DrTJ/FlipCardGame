using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoyaMVC_EN.AccountManagement;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Web.Mvc;

namespace FlipCardGame.Models.Repositories
{
    public class GeneralRepository : RepositoryBase, IRepositoryBase<DAL.FlipCardGameEntities>
    {
        public Repositories.SitePagesRepository repoSitePages;

        public GeneralRepository() { repoSitePages = new SitePagesRepository(); }

        public DAL.SitePages GetAboutPage() {
            return this.repoSitePages.GetSitePage("AboutPage");
        }


        #region General Functions

        public RoyaMVC_EN.UploadFileData UploadData(System.Web.HttpPostedFileBase postedFile) {
            var res = RepositoryBase.UploadFile(postedFile);
            return res;
        }

        #endregion


        public static byte[] GetNoPhotoImage(int width, int height) {
            return GetNoPhotoImage(width, height, ImageFormat.Jpeg);
        }

        public static byte[] GetNoPhotoImage(int width, int height, ImageFormat format) {
            return ConvertToBytes(RoyaMVC_EN.Properties.Resources.NoPhotoIcon, width, height, format);
        }

        public static byte[] GetHumanNoPhotoImage(int width, int height, ImageFormat format) {
            return ConvertToBytes(RoyaMVC_EN.Properties.Resources.HumanNoPhotoIcon, width, height, format);
        }

        public static byte[] ConvertToBytes(Bitmap resourceData, int width, int height, ImageFormat format) {
            using (var mem = new MemoryStream()) {
                resourceData.Save(mem, format);
                var resBytes = mem.ToArray();
                var res = RoyaMVC_EN.HelperClass.ResizeImageBytes(resBytes, width, height, format);
                return res;
            }
        }

        public byte[] GetImageBytesOrNoPhotoBytes(byte[] image, ImageFormat format) {
            using (var mem = new MemoryStream()) {
                RoyaMVC_EN.Properties.Resources.NoPhotoIcon.Save(mem, format);
                var noPhotoBytes = mem.ToArray();

                return GetImageBytesOrDefault(image, noPhotoBytes, format);
            }
        }

        public byte[] GetImageBytesOrDefault(byte[] image, byte[] defaultImageBytes, ImageFormat format) {
            if (image.Length == 0) {
                return defaultImageBytes;
            }
            else
                return image;
        }
    }
}
