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
using System.Threading;
using System.Security.Cryptography;

namespace StudentskiDom
{
    public partial class Form1 : Form
    {
        List<Student> studentList = new List<Student>();
        string connectionString = @"Data Source = database.db";
        public static string warningStudId = "";
        public static string studentWarning;
        public static string opomena;
        public static List<string> opomene = new List<string>();
        public static int brojStudenata ;
        List<string> spisakStudenata = new List<string>();
        
        public Form1()
        {
            InitializeComponent();
            brojStudenata = 0;
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
                            int i = 0;
                            Student stud = new Student(rdr["Ime"].ToString(), rdr["Prezime"].ToString(),
                                rdr["Godiste"].ToString(), rdr["Pol"].ToString(), rdr["Fakultet"].ToString(),
                                rdr["GodinaStudija"].ToString());
                            stud.Id = rdr["Id"].ToString();
                            stud.Room = rdr["Soba"].ToString();
                            stud.Warning = rdr["Opomena"].ToString();
                            studentList.Add(stud);
                            brojStudenata++;
                            searchComboBox.Items.Add(stud.Id + " " + stud.FirstName + " " +stud.LastName);
                            spisakStudenata.Add(stud.FirstName + " " + stud.LastName);
                            if (rdr["Soba"].ToString() == "")
                                comboBox1.Items.Add(stud.Id + " " +stud.FirstName + " " + stud.LastName);

                            if (rdr["Soba"].ToString() != "")
                            {
                                Button btn = this.Controls.Find(rdr["Soba"].ToString(), true).FirstOrDefault() as Button;
                                btn.Text = rdr["Id"].ToString() + " " + rdr["Ime"].ToString() + " " + rdr["Prezime"].ToString();
                            }
                            
                        }

                    }
                }

                con.Close();
            }
        }
      
        private void floorDownBtn_Click(object sender, EventArgs e)
        {
            if (currentFlorlabel.Text == "1")
            {
                MessageBox.Show("Na prvom ste spratu!");
            }
            else if (currentFlorlabel.Text == "2")
            {
                sprat2.Visible = false;
                sprat1.Visible = true;
                currentFlorlabel.Text = "1";
            }
            else if (currentFlorlabel.Text == "3")
            {
                sprat3.Visible = false;
                sprat2.Visible = true;
                currentFlorlabel.Text = "2";
            }
            
            
        }

        private void floorUpBtn_Click(object sender, EventArgs e)
        {
            if (currentFlorlabel.Text == "1")
            {
                sprat1.Visible = false;
                sprat2.Visible = true;
                currentFlorlabel.Text = "2";
            }
            else if (currentFlorlabel.Text == "2")
            {
                sprat2.Visible = false;
                sprat3.Visible = true;
                currentFlorlabel.Text = "3";
            }
            else if (currentFlorlabel.Text == "3")
            {
                MessageBox.Show("Na poslednjem ste spratu!");
            }
            
        }
        
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void moveStudentBtn_MouseDown(object sender, MouseEventArgs e)
        {
            moveStudentBtn.DoDragDrop(moveStudentBtn.Text, DragDropEffects.Copy);
        }

        private void button_DragDrop(object sender, DragEventArgs e)
        {
            Button btn = (Button)sender;
            string pol = "";
            int brojZauzetihKreveta = 0;
            string[] idStudenta = moveStudentBtn.Text.Split(' ', '\t');
            if (btn.Text == "")
            {

                try
                {

                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        con.Open();
                        string stm = "SELECT * FROM Student WHERE Student.Soba LIKE '%" + btn.Name.Substring(btn.Name.Length - 3) + "'";
                        using (SQLiteCommand cmds = new SQLiteCommand(stm, con))
                        {
                            using (SQLiteDataReader rdr = cmds.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    pol = rdr["Pol"].ToString();
                                    brojZauzetihKreveta++;
                                }
                            }
                        }
                        if(brojZauzetihKreveta > 0)
                        {
                            if (genderLabel.Text == pol)
                            {

                                btn.Text = (string)e.Data.GetData(DataFormats.Text);
                                SQLiteCommand cmd = new SQLiteCommand();
                                cmd.CommandText = "UPDATE student SET Soba = @brsobe WHERE Id=" + idStudenta[0];
                                cmd.Connection = con;
                                cmd.Parameters.Add(new SQLiteParameter("@brsobe", btn.Name));



                                int i = cmd.ExecuteNonQuery();

                                if (i == 1)
                                {
                                    MessageBox.Show("Uspjesno prebacen student");
                                }
                                roomLabel.Text = btn.Name.Substring(btn.Name.Length - 3);
                                comboBox1.Items.Remove((string)e.Data.GetData(DataFormats.Text));
                                comboBox1.Text = "";
                                moveStudentBtn.Enabled = false;
                            }
                            else
                            {
                                MessageBox.Show("Ne mogu muskarci i djevojke u istu sobu");
                            }
                        }
                        else
                        {
                            btn.Text = (string)e.Data.GetData(DataFormats.Text);
                            SQLiteCommand cmd = new SQLiteCommand();
                            cmd.CommandText = "UPDATE student SET Soba = @brsobe WHERE Id=" + idStudenta[0];
                            cmd.Connection = con;
                            cmd.Parameters.Add(new SQLiteParameter("@brsobe", btn.Name));

                            comboBox1.Items.Remove((string)e.Data.GetData(DataFormats.Text));
                            comboBox1.Text = "";
                            int i = cmd.ExecuteNonQuery();

                            if (i == 1)
                            {
                                MessageBox.Show("Uspjesno prebacen student");
                            }
                            roomLabel.Text = btn.Name.Substring(btn.Name.Length - 3);

                            moveStudentBtn.Enabled = false;
                        }
                    }    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }                
            else
                MessageBox.Show("To mjesto je zauzeto");     
        }

        private void button_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login lf = new Login();
            lf.Show();
            Visible = false;
        }

        private void addStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddStudent asf = new AddStudent();
            asf.Show();
            Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void addEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmployee aef = new AddEmployee();
            aef.Show();
            Visible = false;
        }

        private void searchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //comboBox1.SelectedIndex = -1;
            comboBox1.Text = "";
            
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();
                    string[] idStudenta = searchComboBox.Text.Split(' ', '\t');
                    string stm = "SELECT * FROM student WHERE student.Id =" + idStudenta[0];
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                        {
                            using (SQLiteDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    nameLabel.Text = rdr["Ime"].ToString();
                                    lastNameLabel.Text = rdr["Prezime"].ToString();
                                    birthLabel.Text = rdr["Godiste"].ToString();
                                    genderLabel.Text = rdr["Pol"].ToString();
                                    fakultyLabel.Text = rdr["Fakultet"].ToString();
                                    yearLabel.Text = rdr["GodinaStudija"].ToString();
                                    IDLabel.Text = rdr["Id"].ToString();
                                    if (rdr["Soba"].ToString() == "")
                                        roomLabel.Text = "Nema sobu";
                                    else
                                        roomLabel.Text = rdr["Soba"].ToString().Substring(rdr["Soba"].ToString().Length - 3);
                                    if (rdr["Opomena"].ToString() != "")
                                        button1.BackColor = Color.Red;
                                    else
                                        button1.BackColor = Color.White;
                                    if (rdr["Soba"].ToString() != "")
                                        moveStudentBtn.Enabled = false;
                                    else
                                        moveStudentBtn.Enabled = true;
                                if (rdr["Soba"].ToString() == "")
                                    moveStudentBtn.Enabled = true;
                                else
                                    moveStudentBtn.Enabled = false;
                                moveStudentBtn.Text = rdr["Id"].ToString() + " " + rdr["Ime"].ToString() + " " + rdr["Prezime"].ToString();
                                }

                            }
                        }

                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }        
            
            
        }

        private void izbaciIzSobeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            var tsItem = (ToolStripMenuItem)sender;
            var cms = (ContextMenuStrip)tsItem.Owner;
            Button tbx = this.Controls.Find(cms.SourceControl.Name, true).FirstOrDefault() as Button;
            string[] idStudenta = tbx.Text.Split(' ', '\t');

            if (tbx.Text != "")
            {
                try
                {
                    if (tbx.Text == moveStudentBtn.Text)
                        roomLabel.Text = "Nema sobu";

                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.CommandText = "UPDATE student SET Soba = @brsobe WHERE Id=" + idStudenta[0];
                        cmd.Connection = con;
                        cmd.Parameters.Add(new SQLiteParameter("@brsobe", ""));

                        con.Open();

                        string stm = "SELECT * FROM student WHERE Id = " + idStudenta[0];

                        using (SQLiteCommand cmds = new SQLiteCommand(stm, con))
                        {
                            using (SQLiteDataReader rdr = cmds.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    comboBox1.Items.Add(rdr["Id"].ToString() + " " + rdr["Ime"].ToString()
                                                        + " " + rdr["Prezime"].ToString());
                                }
                            }
                        }

                        int i = cmd.ExecuteNonQuery();

                        if (i == 1)
                        {
                            MessageBox.Show("Uspjesno izbacen iz sobe student");
                        }
                        

                        con.Close();    
                    }
                    moveStudentBtn.Text = "";
                    nameLabel.Text = "";
                    lastNameLabel.Text = "";
                    birthLabel.Text = "";
                    genderLabel.Text = "";
                    fakultyLabel.Text = "";
                    yearLabel.Text = "";
                    roomLabel.Text = "";
                    IDLabel.Text = "";
                    button1.BackColor = Color.White;
                    searchComboBox.Text = "";
                    comboBox1.Text = "";
                    tbx.Text = "";
                    moveStudentBtn.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Nema studenta u tom krevetu");
            
        }

        private void dodajOpomenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 2; i++)
            {
                var tsItem = (ToolStripMenuItem)sender;
                var cms = (ContextMenuStrip)tsItem.Owner;
                Button tbx = this.Controls.Find(cms.SourceControl.Name, true).FirstOrDefault() as Button;
                Warning wf = new Warning();
                string[] idStudenta = tbx.Text.Split(' ', '\t');
                warningStudId = idStudenta[0];

                if (tbx.Text == "")
                {
                    if(i == 0)
                    MessageBox.Show("Nema studenta u tom krevetu");
                }
                else
                {

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
                                   if(idStudenta[0] == rdr["Id"].ToString())
                                    {
                                        opomena = rdr["Opomena"].ToString();
                                    }
                                 
                                }
                                 
                            }
                        }

                        con.Close();
                    }


                    //opomena = opomene[Int32.Parse(idStudenta[0])-1];
                    if (i == 1)
                        wf.ShowDialog();

                    moveStudentBtn.Text = "";
                    nameLabel.Text = "";
                    lastNameLabel.Text = "";
                    birthLabel.Text = "";
                    genderLabel.Text = "";
                    fakultyLabel.Text = "";
                    yearLabel.Text = "";
                    roomLabel.Text = "";
                    IDLabel.Text = "";
                    button1.BackColor = Color.White;
                    searchComboBox.Text = "";
                    comboBox1.Text = "";
                    moveStudentBtn.Enabled = false;
                }
            }
            


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string stm = "";
            string[] idStudenta = moveStudentBtn.Text.Split(' ', '\t');
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                if (moveStudentBtn.Text == "")
                    MessageBox.Show("Niste izabrali studenta");
                else
                {
                    stm = "SELECT * FROM student WHERE Id =" + idStudenta[0];

                    using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (rdr["Opomena"].ToString() == "")
                                    MessageBox.Show("Student nema opomena!");
                                else
                                    MessageBox.Show(rdr["Opomena"].ToString());
                            }

                        }
                    }
                }                   
                 
                con.Close();
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //searchComboBox.SelectedIndex = -1;
            searchComboBox.Text = "";
            
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();
                    string[] idStudenta = comboBox1.Text.Split(' ', '\t');
                    string stm = "SELECT * FROM student WHERE student.Id =" + idStudenta[0];

                    using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                nameLabel.Text = rdr["Ime"].ToString();
                                lastNameLabel.Text = rdr["Prezime"].ToString();
                                birthLabel.Text = rdr["Godiste"].ToString();
                                genderLabel.Text = rdr["Pol"].ToString();
                                fakultyLabel.Text = rdr["Fakultet"].ToString();
                                yearLabel.Text = rdr["GodinaStudija"].ToString();
                                IDLabel.Text = rdr["Id"].ToString();
                                if (rdr["Soba"].ToString() == "")
                                    roomLabel.Text = "Nema sobu";
                                else
                                    roomLabel.Text = rdr["Soba"].ToString().Substring(rdr["Soba"].ToString().Length - 3);
                                if (rdr["Opomena"].ToString() != "")
                                    button1.BackColor = Color.Red;
                                else
                                    button1.BackColor = Color.White;
                                if (rdr["Soba"].ToString() != "")
                                    moveStudentBtn.Enabled = false;
                                else
                                    moveStudentBtn.Enabled = true;
                            if (rdr["Soba"].ToString() == "")
                                moveStudentBtn.Enabled = true;
                            else
                                moveStudentBtn.Enabled = false;
                                moveStudentBtn.Text = rdr["Id"].ToString() + " " + rdr["Ime"].ToString() + " " + rdr["Prezime"].ToString();
                            }

                        }
                    }

                    con.Close();
                }
            
            
            
        }
        

        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            string stm = "SELECT * FROM student";

            if (e.KeyCode == Keys.Enter)
            {
                dataGridView1.Rows.Clear();

                    if (comboBox2.SelectedItem.ToString() == "Ime" || comboBox2.SelectedItem.ToString() == "Prezime")
                    {
                    if(textBox1.Text != "")
                        stm = stm + " WHERE student." + comboBox2.Text + " = " + "\'" + textBox1.Text + "\'" + "COLLATE NOCASE";
                    }
                    else if (comboBox2.SelectedItem.ToString() == "Soba")
                    {

                    if (textBox1.Text != "")
                        stm = stm + " WHERE student." + comboBox2.Text + " LIKE " + "\'_" + textBox1.Text + "\'" + "COLLATE NOCASE";
                    }
                    else if (comboBox2.SelectedItem.ToString() == "Sprat")
                    {

                    if (textBox1.Text != "")
                        stm = stm + " WHERE student.Soba" + " LIKE " + "\'_" + textBox1.Text + "%\'" + "COLLATE NOCASE";
                    }
                
                

                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {

                    con.Open();
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                        {
                            using (SQLiteDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    if (rdr["Soba"].ToString()!= "")
                                    {
                                        dataGridView1.Rows.Add(rdr["Id"].ToString(), rdr["Ime"].ToString(),
                                        rdr["Prezime"].ToString(), rdr["Soba"].ToString().Substring(1, 3));
                                    }
                                    else if (rdr["Soba"].ToString() == "")
                                    {
                                        dataGridView1.Rows.Add(rdr["Id"].ToString(), rdr["Ime"].ToString(),
                                        rdr["Prezime"].ToString(), "");
                                    }
                                }

                            }
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            
        }

        private void a108_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            
            if (btn.Text != "")
            {
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();
                    string[] idStudenta = btn.Text.Split(' ', '\t');
                    string stm = "SELECT * FROM student WHERE student.Id =" + idStudenta[0];
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                        {
                            using (SQLiteDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    nameLabel.Text = rdr["Ime"].ToString();
                                    lastNameLabel.Text = rdr["Prezime"].ToString();
                                    birthLabel.Text = rdr["Godiste"].ToString();
                                    genderLabel.Text = rdr["Pol"].ToString();
                                    fakultyLabel.Text = rdr["Fakultet"].ToString();
                                    yearLabel.Text = rdr["GodinaStudija"].ToString();
                                    IDLabel.Text = rdr["Id"].ToString();
                                    if (rdr["Soba"].ToString() == "")
                                        roomLabel.Text = "Nema sobu";
                                    else
                                        roomLabel.Text = rdr["Soba"].ToString().Substring(rdr["Soba"].ToString().Length - 3);
                                    if (rdr["Opomena"].ToString() != "")
                                        button1.BackColor = Color.Red;
                                    else
                                        button1.BackColor = Color.White;
                                    if (rdr["Soba"].ToString() != "")
                                        moveStudentBtn.Enabled = false;
                                    else
                                        moveStudentBtn.Enabled = true;
                                    
                                }

                            }
                        }
                        con.Close();
                        comboBox1.Text = "";
                        searchComboBox.Text = "";
                        moveStudentBtn.Text = btn.Text;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        
                    }
                }
            }
            else if (btn.Text == "") 
             {
                nameLabel.Text = "-";
                lastNameLabel.Text = "-";
                birthLabel.Text = "-";
                genderLabel.Text = "-";
                yearLabel.Text = "-";
                fakultyLabel.Text = "-";
                IDLabel.Text = "-";
                roomLabel.Text = "-";
                button1.BackColor = Color.White;
                comboBox1.Text = "";
                searchComboBox.Text = "";
                moveStudentBtn.Text = btn.Text;
                MessageBox.Show("Nema studenta u tom krevetu");
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            moveStudentBtn.Enabled = false;
            if (Login.dopustenja == "Čuvar")
            {
                searchComboBox.Enabled = false; 
                comboBox1.Enabled = false;
                addStudentToolStripMenuItem.Enabled = false;
                addEmployeeToolStripMenuItem.Enabled = false;
                removeStudentEmployeeToolStripMenuItem.Enabled = false;
                contextMenuStrip1.Enabled = false;
            }
            if(Login.dopustenja == "Recepcionar")
            {
                addStudentToolStripMenuItem.Enabled = false;
                addEmployeeToolStripMenuItem.Enabled = false;
                removeStudentEmployeeToolStripMenuItem.Enabled = false;
            }
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void removeStudentEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove_Student_or_Employee rse = new Remove_Student_or_Employee();
            rse.Show();
            Visible = false;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            if(comboBox2.SelectedIndex == 3)
            {
                textBox1.MaxLength = 1;
            }
            else if(comboBox2.SelectedIndex == 2)
            {
                textBox1.MaxLength = 3;
            }
            else
            {
                textBox1.MaxLength = 32767;
            }
        }
    }
    
}
   