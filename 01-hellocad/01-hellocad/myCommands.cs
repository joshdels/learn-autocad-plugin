

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(_01_hellocad.MyCommands))]

namespace _01_hellocad
{
    public class MyCommands
    {
        [CommandMethod("HELLOCAD")]
        public void HelloCad() // This method can have any name
        {
            // Put your command code here
            Document doc = 
                AcadApp.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            if (doc != null)
            {
                ed = doc.Editor;
                ed.WriteMessage("Hello, this is your first command.");

            }
        }

     
    }
}
