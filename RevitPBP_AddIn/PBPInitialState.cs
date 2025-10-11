using Autodesk.Revit.DB;
using System.Linq;

namespace RevitPBP_AddIn
{
    // Speichert den Anfangszustand von Projekt-/Vermessungspunkten und Projektlage

    public static class PBPInitialState
    {
        private static ProjectPosition _initialProjectPos;
        private static double? _initialSurveyNS, _initialSurveyEW, _initialSurveyElev;

        // Aktuellen Zustand beim Start des Add-ins speichern
        public static void SaveInitialState(Document doc)
        {
            // Projektbasispunkt speichern (Position)
            _initialProjectPos = doc.ActiveProjectLocation.GetProjectPosition(XYZ.Zero);

            // Vermessungspunkt speichern (falls vorhanden)
            var surveyPoint = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_SharedBasePoint)
                .WhereElementIsNotElementType()
                .FirstOrDefault();

            if (surveyPoint != null)
            {
                var ns = surveyPoint.get_Parameter(BuiltInParameter.BASEPOINT_NORTHSOUTH_PARAM);
                var ew = surveyPoint.get_Parameter(BuiltInParameter.BASEPOINT_EASTWEST_PARAM);
                var elev = surveyPoint.get_Parameter(BuiltInParameter.BASEPOINT_ELEVATION_PARAM);

                _initialSurveyNS = ns?.AsDouble();
                _initialSurveyEW = ew?.AsDouble();
                _initialSurveyElev = elev?.AsDouble();
            }
        }

        // Projektbasispunkt zurücksetzen (Project Base Point)
        public static void ResetAllBasePoints(Document doc)
        {
            // Reset project base point (location)
            if (_initialProjectPos != null)
            {
                using (Transaction t = new Transaction(doc, "Projekt-Basispunkt zurücksetzen"))
                {
                    t.Start();
                    doc.ActiveProjectLocation.SetProjectPosition(
                        XYZ.Zero,
                        new ProjectPosition(
                            _initialProjectPos.EastWest,
                            _initialProjectPos.NorthSouth,
                            _initialProjectPos.Elevation,
                            _initialProjectPos.Angle
                        )
                    );
                    t.Commit();
                }
            }

            // Vermessungspunkt zurücksetzen (Survey Point)
            var surveyPoint = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_SharedBasePoint)
                .WhereElementIsNotElementType()
                .FirstOrDefault();

            if (surveyPoint != null && _initialSurveyNS.HasValue && _initialSurveyEW.HasValue && _initialSurveyElev.HasValue)
            {
                using (Transaction t = new Transaction(doc, "Vermessungspunkt zurücksetzen"))
                {
                    t.Start();
                    var ns = surveyPoint.get_Parameter(BuiltInParameter.BASEPOINT_NORTHSOUTH_PARAM);
                    var ew = surveyPoint.get_Parameter(BuiltInParameter.BASEPOINT_EASTWEST_PARAM);
                    var elev = surveyPoint.get_Parameter(BuiltInParameter.BASEPOINT_ELEVATION_PARAM);

                    if (ns != null && !ns.IsReadOnly)
                        ns.Set(_initialSurveyNS.Value);
                    if (ew != null && !ew.IsReadOnly)
                        ew.Set(_initialSurveyEW.Value);
                    if (elev != null && !elev.IsReadOnly)
                        elev.Set(_initialSurveyElev.Value);

                    t.Commit();
                }
            }
        }
    }
}
