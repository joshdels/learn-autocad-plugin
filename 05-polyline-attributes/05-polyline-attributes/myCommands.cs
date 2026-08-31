using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(_05_polyline_attributes.MyCommands))]
[assembly: ExtensionApplication(typeof(_05_polyline_attributes.PluginExtension))]

namespace _05_polyline_attributes
{
    public class MyCommands
    {
        [CommandMethod("AnalyzePolyline")]
        public void AnalyzePolyline()
        {
            Document? doc =
                AcadApp.DocumentManager.MdiActiveDocument;

            if (doc == null)
            {
                return;
            }

            Editor ed = doc.Editor;
            Database db = doc.Database;

            PromptSelectionOptions options =
                new PromptSelectionOptions();

            options.MessageForAdding =
                "\nSelect a polyline";

            PromptSelectionResult result =
                ed.GetSelection(options);

            if (result.Status != PromptStatus.OK)
            {
                return;
            }

            using (Transaction tr =
                db.TransactionManager.StartTransaction())
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
                            OpenMode.ForRead) as Entity;

                    if (entity is Polyline polyline)
                    {
                        ed.WriteMessage("\n--- Parcel ---");

                        // Check if closed
                        ed.WriteMessage(
                            $"\nClosed: {polyline.Closed}");

                        // Number of vertices
                        int vertexCount =
                            polyline.NumberOfVertices;

                        ed.WriteMessage(
                            $"\nVertices: {vertexCount}");

                        // Perimeter
                        ed.WriteMessage(
                            $"\nPerimeter: {polyline.Length}");

                        // Area
                        if (polyline.Closed)
                        {
                            ed.WriteMessage(
                                $"\nArea: {polyline.Area}");
                        }
                        else
                        {
                            ed.WriteMessage(
                                "\nArea: Cannot calculate - polyline is open.");
                        }

                        // Vertex coordinates
                        ed.WriteMessage("\nVertex Coordinates:");

                        for (int i = 0; i < vertexCount; i++)
                        {
                            Point3d point =
                                polyline.GetPoint3dAt(i);

                            ed.WriteMessage(
                                $"\nVertex {i}: " +
                                $"X={point.X}, " +
                                $"Y={point.Y}, " +
                                $"Z={point.Z}");
                        }
                    }
                }

                tr.Commit();
            }
        }
    }
}