using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(_01_hellocad.HelloCommand))]

namespace _01_hellocad
{
    public class HelloCommand
    {

        [CommandMethod("HELLOCAD")]
        public void HelloCad()
        {
            // Get the currently active AutoCAD drawing.
            Document? doc =
                AcadApp.DocumentManager.MdiActiveDocument;

            // Make sure a drawing is actually open.
            if (doc == null)
                return;

            // Get AutoCAD's command-line interface.
            Editor ed = doc.Editor;

            // Write a message to the AutoCAD command line.
            ed.WriteMessage(
                "\nHello, this is your first command."
            );
        }


        [CommandMethod("CALLME")]
        public void CallMe()
        {
            // Get the currently active AutoCAD drawing.
            Document? doc =
                AcadApp.DocumentManager.MdiActiveDocument;

            // Make sure a drawing is actually open.
            if (doc == null)
                return;

            // Get the command-line interface.
            Editor ed = doc.Editor;

            // Write a message to AutoCAD.
            ed.WriteMessage(
                "\nJoshua is here wahhahaha."
            );
        }


        [CommandMethod("READALLENTITIES")]
        public void ReadAllEntities()
        {

            Document? doc =
                AcadApp.DocumentManager.MdiActiveDocument;

            if (doc == null)
                return;


            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr =
                   db.TransactionManager.StartTransaction())
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


                int count = 0;

                foreach (ObjectId entId in btr)
                {
                    Entity? ent =
                        tr.GetObject(
                            entId,
                            OpenMode.ForRead
                        ) as Entity;

                    if (ent == null)
                        continue;


                    count++;

                    ed.WriteMessage(
                        $"\nFound entity type: {ent.GetType().Name}"
                    );
                }

                ed.WriteMessage($"\nTotal entities: {count}");

                tr.Commit();
            }
        }

        [CommandMethod("LISTENTITIES")]
        public static void ListEntities()
        {
            Document acDoc =
                AcadApp.DocumentManager.MdiActiveDocument;

            Database acCurDb =
                acDoc.Database;

            using (Transaction acTrans =
                   acCurDb.TransactionManager.StartTransaction())
            {
                // Get the Block Table
                BlockTable acBlkTbl =
                    acTrans.GetObject(
                        acCurDb.BlockTableId,
                        OpenMode.ForRead
                    ) as BlockTable;

                // Get Model Space
                BlockTableRecord acBlkTblRec =
                    acTrans.GetObject(
                        acBlkTbl[BlockTableRecord.ModelSpace],
                        OpenMode.ForRead
                    ) as BlockTableRecord;

                int nCnt = 0;

                acDoc.Editor.WriteMessage(
                    "\nModel space objects:"
                );

                // Loop through objects in Model Space
                foreach (ObjectId acObjId in acBlkTblRec)
                {
                    acDoc.Editor.WriteMessage(
                        "\n" + acObjId.ObjectClass.DxfName
                    );

                    nCnt = nCnt + 1;
                }

                if (nCnt == 0)
                {
                    acDoc.Editor.WriteMessage(
                        "\nNo objects found"
                    );
                }

                acTrans.Commit();
            }
        }
    }
}

