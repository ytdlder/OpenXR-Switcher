namespace OpenXR_Switcher
{
    partial class WindowMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowMain));
            toolTipActive = new ToolTip(components);
            toolTipInactive = new ToolTip(components);
            toolTipNotFound = new ToolTip(components);
            toolTipRefresh = new ToolTip(components);
            toolTipEdit = new ToolTip(components);
            toolTipReset = new ToolTip(components);
            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.SuspendLayout();
            flowRuntimes = new FlowLayoutPanel();
            flowLayers = new FlowLayoutPanel();
            buttonRefresh = new Button();
            panelEdit = new Panel();
            panelEdit.SuspendLayout();
            labelEdit = new System.Windows.Forms.Label();
            buttonReset = new Button();
            maskedTextBox = new MaskedTextBox();
            buttonSave = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // toolTipActive
            // 
            toolTipActive.ToolTipIcon = ToolTipIcon.Info;
            toolTipActive.ToolTipTitle = "Active";
            // 
            // toolTipInactive
            // 
            toolTipInactive.ToolTipIcon = ToolTipIcon.Info;
            toolTipInactive.ToolTipTitle = "Inactive";
            // 
            // toolTipNotFound
            // 
            toolTipNotFound.ToolTipIcon = ToolTipIcon.Error;
            toolTipNotFound.ToolTipTitle = "Disabled";
            // 
            // flowRuntimes
            // 
            flowRuntimes.AutoScroll = true;
            flowRuntimes.Dock = DockStyle.Fill;
            flowRuntimes.Location = new Point(3, 3);
            flowRuntimes.Name = "flowRuntimes";
            flowRuntimes.Size = new Size(494, 494);
            flowRuntimes.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Controls.Add(flowRuntimes, 0, 0);
            tableLayoutPanel.Controls.Add(flowLayers, 1, 0);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel.Size = new Size(1000, 500);
            tableLayoutPanel.TabIndex = 0;
            // 
            // flowLayers
            // 
            flowLayers.AutoScroll = true;
            flowLayers.Dock = DockStyle.Fill;
            flowLayers.Location = new Point(503, 3);
            flowLayers.Name = "flowLayers";
            flowLayers.Size = new Size(494, 494);
            flowLayers.TabIndex = 1;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonRefresh.AutoSize = true;
            buttonRefresh.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            buttonRefresh.BackColor = Color.LightSkyBlue;
            buttonRefresh.Cursor = Cursors.Help;
            buttonRefresh.FlatAppearance.BorderColor = Color.RoyalBlue;
            buttonRefresh.FlatAppearance.MouseDownBackColor = Color.DodgerBlue;
            buttonRefresh.FlatStyle = FlatStyle.Flat;
            buttonRefresh.Location = new Point(922, 6);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(58, 27);
            buttonRefresh.TabIndex = 0;
            buttonRefresh.TabStop = false;
            buttonRefresh.Text = "Refresh";
            toolTipRefresh.SetToolTip(buttonRefresh, "Reload data from Registry!");
            buttonRefresh.UseVisualStyleBackColor = false;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // panelEdit
            // 
            panelEdit.AutoSize = true;
            panelEdit.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panelEdit.BackColor = SystemColors.Highlight;
            panelEdit.BorderStyle = BorderStyle.FixedSingle;
            panelEdit.CausesValidation = false;
            panelEdit.Controls.Add(labelEdit);
            panelEdit.Controls.Add(buttonReset);
            panelEdit.Controls.Add(maskedTextBox);
            panelEdit.Controls.Add(buttonCancel);
            panelEdit.Controls.Add(buttonSave);
            panelEdit.Location = new Point(148, 302);
            panelEdit.Name = "panelEdit";
            panelEdit.Padding = new Padding(5, 5, 2, 2);
            panelEdit.Size = new Size(444, 63);
            panelEdit.Visible = false;
            // 
            // labelEdit
            // 
            labelEdit.Dock = DockStyle.Top;
            labelEdit.Enabled = false;
            labelEdit.FlatStyle = FlatStyle.Flat;
            labelEdit.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelEdit.ForeColor = Color.White;
            labelEdit.Location = new Point(5, 5);
            labelEdit.Margin = new Padding(0);
            labelEdit.Name = "labelEdit";
            labelEdit.Size = new Size(435, 23);
            labelEdit.Text = "Enter a new custom name:";
            // 
            // buttonReset
            // 
            buttonReset.Location = new Point(343, 3);
            buttonReset.Name = "buttonReset";
            buttonReset.Size = new Size(60, 23);
            buttonReset.TabIndex = 1003;
            buttonReset.Text = "Reset";
            buttonReset.UseVisualStyleBackColor = true;
            toolTipReset.SetToolTip(buttonReset, "Change the display name back to its default");
            buttonReset.BringToFront();
            buttonReset.Click += evtbuttonReset_Click;
            // 
            // maskedTextBox
            // 
            maskedTextBox.Anchor = AnchorStyles.Left;
            maskedTextBox.AsciiOnly = true;
            maskedTextBox.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            maskedTextBox.HidePromptOnLeave = true;
            maskedTextBox.Location = new Point(5, 29);
            maskedTextBox.Name = "maskedTextBox";
            maskedTextBox.Size = new Size(300, 27);
            maskedTextBox.TabIndex = 1000;
            maskedTextBox.KeyDown += new KeyEventHandler(maskedTextBox_KeyDown);
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(311, 29);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(60, 27);
            buttonSave.TabIndex = 1001;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += evtbuttonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(377, 29);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(60, 27);
            buttonCancel.TabIndex = 1002;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += evtbuttonCancel_Click;
            // 
            // WindowMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 500);
            Controls.Add(buttonRefresh);
            Controls.Add(tableLayoutPanel);
            Controls.Add(panelEdit);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(800, 400);
            Name = "WindowMain";
            Text = "OpenXR Switcher";
            Load += WindowMain_Load;
            Shown += WindowMain_Shown;
            Resize += WindowMain_Resize;
            tableLayoutPanel.ResumeLayout(false);
            panelEdit.ResumeLayout(false);
            panelEdit.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public ToolTip toolTipActive;
        public ToolTip toolTipInactive;
        public ToolTip toolTipNotFound;
        public ToolTip toolTipRefresh;
        public ToolTip toolTipEdit;
        public ToolTip toolTipReset;
        public TableLayoutPanel tableLayoutPanel;
        public FlowLayoutPanel flowRuntimes;
        public FlowLayoutPanel flowLayers;
        public Panel panelEdit;
        public MaskedTextBox maskedTextBox;
        public Button buttonRefresh;
        public Button buttonSave;
        private Button buttonCancel;
        private Button buttonReset;
        private System.Windows.Forms.Label labelEdit;
    }
}
