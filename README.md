# OpenAEC
### Open Source initiative to collaborate on Revit tools for the sake of saving AEC Industry. :-P Hopefully we can all spend less time re-inventing the wheel, and instead just pull all these tools into a single place, where we can manage and build them together. 

<p align="center">
<img src="https://github.com/design-technology/OpenAEC/blob/master/_Graphics/Logo/FullLogo.png" align="center"></img>
</p>

## Rules
* Please make sure to use the `Resources` file. Let's not leave any user facing strings hard coded. Maybe in the future we can accomodate localization and that will make it super easy. 
* In order to get your plug-in displayed on the Ribbon, please add `Class Attributes` to your `ExternalCommand` per this post: [AddinManager](https://github.com/design-technology/OpenAEC/wiki/Addin-Manager)
* When commenting please be succinct and leave your name behind. Don't tell me what your code is doing. Tell me why you are doing certain things if they are out of the ordinary. Ex: 
```
    // (Konrad) Deliberately skipping class attributes here to avoid having this command,
    // be recognized when AddinManager parses the source DLL for addins. This plug-in's
    // button and tab will be created manually on startup.
```
