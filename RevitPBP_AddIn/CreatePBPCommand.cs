using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitPBP_AddIn
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CreatePBPCommand : IExternalCommand
    {
        // Statische Felder für Dokument und Ereignishandler über Formulare hinweg
        public static Document Doc;
        public static PBPExternalEventHandler Handler;
        public static ExternalEvent ExtEvent;

        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // 1. Anfangszustand für Projekt- und Vermessungspunkte speichern (für mögliches Zurücksetzen)
            PBPInitialState.SaveInitialState(doc);

            // 2. Dokument für formularübergreifenden Zugriff speichern
            Doc = doc;

            // 3. Handler und externes Ereignis erstellen (nur einmal pro Sitzung)
            if (Handler == null)
                Handler = new PBPExternalEventHandler(uidoc);
            if (ExtEvent == null)
                ExtEvent = ExternalEvent.Create(Handler);

            // 4. Hauptmenü-Formular anzeigen (SPdigital_tools)
            SPdigital_tools mainMenu = new SPdigital_tools();
            mainMenu.Show();

            // 5. Immer „Succeeded“ zurückgeben — Befehl bleibt aktiv, bis das Formular geschlossen wird
            return Result.Succeeded;
        }
    }
}
