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
                            opomene.Add(rdr["Opomena"].ToString());
                            
                        }

                    }
                }

                con.Close();
            }
        }
      
        private void floorDownBtn_Click(object sender, EventArgs e)
        {

        }

        private void floorUpBtn_Click(object sender, EventArgs e)
        {

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
                                cmd.CommandText = "UPDATE student SET Soba = @brsobe WHERE Id=" + moveStudentBtn.Text.Substring(0, 1);
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
                            cmd.CommandText = "UPDATE student SET Soba = @brsobe WHERE Id=" + moveStudentBtn.Text.Substring(0, 1);
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
            
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                string rbr = searchComboBox.Text.Substring(0, 1);
                string stm = "SELECT * FROM student WHERE student.Id =" + rbr;
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

                                moveStudentBtn.Text = rdr["Id"].ToString() + " " + rdr["Ime"].ToString() + " " + rdr["Prezime"].ToString();
                            }

                        }
                    }
                    comboBox1.Text = "";
                    con.Close();
                }
                catch(Exception ex)
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
            
            if (tbx.Text != "")
            {
                try
                {
                    if (tbx.Text == moveStudentBtn.Text)
                        roomLabel.Text = "Nema sobu";

                    using (SQLiteConnection con = new SQLiteConnection(connectionString))
                    {
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.CommandText = "UPDATE student SET Soba = @brsobe WHERE Id=" + tbx.Text.Substring(0, 1);
                        cmd.Connection = con;
                        cmd.Parameters.Add(new SQLiteParameter("@brsobe", ""));

                        con.Open();

                        string stm = "SELECT * FROM student WHERE Id = " + tbx.Text.Substring(0, 1);

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
                    moveStudentBtn.Enabled = true;
                    tbx.Text = "";
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
                opomena = opomene[Int32.Parse(tbx.Text.Substring(0, 1)) - 1];

                if (tbx.Text == "")
                {
                    MessageBox.Show("Nema studenta u tom krevetu");
                }
                else
                {
                    if(i == 1)
                        wf.ShowDialog();

                }
            }
            


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string stm = "";
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                if (moveStudentBtn.Text == "")
                    MessageBox.Show("Niste izabrali studenta");
                else
                {
                    stm = "SELECT * FROM student WHERE student.Id =" + moveStudentBtn.Text.Substring(0, 1);

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
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                con.Open();
                string rbr = comboBox1.Text.Substring(0, 1);
                string stm = "SELECT * FROM student WHERE student.Id =" + rbr;

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
                            moveStudentBtn.Text = rdr["Id"].ToString() + " " + rdr["Ime"].ToString() + " " + rdr["Prezime"].ToString();
                        }

                    }
                }
                searchComboBox.Text = "";
                con.Close();
            }
        }
        

        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dataGridView1.Rows.Clear();
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    string stm = "SELECT * FROM student WHERE student."+comboBox2.Text+" LIKE "+"\'_"+textBox1.Text+"\'";
                    con.Open();
                    try
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                        {
                            using (SQLiteDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    dataGridView1.Rows.Add(rdr["Id"].ToString(), rdr["Ime"].ToString(),
                                        rdr["Prezime"].ToString(), rdr["Soba"].ToString().Substring(1, 3));
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
            moveStudentBtn.Text = btn.Text;
            if(btn.Text != "")
            {
                using (SQLiteConnection con = new SQLiteConnection(connectionString))
                {
                    con.Open();
                    string rbr = btn.Text.Substring(0, 1);
                    string stm = "SELECT * FROM student WHERE student.Id =" + rbr;
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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
   