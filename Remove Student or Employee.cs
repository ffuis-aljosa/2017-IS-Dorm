using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

namespace StudentskiDom
{
    public partial class Remove_Student_or_Employee : Form
    {

        string connectionString = @"Data Source = database.db";
        public Remove_Student_or_Employee()
        {
            InitializeComponent();
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.SelectedIndex = -1;
            comboBox2.Enabled = true;
            
            if (comboBox1.SelectedItem.ToString() == "Student")
            {
                chooseLabel.Text = "Spisak studenata";
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();

                    string stm = "SELECT * FROM student";

                    using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (rdr["Soba"].ToString() == "")
                                {
                                    comboBox2.Items.Add(rdr["Id"].ToString() + " " + rdr["Ime"].ToString() +
                                        " " + rdr["Prezime"].ToString());
                                }

                            }

                        }
                    }

                    con.Close();
                }
            }
            else if (comboBox1.SelectedItem.ToString() == "Zaposleni")
            {
                chooseLabel.Text = "Spisak zaposlenih";
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();

                    string stm = "SELECT * FROM zaposleni";

                    using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                comboBox2.Items.Add(rdr["Ime"].ToString() + " " + rdr["Prezime"].ToString());
                            }

                        }
                    }

                    con.Close();
                }
                comboBox2.Items.Remove(Login.ulogovaniImePrezime);
            }

        }

        private void Remove_Student_or_Employee_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string connectionString = @"Data Source = database.db";
            if (comboBox1.SelectedIndex == 0)
            {
                try
                {
                    string[] idStudenta = comboBox2.Text.Split(' ', '\t');
                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.CommandText = "DELETE FROM student WHERE student.Id = " + idStudenta[0];
                        cmd.Connection = con;
                        con.Open();
                        int i = cmd.ExecuteNonQuery();

                        if (i == 1)
                        {
                            MessageBox.Show("Uspjesno uklonjen student " + idStudenta[1] + " " + idStudenta[2]);
                        }
                        comboBox2.SelectedIndex = -1;
                        comboBox2.Items.Remove(idStudenta[0]+" "+ idStudenta[1] + " "+ idStudenta[2]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                try
                {
                    string[] zaposlenik = comboBox2.Text.Split(' ', '\t');
                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.CommandText = "DELETE FROM zaposleni WHERE zaposleni.Ime = \'" + zaposlenik[0] + 
                            "\' AND zaposleni.Prezime = \'" + zaposlenik[1]+"\'";
                        cmd.Connection = con;
                        con.Open();
                        int i = cmd.ExecuteNonQuery();

                        if (i == 1)
                        {
                            MessageBox.Show("Uspjesno uklonjen zaposleni " + zaposlenik[0] + " " + zaposlenik[1]);
                        }
                        comboBox2.SelectedIndex = -1;

                        comboBox2.Items.Remove(zaposlenik[0] + " " + zaposlenik[1]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            comboBox2.SelectedIndex = -1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex == -1)
            button1.Enabled = false;
            else
            button1.Enabled = true;
        }
    }
}
