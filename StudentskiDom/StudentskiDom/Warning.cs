using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace StudentskiDom
{
    public partial class Warning : Form
    {
        public static string warn = "";
        string connectionString = @"Data Source = database.db";

        public Warning()
        {  
                InitializeComponent();
            warningTextBox.Text = Form1.opomena;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source = database.db";
            try
            {
;
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.CommandText = "UPDATE student SET Opomena = @warn WHERE Id=" + Form1.warningStudId;
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SQLiteParameter("@warn", warningTextBox.Text));

                    con.Open();
                    
                    int i = cmd.ExecuteNonQuery();

                    if (i == 1)
                    {
                        MessageBox.Show("Uspjesno dodana opomena studentu");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Warning_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            warningTextBox.Text = Form1.opomena;
        }
    }
}
