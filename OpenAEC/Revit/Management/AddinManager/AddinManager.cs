using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace OpenAEC.Revit.Management.AddinManager
{
    // (Konrad) Deliberately skipping class attributes here to avoid having this command,
    // be recognized when AddinManager parses the source DLL for addins. This plug-in's
    // button and tab will be created manually on startup.
    [Transaction(TransactionMode.Manual)]
    public class AddinManager : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            var uiApp = commandData.Application;

            try
            {
                var model = new Wpf.ViewModel.MainViewModel
                {
                    CurrentPageViewModel = new AddinManagerViewModel(uiApp),
                    WindowTitle = "Addin Manager"
                };

                var view = new Wpf.View.MainWindow
                {
                    DataContext = model,
                    Height = 800,
                    MinHeight = 800,
                    Width = 800,
                    MinWidth = 800
                };

                view.ShowDialog();

                return Result.Succeeded;
            }
            catch (Exception x)
            {
                message = x.Message;
                return Result.Failed;
            }
        }
    }
}