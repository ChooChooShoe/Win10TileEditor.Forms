using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenPainter.ColorPicker;
using Aga.Controls.Tree;

namespace Win10TileEditor
{
    public partial class MainForm : Form
    {
        public TileMaker tileMaker { get; set; }
        public string image70FileName { get; private set; }
        public Image image70Image { get; private set; }
        public string image150FileName { get; private set; }
        public Image image150Image { get; private set; }

        public MainForm(TileMaker tileMaker)
        {
            this.tileMaker = tileMaker;
            InitializeComponent();
            this.postLoadComponent();

        }

        private void postLoadComponent()
        {
            //ListDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms));
            //ListDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Programs));
            //this.treeViewAdv1.UseColumns = true;
        }

        internal void loadItemData(FileItem item)
        {

            this.textExePath.Text = item.TargetPath;
            bool clearImages = true;
            
            if(item.ManifestData != null)
            {
                if(item.ManifestData.UseDarkText)
                    this.radioButtonDarkText.Select();
                else
                    this.radioButtonLightText.Select();
                this.checkBoxShowName.Checked = item.ManifestData.ShowName;
                this.comboBoxColor.Text = item.ManifestData.ColorText;

                if (item.ManifestData.hasImageData)
                {
                    this.image150FileName = item.ManifestData.Path + item.ManifestData.image150Name;
                    this.text150ImagePath.Text = this.image150FileName;
                    this.pictureBox150.Image = LoadImageFile(this.image150FileName);
                    this.image70FileName = item.ManifestData.Path + item.ManifestData.image70Name;
                    this.text70ImagePath.Text = this.image70FileName;
                    this.pictureBox70.Image = LoadImageFile(this.image70FileName);
                    clearImages = false;
                }
            }
            if (clearImages)
            {
                this.image150FileName = null;
                this.text150ImagePath.Text = "";
                this.pictureBox150.Image = global::Win10TileEditor.Properties.Resources.NoIcon150x1501;
                this.image70FileName = null;
                this.text70ImagePath.Text = "";
                this.pictureBox70.Image = global::Win10TileEditor.Properties.Resources.NoIcon70x70;
            }
        }

        /*private void ListDirectory(string path)
        {
            var stack = new Stack<Node>();
            var rootDirectory = new DirectoryInfo(path);
            var node = ((TreeModel)(this.treeViewAdv1.Model)).Root;
            node.Tag = rootDirectory;
            stack.Push(node);

            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                var directoryInfo = (DirectoryInfo)currentNode.Tag;
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    var childDirectoryNode = new Node(directory.Name) { Tag = directory };
                    currentNode.Nodes.Add(childDirectoryNode);
                    stack.Push(childDirectoryNode);
                }
                foreach (var file in directoryInfo.GetFiles("*.lnk"))
                    currentNode.Nodes.Add(new Node(file.Name));
            }
        }*/


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Open 150x150 Image";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.image150FileName = openFileDialog.FileName;
                this.pictureBox150.Image = LoadImageFile(this.image150FileName);
            }
        }

        private void buttonImage70_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Open 70x70 Image";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.image70FileName = openFileDialog.FileName;
                this.pictureBox70.Image = LoadImageFile(this.image70FileName);
            }
        }

        private Image LoadImageFile(string fileName)
        {
            Image image = null;
            try
            {
                image = Image.FromFile(fileName);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to load The file '" + fileName + "'. Is it a valid image file?", "An Error has occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (ex.Source != null)
                    Console.WriteLine("IOException source: {0}", ex.Source);
            }
            return image;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox150_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background 
            //e.DrawBackground();

            // Get the item text    
            string text;
            if (e.Index >= 0)
                text = ((ComboBox)sender).Items[e.Index].ToString();
            else
                text = "NONE";

            e.Graphics.FillRectangle(new SolidBrush(Color.FromName(text)), e.Bounds.X + 55, e.Bounds.Y + 2, 100, e.Bounds.Height - 4);
            e.Graphics.DrawRectangle(new Pen(Color.Black, 1), e.Bounds.X + 55, e.Bounds.Y + 2, 100, e.Bounds.Height - 4);
            e.Graphics.DrawString(text, e.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
            //e.Graphics.DrawRectangle(new Pen(brush), e.Bounds);

            // Draw the focus rectangle if the mouse hovers over an item.
            //e.DrawFocusRectangle();

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (this.colorPicker == null || this.colorPicker.IsDisposed)
            {
                this.colorPicker = new ColorPickerForm(this.userSelectedColor);
                this.colorPicker.Show(this);
                this.colorPicker.ColorChange += new ColorPickerForm.ColorChangeEventHandler(frmColorPicker_ColorChange);

            }
        }

        private void frmColorPicker_ColorChange(ColorPickerForm sender, EventArgs e)
        {
            this.userSelectedColor = sender.PrimaryColor;
            this.pictureBox150.BackColor = userSelectedColor;
            this.pictureBox70.BackColor = userSelectedColor;
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupOptional_Enter(object sender, EventArgs e)
        {

        }

        private void textExePath_TextChanged(object sender, EventArgs e)
        {

        }

        private void listLinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            //object item = this.listLinks.SelectedItems[0].Text;
        }

        private void treeViewLinks_Click(object sender, EventArgs e)
        {
        }

        private void treeViewLinks_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 2;
        }
    }
}
