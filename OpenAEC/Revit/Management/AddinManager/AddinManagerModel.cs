using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using OpenAEC.Revit.Utilities;

namespace OpenAEC.Revit.Management.AddinManager
{
    public static class Serialization
    {
        /// <summary>
        /// Deserializes JSON string into Collection of AddinWrappers.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        /// <returns></returns>
        public static ObservableCollection<AddinWrapper> Deserialize(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            var deserialized = JsonConvert.DeserializeObject<ObservableCollection<AddinWrapper>>(jsonString, settings);

            return deserialized;
        }

        /// <summary>
        /// Serializes Addins selections for future use.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        /// <param name="addins">Collection of AddinWrappers to be serialized.</param>
        /// <returns></returns>
        public static string Serialize(string filePath, ObservableCollection<AddinWrapper> addins)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            var jsonString = JsonConvert.SerializeObject(addins, Formatting.Indented, settings);
            File.WriteAllText(filePath, jsonString);

            return new FileInfo(filePath).Length > 0 ? filePath : string.Empty;
        }
    }

    public class AddinManagerModel
    {
        public UIApplication UiApp { get;}

        public AddinManagerModel(UIApplication app)
        {
            UiApp = app;
        }

        public void UpdateAddins(ObservableCollection<AddinWrapper> addins)
        {
            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Properties.Resources.AddinManager_SettingsFileName;
            Serialization.Serialize(filePath, addins);
        }

        /// <summary>
        /// Retrieves all of the External Command addins created in this DLL.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<AddinWrapper> LoadAddins()
        {
            var output = new ObservableCollection<AddinWrapper>();
            var dic = new Dictionary<string, AddinWrapper>();

            // (Konrad) Retrieves information about External Commands currently in the loaded DLL.
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var name = Assembly.GetExecutingAssembly().GetName().Name + ".dll";
            if (directory == null) return output;

            var path = Path.Combine(directory, name);
            var asm = Assembly.LoadFrom(path);
            if (asm == null) return output;

            var asmTypes = asm.GetTypes().Where(x => x.GetInterface("IExternalCommand") != null);
            foreach (var type in asmTypes)
            {
                var nameAttribute =
                    type.GetCustomAttributes(typeof(NameAttribute), true)
                        .FirstOrDefault() as NameAttribute;
                var descAttribute = type.GetCustomAttributes(typeof(DescriptionAttribute), true)
                    .FirstOrDefault() as DescriptionAttribute;
                var imageAttribute = type.GetCustomAttributes(typeof(ImageAttribute), true)
                    .FirstOrDefault() as ImageAttribute;
                var buttonTextAttribute = type.GetCustomAttributes(typeof(ButtonTextAttribute), true)
                    .FirstOrDefault() as ButtonTextAttribute;
                var panelNameAttribute = type.GetCustomAttributes(typeof(PanelNameAttribute), true)
                    .FirstOrDefault() as PanelNameAttribute;
                if (nameAttribute == null 
                    || descAttribute == null 
                    || imageAttribute == null
                    || buttonTextAttribute == null
                    || panelNameAttribute == null) continue;

                var aw = new AddinWrapper
                {
                    Name = nameAttribute.Name,
                    Description = descAttribute.Description,
                    Image = imageAttribute.Image,
                    ImageName = imageAttribute.ImageName, // (Konrad) Image name is assigned from Image attribute so order matters here.
                    Panel = panelNameAttribute.PanelName,
                    ButtonText = buttonTextAttribute.ButtonText,
                    FullName = type.FullName
                };
                dic.Add(aw.Name, aw);
            }

            // (Konrad) Deserializes previously stored information and updates the "Install" flag.
            var filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Properties.Resources.AddinManager_SettingsFileName;
            var storedAddins = File.Exists(filePath) ? Serialization.Deserialize(filePath) : null;
            if (storedAddins != null)
            {
                foreach (var addin in storedAddins)
                {
                    if (dic.ContainsKey(addin.Name))
                    {
                        dic[addin.Name].Install = addin.Install;
                    }
                }
            }
            
            output = new ObservableCollection<AddinWrapper>(dic.Values.ToList().OrderBy(x => x.Name));
            return output;
        }
    }
}