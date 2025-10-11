using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitPBP_AddIn
{
    public partial class SPdigital_tools : Form
    {
        public SPdigital_tools()
        {
            InitializeComponent();
        }

        private void butPBPErstellen_Click(object sender, EventArgs e)
        {
            var inputForm = new InputForm(
                CreatePBPCommand.Doc,
                CreatePBPCommand.Handler,
                CreatePBPCommand.ExtEvent);

                inputForm.Show();
        }
    }
}
