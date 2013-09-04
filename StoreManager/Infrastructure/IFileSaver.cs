using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace StoreManager.Infrastructure {

    public interface IFileSaver {
        string Save(HttpPostedFileBase fileBase, string fileName = "");
        void Delete(string fileName);
    }

    public abstract class FileSaverBase : IFileSaver {
        public static readonly Func<HttpPostedFileBase, string> SuggestFileName = arg => Guid.NewGuid() + "-" + Path.GetFileName(arg.FileName);
        public static readonly Action<string> EnsureFoldersExist =
            path => {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            };

        private const int LargeThumbWidth = 480;
        private const int LargeThumbHeight = 300;
        private const int SmallThumbWidth = 196;
        private const int SmallThumbHeight = 144;

        public string OriginalFolder { get; set; }
        public string ThumbnailFolder { get; set; }
        public string TempFolder { get; set; }

        public string Save(HttpPostedFileBase file, string fileName = "") {
            fileName = string.IsNullOrEmpty(fileName) ? SuggestFileName(file) : fileName;
            var tempSaveLocation = Path.Combine(TempFolder, fileName);

            EnsureFoldersExist(TempFolder);
            EnsureFoldersExist(OriginalFolder);
            EnsureFoldersExist(ThumbnailFolder);

            file.SaveAs(tempSaveLocation);

            SaveOriginal(tempSaveLocation, fileName);
            SaveThumbnail(tempSaveLocation, fileName);

            File.Delete(tempSaveLocation);

            return fileName;
        }

        private void SaveOriginal(string tempFile, string fileName) {
            const int targetWidth = LargeThumbWidth;
            const int targetHeight = LargeThumbHeight;

            var bitmap = new Bitmap(tempFile);
            var location = Path.Combine(OriginalFolder, fileName);

            ResizeAndSave(targetWidth, targetHeight, bitmap, location);
        }

        private void SaveThumbnail(string tempFile, string fileName) {
            const int targetWidth = SmallThumbWidth;
            const int targetHeight = SmallThumbHeight;

            var bitmap = new Bitmap(tempFile);
            var location = Path.Combine(ThumbnailFolder, fileName);

            ResizeAndSave(targetWidth, targetHeight, bitmap, location);
        }

        private static void ResizeAndSave(int targetWidth, int targetHeight, Image bitmap, string location) {
            var resizedBitmap = new Bitmap(targetWidth, targetHeight);
            var graphics = Graphics.FromImage(resizedBitmap);

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.FillRectangle(Brushes.White, 0, 0, targetWidth, targetHeight);
            graphics.DrawImage(bitmap, 0, 0, targetWidth, targetHeight);

            resizedBitmap.Save(location);

            bitmap.Dispose();
            graphics.Dispose();
            resizedBitmap.Dispose();
        }

        public void Delete(string fileName) {
            var original = Path.Combine(OriginalFolder, fileName);
            var thumbnail = Path.Combine(ThumbnailFolder, fileName);

            if (File.Exists(original)) File.Delete(original);
            if (File.Exists(thumbnail)) File.Delete(thumbnail);
        }
    }
    
    public class PictureSaver : FileSaverBase {
        public PictureSaver() {
            OriginalFolder = HostingEnvironment.MapPath("~/App_Data/Pictures");
            ThumbnailFolder = HostingEnvironment.MapPath("~/App_Data/Pictures/Thumbs");
            TempFolder = HostingEnvironment.MapPath("~/App_Data/Pictures/Temp");
        }
    }
}