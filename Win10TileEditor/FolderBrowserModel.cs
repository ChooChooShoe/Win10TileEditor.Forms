using System;
using System.Collections.Generic;

using Aga.Controls.Tree;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Threading;
using IWshRuntimeLibrary;

namespace Win10TileEditor
{
	public class LinkBrowserModel: ITreeModel
	{
		private BackgroundWorker _worker;
        private List<BaseItem> _itemsToRead;
        private Stack<BaseItem> _DirsToReadStack;
        private Dictionary<string, List<BaseItem>> _knownDirsCache = new Dictionary<string, List<BaseItem>>();
        
        public LinkBrowserModel()
        {
            _itemsToRead = new List<BaseItem>();
            _DirsToReadStack = new Stack<BaseItem>();

            _worker = new BackgroundWorker();
			_worker.WorkerReportsProgress = true;
			_worker.DoWork += new DoWorkEventHandler(ReadFilesProperties);
			_worker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);



            //var rootDirectory = new DirectoryInfo(path);
            //var node = ((TreeModel)(this.treeViewAdv1.Model)).Root;
            //node.Tag = rootDirectory;
            //_itemsToReadStack.Push(node);
        }

		void ReadFilesProperties(object sender, DoWorkEventArgs e)
		{

            var shell = new IWshRuntimeLibrary.WshShell();
            
            /*
            while (_DirsToReadStack.Count > 0)
            {
                BaseItem currentNode = _DirsToReadStack.Pop();
                var directoryInfo = currentNode.DirectoryInfo;

                
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    var childDirectoryNode = new Node(directory.Name) { Tag = directory };
                    currentNode.Nodes.Add(childDirectoryNode);
                    _DirsToReadStack.Push(childDirectoryNode);

                }
                foreach (var file in directoryInfo.GetFiles("*.lnk"))
                    currentNode.Nodes.Add(new Node(file.Name));
            }*/

            while (_DirsToReadStack.Count > 0)
            {
                BaseItem item = _DirsToReadStack.Pop();
                
				if (item is FolderItem)
				{
					item.Date = item.DirectoryInfo.LastAccessTime;
				}
				else if (item is FileItem)
				{
					FileInfo info = new FileInfo(item.ItemPath);
					item.Size = info.Length;
					item.Date = info.CreationTime;
                    ((FileItem)item).LoadShortcut(shell);
				}
				_worker.ReportProgress(0, item);
			}
		}

		void ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			OnNodesChanged(e.UserState as BaseItem);
		}

		private TreePath GetPath(BaseItem item)
		{
			if (item == null)
				return TreePath.Empty;
			else
			{
				Stack<object> stack = new Stack<object>();
				while (item != null)
				{
					stack.Push(item);
					item = item.Parent;
				}
				return new TreePath(stack.ToArray());
			}
		}

		public System.Collections.IEnumerable GetChildren(TreePath treePath)
		{
            List<BaseItem> items = null;
			if (treePath.IsEmpty())
			{
				if (_knownDirsCache.ContainsKey("ROOT"))
					items = _knownDirsCache["ROOT"];
				else
				{
					items = new List<BaseItem>();
					_knownDirsCache.Add("ROOT", items);

                    //Root has poth Progarms and CommonPrograms
                    foreach (string programFolder in TileMaker.ProgramLinkFolders)
                    {
                        FolderItem root = new FolderItem(programFolder, null, new DirectoryInfo(programFolder), this);
                        buildFiles(items, root);
                    }

                    if (!_worker.IsBusy)
                        _worker.RunWorkerAsync();
                }
			}
			else
			{
				BaseItem parent = treePath.LastNode as BaseItem;
				if (parent != null)
				{
					if (_knownDirsCache.ContainsKey(parent.ItemPath))
						items = _knownDirsCache[parent.ItemPath];
					else
					{
						items = new List<BaseItem>();
                        buildFiles(items, parent);
                        _knownDirsCache.Add(parent.ItemPath, items);
						//_itemsToRead.AddRange(items);

                        if (!_worker.IsBusy)
							_worker.RunWorkerAsync();
					}
				}
            }
            //Thread.Sleep(2000); //emulate time consuming operation
            return items;
		}
        // Fills items with children (dirs and files) of the parent. One level deep.
        private List<BaseItem> buildFiles(List<BaseItem> items, BaseItem parent)
        {
            if (items == null)
                items = new List<BaseItem>();
            BaseItem item;
            foreach (var dir in parent.DirectoryInfo.GetDirectories())
            {
                item = new FolderItem(dir.FullName, parent, dir, this);
                items.Add(item);
                _DirsToReadStack.Push(item);
            }
            foreach (var file in parent.DirectoryInfo.GetFiles("*.lnk"))
            {
                item = new FileItem(file.FullName, parent, this);
                items.Add(item);
                _DirsToReadStack.Push(item);
            }
            return items;
        }

        public bool IsLeaf(TreePath treePath)
		{
			return treePath.LastNode is FileItem;
		}

		public event EventHandler<TreeModelEventArgs> NodesChanged;
		internal void OnNodesChanged(BaseItem item)
		{
			if (NodesChanged != null)
			{
				TreePath path = GetPath(item.Parent);
				NodesChanged(this, new TreeModelEventArgs(path, new object[] { item }));
			}
		}

		public event EventHandler<TreeModelEventArgs> NodesInserted;
		public event EventHandler<TreeModelEventArgs> NodesRemoved;
		public event EventHandler<TreePathEventArgs> StructureChanged;
		public void OnStructureChanged()
		{
			if (StructureChanged != null)
				StructureChanged(this, new TreePathEventArgs());
		}
	}
}
