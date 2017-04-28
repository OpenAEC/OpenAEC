using System.IO;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.UI;

namespace OpenAEC.Ribbon
{
    public class Panel
    {
        public static void Initialise(UIControlledApplication app)
        {
            // (Konrad) By default we only create OpenAEC Tab and Management panel.
            // All other panels and buttons will be created from AddinManager file.
            app.CreateRibbonTab(Properties.Resources.Ribbon_TabName);
            var panelManagement = app.CreateRibbonPanel(Properties.Resources.Ribbon_TabName, Properties.Resources.Ribbon_TabNameManagement);

            var pbAddinManager = RibbonUtilities.CreatePushButton(
                Properties.Resources.AddinManager_Name,
                Properties.Resources.AddinManager_ButtonText,
                "OpenAEC.Revit.Management.AddinManager.AddinManager",
                nameof(Properties.Resources.addinManager32),
                nameof(Properties.Resources.addinManager16),
                Properties.Resources.AddinManager_Description);
            pbAddinManager.AvailabilityClassName = Properties.Resources.Ribbon_ZeroDocAvailability;

            panelManagement.AddItem(pbAddinManager);

            // (Konrad) We are checking for AddinManager Settings file to dynamically create
            // remaining Panels/Buttons as needed.
            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                           Properties.Resources.AddinManager_SettingsFileName;
            var storedAddins = File.Exists(filePath)
                ? Revit.Management.AddinManager.Serialization.Deserialize(filePath)
                : null;
            if (storedAddins == null) return;
            foreach (var addin in storedAddins)
            {
                if (!addin.Install) continue;
                try
                {
                    var panel = app.GetRibbonPanels(Properties.Resources.Ribbon_TabName)
                                    .FirstOrDefault(x => x.Name == addin.Panel) ??
                                app.CreateRibbonPanel(Properties.Resources.Ribbon_TabName, addin.Panel);

                    var pb = RibbonUtilities.CreatePushButton(
                        addin.Name,
                        addin.ButtonText,
                        addin.FullName,
                        addin.ImageName,
                        addin.Description);

                    panel.AddItem(pb);
                }
                catch
                {
                    //ignored
                }
            }
        }
    }
}
