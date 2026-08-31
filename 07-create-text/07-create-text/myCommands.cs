

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(_07_create_text.MyCommands))]
[assembly: ExtensionApplication(typeof(_07_create_text.PluginExtension))]

namespace _07_create_text
{
    public class MyCommands
    {
        [CommandMethod("ParcelText")]
        public void CreateParcelText()
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            //Point
            PromptPointResult pointResult =
                ed.GetPoint("\nPick location for parcel text: ");

            if (pointResult.Status != PromptStatus.OK)
            { 
                return; 
            }

            Point3d location = pointResult.Value;

            // Pin
            PromptStringOptions pinOptions =
                new PromptStringOptions("\nEnter PIN: ");

            pinOptions.AllowSpaces = true;

            PromptResult pinResult = ed.GetString(pinOptions);

            if (pinResult.Status != PromptStatus.OK)
            {
                return;
            }

            // Lot
            PromptStringOptions lotOptions =
                new PromptStringOptions("\nEnter Lot Number: ");

            lotOptions.AllowSpaces = true;

            PromptResult lotResult = ed.GetString(lotOptions);

            if (lotResult.Status != PromptStatus.OK)
            {
                return;
            }

            // Owner
            PromptStringOptions ownerOptions =
                new PromptStringOptions("\nEnter Owner:");

            ownerOptions.AllowSpaces = true;

            PromptResult ownerResult = ed.GetString(ownerOptions);

            if (ownerResult.Status != PromptStatus.OK)
            {
                return;
            }

            // Area 
            PromptStringOptions areaOptions =
                new PromptStringOptions("\nEnter Area: ");

            areaOptions.AllowSpaces = true;

            PromptResult areaResult = ed.GetString(areaOptions);

            if (areaResult.Status != PromptStatus.OK)
            {
                return;
            }

            string parcelText =
                $"PIN: {pinResult.StringResult}\\P" +
                $"LOT: {lotResult.StringResult}\\P" +
                $"OWNER: {ownerResult.StringResult}\\P" +
                $"AREA: {areaResult.StringResult}";

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable? bt =
                    tr.GetObject(
                        db.BlockTableId,
                        OpenMode.ForRead
                    ) as BlockTable;

                BlockTableRecord? modelSpace =
                    tr.GetObject(
                        bt[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite
                    ) as BlockTableRecord;

                MText text = new MText();

                text.Contents = parcelText;
                text.Location = location;
                text.Height = 1;
                text.Attachment = AttachmentPoint.MiddleLeft;

                modelSpace?.AppendEntity(text);

                tr.AddNewlyCreatedDBObject(text, true);
                tr.Commit();
            }
            ed.WriteMessage("\nParcel text created");
        }
    }
}
