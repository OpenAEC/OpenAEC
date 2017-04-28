using Autodesk.Revit.UI;
using OpenAEC.Ribbon;

namespace OpenAEC
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // create ribbon
            Panel.Initialise(application);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
