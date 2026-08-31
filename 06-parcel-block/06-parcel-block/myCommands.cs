using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(_06_parcel_block.MyCommands))]
[assembly: ExtensionApplication(typeof(_06_parcel_block.PluginExtension))]

namespace _06_parcel_block
{
   public class MyCommands
    {
        [CommandMethod("CreateBlock")]
        public void CreateBock()
        {
            Document? docs = AcadApp.DocumentManager.MdiActiveDocument;

            Database db = docs.Database;
            Editor ed = docs.Editor;

            if (docs == null)
            {
                return;
            }

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable blockTable =
                    (BlockTable)tr.GetObject(
                        db.BlockTableId,
                        OpenMode.ForRead);

                if (blockTable.Has("PARCEL"))
                {
                    ed.WriteMessage(
                        "\nParcel block already exists");
                }

                blockTable.UpgradeOpen();

                BlockTableRecord blockDefinition =
                    new BlockTableRecord();

                blockDefinition.Name = "PARCEL";

                blockTable.Add(blockDefinition);

                tr.AddNewlyCreatedDBObject(
                    blockDefinition,
                    true
                );

                tr.Commit();

                ed.WriteMessage("\nParcel block Created");
             }            
        }

        [CommandMethod("InsertBlock")]
        public void InsertBlock()
        {
            Document? docs = AcadApp.DocumentManager.MdiActiveDocument;

            Database db = docs.Database;
            Editor ed = docs.Editor;

            if (docs  == null)
            {
                return;
            }

            PromptPointResult pointResult =
                ed.GetPoint("\nSpeccify insert point: ");

            if (pointResult.Status != PromptStatus.OK)
            {
                return;
            }

            Point3d insertionPoint = pointResult.Value;

            using(Transaction tr = db.TransactionManager.StartTransaction() )
            {
                BlockTable blockTable =
                    (BlockTable)tr.GetObject(
                        db.BlockTableId, 
                        OpenMode.ForRead
                     );

                if (!blockTable.Has("PARCEL"))
                {
                    ed.WriteMessage(
                        "\nPARCEL block does not exist.");

                    return;
                }

                ObjectId blockId = 
                    blockTable["PARCEL"];

                BlockTableRecord currentSpace =
                    (BlockTableRecord)tr.GetObject(
                        db.CurrentSpaceId,
                        OpenMode.ForWrite
                    );

                BlockReference blockReference =
                    new BlockReference(
                        insertionPoint,
                        blockId);

                currentSpace.AppendEntity(
                    blockReference);

                tr.AddNewlyCreatedDBObject(
                    blockReference,
                    true);

                tr.Commit();

                ed.WriteMessage(
                    "\nPARCEL block inserted!");
            }

        }
    }
}
