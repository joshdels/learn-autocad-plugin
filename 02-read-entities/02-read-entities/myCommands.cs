

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(_02_read_entities.MyCommands))]

namespace _02_read_entities
{
    public class MyCommands
    {
        [CommandMethod("ReadPolylineCoordinates")]
        public void ReadPolylineCoordinates()
        {
            Document? doc = AcadApp.DocumentManager.MdiActiveDocument;


            if (doc == null)
            {
                return;
            }

            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt =
                    (BlockTable)tr.GetObject(
                        db.BlockTableId,
                        OpenMode.ForRead
                    );

                BlockTableRecord btr =
                (BlockTableRecord)tr.GetObject(
                    bt[BlockTableRecord.ModelSpace],
                    OpenMode.ForRead
                );

                foreach (ObjectId id in btr)
                {
                    Entity? entity = tr.GetObject(id, OpenMode.ForRead) as Entity;

                    if (entity is Polyline polyline)
                    {
                        ed.WriteMessage(
                            $"\nPolyline found: {polyline.NumberOfVertices} vertices"
                        );

                        for (int i = 0; i < polyline.NumberOfVertices; i++)
                        {
                            Point2d point = polyline.GetPoint2dAt(i);

                            ed.WriteMessage(
                                $"\n Vertex {i + 1}: X={point.X}, Y={point.X}"
                            );
                        }
                    }

                }
                tr.Commit();
            }

        }

        [CommandMethod("CirlceStatus")]
        public void CircleStatus()
        {
            Document? doc = AcadApp.DocumentManager.MdiActiveDocument; // get active layer

            if (doc == null)
            {
                return;
            }

            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt =
                    (BlockTable)tr.GetObject(
                        db.BlockTableId,
                        OpenMode.ForRead
                    );

                BlockTableRecord btr =
                (BlockTableRecord)tr.GetObject(
                    bt[BlockTableRecord.ModelSpace],
                    OpenMode.ForRead
                );

                foreach (ObjectId id in btr)
                {
                    Entity? entity = tr.GetObject(id, OpenMode.ForRead) as Entity;

                    if (entity is Circle circle)
                    {
                        Point3d center = circle.Center;
                        double radius = circle.Radius;

                        ed.WriteMessage(
                            $"\nCircle found!" +
                            $"\n  Center: X={center.X}, Y={center.Y}, Z={center.Z}" +
                            $"\n  Radius: {radius}"
                        );
                    }
                }
                tr.Commit();
            }
        }
    }
}
