using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(_04_write_read_combo.MyCommands))]
[assembly: ExtensionApplication(typeof(_04_write_read_combo.PluginExtension))]

namespace _04_write_read_combo
{
    public class MyCommands
    {
        [CommandMethod("DetectLines")]
        public void DetectLines()
        {
            Document? doc =
                AcadApp.DocumentManager.MdiActiveDocument;

            if (doc == null)
            {
                return;
            }

            Editor ed = doc.Editor;
            Database db = doc.Database;

            int count = 0;

            using (Transaction tr =
                db.TransactionManager.StartTransaction())
            {
                BlockTable bt =
                    (BlockTable)tr.GetObject(
                        db.BlockTableId,
                        OpenMode.ForRead);

                BlockTableRecord btr =
                    (BlockTableRecord)tr.GetObject(
                        bt[BlockTableRecord.ModelSpace],
                        OpenMode.ForRead);

                foreach (ObjectId id in btr)
                {
                    Entity? entity =
                        tr.GetObject(
                            id,
                            OpenMode.ForWrite) as Entity;

                    if (entity is Line line)
                    {
                        line.ColorIndex = 1;
                        count++;
                    }
                }

                tr.Commit();
            }

            // Only report after checking everything
            if (count == 0)
            {
                ed.WriteMessage(
                    "\nNo lines found. You're good to go!");
            }
            else
            {
                ed.WriteMessage(
                    $"\nFound {count} line(s). All lines have been colored red.");
            }
        }
    }
}