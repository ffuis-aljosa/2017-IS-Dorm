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
using System.Security.Cryptography;

namespace StudentskiDom
{
    public partial class AddEmployee : Form
    {
        List<Employee> employeeList = new List<Employee>();
        List<string> usernameList = new List<string>();
        string connectionString;
        public AddEmployee()
        {
            InitializeComponent();
            connectionString = @"Data Source = database.db";
            Database baza = new Database();

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
                            Employee emplo = new Employee(rdr["Ime"].ToString(), rdr["Prezime"].ToString(),
                                rdr["Godiste"].ToString(), rdr["Pozicija"].ToString(), rdr["Username"].ToString(),
                                rdr["Password"].ToString());
                            employeeList.Add(emplo);
                            usernameList.Add(rdr["Username"].ToString());
                        }

                    }
                }
                con.Close();
            }

        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (firstNameTextBox.Text == "" || LastNameTextBox.Text == "" || textBox1.Text == "" ||
                passTextBox.Text == "" || confPassTextBox.Text == "" ||
                (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false))
            {
                MessageBox.Show("Niste unijeli sve podatke");
            }
            else
            {
                if(usernameList.Contains(textBox1.Text))
                {
                    MessageBox.Show("To korisnicko ime je vec zauzeto!");
                    
                }
                else
                {
                    if (passTextBox.Text != confPassTextBox.Text)
                    {
                        MessageBox.Show("Sifra i ponovljena sifra se ne poklapaju!");
                        
                    }
                    else
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

                                cmd.Parameters.Add(new SQLiteParameter("@username", textBox1.Text));
                                cmd.Parameters.Add(new SQLiteParameter("@password", getSHA1(passTextBox.Text)));
                                
                                int i = cmd.ExecuteNonQuery();
                                if (i == 0)
                                {
                                    MessageBox.Show("Created");
                                }
                                MessageBox.Show("Dodali ste zaposlenog " + firstNameTextBox.Text + " " + LastNameTextBox.Text);
                                usernameList.Add(textBox1.Text);
                                con.Close();

                                firstNameTextBox.Text = "";
                                LastNameTextBox.Text = "";
                                textBox1.Text = "";
                                passTextBox.Text = "";
                                confPassTextBox.Text = "";
                                radioButton1.Checked = false;
                                radioButton2.Checked = false;
                                radioButton3.Checked = false;

                                dateTimePicker1.Value = new DateTime(1993, 04, 16);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
          
         


        }

        private void AddEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            Visible = false;
        }

        public string getSHA1(string text)
        {
            SHA1CryptoServiceProvider sh = new SHA1CryptoServiceProvider();
            sh.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] re = sh.Hash;
            StringBuilder sb = new StringBuilder();
            foreach (byte b in re)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        private void AddEmployee_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = DateTime.Now;

            dateTimePicker1.Value = new DateTime(1993, 04, 16);
        }
    
    }     
}
