using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.SQLite;
using System.IO;

namespace StudentskiDom
{
    public partial class AddEmployee : Form
    {

        string connectionString;
        public AddEmployee()
        {
            InitializeComponent();
            connectionString = @"Data Source = database.db";
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (firstNameTextBox.Text != "" && LastNameTextBox.Text != "")
            {
                try
                {
                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        con.Open();
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.CommandText = @"INSERT INTO zaposleni (Ime, Prezime, Godiste, Pozicija, 
                                                                   Username, Password)
                                     VALUES (@name, @surname, @dateofbirth, @position, @username, @password)";
                        cmd.Connection = con;
                        cmd.Parameters.Add(new SQLiteParameter("@name", firstNameTextBox.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@surname", LastNameTextBox.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@dateofbirth", dateTimePicker1.Text));

                        if (radioButton1.Checked)
                        {
                            cmd.Parameters.Add(new SQLiteParameter("@position", radioButton1.Text));
                        }
                        else if (radioButton2.Checked)
                        {
                            cmd.Parameters.Add(new SQLiteParameter("@position", radioButton2.Text));
                        }
                        else if (radioButton3.Checked)
                        {
                            cmd.Parameters.Add(new SQLiteParameter("@position", radioButton3.Text));
                        }

                        string stm = "SELECT * FROM student";

                        

                        cmd.Parameters.Add(new SQLiteParameter("@username", textBox1.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@password", passTextBox.Text));

                        //con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i == 0)
                        {
                            MessageBox.Show("Created");
                        }
                        MessageBox.Show("Dodali ste zaposlenog " + firstNameTextBox.Text + " " + LastNameTextBox.Text);
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
                MessageBox.Show("Niste unijeli ispravne podatke");

            if (passTextBox.Text != confPassTextBox.Text)
            {
                wrongPassBtn.Visible = true;
            } 
            else
            {
                wrongPassBtn.Visible = false;
            }
        }

        private void AddEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            Visible = false;
        }
    }
}
