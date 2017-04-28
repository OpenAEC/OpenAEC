using System;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace OpenAEC.Ribbon
{
    /// <summary>
    /// Returns the availability of an external command in Zero Doc State.
    /// </summary>
    public class Availability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication a, CategorySet b)
        {
            return true;
        }
    }

    internal class RibbonUtilities
    {
        /// <summary>
        /// Returns a new Revit PushButtonData object.
        /// </summary>
        /// <param name="buttonName">The name for the button.</param>
        /// <param name="buttonText">Text to appear in the UI, on the button.</param>
        /// <param name="className">Internal class name.</param>
        /// <param name="image32Name">Name of the resource image for large icon.</param>
        /// <param name="image16Name">Name of the resource image for small icon.</param>
        /// <param name="tooltipText">Text to appear in the UI when hovering over button.</param>
        /// <returns></returns>
        internal static PushButtonData CreatePushButton(string buttonName, string buttonText, string className, string image32Name, string image16Name, string tooltipText = "")
        {
            try
            {
                if (string.IsNullOrEmpty(buttonName)
                    || string.IsNullOrEmpty(buttonText)
                    || string.IsNullOrEmpty(className))
                {
                    throw new Exception(Properties.Resources.Error_PushbuttonInvalidArgs);
                }

                var buttonData = new PushButtonData(
                      buttonName,
                      buttonText,
                      Assembly.GetExecutingAssembly().Location,
                      className
                      );

                var img32 = Properties.Resources.ResourceManager.GetObject(image32Name) as Bitmap;
                var img16 = Properties.Resources.ResourceManager.GetObject(image16Name) as Bitmap;
                if (img32 != null)
                {
                    buttonData.LargeImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(img32.GetHbitmap(),
                        IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                if (img16 != null)
                {
                    buttonData.Image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(img16.GetHbitmap(),
                        IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                buttonData.ToolTip = tooltipText;

                return buttonData;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}