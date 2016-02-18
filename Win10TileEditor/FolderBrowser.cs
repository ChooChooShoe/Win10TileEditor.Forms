using System;
using System.ComponentModel;
using System.Windows.Forms;

using Aga.Controls.Tree.NodeControls;
using Aga.Controls.Tree;

namespace Win10TileEditor
{
	public partial class FolderBrowser : UserControl
	{
		private class ToolTipProvider: IToolTipProvider
		{
			public string GetToolTip(TreeNodeAdv node, NodeControl nodeControl)
			{
				if (node.Tag is RootItem)
					return null;
				else
					return "Second click to rename node";
			}
		}

		public FolderBrowser()
		{
			InitializeComponent();

            //cboxGrid.DataSource = System.Enum.GetValues(typeof(GridLineStyle));
			//cboxGrid.SelectedItem = GridLineStyle.HorizontalAndVertical;

            //cbLines.Checked = _treeView.ShowLines;

			ncName.ToolTipProvider = new ToolTipProvider();
			ncName.EditorShowing += new CancelEventHandler(_name_EditorShowing);

			treeView.Model = new SortedTreeModel(new LinkBrowserModel());

		}

		void _name_EditorShowing(object sender, CancelEventArgs e)
		{
			if (treeView.CurrentNode.Tag is RootItem)
				e.Cancel = true;
		}

		private void _treeView_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				NodeControlInfo info = treeView.GetNodeControlInfoAt(e.Location);
				if (info.Control != null)
				{
					Console.WriteLine(info.Bounds);
				}
			}
		}

		private void _treeView_ColumnClicked(object sender, TreeColumnEventArgs e)
		{
            TreeColumn clicked = e.Column;
            if (clicked.SortOrder == SortOrder.Ascending)
                clicked.SortOrder = SortOrder.Descending;
            else
                clicked.SortOrder = SortOrder.Ascending;

            (treeView.Model as SortedTreeModel).Comparer = new FolderItemSorter(clicked.Header, clicked.SortOrder);
		}
        
		private void _treeView_NodeMouseClick(object sender, TreeNodeAdvMouseEventArgs e)
		{
			if(e.Node.Tag is FileItem)
            {
                FileItem item = (FileItem)e.Node.Tag;
                ((MainForm)this.ParentForm).loadItemData(item);


            }
		}

        private void _treeView_SelectionChanged(object sender, EventArgs e)
        {
            
        }
    }
}
