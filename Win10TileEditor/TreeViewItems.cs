using System;
using System.Collections;
using Aga.Controls.Tree;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Xml;

namespace Win10TileEditor
{
    public abstract class LinkBrowserTreeViewItem : TreeViewItem
        {
            public Image IconImage { get; set; }

            Uri _imageUrl = null;
            Image _image = null;
            TextBlock _textBlock = null;
            private DirectoryInfo subDirectory;

            public Uri ImageUrl
            {
                get { return _imageUrl; }
                set
                {
                    _imageUrl = value;
                    _image.Source = new BitmapImage(value);
                }
            }

            public string Text
            {
                get { return _textBlock.Text; }
                set { _textBlock.Text = value; }
            }

        }

        public class FolderViewItem : LinkBrowserTreeViewItem
        {
            public DirectoryInfo Directory { get; set; }

            public FolderViewItem(DirectoryInfo directory)
            {
                Directory = directory;
                //Header = directory.Name;
                CreateTreeViewItemTemplate();
            }

            private void CreateTreeViewItemTemplate()
            {
                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;

                var _image = new Image();
                _image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                _image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                _image.Width = 16;
                _image.Height = 16;
                _image.Margin = new Thickness(2);
                //_image.Source = (ImageSource)App.Current.Resources["icon_folder"];

                stack.Children.Add(_image);

                var _textBlock = new TextBlock();
                _textBlock.Margin = new Thickness(2);
                _textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                _textBlock.Text = Directory.Name;

                stack.Children.Add(_textBlock);

                Header = stack;
            }
        }

        public class LinkViewItem : LinkBrowserTreeViewItem
        {
            public FileInfo File { get; set; }

            public LinkViewItem(FileInfo file)
            {
                File = file;
                Header = file.Name + "  - Loading...";
                //CreateTreeViewItemTemplate();
            }

            private void CreateTreeViewItemTemplate()
            {
                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;

                var _image = new Image();
                _image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                _image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                _image.Width = 16;
                _image.Height = 16;
                _image.Margin = new Thickness(2);

                stack.Children.Add(_image);

                var _textBlock = new TextBlock();
                _textBlock.Margin = new Thickness(2);
                _textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                stack.Children.Add(_textBlock);

                Header = stack;
            }
            public void LoadShortcut(IWshRuntimeLibrary.WshShell shell)
            {
                Console.WriteLine("OPEN SHORTCUT: " + File.Name);
                IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut(File.FullName);
                //Description = shortcut.Description;
                //TargetPath = shortcut.TargetPath;
                //if (TargetPath != null && TargetPath.Length > 3)
                //{
                //    var file = new FileInfo(TargetPath.Substring(0, TargetPath.Length - 3) + "VisualElementsManifest.xml");
                //    ManifestData = new VisualManifest(file);
                //}
            }
        }

        public class VisualManifest
        {
            internal string Path
            {
                get
                {
                    if (FileName == null)
                        return null;
                    return this.FileName.DirectoryName;
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
            public FileInfo FileName { get; set; }
            public string image150Name { get; set; }
            public string image70Name { get; set; }

            public VisualManifest(FileInfo file)
            {
                this.FileName = file;
                loadFromFile();
            }

            public bool loadFromFile()
            {
                if (FileName == null || !FileName.Exists)
                    return false;
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(FileName.FullName);

                    var node = xml.DocumentElement.SelectSingleNode("/Application/VisualElements");
                    this.ColorText = node.Attributes["BackgroundColor"].Value;
                    this.ShowName = node.Attributes["ShowNameOnSquare150x150Logo"].InnerText == "on";
                    this.UseDarkText = node.Attributes["ForegroundText"].InnerText == "dark";
                    if (node.Attributes["Square150x150Logo"] != null)
                    {
                        this.image150Name = node.Attributes["Square150x150Logo"].InnerText;
                        this.image70Name = node.Attributes["Square70x70Logo"].InnerText;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to load The file '" + FileName + "'. VISUAL XML", "An Error has occured");
                    if (ex.Source != null)
                        Console.WriteLine("IOException source: {0}", ex.Source);
                    return false;
                }
            }
            public bool saveToFile()
            {

                if (FileName == null)
                    return false;
                try
                {
                    XmlDocument xml = new XmlDocument();
                    var app = xml.CreateElement("Application");
                    app.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    var element = xml.CreateElement("VisualElements");
                    app.AppendChild(element);
                    xml.AppendChild(app);

                    element.SetAttribute("BackgroundColor", this.ColorText);
                    element.SetAttribute("ShowNameOnSquare150x150Logo", this.ShowName ? "on" : "off");
                    element.SetAttribute("ForegroundText", this.UseDarkText ? "dark" : "light");

                    if (this.image150Name != null)
                    {
                        element.SetAttribute("Square150x150Logo", this.image150Name);

                        if (this.image70Name != null)
                            element.SetAttribute("Square70x70Logo", this.image70Name);
                        else
                            element.SetAttribute("Square70x70Logo", this.image150Name);
                    }
                    else if (this.image70Name != null)
                    {
                        element.SetAttribute("Square150x150Logo", this.image70Name);
                        element.SetAttribute("Square70x70Logo", this.image150Name);
                    }
                    xml.Save(this.FileName.FullName);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to load The file '" + FileName + "'. VISUAL XML", "An Error has occured");
                    if (ex.Source != null)
                        Console.WriteLine("IOException source: {0}", ex.Source);
                    return false;
                }
            }
        }
}