using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using WinForms = System.Windows.Forms;

namespace RevitPBP_AddIn
{
    public partial class InputForm : WinForms.Form
    {
        // --- Für die Interaktion mit Revit via ExternalEvent ---
        private PBPExternalEventHandler _handler;
        private ExternalEvent _extEvent;
        private Document _doc;

        // --- Zum Speichern der Benutzereingaben ---
        private string _pbpName;
        private double _pbpX, _pbpY, _pbpZ, _pbpLage, _pbpHöhe;

        public string SelectedFamilyPath { get; private set; }
        public bool UseStandardFamily { get; private set; } = true;

        // --- EPSG-Standardwerte aus dem Form-Designer ---
        private readonly string _lageDefaultText;
        private readonly string _hoeheDefaultText;

        // --- Konstruktor ---
        public InputForm(Document doc, PBPExternalEventHandler handler, ExternalEvent extEvent)
        {
            InitializeComponent();
            _doc = doc;
            _handler = handler;
            _extEvent = extEvent;

            // EPSG-Standardwerte aus dem Designer speichern (z. B. „7837“ und „5684“)
            _lageDefaultText = textBoxLage.Text;
            _hoeheDefaultText = textBoxHöhe.Text;

            // Tooltips
            Hinweis1.SetToolTip(buttonFamilieHerunterladen, "Tipp: Sie können eine eigene Revit-Familie (*.rfa) auswählen.");
            Hinweis1.SetToolTip(buttonStandardFamilie, "Tipp: Verwendet die Standard-Koordinationskörper-Familie aus dem System.");
            Hinweis1.SetToolTip(textBoxX, "Geben Sie hier den X-Wert ein.");
            Hinweis1.SetToolTip(textBoxY, "Geben Sie hier den Y-Wert ein.");
            Hinweis1.SetToolTip(textBoxZ, "Geben Sie hier den Z-Wert ein.");
            Hinweis1.SetToolTip(textBoxLage, "Geben Sie hier den EPSG Lage ein.");
            Hinweis1.SetToolTip(textBoxHöhe, "Geben Sie hier den EPSG Höhe ein.");
        }

        // --- Standardfamilie verwenden ---
        private void buttonStandardFamilie_Click(object sender, EventArgs e)
        {
            if (!ParseAndStoreInputs()) return;

            UseStandardFamily = true;
            SelectedFamilyPath = null;

            _handler.SetCreatePBPCommand(_pbpX, _pbpY, _pbpZ, _pbpLage, _pbpHöhe, UseStandardFamily, SelectedFamilyPath, textBoxName.Text);
            _extEvent.Raise();
        }

        // --- Eigene Familie verwenden ---
        private void buttonFamilieHerunterladen_Click(object sender, EventArgs e)
        {
            if (!ParseAndStoreInputs()) return;

            using (var openFileDialog = new WinForms.OpenFileDialog())
            {
                openFileDialog.Filter = "Revit-Familien (*.rfa)|*.rfa";
                openFileDialog.Title = "Wählen Sie eine Revit-Familie aus";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (openFileDialog.ShowDialog() == WinForms.DialogResult.OK)
                {
                    SelectedFamilyPath = openFileDialog.FileName;
                    UseStandardFamily = false;
                    WinForms.MessageBox.Show("Familie ausgewählt:\n" + SelectedFamilyPath, "Familie gewählt",
                        WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information);

                    _handler.SetCreatePBPCommand(_pbpX, _pbpY, _pbpZ, _pbpLage, _pbpHöhe, UseStandardFamily, SelectedFamilyPath, textBoxName.Text);
                    _extEvent.Raise();
                }
            }
        }

        // --- Neue PBP (Reset) + Formular bereinigen, außer Lage/Höhe ---
        private void buttonNeuePBP_Click(object sender, EventArgs e)
        {
            // 1) Felder leeren (wie beim Start), aber EPSG aus dem Designer behalten/zurücksetzen
            ResetFormFieldsKeepEpsg();
            // Fokus auf Name
            textBoxName.Focus();

            // 2) Reset-Logik
            _handler.SetResetAllCommand();
            _extEvent.Raise();
        }

        // --- Exit/Beenden ---
        private void buttonBeenden_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- Prüfung und Parsing der Benutzereingaben ---
        private bool ParseAndStoreInputs()
        {
            string name = textBoxName.Text;

            // Zur Sicherheit: Punkt als Dezimaltrennzeichen benutzen
            bool okX = double.TryParse(textBoxX.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var x);
            bool okY = double.TryParse(textBoxY.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var y);
            bool okZ = double.TryParse(textBoxZ.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var z);

            if (!okX) { WinForms.MessageBox.Show("Bitte gültigen X-Wert eingeben."); return false; }
            if (!okY) { WinForms.MessageBox.Show("Bitte gültigen Y-Wert eingeben."); return false; }
            if (!okZ) { WinForms.MessageBox.Show("Bitte gültigen Z-Wert eingeben."); return false; }

            // EPSG-Standardwerte aus dem Designer
            double defLage = double.TryParse(_lageDefaultText, NumberStyles.Any, CultureInfo.InvariantCulture, out var dl) ? dl : 0;
            double defHoehe = double.TryParse(_hoeheDefaultText, NumberStyles.Any, CultureInfo.InvariantCulture, out var dh) ? dh : 0;

            bool okLage = double.TryParse(textBoxLage.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var lage);
            bool okHoehe = double.TryParse(textBoxHöhe.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var hoehe);

            if (!okLage) lage = defLage;
            if (!okHoehe) hoehe = defHoehe;

            _pbpName = name?.Trim();
            _pbpX = x;
            _pbpY = y;
            _pbpZ = z;
            _pbpLage = lage;
            _pbpHöhe = hoehe;

            return true;
        }

        // --- Eigenschaften für den Zugriff auf Werte nach dem Dialog (falls benötigt) ---
        public string PBPName => _pbpName;
        public double PBPX => _pbpX;
        public double PBPY => _pbpY;
        public double PBPZ => _pbpZ;
        public double PBPLage => _pbpLage;
        public double PBPHöhe => _pbpHöhe;

        private void InputForm_Load(object sender, EventArgs e)
        {
        }

        // --- Formular bereinigen, EPSG aus dem Designer beibehalten/zurücksetzen ---
        private void ResetFormFieldsKeepEpsg()
        {
            foreach (var c in GetAllControls(this))
            {
                if (c == textBoxLage)
                {
                    textBoxLage.Text = _lageDefaultText;
                    continue;
                }
                if (c == textBoxHöhe)
                {
                    textBoxHöhe.Text = _hoeheDefaultText;
                    continue;
                }

                if (c is WinForms.TextBox tb) tb.Text = string.Empty;
                else if (c is WinForms.ComboBox cb) cb.SelectedIndex = -1;
                else if (c is WinForms.CheckBox chk) chk.Checked = false;
                else if (c is WinForms.NumericUpDown nud) nud.Value = 0;
                else if (c is WinForms.DateTimePicker dtp) dtp.Value = DateTime.Today;
            }
        }

        // Rekursives Durchlaufen aller Steuerelemente (nur WinForms)
        private static IEnumerable<WinForms.Control> GetAllControls(WinForms.Control root)
        {
            foreach (WinForms.Control c in root.Controls)
            {
                yield return c;
                foreach (var child in GetAllControls(c))
                    yield return child;
            }
        }
    }
}
