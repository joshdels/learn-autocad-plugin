using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(_03_write_objects.MyCommands))]
[assembly: ExtensionApplication(typeof(_03_write_objects.PluginExtension))]

namespace _03_write_objects
{
    public class MyCommands
    {

        [CommandMethod("LineAdd")]
        public void LineAddCommand()
        {
            Database db = HostApplicationServices.WorkingDatabase;

            using (Transaction tr =
                db.TransactionManager.StartTransaction())
            {
                BlockTable bt =
                    (BlockTable)tr.GetObject(
                        db.BlockTableId,
                        OpenMode.ForRead);

                BlockTableRecord ms =
                    (BlockTableRecord)tr.GetObject(
                        bt[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite);

                Line line = new Line(
                    new Point3d(-10, 0, 0),
                    new Point3d(10, 0, 0));

                ms.AppendEntity(line);
                tr.AddNewlyCreatedDBObject(line, true);

                tr.Commit();
            }
        }


        [CommandMethod("CircleAdd")]
        public void CircleAddCommand()
        {
            Database db = HostApplicationServices.WorkingDatabase;

            using (Transaction tr =
                db.TransactionManager.StartTransaction())
            {
                BlockTable bt =
                    (BlockTable)tr.GetObject(
                        db.BlockTableId,
                        OpenMode.ForRead);

                BlockTableRecord ms =
                    (BlockTableRecord)tr.GetObject(
                        bt[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite);

                Circle circle = new Circle(
                    new Point3d(50, 50, 0),
                    Vector3d.ZAxis,
                    5);

                ms.AppendEntity(circle);

                tr.AddNewlyCreatedDBObject(
                    circle,
                    true);

                tr.Commit();
            }
        }


        [CommandMethod("DrawPolyline")]
        public void DrawPolyline()
        {
            Document doc =
                AcadApp.DocumentManager.MdiActiveDocument;

            doc.SendStringToExecute(
                "_.PLINE ",
                true,
                false,
                false);
        }

        [CommandMethod("ClearAll")]
        public void ClearAll()
        {
            Document? doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = 
                    (BlockTable)tr.GetObject(
                        db.BlockTableId, 
                        OpenMode.ForRead);

                BlockTableRecord modelSpace =
                (BlockTableRecord)tr.GetObject(
                    bt[BlockTableRecord.ModelSpace],
                    OpenMode.ForWrite);

                foreach (ObjectId id in modelSpace)
                {
                    Entity entity =
                        tr.GetObject(id, OpenMode.ForWrite) as Entity;

                    if (entity != null)
                    {
                        entity.Erase();
                    }
                }

                tr.Commit();
            }

            ed.WriteMessage("\nAll modeel-space entities removed.");
        }

        [CommandMethod("ColorLines")]
        public void ColorLines()
        {
            Document? doc = AcadApp.DocumentManager.MdiActiveDocument;

            Editor ed = doc.Editor;
            Database db = doc.Database;

            if (doc == null)
            {
                return;
            }

            PromptSelectionOptions options =
                new PromptSelectionOptions();

            options.MessageForAdding =
                "\nSelect lines to color red: ";

            PromptSelectionResult result =
                ed.GetSelection(options);

            if (result.Status != PromptStatus.OK)
            {
                return;
            }

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (SelectedObject selectedObject in result.Value)
                {
                    if (selectedObject == null )
                    {
                        continue; 
                    }

                    Entity entity =
                        tr.GetObject(
                            selectedObject.ObjectId, 
                            OpenMode.ForWrite) as Entity;

                    if (entity is Line line)
                    {
                        line.ColorIndex = 1;
                    }
                }

                tr.Commit();
            }

            ed.WriteMessage(
                "\nSelected liens have been colored red");
        }

        [CommandMethod("ColorEntites")]
        public void ColorEntities()
        {
            Document? doc = AcadApp.DocumentManager.MdiActiveDocument;

            Editor ed = doc.Editor;
            Database db = doc.Database;

            if (doc == null)
            {
                return;
            }

            PromptSelectionOptions options =
                new PromptSelectionOptions();

            options.MessageForAdding =
                "\nSelect entities to color red: ";

            PromptSelectionResult result =
                ed.GetSelection(options);

            if (result.Status != PromptStatus.OK)
            {
                return;
            }

            int count = 0;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (SelectedObject selectedObject in result.Value)
                {
                    if (selectedObject == null)
                    {
                        continue;
                    }

                    Entity? entity =
                        tr.GetObject(
                            selectedObject.ObjectId,
                            OpenMode.ForWrite) as Entity;



                    if (entity == null)
                    {
                        continue;
                    }

                    entity.ColorIndex = 1;

                    count++;
                }

                tr.Commit();
            }

            ed.WriteMessage(
                 $"\nColored {count} entities red.");
        }
    
    }
}