﻿using System;
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
    public partial class AddStudent : Form
    {

        string connectionString;

        public AddStudent()
        {
            InitializeComponent();
            connectionString = @"Data Source = database.db";

            
        }
        private void AddStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            Visible = false;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (firstNameTextBox.Text == "" || LastNameTextBox.Text == "" || comboBox1.Text == "" 
                || comboBox2.Text == "" || (radioButton1.Checked ==false && radioButton2.Checked == false))
            {
                MessageBox.Show("Niste unijeli sve podatke");

            }
            else
            {
                try
                {
                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.CommandText = @"INSERT INTO student (Ime, Prezime, Godiste, Pol, Fakultet,
                                            GodinaStudija)
                                     VALUES (@name, @surname, @dateofbirth, @gender, @fakulty, @year)";
                        cmd.Connection = con;
                        cmd.Parameters.Add(new SQLiteParameter("@name", firstNameTextBox.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@surname", LastNameTextBox.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@dateofbirth", dateTimePicker1.Text));
                        if (radioButton1.Checked)
                        {
                            cmd.Parameters.Add(new SQLiteParameter("@gender", radioButton1.Text));
                        }
                        else if (radioButton2.Checked)
                        {
                            cmd.Parameters.Add(new SQLiteParameter("@gender", radioButton2.Text));
                        }
                        cmd.Parameters.Add(new SQLiteParameter("@fakulty", comboBox1.Text));
                        cmd.Parameters.Add(new SQLiteParameter("@year", comboBox2.Text));

                        con.Open();
                        int i = cmd.ExecuteNonQuery();
                        if (i == 0)
                        {
                            MessageBox.Show("Created");
                        }
                        MessageBox.Show("Dodali ste studenta " + firstNameTextBox.Text + " " + LastNameTextBox.Text);

                        firstNameTextBox.Text = "";
                        LastNameTextBox.Text = "";
                        comboBox1.SelectedIndex = -1;
                        comboBox2.SelectedIndex = -1;
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        dateTimePicker1.Value = new DateTime(1993, 04, 16);
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void AddStudent_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = DateTime.Now;

            dateTimePicker1.Value = new DateTime(1993, 04, 16);
        }
    }
    }
