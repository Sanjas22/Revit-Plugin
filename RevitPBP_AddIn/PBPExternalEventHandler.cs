using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace RevitPBP_AddIn
{
    public class PBPCommandData
    {
        public enum PBPAction
        {
            None,
            CreatePBP,
            ResetAll
        }
        public PBPAction Action { get; set; } = PBPAction.None;

        // PBP erstellen:
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        // public double Lage { get; set; }
        public double Lage { get; set; } = 7837;
        // public double Hoehe { get; set; }
        public double Hoehe { get; set; } = 5684;
        public string FamilyPath { get; set; }
        public bool UseStandardFamily { get; set; } = true;
        public string PBPName { get; set; }
    }

    public class PBPExternalEventHandler : IExternalEventHandler
    {
        private readonly UIDocument _uidoc;
        private readonly Document _doc;

        public PBPCommandData CommandData { get; set; } = new PBPCommandData();

        // --- Zur Nachverfolgung der ID der zuletzt geladenen Familie
        private ElementId _lastLoadedFamilyId = ElementId.InvalidElementId;

        public PBPExternalEventHandler(UIDocument uidoc)
        {
            _uidoc = uidoc;
            _doc = uidoc.Document;
        }

        public string GetName() => "PBPExternalEventHandler";

        public void Execute(UIApplication app)
        {
            try
            {
                switch (CommandData.Action)
                {
                    case PBPCommandData.PBPAction.CreatePBP:
                        CreatePBP();
                        break;
                    case PBPCommandData.PBPAction.ResetAll:
                        ResetAll();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Fehler", ex.ToString());
            }
            finally
            {
                // Nach der Ausführung Action zurücksetzen
                CommandData.Action = PBPCommandData.PBPAction.None;
            }
        }

        // ==== Methoden zur Befehlsübertragung ====
        public void SetCreatePBPCommand(
            double x, double y, double z, double lage, double hoehe,
            bool useStandardFamily, string familyPath, string pbpName)
        {
            CommandData.Action = PBPCommandData.PBPAction.CreatePBP;
            CommandData.X = x;
            CommandData.Y = y;
            CommandData.Z = z;
            // CommandData.Lage = lage;
            // CommandData.Hoehe = hoehe;
            CommandData.Lage = (lage > 0) ? lage : CommandData.Lage;
            CommandData.Hoehe = (hoehe > 0) ? hoehe : CommandData.Hoehe;
            CommandData.UseStandardFamily = useStandardFamily;
            CommandData.FamilyPath = familyPath;
            CommandData.PBPName = pbpName;
        }

        public void SetResetAllCommand()
        {
            CommandData.Action = PBPCommandData.PBPAction.ResetAll;
        }

        // ==== Aktionen ====

        private void CreatePBP()
        {
            double factor = 3.280839895013123;
            XYZ insertionPoint = new XYZ(
                CommandData.X * factor,
                CommandData.Y * factor,
                CommandData.Z * factor);

            // 1. Projektstandort verschieben (Move project location)
            ShiftProjectOrigin(_doc, insertionPoint);

            // 2. (entfernt) Verschiebung des Vermessungspunkts (Survey Point) über Parameter — entfernt, um „doppelte“ Koordinaten zu vermeiden.
            // Hinweis: Dieser Block existiert in der alten Code-Version; bei Bedarf dort nachsehen bzw. wiederverwenden.

            // 3. Familie laden (Load family)
            string familyPath = CommandData.UseStandardFamily || string.IsNullOrEmpty(CommandData.FamilyPath)
                ? @"C:blabla\blabla"
                : CommandData.FamilyPath;
            Family family = null;

            using (Transaction trans = new Transaction(_doc, "Familie laden"))
            {
                trans.Start();
                try
                {
                    _doc.LoadFamily(familyPath, out family);
                    if (family != null)
                        _lastLoadedFamilyId = family.Id;
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Familie laden", $"Fehler beim Laden der Familie:\n{ex.Message}");
                }
                trans.Commit();
            }

            if (family == null)
            {
                TaskDialog.Show("Familie laden", "Die Familie konnte nicht geladen werden. Vorgang abgebrochen.");
                return;
            }

            FamilySymbol familySymbol = null;
            foreach (ElementId id in family.GetFamilySymbolIds())
            {
                familySymbol = _doc.GetElement(id) as FamilySymbol;
                if (familySymbol != null) break;
            }

            if (familySymbol == null)
            {
                TaskDialog.Show("Familie", "Kein FamilySymbol in der geladenen Familie gefunden. Vorgang abgebrochen.");
                return;
            }

            if (!familySymbol.IsActive)
            {
                using (Transaction t = new Transaction(_doc, "Familien-Typ aktivieren"))
                {
                    t.Start();
                    familySymbol.Activate();
                    t.Commit();
                }
            }

            // 4. Familie bei (0,0,0) platzieren (Place family at (0,0,0))
            using (Transaction transaction = new Transaction(_doc, "Familieninstanz platzieren"))
            {
                transaction.Start();

                FamilyInstance pbpInstance = _doc.Create.NewFamilyInstance(
                    new XYZ(0, 0, 0),
                    familySymbol,
                    StructuralType.NonStructural);

                string koordinatenText =
                    $"PBP ({CommandData.X},{CommandData.Y},{CommandData.Z})\n" +
                    $"Lage DBRef 2016 (EPSG {CommandData.Lage})\n" +
                    $"Höhe DE DHHN2016 NH (EPSG {CommandData.Hoehe})";

                Parameter beschriftungParam = pbpInstance.LookupParameter("PBP_Koordinaten");
                if (beschriftungParam != null && !beschriftungParam.IsReadOnly)
                {
                    beschriftungParam.Set(koordinatenText);
                }

                transaction.Commit();
            }

            // --- Nach IFC-Export fragen ---
            var exportDecision = System.Windows.Forms.MessageBox.Show(
                "Möchten Sie den erstellten PBP als IFC exportieren?",
                "IFC-Export",
                System.Windows.Forms.MessageBoxButtons.YesNo,
                System.Windows.Forms.MessageBoxIcon.Question
            );

            if (exportDecision == System.Windows.Forms.DialogResult.Yes)
            {
                ExportIFC();
            }
        }

        private void ExportIFC()
        {
            string ifcExportPath = null;
            using (var saveFileDialog = new System.Windows.Forms.SaveFileDialog())
            {
                saveFileDialog.Filter = "IFC Dateien (*.ifc)|*.ifc";
                saveFileDialog.Title = "Wählen Sie einen Speicherort für die IFC-Datei";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string filename = string.IsNullOrWhiteSpace(CommandData.PBPName) ? "Export.ifc" : CommandData.PBPName + ".ifc";
                saveFileDialog.FileName = filename;
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    ifcExportPath = saveFileDialog.FileName;
            }

            if (!string.IsNullOrEmpty(ifcExportPath))
            {
                IFCExportOptions options = new IFCExportOptions();
                bool success = false;
                using (Transaction tx = new Transaction(_doc, "IFC Export"))
                {
                    tx.Start();
                    try
                    {
                        success = _doc.Export(
                            System.IO.Path.GetDirectoryName(ifcExportPath),
                            System.IO.Path.GetFileName(ifcExportPath),
                            options
                        );
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("IFC Export Fehler", ex.Message);
                    }
                    tx.Commit();
                }
                if (success)
                    TaskDialog.Show("Export abgeschlossen", $"IFC-Datei wurde gespeichert:\n{ifcExportPath}");
                else
                    TaskDialog.Show("Exportfehler", "IFC-Export war nicht erfolgreich.");
            }
        }

        private void ResetAll()
        {
            // Zurücksetzen von Projekt/Standort (Punkte)
            PBPInitialState.ResetAllBasePoints(_doc);

            // 1. Alle PBP-Familieninstanzen löschen
            using (Transaction tx = new Transaction(_doc, "Remove PBP family instances"))
            {
                tx.Start();
                var instances = new FilteredElementCollector(_doc)
                    .OfClass(typeof(FamilyInstance))
                    .Cast<FamilyInstance>()
                    .Where(inst => _lastLoadedFamilyId != ElementId.InvalidElementId
                        ? inst.Symbol.Family.Id == _lastLoadedFamilyId
                        : inst.Symbol.Family.Name.Contains("Koordinationskoerper"))
                    .ToList();
                foreach (var instance in instances)
                    _doc.Delete(instance.Id);
                tx.Commit();
            }

            // 2. Familie selbst aus dem Projekt löschen
            if (_lastLoadedFamilyId != ElementId.InvalidElementId)
            {
                Family loadedFamily = _doc.GetElement(_lastLoadedFamilyId) as Family;
                if (loadedFamily != null)
                {
                    using (Transaction tx = new Transaction(_doc, "Remove Loaded PBP Family"))
                    {
                        tx.Start();
                        _doc.Delete(_lastLoadedFamilyId);
                        tx.Commit();
                    }
                    _lastLoadedFamilyId = ElementId.InvalidElementId;
                    return;
                }
            }

            // Fallback: nach Name
            string fallbackFamilyName = "DB_Koordinationskoerper_ModelTextalsParam";
            Family targetFamily = new FilteredElementCollector(_doc)
                .OfClass(typeof(Family))
                .Cast<Family>()
                .FirstOrDefault(f => f.Name == fallbackFamilyName);

            if (targetFamily != null)
            {
                using (Transaction tx = new Transaction(_doc, "Remove PBP Family"))
                {
                    tx.Start();
                    _doc.Delete(targetFamily.Id);
                    tx.Commit();
                }
            }
        }

        // --- Verschiebung des Projektursprungs (Dienstprogramm)
        private void ShiftProjectOrigin(Document doc, XYZ shiftVector)
        {
            ProjectLocation projLocation = doc.ActiveProjectLocation;
            ProjectPosition currentPosition = projLocation.GetProjectPosition(XYZ.Zero);

            double newEastWest = currentPosition.EastWest + shiftVector.X;
            double newNorthSouth = currentPosition.NorthSouth + shiftVector.Y;
            double newElevation = currentPosition.Elevation + shiftVector.Z;

            ProjectPosition newPosition = new ProjectPosition(newEastWest, newNorthSouth, newElevation, currentPosition.Angle);

            using (Transaction t = new Transaction(doc, "Projekt neu positionieren"))
            {
                t.Start();
                projLocation.SetProjectPosition(XYZ.Zero, newPosition);
                t.Commit();
            }
        }
    }
}
