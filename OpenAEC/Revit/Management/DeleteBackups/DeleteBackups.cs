using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OpenAEC.Revit.Utilities;
using OpenAEC.Utilities;

namespace OpenAEC.Revit.Management.DeleteBackups
{
    [Name(nameof(Properties.Resources.DeleteBackups_Name), typeof(Properties.Resources))]
    [Description(nameof(Properties.Resources.DeleteBackups_Description), typeof(Properties.Resources))]
    [Image(nameof(Properties.Resources.deleteBackups32), typeof(Properties.Resources))]
    [PanelName(nameof(Properties.Resources.Ribbon_TabNameManagement), typeof(Properties.Resources))]
    [ButtonText(nameof(Properties.Resources.DeleteBackups_ButtonText), typeof(Properties.Resources))]
    [Transaction(TransactionMode.Manual)]
    public class DeleteBackups : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            var uiApp = commandData.Application;
            var doc = uiApp.ActiveUIDocument.Document;

            // Obtain user confirmation
            var confirmation = new TaskDialog(Properties.Resources.DeleteBackups_MessageTitle)
            {
                MainIcon = TaskDialogIcon.TaskDialogIconWarning,
                MainInstruction = Properties.Resources.DeleteBackups_MainInstruction
            };
            confirmation.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, 
                Properties.Resources.DeleteBackups_YesDelete, 
                Properties.Resources.DeleteBackups_InstructionDescription);
            confirmation.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, 
                Properties.Resources.DeleteBackups_NoDontDelete);

            // Cancel if user hasn't explicitly clicked YES.
            if (confirmation.Show() != TaskDialogResult.CommandLink1) return Result.Cancelled;

            try
            {
                FileInfo file;

                if (doc.PathName == string.Empty)
                {
                    throw new Exception(Properties.Resources.DeleteBackups_FileNotSavedException);
                }
                // Determine which document location to use.
                if (doc.IsWorkshared)
                {
                    var modelPathName = ModelPathUtils.ConvertModelPathToUserVisiblePath(doc.GetWorksharingCentralModelPath());
                    file = new FileInfo(modelPathName);
                }
                else file = new FileInfo(doc.PathName);

                // Get all files from parent directory that match a regex pattern.
                var parentDir = file.Directory?.Parent;
                const string regexPattern = @".*\.\d{4}\.(rfa|rvt)";
                if (parentDir == null) return Result.Succeeded;
                var files = Directory.GetFiles(parentDir.FullName, "*.rfa", SearchOption.AllDirectories)
                    .Where(x => Regex.IsMatch(x, regexPattern)).ToList();

                var familyCount = 0;
                var projectCount = 0;
                var n = files.Count;
                var s = "{0} of " + n + " Files processed...";
                string pfCaption = Properties.Resources.DeleteBackups_ProgressFormTitle;
                using (var pf = new ProgressForm(pfCaption, s, n))
                {
                    pf.Show();
                    foreach (var f in files)
                    {
                        try
                        {
                            File.Delete(f);
                        }
                        catch
                        {
                            // ignored
                        }
                        pf.Increment();
                        if (f.Contains(".rvt")) projectCount++;
                        if (f.Contains(".rfa")) familyCount++;
                    }
                }
                TaskDialog.Show(Properties.Resources.DeleteBackups_MessageTitle, 
                    string.Format("You have sucessfully deleted :" +
                                  Environment.NewLine +
                                  familyCount + " family backup files." + 
                                  Environment.NewLine +
                                  projectCount + " project backup files"));
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




