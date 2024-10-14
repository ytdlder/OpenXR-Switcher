using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace OpenXR_Switcher
{
    public partial class WindowMain : Form
    {
        public WindowMain()
        {
            InitializeComponent();
        }

        private void RepositionRefreshButton()
        {
            int ScrollbarWidth = 17;
            int MarginsBordersPadding = 19;

            int PosX = WindowMain.ActiveForm.Width - buttonRefresh.Width - MarginsBordersPadding;

            if (flowLayers.VerticalScroll.Visible)
            {
                PosX -= ScrollbarWidth;
                buttonRefresh.Location = new Point(PosX, 6);
            }
            else
            {
                buttonRefresh.Location = new Point(PosX, 6);
            }
        }

        private void WindowMain_Load(object sender, EventArgs e)
        {
            Functions.GetRuntimes();

            Functions.GetActiveRuntime();

            Functions.CheckArguments();

            Functions.AddRuntimes();

            Functions.GetAddLayers();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            flowRuntimes.Controls.Clear();
            flowLayers.Controls.Clear();

            WindowMain_Load(sender, e);

            RepositionRefreshButton();
        }

        private void WindowMain_Resize(object sender, EventArgs e)
        {
            RepositionRefreshButton();
        }

        private void WindowMain_Shown(object sender, EventArgs e)
        {
            RepositionRefreshButton();
        }

        private void ResetOverlay()
        {
            Functions.saved_name = "";
            maskedTextBox.Text = "";

            tableLayoutPanel.Enabled = true;        // Enabled | Visible
            buttonRefresh.Visible = true;
            panelEdit.Visible = false;
        }

        private void evtbuttonCancel_Click(object sender, EventArgs e)
        {
            ResetOverlay();
        }

        private void evtbuttonSave_Click(object sender, EventArgs e)
        {
            if(!Functions.WriteToReg("HKCU", Functions.reg_usersetnames, Functions.saved_name, maskedTextBox.Text, RegistryValueKind.String))
                MessageBox.Show("Couldn't write to registry!", "ERROR", MessageBoxButtons.OK);

            ResetOverlay();

            buttonRefresh.PerformClick();
        }
        
        private void evtbuttonReset_Click(object sender, EventArgs e)
        {
            if (!Functions.WriteToReg("HKCU", Functions.reg_usersetnames, "#DELETE#", Functions.saved_name, RegistryValueKind.String))
                MessageBox.Show("Couldn't delete value from registry!", "ERROR", MessageBoxButtons.OK);

            ResetOverlay();

            buttonRefresh.PerformClick();
        }

        private void maskedTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSave.PerformClick();
            }
        }
    }
}
