using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureDatabase
{
    public partial class SaveImage : Form
    {
        private string name;
        private string category;
        private string sBase64;
        
        PostgreSQL pSQL;
        private bool clickedSubmit;

        public string Category { get => category; set => category = value; }
        public bool ClickedSubmit { get => clickedSubmit; set => clickedSubmit = value; }

        public SaveImage(string sBase64)
        {
            InitializeComponent();
            pSQL = new PostgreSQL();
            this.sBase64 = sBase64;
            fillComboBox();
            ClickedSubmit = false;


        }

        private void fillComboBox()
        {
            string[] categories = pSQL.getCategories();
                foreach (string item in categories)
                {
                    comboBox1.Items.Add(item);
                } 
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            ClickedSubmit = true;
            Category = "";
            name = textBox1.Text;
            Category = comboBox1.Text.Trim();
            bool isNameInCategory = pSQL.isNameInCategory(name, Category);
            if (isNameInCategory)
                labelError.Text = "Error: Name already exists in Category.Please enter another.";
            if(Category==null || Category=="")
                labelError.Text = "Error: Category can't be empty.Please enter one.";
            if (name!=null && Category!=null && !isNameInCategory && Category != "")
            {
                pSQL.addToDB(name, Category, sBase64);
                this.Close();
            }
            
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            labelError.Text = "";
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            labelError.Text = "";
        }

        //private void SaveImage_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    if (string.Equals((sender as SaveImage).Name, @"CloseButton"))
        //    {
        //        e.Cancel = true;
        //    }

        //}
    }
}
