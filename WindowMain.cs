using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OpenXR_Switcher
{
    public partial class WindowMain : Form
    {
        public WindowMain()
        {
            InitializeComponent();
        }

        private void WindowMain_Load(object sender, EventArgs e)
        {
            Functions.GetRuntimes();

            Functions.GetActiveRuntime();

            Functions.CheckArguments();

            Functions.AddRuntimes();

            Functions.GetAddLayers();
        }
    }
}
