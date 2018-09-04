using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace UtiltyManagemnt
{
    /// <summary>
    /// Summary description for ThumbnailUtilities
    /// </summary>
    public class ThumbnailUtilities
    {

        private const string ThumbnailDir = "Thumbnails";
        private const string NewDir = "artimg";


        /// <summary>
        /// Deletes a thumbnail image from the file system.
        /// </summary>
        /// <param name="imagePath">The full phyiscal path of the image.</param>
        public static void DeleteThumbnail(string imagePath)
        {
            File.Delete(imagePath);
        }
        /// <summary>
        /// Creates a thumbnail for an image in a directory called Thumbnail under the original image
        /// path.
        /// </summary>
        /// <param name="imagePath">The full path to the source image.</param>
        /// <returns>The filename of the resulting thumbnail file.</returns>
        public static string CreateThumbnail(string imagePath)
        {
            using (Bitmap image = new Bitmap(imagePath))
            {
                int _width = 150;
                int _height = 150;

                int widthOrig = image.Width;
                int heightOrig = image.Height;

                // subsample factors
                float fx = widthOrig / _width;
                float fy = heightOrig / _height;

                // must fit in thumbnail size
                float f = Math.Max(fx, fy);
                if (f < 1) f = 1;

                int widthTh = (int)(widthOrig / f);
                int heightTh = (int)(heightOrig / f);

                Bitmap thumbnail = (Bitmap)image.GetThumbnailImage(widthTh, heightTh,
                    new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);

                string thumbnailDir = GetDirectory(imagePath) + Path.DirectorySeparatorChar + ThumbnailDirectory;

                if (!Directory.Exists(thumbnailDir)) Directory.CreateDirectory(thumbnailDir);

                string thumbnailPath = thumbnailDir + Path.DirectorySeparatorChar + GetFilename(imagePath);
                thumbnail.Save(thumbnailPath, ImageFormat.Jpeg);

                return GetFilename(imagePath);

            }
        }

        public static string CreateNew(string imagePath)
        {
            using (Bitmap image = new Bitmap(imagePath))
            {
                int _width = 500;
                int _height = 375;

                int widthOrig = image.Width;
                int heightOrig = image.Height;

                // subsample factors
                float fx = widthOrig / _width;
                float fy = heightOrig / _height;

                // must fit in thumbnail size
                float f = Math.Max(fx, fy);
                if (f < 1) f = 1;

                int widthTh = (int)(widthOrig / f);
                int heightTh = (int)(heightOrig / f);

                Bitmap thumbnail = (Bitmap)image.GetThumbnailImage(widthTh, heightTh,
                    new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);

                string thumbnailDir = GetDirectory(imagePath) + Path.DirectorySeparatorChar + NewDirectory;

                if (!Directory.Exists(thumbnailDir)) Directory.CreateDirectory(thumbnailDir);

                string thumbnailPath = thumbnailDir + Path.DirectorySeparatorChar + GetFilename(imagePath);
                thumbnail.Save(thumbnailPath, ImageFormat.Jpeg);

                return GetFilename(imagePath);

            }
        }

        /// <value>The name of the directory where thumbnails are stored</value>
        public static string ThumbnailDirectory => ThumbnailDir;

        public static string NewDirectory => NewDir;

        /// <summary>
        /// Callback used when creating a thumbnail.
        /// </summary>
        /// <returns>false.</returns>
        private static bool ThumbnailCallback() { return false; }

        /// <summary>
        /// Returns the filename from a file's full path. e.g. x.jgp returned from C:\Pictures\x.jpg
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <returns>The filename as a string.</returns>
        private static string GetFilename(string path)
        {

            FileInfo info = new FileInfo(path);
            return info.Name;

        }

        /// <summary>
        /// Returns the path to the directory where a file resides.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <returns>The directory as a string.</returns>
        private static string GetDirectory(string path)
        {
            FileInfo info = new FileInfo(path);
            return info.Directory?.FullName;

        }

    }
}
