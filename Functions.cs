using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
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
        private static string   wmr = @"C:\Windows\System32\MixedRealityRuntime.json";
        private static Color    backgroundactive = Color.LightGreen;
        private static Color    backgroundinactive = Color.IndianRed;
        private static Color    backgroundnotfound = Color.Ivory;
        private static string   activeruntime = "";
        private static string[] allruntimes;
        private static string   reg_activeruntime = @"SOFTWARE\Khronos\OpenXR\1";
        private static string   reg_activeruntime_name = "ActiveRuntime";
        private static string   reg_availableruntimes = @"SOFTWARE\Khronos\OpenXR\1\AvailableRuntimes";
        private static string   reg_alllayers = @"SOFTWARE\Khronos\OpenXR\1\ApiLayers\Implicit";
        private static string   layer_active = "0";
        private static string   layer_inactive = "1";

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
                            if (WriteToReg(reg_activeruntime, reg_activeruntime_name, SetRuntimeTo, RegistryValueKind.String))
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

            if (allruntimes.Length == 0)
            {
                MessageBox.Show("No 'Khronos' registry key found; no OpenXR runtime installed?\n\nPress OK to exit.", "ERROR", MessageBoxButtons.OK);
                Application.Exit();
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

        private static void AddPanel(FlowLayoutPanel layoutpanel, string filepath, string value=null)
        {
            string filepathname = Path.GetFileName(filepath);
            string[] temp = filepathname.Split('_', '-');
            int image_width_border = 3;

            if (string.Equals(filepathname, "MixedRealityRuntime.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "WMR";
            else if (string.Equals(filepathname, "openxr-oculus-compatibility.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "VirtualDesktop Oculus Compatibility";
            else if (string.Equals(filepathname, "XrApiLayer_dlvr.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "Almalence DigitalLense";
            else if (string.Equals(filepathname, "UltraleapHandTracking.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "Ultraleap HandTracking";
            else if (string.Equals(filepathname, "XR_APILAYER_MBUCCHIA_toolkit.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "OpenXR Toolkit";
            else if (string.Equals(filepathname, "openxr-api-layer.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "QuadViews DFR";
            else if (string.Equals(filepathname, "XR_APILAYER_NOVENDOR_XRNeckSafer.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "NeckSafer";
            else if (string.Equals(filepathname, "OpenKneeboard-OpenXR.json", StringComparison.OrdinalIgnoreCase))
                filepathname = "OpenKneeBoard";
            else
                filepathname = temp[0].ToUpper();


            // --------------------------------------------------------
            Panel panel = new Panel();
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Dock = DockStyle.Top;
            panel.Location = new Point(0, 0);
            panel.AutoSize = true;
            panel.AutoSizeMode = AutoSizeMode.GrowOnly;

            panel.Tag = filepath;

            // --------------------------------------------------------
            if (value == null)
            {
                // for runtimes only
                PictureBox logo = new PictureBox();
                logo.BackColor = Color.White;           // Wheat
                logo.BorderStyle = BorderStyle.None;
                logo.Dock = DockStyle.Left;
                logo.Location = new Point(0, 0);
                logo.Size = new Size(64, 64);
                logo.SizeMode = PictureBoxSizeMode.Zoom;
                image_width_border = 70;

                switch (filepathname)
                {
                    case "OCULUS": logo.Image = Properties.Resources.logo_oculus_meta; break;
                    case "STEAMXR": logo.Image = Properties.Resources.logo_steamvr; break;
                    case "VARJO": logo.Image = Properties.Resources.logo_varjo; break;
                    case "VIVEVR": logo.Image = Properties.Resources.logo_vive; break;
                    case "PIMAX": logo.Image = Properties.Resources.logo_pimax; break;
                    case "WMR": logo.Image = Properties.Resources.logo_wmr; break;
                    default: logo.Image = Properties.Resources.logo_na; break;
                }

                panel.Controls.Add(logo);
            }

            // --------------------------------------------------------
            ExRichTextBox head = new ExRichTextBox();
            head.Selectable = false;
            head.BorderStyle = BorderStyle.None;
            head.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            head.Location = new Point(image_width_border, 3);
            head.Multiline = false;
            head.Size = new Size(200, 29);
            head.ReadOnly = true;
            head.Cursor = Cursors.Hand;

            head.Text = filepathname;

            panel.Controls.Add(head);

            // --------------------------------------------------------
            RichTextBox text = new RichTextBox();
            text.BorderStyle = BorderStyle.None;
            text.Font = new Font("Segoe UI", 16F);
            text.Location = new Point(image_width_border, 32);
            text.Multiline = false;
            text.Size = new Size(200, 29);
            text.ReadOnly = true;

            text.Text = filepath;

            panel.Controls.Add(text);

            // --------------------------------------------------------
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
            mainWindow.toolTipActive.SetToolTip(panel, tooltiptext);

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
                    mainWindow.toolTipActive.SetToolTip(c, tooltiptext);
                }
            }
        }

        private static void SetPanelInactive(this Panel panel, bool islayer = false)
        {
            string tooltiptext = "This is currently inactive and can be enabled.";

            panel.BackColor = backgroundinactive;
            panel.Cursor = Cursors.Hand;
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

        private static void SetActiveRuntime(this Panel panel)
        {
            string JsonFile = panel.Tag.ToString();

            if (WriteToReg(reg_activeruntime, reg_activeruntime_name, JsonFile, RegistryValueKind.String))
            {
                panel.BorderStyle = BorderStyle.FixedSingle;

/*mainWindow.toolTipActive.RemoveAll();
mainWindow.toolTipInactive.RemoveAll();
mainWindow.toolTipNotFound.RemoveAll();*/

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

            if (WriteToReg(reg_alllayers, JsonFile, value, RegistryValueKind.DWord))
            {
                foreach (Control c in panel.Controls)
                {
                    if (c is RichTextBox)
                    {
                        mainWindow.toolTipActive.Hide(panel);
                        mainWindow.toolTipActive.SetToolTip(c, "");

                        mainWindow.toolTipInactive.Hide(panel);
                        mainWindow.toolTipInactive.SetToolTip(c, "");
                    }
                }


                if (value == layer_inactive)
                {
                    mainWindow.toolTipInactive.Hide(panel);
                    mainWindow.toolTipInactive.SetToolTip(panel, "");

                    SetPanelInactive(panel, true);
                }   
                else
                {
                    mainWindow.toolTipActive.Hide(panel);
                    mainWindow.toolTipActive.SetToolTip(panel, "");

                    SetPanelActive(panel, true);
                }
            }
        }

        private static bool WriteToReg(string Key, string Name, string Value, RegistryValueKind Type)
        {
            RegistryKey regkey;

            if (Registry.LocalMachine.OpenSubKey(Key) != null)
            {
                try
                {
                    regkey = Registry.LocalMachine.OpenSubKey(Key, true);
                }
                catch
                {
                    MessageBox.Show("Couldn't open registry with write access!", "ERROR", MessageBoxButtons.OK);
                    return false;
                }

                try
                {
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\" + Key, Name, Value, Type);
                    return true;
                }
                catch
                {
                    MessageBox.Show("Couldn't write to registry!", "ERROR", MessageBoxButtons.OK);
                    return false;
                }
            }
            else
                return false;
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
    }
}
