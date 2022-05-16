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

namespace PictureDatabase
{
    public partial class Form1 : Form
    {
        public static readonly List<string> ImageExtensions = new List<string> { ".jpg", ".jpe", ".bmp", ".gif", ".png" };
        private SaveImage saveImageForm;
        PostgreSQL pSQL;
        private string savedFormCategory;
        private bool clickedSubmit;

        public Form1()
        {
            InitializeComponent();
            pSQL = new PostgreSQL();
            fillComboBox();
            filterComboBox.SelectedIndex = 0;


        }

        void Form_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the Dataformat of the data can be accepted
            // (we only accept file drops from Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) { 
                e.Effect = DragDropEffects.Copy; // Okay
            } else { 
                e.Effect = DragDropEffects.None; // Unknown data, ignore it
            }

            //if (ImageExtensions.Contains(Path.GetExtension(f).ToUpperInvariant()))
            //{
            
            //}

        }

        private void saveImage(object sender, DragEventArgs e)
        {
            string base64=null;
            

            // Extract the data from the DataObject-Container into a string list
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            string filePath=null;
            if (FileList.Length == 1 )
            {
                   filePath = FileList[0].ToString();
            }
            else
            {
                MessageBox.Show("Fehler: zu viele Dateien");
            }
            if (ImageExtensions.Contains(Path.GetExtension(filePath)))
            {
                //valid Image
                // save to DB Method
                byte[] imageInByte = File.ReadAllBytes(filePath);
                base64 = Convert.ToBase64String(imageInByte);
                saveImageForm = new SaveImage(base64);

                saveImageForm.FormClosed += new FormClosedEventHandler(this.forwasclosed);
                clickedSubmit = saveImageForm.ClickedSubmit;
                saveImageForm.ShowDialog();
                
                

                //PostgreSQL pSQL = new PostgreSQL();
                //pSQL.addToDB("minecraft","Game", base64);
            }


            pictureBox1.Image = Image.FromFile(filePath);
        }

        private void forwasclosed(object sender, FormClosedEventArgs e)
        {

            if (clickedSubmit) {
                savedFormCategory = saveImageForm.Category;
                filterComboBox.Text = savedFormCategory;
                clearAndPopulateList(filterComboBox.Text);
                listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                //listBox1.SelectedIndex = 0;
                //loadImageFromList();
            }
            
        }

        private void populateImageList(string category)
        {
            List<string> stringList = pSQL.getImageList(category);
            foreach (string item in stringList)
            {
                listBox1.Items.Add(item);
                //listBox1.SelectedIndex = 0;
            }
        }

        public void clearAndPopulateList(string category)
        {
            listBox1.Items.Clear();
            populateImageList(category);
        }

        public void loadImageFromList()
        {
            string iName = listBox1.SelectedItem.ToString();
            string iCategory = filterComboBox.Text;
            string iString = pSQL.loadImage(iName, iCategory);
            if (iString != null)
            {
                byte[] pic = Convert.FromBase64String(iString);

                using (MemoryStream ms = new MemoryStream(pic))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }

                label_pictureName.Text = iName;
                label_category.Text = iCategory;

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                loadImageFromList();
            }
            

        }

        private void fillComboBox()
        {
            string[] categories = pSQL.getCategories();
            foreach (string item in categories)
            {
                filterComboBox.Items.Add(item);
            }
        }

        private void filterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearAndPopulateList(filterComboBox.Text);
           
        }
    }
}
