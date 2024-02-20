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
            tableLayoutPanel = new TableLayoutPanel();
            flowRuntimes = new FlowLayoutPanel();
            flowLayers = new FlowLayoutPanel();
            toolTipActive = new ToolTip(components);
            toolTipInactive = new ToolTip(components);
            toolTipNotFound = new ToolTip(components);
            tableLayoutPanel.SuspendLayout();
            SuspendLayout();
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
            tableLayoutPanel.Size = new Size(984, 511);
            tableLayoutPanel.TabIndex = 0;
            // 
            // flowRuntimes
            // 
            flowRuntimes.AutoScroll = true;
            flowRuntimes.Dock = DockStyle.Fill;
            flowRuntimes.Location = new Point(3, 3);
            flowRuntimes.Name = "flowRuntimes";
            flowRuntimes.Size = new Size(486, 505);
            flowRuntimes.TabIndex = 0;
            // 
            // flowLayers
            // 
            flowLayers.AutoScroll = true;
            flowLayers.Dock = DockStyle.Fill;
            flowLayers.Location = new Point(495, 3);
            flowLayers.Name = "flowLayers";
            flowLayers.Size = new Size(486, 505);
            flowLayers.TabIndex = 1;
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
            // WindowMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 511);
            Controls.Add(tableLayoutPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(800, 400);
            Name = "WindowMain";
            Text = "OpenXR Switcher";
            Load += WindowMain_Load;
            tableLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel;
        public FlowLayoutPanel flowRuntimes;
        public FlowLayoutPanel flowLayers;
        public ToolTip toolTipActive;
        public ToolTip toolTipInactive;
        public ToolTip toolTipNotFound;
    }
}
