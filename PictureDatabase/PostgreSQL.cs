using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace PictureDatabase
{
    class PostgreSQL
    {
        NpgsqlConnection conn;
        string connectionString = "Server=10.8.0.129; Port=5432; User Id=Denis; Password=Start1234; Database=postgres";

        public PostgreSQL()
        {
            conn = new NpgsqlConnection(connectionString);
        }

        public void addToDB(string name, string category, string base64)
        {
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                NpgsqlCommand command = conn.CreateCommand();
                string c = "select i_add_Table_Data('" + name+"','" + category + "','" + base64 + "')";
                command.CommandText = c;

                // Execute the procedure and obtain a result set
                NpgsqlDataReader dr = command.ExecuteReader();
                //while (dr.Read())
                //{
                //    result.Add(dr.GetString(0));
                //}
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fehler bei addToDB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conn.Close();
        }

        public string[] getCategories()
        {
            List<string> result = new List<string>();
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                NpgsqlCommand command = conn.CreateCommand();
                string c = "select i_get_Table_categories()";
                command.CommandText = c;

                // Execute the procedure and obtain a result set
                NpgsqlDataReader dr = command.ExecuteReader();
                
                while (dr.Read())
                {
                    string a = dr.GetString(0);
                    //string[] a = dr.GetString(0);

                    result.Add(a);
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fehler bei getCategories()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            conn.Close();
            return result.ToArray();
        }

        public bool isNameInCategory(string name, string category)
        {
            int a=-1;
            bool b = false;
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                NpgsqlCommand command = conn.CreateCommand();
                string c = "select i_get_int_isNameAlreadyInCategory('"+name+"','" + category + "')";
                command.CommandText = c;

                // Execute the procedure and obtain a result set
                NpgsqlDataReader dr = command.ExecuteReader();
                if(dr.HasRows)
                while (dr.Read())
                {
                    //a = (int)Convert.ToInt32(dr.GetInt32(0));
                    b = dr.GetBoolean(0);
                    //string[] a = dr.GetString(0);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fehler bei isNameInCategory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conn.Close();
            Debug.WriteLine(a);
            if (a==1)
            {
                //return true;
            }
            //return false;
            return b;
        }

        public List<string> getImageList(string category)
        {
            List<string> result = new List<string>();
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                NpgsqlCommand command = conn.CreateCommand();
                string c = "select i_get_Table_NamesInCategory('"+ category+ "')";
                command.CommandText = c;

                // Execute the procedure and obtain a result set
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(dr.GetString(0));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fehler bei getImageList", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conn.Close();
            return result;
        }

        public string loadImage(string name, string category)
        {
            string iString = null;
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            try
            {
                NpgsqlCommand command = conn.CreateCommand();
                string c = "select i_get_ImageString('" + name + "','" + category + "')";
                command.CommandText = c;

                // Execute the procedure and obtain a result set
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    iString = dr.GetString(0);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fehler bei loadImage", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conn.Close();
            return iString;
        }
    }
}
