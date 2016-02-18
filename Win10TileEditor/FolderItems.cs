using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Win10TileEditor
{
	public abstract class BaseItem
    {
        public DirectoryInfo DirectoryInfo { get; set; }
        public string ItemPath { get; set; }
		public Image Icon { get; set; }
		public long Size { get; set; }
		public DateTime Date { get; set; }
        public BaseItem Parent { get; set; }
        public LinkBrowserModel Owner { get; set; }
        public string TargetPath { get; set; }

        public abstract string Name { get; set; }

        private bool isChecked;
        public bool IsChecked
		{
			get { return isChecked; }
			set 
			{
                isChecked = value;
				if (Owner != null)
					Owner.OnNodesChanged(this);
			}
		}

        public override bool Equals(object obj)
		{
			if (obj is BaseItem)
				return ItemPath.Equals((obj as BaseItem).ItemPath);
			else
				return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return ItemPath.GetHashCode();
		}

        public override string ToString()
        {
            return ItemPath;
        }
	}

    [Obsolete]
    public class RootItem : BaseItem
    {
        
        public RootItem(string name, DirectoryInfo directoryInfo, LinkBrowserModel owner)
        {
            ItemPath = name;
            DirectoryInfo = directoryInfo;
            Owner = owner;
        }

        public override string Name
		{
			get
			{
				return ItemPath;
			}
			set
			{
			}
		}
	}

	public class FolderItem : BaseItem
    {
        public override string Name
        {
            get
            {
                return Path.GetFileName(ItemPath);
            }
            set
            {
                string dir = Path.GetDirectoryName(ItemPath);
                string destination = Path.Combine(dir, value);
                Directory.Move(ItemPath, destination);
                ItemPath = destination;
            }
        }

        public FolderItem(string name, BaseItem parent, DirectoryInfo directoryInfo, LinkBrowserModel owner)
		{
			ItemPath = name;
            Parent = parent;
            DirectoryInfo = directoryInfo;
            Owner = owner;
		}
	}

	public class FileItem : BaseItem
	{
		public override string Name
		{
			get
			{
				return Path.GetFileName(ItemPath);
			}
			set
			{
				string dir = Path.GetDirectoryName(ItemPath);
				string destination = Path.Combine(dir, value);
				File.Move(ItemPath, destination);
				ItemPath = destination;
			}
		}

        public string Description { get; set; }
        public VisualManifest ManifestData { get; internal set; }

        public FileItem(string name, BaseItem parent, LinkBrowserModel owner)
		{
			ItemPath = name;
			Parent = parent;
			Owner = owner;
		}
        public void LoadShortcut(IWshRuntimeLibrary.WshShell shell)
        {
            Console.WriteLine("OPEN SHORTCUT "+ ItemPath);
            IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut(ItemPath);
            Description = shortcut.Description;
            TargetPath = shortcut.TargetPath;
            var file = TargetPath.Substring(0, TargetPath.Length-3) + "visualelementsmanifest.xml";
            Console.WriteLine(file);
            if(File.Exists(file))
                ManifestData = new VisualManifest(file);
        }
	}

    public class VisualManifest
    {
        internal string Path
        {
            get
            {
                return this.File.Substring(0,File.LastIndexOf('\\')+1);
            }
            set { }
        }

        public bool hasImageData
        {
            get
            {
                return image150Name != null && image70Name != null;
            }
            set { }
        }

        public bool ShowName { get; set; }
        public bool UseDarkText { get; set; }
        public string ColorText { get; set; }
        public string File { get; set; }
        public string image150Name { get; set; }
        public string image70Name { get; set; }

        public VisualManifest (string file)
        {
            this.File = file;
            loadFromFile();
        }

        private void loadFromFile()
        {
            try {
                XmlDocument xml = new XmlDocument();
                xml.Load(File);

                var node = xml.DocumentElement.SelectSingleNode("/Application/VisualElements");
                this.ColorText = node.Attributes["BackgroundColor"].Value;
                this.ShowName = node.Attributes["ShowNameOnSquare150x150Logo"].InnerText == "on";
                this.UseDarkText = node.Attributes["ForegroundText"].InnerText == "dark";
                if(node.Attributes["Square150x150Logo"] != null)
                {
                    this.image150Name = node.Attributes["Square150x150Logo"].InnerText;
                    this.image70Name = node.Attributes["Square70x70Logo"].InnerText;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Unable to load The file '" + File + "'. VISUAL XML", "An Error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (ex.Source != null)
                    Console.WriteLine("IOException source: {0}", ex.Source);
            }
        }
    }
}
