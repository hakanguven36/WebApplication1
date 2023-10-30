using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Newtonsoft.Json;
using LabelSeperator.Models;
using System.Collections.Generic;

namespace LabelSeperator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pathJson = "label.json";
                string pathHam = "Ham";
                
                DirectoryInfo hamFolderDI = new DirectoryInfo(pathHam);
                if (!hamFolderDI.Exists)
                    throw new Exception("Error: 'Ham' folder couldn't found!");

                if(hamFolderDI.GetFiles().Length == 0)
                    throw new Exception("Error: 'Ham' folder was empty!");

                FileInfo jsonFI = new FileInfo(pathJson);
                if (!jsonFI.Exists)
                    throw new Exception("Error: Json file couldn't found!");

                string jsonFile = File.ReadAllText(jsonFI.FullName);
                if (string.IsNullOrWhiteSpace(jsonFile))
                    throw new Exception("Error: Json dosyası boş!");

                Console.WriteLine("Please, write square's one side length:");
                string sizeStr = Console.ReadLine();
                int sizeInt = int.Parse(sizeStr);
                Size size = new Size(sizeInt, sizeInt);

                /*--------------------*/

                Project project = JsonConvert.DeserializeObject<Project>(jsonFile);
                Console.WriteLine("Şu proje işleniyor:", project.projectName);

                List<string> annoList = project.annoList;
                if (annoList.Count == 0)
                    throw new Exception("Error: annotations doesn't exist in json file!");

                foreach (Photo photo in project.photos)
                {

                    if (photo.photoLabels.Count == 0)
                        continue;

                    foreach (Label label in photo.photoLabels)
                    {
                        string annoName = annoList[label.annoID];
                        DirectoryInfo di = Directory.CreateDirectory(annoName);

                        Models.Rectangle rect = BBox(label.points);

                        Image image = new Bitmap(photo.photoName);

                        image = CropAndResize(image, rect, size);

                        image.Save(Path.Combine(di.FullName, GetUniqueFileName("")));
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to exit...");
            }

            Console.WriteLine("Program finished successfully.");
            Console.Read();
        }

        private static Image CropAndResize(Image image, Models.Rectangle rect, Size size)
        {
            return ResizeImage(CropImage(image, rect), size);
        }

        private static Models.Rectangle BBox(List<Models.Point> points)
        {
            Models.Rectangle rect = new Models.Rectangle();

            if (points.Count == 0)
                return rect;

            rect.beginx = int.MaxValue;
            rect.beginy = int.MaxValue;
            rect.endx = 0;
            rect.endy = 0;

            foreach (var p in points)
            {
                int px_int = (int)p.x;
                int py_int = (int)p.y;
                rect.beginx = rect.beginx < px_int ? px_int : rect.beginx;
                rect.beginy = rect.beginy < py_int ? py_int : rect.beginy;
                rect.endx = rect.endx > px_int ? px_int : rect.endx;
                rect.endy = rect.endy > py_int ? py_int : rect.endy;
            }

            return rect;
        }

        private static Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            // Calculate width and height with new desired size
            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            nPercent = Math.Min(nPercentW, nPercentH);
            // New Width and Height
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.Bicubic;
            // Draw image with new width and height
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);

            #region Orientation Fix
            // Fix orientation if needed.
            /*
            if (imgToResize.PropertyIdList.Contains(OrientationKey))
            {
                var orientation = (int)imgToResize.GetPropertyItem(OrientationKey).Value[0];
                switch (orientation)
                {
                    case NotSpecified: // Assume it is good.
                    case NormalOrientation:
                        // No rotation required.
                        break;
                    case MirrorHorizontal:
                        b.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case UpsideDown:
                        b.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case MirrorVertical:
                        b.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case MirrorHorizontalAndRotateRight:
                        b.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case RotateLeft:
                        b.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case MirorHorizontalAndRotateLeft:
                        b.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case RotateRight:
                        b.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        throw new NotImplementedException("An orientation of " + orientation + " isn't implemented.");
                }
            }
            */
            #endregion

            g.Dispose();
            return (Image)b;
        }

        private static Image CropImage(Image img, Models.Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            System.Drawing.Rectangle cropping = new System.Drawing.Rectangle(cropArea.beginx, cropArea.beginy, cropArea.endx - cropArea.beginx, cropArea.endy - cropArea.endy);
            return bmpImage.Clone(cropping, bmpImage.PixelFormat);
        }

        private static string GetUniqueFileName(string fileName)
        {
            return Guid.NewGuid().ToString() + Path.GetExtension(fileName);
        }
    }
}
