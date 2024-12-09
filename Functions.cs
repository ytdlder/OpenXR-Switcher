using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

// Debug.WriteLine();
// MessageBox.Show("Message", "Title", MessageBoxButtons.OK);

namespace OpenXR_Switcher
{
    public class ExRichTextBox : RichTextBox
    {
        public ExRichTextBox()
        {
            Selectable = true;
        }
        const int WM_SETFOCUS = 0x0007;
        const int WM_KILLFOCUS = 0x0008;

        ///<summary>
        /// Enables or disables selection highlight. 
        /// If you set `Selectable` to `false` then the selection highlight
        /// will be disabled. 
        /// It's enabled by default.
        ///</summary>
        [DefaultValue(true)]
        public bool Selectable { get; set; }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SETFOCUS && !Selectable)
                m.Msg = WM_KILLFOCUS;

            base.WndProc(ref m);
        }
    }

    public static class Functions
    {
        // PUBLIC VARIABLES
        // *********************************************************************
        public static string    reg_usersetnames    = @"SOFTWARE\Hotshot\OpenXRSwitcher";
        public static string    saved_name          = "";
        private static string[,] allusersetnames;
        private static string   wmr                 = @"C:\Windows\System32\MixedRealityRuntime.json";
        private static Color    backgroundactive    = Color.LightGreen;
        private static Color    backgroundinactive  = Color.IndianRed;
        private static Color    backgroundnotfound  = Color.Ivory;
        private static string   activeruntime       = "";
        private static string[] allruntimes;
        private static string   reg_activeruntime   = @"SOFTWARE\Khronos\OpenXR\1";
        private static string   reg_activeruntime_name = "ActiveRuntime";
        private static string   reg_availableruntimes = @"SOFTWARE\Khronos\OpenXR\1\AvailableRuntimes";
        private static string   reg_alllayers       = @"SOFTWARE\Khronos\OpenXR\1\ApiLayers\Implicit";
        private static string   layer_active        = "0";
        private static string   layer_inactive      = "1";

        // get access to Main-Class elements
        private static WindowMain mainWindow = (WindowMain)Application.OpenForms[0];


        // FUNCTIONS
        // *********************************************************************
        public static void CheckArguments()
        {
            string[] args = Environment.GetCommandLineArgs();

            // arg[0] = FullPathToAppExe
            if (args.Length > 1)
            {
                if (args.Contains("/h") || args.Contains("/H") || args.Contains("-h") || args.Contains("-H"))
                {
                    MessageBox.Show("You can switch directly to an OpenXR runtime by passing it as a parameter."
                        + Environment.NewLine + "eg. 'steam' for SteamVR"
                        + Environment.NewLine + "or 'pimax' for Pimax-OpenXR"
                        + Environment.NewLine + "-> only if found in the registry of course!", "Info", MessageBoxButtons.OK);
                }
                else
                {
                    var SetRuntimeTo = Array.Find(allruntimes, str => str.IndexOf(args[1], StringComparison.OrdinalIgnoreCase) >= 0);

                    if (SetRuntimeTo != null)
                    {
                        if (SetRuntimeTo == activeruntime)
                        {
                            MessageBox.Show("Selected runtime already active!"
                                + Environment.NewLine + "Exiting...", "Info", MessageBoxButtons.OK);
                        }
                        else
                        {
                            if (WriteToReg("HKLM", reg_activeruntime, reg_activeruntime_name, SetRuntimeTo, RegistryValueKind.String))
                                MessageBox.Show("Runtime successfully switched!", "Info", MessageBoxButtons.OK);
                        }
                    }
                }

                Application.Exit();
            }
        }

        public static void GetActiveRuntime()
        {
            RegistryKey regkey = Registry.LocalMachine.OpenSubKey(reg_activeruntime);

            if (regkey != null)
            {
                activeruntime = regkey.GetValue(reg_activeruntime_name).ToString();
            }
        }

        public static void GetRuntimes()
        {
            List<string> runtimelist = new List<string>();

            if (File.Exists(wmr))
            {
                runtimelist.Add(wmr);
            }

            allruntimes = runtimelist.ToArray();

            RegistryKey regkey = Registry.LocalMachine.OpenSubKey(reg_availableruntimes);

            if (regkey != null)
            {
                string[] temp = regkey.GetValueNames();
                allruntimes = allruntimes.Concat(temp).ToArray();

                regkey.Close();
            }
            else
            {
                MessageBox.Show("No 'Khronos' registry key or OpenXR subkeys found; no OpenXR runtime installed?\n\nPress OK to exit.", "ERROR", MessageBoxButtons.OK);
                Application.Exit();
            }

            // get user-set names for runtimes and layers
            regkey = Registry.CurrentUser.OpenSubKey(reg_usersetnames);

            if (regkey != null)
            {
                string[] original_names = regkey.GetValueNames();

                if (original_names != null)
                {
                    //string[,] user_names = new string[original_names.Length, 2];
                    allusersetnames = new string[original_names.Length, 2];

                    for (int i = 0; i < original_names.Length; i++)
                    {
                        allusersetnames[i, 0] = original_names[i];
                        allusersetnames[i, 1] = regkey.GetValue(original_names[i]).ToString();
                    }
                    regkey.Close();
                }
            }
        }

        public static void AddRuntimes()
        {
            for (int i = 0; i < allruntimes.Length; i++)
            {
                AddPanel(mainWindow.flowRuntimes, allruntimes[i]);
            }
        }

        public static void GetAddLayers()
        {
            RegistryKey regkey = Registry.LocalMachine.OpenSubKey(reg_alllayers);

            if (regkey != null)
            {
                string[] names = regkey.GetValueNames();

                if (names != null)
                {
                    string[,] layers = new string[names.Length, 2];

                    for (int i = 0; i < names.Length; i++)
                    {
                        layers[i, 0] = names[i];
                        layers[i, 1] = regkey.GetValue(names[i]).ToString();
                    }
                    regkey.Close();

                    for (int i = 0; i < names.Length; i++)
                    {
                        AddPanel(mainWindow.flowLayers, layers[i, 0], layers[i, 1]);
                    }
                }
            }
        }

        private static void SetActiveRuntime(this Panel panel)
        {
            string JsonFile = panel.Tag.ToString();

            if (WriteToReg("HKLM", reg_activeruntime, reg_activeruntime_name, JsonFile, RegistryValueKind.String))
            {
                panel.BorderStyle = BorderStyle.FixedSingle;

                foreach (Control c in panel.Parent.Controls)
                {
                    if (c is Panel)
                    {
                        JsonFile = c.Tag.ToString();

                        if (c == panel)
                            SetPanelActive(c as Panel);
                        else if (File.Exists(@JsonFile))
                            SetPanelInactive(c as Panel);
                        else
                            SetPanelNotFound(c as Panel);
                    }
                }
            }
        }

        private static void SetActiveLayer(this Panel panel, string value)
        {
            string JsonFile = panel.Tag.ToString();

            if (WriteToReg("HKLM", reg_alllayers, JsonFile, value, RegistryValueKind.DWord))
            {
                if (value == layer_inactive)
                {
                    SetPanelInactive(panel, true);
                }
                else
                {
                    SetPanelActive(panel, true);
                }
            }
        }

        public static bool WriteToReg(string Hive, string Key, string Name, string Value, RegistryValueKind Type)
        {
            RegistryKey regkey;

            try
            {
                if (Hive == "HKLM")
                    regkey = Registry.LocalMachine.OpenSubKey(Key, true);
                else
                    regkey = Registry.CurrentUser.CreateSubKey(Key, true);
            }
            catch
            {
                MessageBox.Show("Couldn't open >"+ Hive + "< registry with write access!", "ERROR", MessageBoxButtons.OK);
                return false;
            }

            if (Name == "#DELETE#")
            {
                try
                {
                    regkey.DeleteValue(Value);
                    return true;
                }
                catch
                {
                    MessageBox.Show("Couldn't delete from registry!", "ERROR", MessageBoxButtons.OK);
                    return false;
                }
            }
            else
            {
                try
                {
                    regkey.SetValue(Name, Value, Type);
                    return true;
                }
                catch
                {
                    MessageBox.Show("Couldn't write to registry!", "ERROR", MessageBoxButtons.OK);
                    return false;
                }
            }
        }

        private static void AddPanel(FlowLayoutPanel layoutpanel, string filepath, string value = null)
        {
            string filename = Path.GetFileName(filepath);

            string[] parts = filepath.Split('\\');
            string filepathname = parts[parts.Length - 2] + '\\' + parts[parts.Length - 1];

            int image_width_border = 3;

            if (string.Equals(filepathname, "System32\\MixedRealityRuntime.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "WMR";

            else if (string.Equals(filepathname, "Virtual Desktop Streamer\\openxr-oculus-compatibility.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "VirtualDesktop Oculus Compatibility";
            else if (string.Equals(filepathname, "DlvrOpenXrLayer\\XrApiLayer_dlvr.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "Almalence DigitalLense";
            else if (string.Equals(filepathname, "OpenXR\\UltraleapHandTracking.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "Ultraleap HandTracking";
            else if (string.Equals(filepathname, "OpenXR-Toolkit\\XR_APILAYER_MBUCCHIA_toolkit.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "OpenXR Toolkit";
            else if (string.Equals(filepathname, "OpenXR-Quad-Views-Foveated\\openxr-api-layer.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "QuadViews DFR";
            else if (string.Equals(filepathname, "OpenXR-Eye-Trackers\\openxr-api-layer.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "SteamVR Eye Tracking / DFR";
            else if (string.Equals(filepathname, "OpenXrApiLayer\\XR_APILAYER_NOVENDOR_XRNeckSafer.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "NeckSafer";
            else if (string.Equals(filepathname, "bin\\OpenKneeboard-OpenXR.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "OpenKneeBoard";

            else
            {
                string[] temp = filename.Split('_', '-');
                filepathname = temp[0].ToUpper();
            }


            // ----------------------------------------------------------------------------------------------------------------
            Panel panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Dock = DockStyle.Top;
            panel.Location = new Point(0, 0);
            panel.AutoSize = true;
            panel.AutoSizeMode = AutoSizeMode.GrowOnly;

            panel.Tag = filepath;

            // ----------------------------------------------------------------------------------------------------------------
            if (value == null)
            {
                // for runtimes only
                PictureBox pictureboxLogo = new PictureBox();
                pictureboxLogo.BackColor = Color.White;           // Wheat
                pictureboxLogo.BorderStyle = BorderStyle.None;
                pictureboxLogo.Dock = DockStyle.Left;
                pictureboxLogo.Location = new Point(0, 0);
                pictureboxLogo.Size = new Size(64, 64);
                pictureboxLogo.SizeMode = PictureBoxSizeMode.Zoom;
                image_width_border = 70;

                switch (filepathname)
                {
                    case "WMR": pictureboxLogo.Image = Properties.Resources.logo_wmr; break;
                    case "OCULUS": pictureboxLogo.Image = Properties.Resources.logo_oculus_meta; break;
                    case "STEAMXR": pictureboxLogo.Image = Properties.Resources.logo_steamvr; break;
                    case "VARJO": pictureboxLogo.Image = Properties.Resources.logo_varjo; break;
                    case "VIVEVR": pictureboxLogo.Image = Properties.Resources.logo_vive; break;
                    case "PIMAX": pictureboxLogo.Image = Properties.Resources.logo_pimax; break;
                    case "PIOPENXR": pictureboxLogo.Image = Properties.Resources.logo_pimax; break;
                    default: pictureboxLogo.Image = Properties.Resources.logo_na; break;
                }

                panel.Controls.Add(pictureboxLogo);
            }

            // Check filepathname with user set name
            string originalname = filepathname;

            // Are there any user-saved names?
            if (allusersetnames != null)
            {
                for (int i = 0; i < (allusersetnames.Length / 2); i++)    // durch 2, weil zweidimensionales Array und Length alle Felder zählt!
                {
                    if (filepathname == allusersetnames[i, 0])
                    {
                        originalname = allusersetnames[i, 0];
                        filepathname = allusersetnames[i, 1];
                        break;
                    }
                }
            }

            // ----------------------------------------------------------------------------------------------------------------
            PictureBox pictureboxEdit = new PictureBox();
            pictureboxEdit.BorderStyle = BorderStyle.None;
            pictureboxEdit.Dock = DockStyle.None;
            pictureboxEdit.Location = new Point(image_width_border, 6);
            pictureboxEdit.Size = new Size(20, 20);
            pictureboxEdit.SizeMode = PictureBoxSizeMode.Zoom;
            pictureboxEdit.Image = Properties.Resources.symb_edit;
            pictureboxEdit.Cursor = Cursors.Cross;
            mainWindow.toolTipEdit.SetToolTip(pictureboxEdit, "Change the display name");
            pictureboxEdit.Click += new EventHandler((sender, e) => evtEditPictureBox_Clicked(sender, e, originalname, filepathname));

            int image2_width_border = image_width_border + 22;

            panel.Controls.Add(pictureboxEdit);

            // ----------------------------------------------------------------------------------------------------------------
            ExRichTextBox head = new ExRichTextBox();
            head.Selectable = false;
            head.BorderStyle = BorderStyle.None;
            head.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            head.Location = new Point(image2_width_border, 3);
            head.Multiline = false;
            head.Size = new Size(200, 29);
            head.ReadOnly = true;
            head.Cursor = Cursors.Hand;

            head.Text = filepathname;

            panel.Controls.Add(head);

            // ----------------------------------------------------------------------------------------------------------------
            RichTextBox text = new RichTextBox();
            text.BorderStyle = BorderStyle.None;
            text.Font = new Font("Segoe UI", 16F);
            text.Location = new Point(image_width_border, 32);
            text.Multiline = false;
            text.Size = new Size(200, 29);
            text.ReadOnly = true;

            text.Text = filepath;

            panel.Controls.Add(text);

            // ----------------------------------------------------------------------------------------------------------------
            head.Width = TextRenderer.MeasureText(text.Text, text.Font).Width;
            text.Width = TextRenderer.MeasureText(text.Text, text.Font).Width;

            if (value == null)
            {
                // for runtimes
                if (filepath == activeruntime)
                {
                    SetPanelActive(panel);
                }
                else if (File.Exists(@filepath))
                {
                    SetPanelInactive(panel);
                }
                else
                {
                    SetPanelNotFound(panel);
                }
            }
            else
            {
                // for layers
                if (!File.Exists(@filepath))
                {
                    SetPanelNotFound(panel);
                }
                else if (value == layer_active)
                {
                    SetPanelActive(panel, true);
                }
                else
                {
                    SetPanelInactive(panel, true);
                }
            }

            layoutpanel.Controls.Add(panel);
        }

        private static void SetPanelActive(this Panel panel, bool islayer=false)
        {
            string tooltiptext = "";

            if (!islayer)
            {
                tooltiptext = "This is the currently active OpenXR runtime.";
                panel.Cursor = Cursors.Arrow;

                panel.MouseEnter -= new EventHandler(evtPanel_MouseEnter);
                panel.MouseLeave -= new EventHandler(evtPanel_MouseLeave);
                panel.Click -= new EventHandler((sender, e) => evtPanel_Clicked(sender, e));
            }
            else
            {
                tooltiptext = "This layer is currenlty active and can be disabled.";
                panel.Cursor = Cursors.Hand;

                panel.MouseEnter += new EventHandler(evtPanel_MouseEnter);
                panel.MouseLeave += new EventHandler(evtPanel_MouseLeave);
                panel.Click += new EventHandler((sender, e) => evtPanel_Clicked(sender, e, layer_inactive));
            }

            panel.BackColor = backgroundactive;

            mainWindow.toolTipActive.Hide(panel);
            mainWindow.toolTipActive.SetToolTip(panel, tooltiptext);
            mainWindow.toolTipInactive.Hide(panel);
            mainWindow.toolTipInactive.SetToolTip(panel, "");

            foreach (Control c in panel.Controls)
            {
                if (c is RichTextBox)
                {
                    if (!islayer)
                    {
                        c.Cursor = Cursors.Arrow;

                        c.MouseEnter -= new EventHandler(evtTextbox_MouseEnter);
                        c.MouseLeave -= new EventHandler(evtTextbox_MouseLeave);
                        c.Click -= new EventHandler((sender, e) => evtTextbox_Clicked(sender, e));
                    }
                    else
                    {
                        c.Cursor = Cursors.Hand;

                        c.MouseEnter += new EventHandler(evtTextbox_MouseEnter);
                        c.MouseLeave += new EventHandler(evtTextbox_MouseLeave);
                        c.Click += new EventHandler((sender, e) => evtTextbox_Clicked(sender, e, layer_inactive));
                    }

                    c.BackColor = backgroundactive;

                    mainWindow.toolTipActive.Hide(c);
                    mainWindow.toolTipActive.SetToolTip(c, tooltiptext);
                    mainWindow.toolTipInactive.Hide(c);
                    mainWindow.toolTipInactive.SetToolTip(c, "");
                }
            }
        }

        private static void SetPanelInactive(this Panel panel, bool islayer = false)
        {
            string tooltiptext = "This is currently inactive and can be enabled.";

            panel.BackColor = backgroundinactive;
            panel.Cursor = Cursors.Hand;

            mainWindow.toolTipActive.Hide(panel);
            mainWindow.toolTipActive.SetToolTip(panel, "");
            mainWindow.toolTipInactive.Hide(panel);
            mainWindow.toolTipInactive.SetToolTip(panel, tooltiptext);

            panel.MouseEnter += new EventHandler(evtPanel_MouseEnter);
            panel.MouseLeave += new EventHandler(evtPanel_MouseLeave);
            if (!islayer)
                panel.Click += new EventHandler((sender, e) => evtTextbox_Clicked(sender, e));
            else
                panel.Click += new EventHandler((sender, e) => evtTextbox_Clicked(sender, e, layer_active));

            foreach (Control c in panel.Controls)
            {
                if (c is RichTextBox)
                {
                    c.BackColor = backgroundinactive;
                    c.Cursor = Cursors.Hand;

                    mainWindow.toolTipActive.Hide(c);
                    mainWindow.toolTipActive.SetToolTip(c, "");
                    mainWindow.toolTipInactive.Hide(c);
                    mainWindow.toolTipInactive.SetToolTip(c, tooltiptext);

                    c.MouseEnter += new EventHandler(evtTextbox_MouseEnter);
                    c.MouseLeave += new EventHandler(evtTextbox_MouseLeave);
                    if (!islayer) 
                        c.Click += new EventHandler((sender, e) => evtTextbox_Clicked(sender, e));
                    else
                        c.Click += new EventHandler((sender, e) => evtTextbox_Clicked(sender, e, layer_active));
                }
            }
        }

        private static void SetPanelNotFound(this Panel panel)
        {
            string tooltiptext = "This JSON file doesn't seem to exist anymore. Reinstall that app or remove this entry from the registry.";

            panel.BackColor = backgroundnotfound;
            panel.Cursor = Cursors.No;
            mainWindow.toolTipNotFound.SetToolTip(panel, tooltiptext);

            foreach (Control c in panel.Controls)
            {
                if (c is RichTextBox)
                {
                    c.BackColor = backgroundnotfound;
                    c.Cursor = Cursors.No;
                    c.Font = new Font(c.Font, FontStyle.Italic);
                    mainWindow.toolTipNotFound.SetToolTip(c, tooltiptext);
                }
            }
        }

        // EVENTS
        // *********************************************************************
        private static void evtPanel_MouseEnter(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;

            panel.BorderStyle = BorderStyle.Fixed3D;
        }
        private static void evtPanel_MouseLeave(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;

            panel.BorderStyle = BorderStyle.FixedSingle;
        }
        private static void evtPanel_Clicked(object sender, EventArgs e, string value=null)
        {
            Panel panel = sender as Panel;

            if (value == null)
                SetActiveRuntime(panel);
            else
                SetActiveLayer(panel, value);
        }        

        private static void evtTextbox_MouseEnter(object sender, EventArgs e)
        {
            RichTextBox textBox = sender as RichTextBox;

            var parent = textBox.Parent as Panel;
            parent.BorderStyle = BorderStyle.Fixed3D;
        }
        private static void evtTextbox_MouseLeave(object sender, EventArgs e)
        {
            RichTextBox textBox = sender as RichTextBox;

            var parent = textBox.Parent as Panel;
            parent.BorderStyle = BorderStyle.FixedSingle;
        }
        private static void evtTextbox_Clicked(object sender, EventArgs e, string value=null)
        {
            RichTextBox textBox = sender as RichTextBox;

            var parent = textBox.Parent as Panel;

            if (value == null)
                SetActiveRuntime(parent);
            else
                SetActiveLayer(parent, value);
        }
        private static void evtEditPictureBox_Clicked(object sender, EventArgs e, string originalname, string filepathname)
        {
            mainWindow.panelEdit.Location = new Point(
                mainWindow.ClientSize.Width / 2 - mainWindow.panelEdit.Size.Width / 2,
                mainWindow.ClientSize.Height / 2 - mainWindow.panelEdit.Size.Height / 2);

            mainWindow.tableLayoutPanel.Enabled = false;        // Enabled | Visible
            mainWindow.buttonRefresh.Visible = false;

            saved_name = originalname;
            mainWindow.maskedTextBox.Text = filepathname;

            mainWindow.panelEdit.BringToFront();

            mainWindow.panelEdit.Visible = true;
        }
    }
}
