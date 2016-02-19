using Aga.Controls.Tree;

namespace Win10TileEditor
{
    partial class FolderBrowser
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView = new TreeViewAdv();
            this.tcName = new TreeColumn();
            this.tcTarget = new TreeColumn();
            this.tcDate = new TreeColumn();
            this.ncIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.ncName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.ncSize = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.ncDate = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.AllowColumnReorder = true;
            this.treeView.AutoRowHeight = true;
            this.treeView.BackColor = System.Drawing.SystemColors.Window;
            this.treeView.Columns.Add(this.tcName);
            this.treeView.Columns.Add(this.tcTarget);
            this.treeView.Columns.Add(this.tcDate);
            this.treeView.DefaultToolTipProvider = null;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView.FullRowSelect = true;
            this.treeView.GridLineStyle = Aga.Controls.Tree.GridLineStyle.Horizontal;
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.LoadOnDemand = true;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Model = null;
            this.treeView.Name = "treeView";
            this.treeView.NodeControls.Add(this.ncIcon);
            this.treeView.NodeControls.Add(this.ncName);
            this.treeView.NodeControls.Add(this.ncSize);
            this.treeView.NodeControls.Add(this.ncDate);
            this.treeView.SelectedNode = null;
            this.treeView.ShowNodeToolTips = true;
            this.treeView.Size = new System.Drawing.Size(760, 327);
            this.treeView.TabIndex = 0;
            this.treeView.UseColumns = true;
            this.treeView.NodeMouseClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this._treeView_NodeMouseClick);
            this.treeView.ColumnClicked += new System.EventHandler<Aga.Controls.Tree.TreeColumnEventArgs>(this._treeView_ColumnClicked);
            this.treeView.SelectionChanged += new System.EventHandler(this._treeView_SelectionChanged);
            this.treeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this._treeView_MouseClick);
            // 
            // tcName
            // 
            this.tcName.Header = "Name";
            this.tcName.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.tcName.TooltipText = "File name";
            this.tcName.Width = 250;
            // 
            // tcTarget
            // 
            this.tcTarget.Header = "Target";
            this.tcTarget.SortOrder = System.Windows.Forms.SortOrder.None;
            this.tcTarget.TooltipText = "Link\'s Targeted File";
            this.tcTarget.Width = 410;
            // 
            // tcDate
            // 
            this.tcDate.Header = "Date";
            this.tcDate.SortOrder = System.Windows.Forms.SortOrder.None;
            this.tcDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tcDate.TooltipText = "File date";
            this.tcDate.Width = 70;
            // 
            // ncIcon
            // 
            this.ncIcon.DataPropertyName = "Icon";
            this.ncIcon.LeftMargin = 1;
            this.ncIcon.ParentColumn = this.tcName;
            this.ncIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // ncName
            // 
            this.ncName.DataPropertyName = "Name";
            this.ncName.IncrementalSearchEnabled = true;
            this.ncName.LeftMargin = 3;
            this.ncName.ParentColumn = this.tcName;
            this.ncName.Trimming = System.Drawing.StringTrimming.Character;
            this.ncName.UseCompatibleTextRendering = true;
            // 
            // ncSize
            // 
            this.ncSize.DataPropertyName = "TargetPath";
            this.ncSize.IncrementalSearchEnabled = true;
            this.ncSize.LeftMargin = 3;
            this.ncSize.ParentColumn = this.tcTarget;
            // 
            // ncDate
            // 
            this.ncDate.DataPropertyName = "Date";
            this.ncDate.IncrementalSearchEnabled = true;
            this.ncDate.LeftMargin = 3;
            this.ncDate.ParentColumn = this.tcDate;
            this.ncDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FolderBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView);
            this.Name = "FolderBrowser";
            this.Size = new System.Drawing.Size(760, 327);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewAdv treeView;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon ncIcon;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ncName;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ncSize;
        private Aga.Controls.Tree.NodeControls.NodeTextBox ncDate;
        private TreeColumn tcName;
        private TreeColumn tcTarget;
        private TreeColumn tcDate;
    }
}
