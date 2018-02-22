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
    public partial class Login : Form
    {
        List<Employee> employeeList = new List<Employee>();
        string connectionString = @"Data Source = database.db";
        public Login()
        {
            InitializeComponent();
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
                        }

                    }
                }
                con.Close();
            }
        }
        public static string dopustenja;
        public static string ulogovaniImePrezime;

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void LoginBtn_Click(object sender, MouseEventArgs e)
        {
            string username = "";
            string password = "";
            for(int i = 0; i < employeeList.Count(); i++)
            {
                if (employeeList[i].Username == userNameTextBox.Text)
                {
                    username = employeeList[i].Username;
                    password = employeeList[i].Password;
                    dopustenja = employeeList[i].Position;
                    ulogovaniImePrezime = employeeList[i].FirstName + " " + employeeList[i].LastName;
                }
            }
             
            if (userNameTextBox.Text == "" || passwordTextBox.Text == "")
            {
                MessageBox.Show("Neispravni podaci");
            }
            else if (userNameTextBox.Text == username && getSHA1(passwordTextBox.Text) == password)
            {
                
                Form1 f1 = new Form1();
                f1.Show();
                Visible = false;
            }
            else
            {
                MessageBox.Show("Neispravni podaci");
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                passwordTextBox.UseSystemPasswordChar = false;
            }
            else
                passwordTextBox.UseSystemPasswordChar = true;
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

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string username = "";
                string password = "";
                for (int i = 0; i < employeeList.Count(); i++)
                {
                    if (employeeList[i].Username == userNameTextBox.Text)
                    {
                        username = employeeList[i].Username;
                        password = employeeList[i].Password;
                        dopustenja = employeeList[i].Position;
                        ulogovaniImePrezime = employeeList[i].FirstName + " " + employeeList[i].LastName;
                    }
                }

                if (userNameTextBox.Text == "" || passwordTextBox.Text == "")
                {
                    MessageBox.Show("Neispravni podaci");
                }
                else if (userNameTextBox.Text == username && getSHA1(passwordTextBox.Text) == password)
                {

                    Form1 f1 = new Form1();
                    f1.Show();
                    Visible = false;
                }
                else
                {
                    MessageBox.Show("Nespravni podaci");
                }
            }
        }
    }
}
