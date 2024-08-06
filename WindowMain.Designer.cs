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
            flowRuntimes = new FlowLayoutPanel();
            tableLayoutPanel = new TableLayoutPanel();
            flowLayers = new FlowLayoutPanel();
            buttonRefresh = new Button();
            toolTipRefresh = new ToolTip(components);
            tableLayoutPanel.SuspendLayout();
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
            // WindowMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 500);
            Controls.Add(buttonRefresh);
            Controls.Add(tableLayoutPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(800, 400);
            Name = "WindowMain";
            Text = "OpenXR Switcher";
            Load += WindowMain_Load;
            Shown += WindowMain_Shown;
            Resize += WindowMain_Resize;
            tableLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public ToolTip toolTipActive;
        public ToolTip toolTipInactive;
        public ToolTip toolTipNotFound;
        private FlowLayoutPanel flowButton;
        public FlowLayoutPanel flowRuntimes;
        private TableLayoutPanel tableLayoutPanel;
        public FlowLayoutPanel flowLayers;
        private Button buttonRefresh;
        public ToolTip toolTipRefresh;
    }
}
