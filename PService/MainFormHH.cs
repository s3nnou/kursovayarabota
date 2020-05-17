using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PService
{
    public partial class MainFormHH : Form
    {
        private worker_table user;

        PSEntity db = new PSEntity();

        public MainFormHH(worker_table client)
        {
            InitializeComponent();
            user = client;
            AboutMeTextSetter();
        }

        private void AboutMeTextSetter()
        {
            label12.Text = user.job.ToString();
            label13.Text = user.name;            
        }

        private void PaymentsMenu_Click(object sender, EventArgs e)
        {
            AboutMeMenu.BackColor = Color.FromArgb(46, 49, 55);
            PaymentsMenu.BackColor = Color.WhiteSmoke;

            layer1.Hide();
            layer2.Show();
        }

        private void AboutMeMenu_Click(object sender, EventArgs e)
        {
            PaymentsMenu.BackColor = Color.FromArgb(46, 49, 55);
            AboutMeMenu.BackColor = Color.WhiteSmoke;

            layer2.Hide();
            layer1.Show();
        }
      
        private void button2_Click(object sender, EventArgs e)
        {
            if(panel11.Visible == true)
            {
                panel11.Hide();
            }
            else
            {
                panel11.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.worker_table.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            db.SaveChanges();
            MessageService.SuccsesfulChange();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure?", "Sample text",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)

                {
                    
                    int delId = (int) item.Cells[0].Value;
                    var deleteRecord = db.worker_table.First(b => b.worker_id == delId);    // get the record. will throw exception if not found.
                    db.worker_table.Remove(deleteRecord);

                    db.SaveChanges();

                    dataGridView1.DataSource = db.worker_table.ToList();

                }

            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            worker_table worker = new worker_table();

            int check = int.Parse(textBox1.Text.Trim());
            if (db.worker_table.Any(o => o.worker_id == check))
            {
                MessageBox.Show("dude u r gay");
            }
            else
            {
                worker.worker_id = int.Parse(textBox1.Text.Trim());
                worker.name = textBox2.Text.Trim();
                worker.phone_number = int.Parse(textBox3.Text.Trim());
                worker.street = textBox4.Text.Trim();
                worker.flat = textBox5.Text.Trim();
                worker.house = textBox6.Text.Trim();
                worker.job = comboBox1.Text.Trim();
                worker.wage = int.Parse(textBox7.Text.Trim());

                db.worker_table.Add(worker);

                db.SaveChanges();

                dataGridView1.DataSource = db.worker_table.ToList();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = db.staff_acc_table.ToList();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            WorkersAdditor.Show();
            AccountEdditor.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WorkersAdditor.Hide();
            AccountEdditor.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            staff_acc_table staff = new staff_acc_table();
            bool flag = true;
            string username = textBox14.Text.Trim();
            int id = int.Parse(textBox11.Text.Trim());

            foreach(staff_acc_table item in db.staff_acc_table)
            {
                if (string.Equals(item.username, username))
                {
                    MessageBox.Show("dude u r gay");
                    flag = false;
                }
            }

            if (flag)
            {
                staff.id = db.staff_acc_table.Count() + 1;
                staff.username = username;
                staff.worker_id = id;
                staff.password = textBox12.Text.Trim();
                staff.role_id = int.Parse(comboBox2.SelectedIndex.ToString());

                db.staff_acc_table.Add(staff);

                db.SaveChanges();

                dataGridView2.DataSource = db.staff_acc_table.ToList();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure?", "Sample text",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in this.dataGridView2.SelectedRows)

                {

                    int delId = (int)item.Cells[2].Value;
                    var deleteRecord = db.staff_acc_table.First(b => b.worker_id == delId);    // get the record. will throw exception if not found.
                    if ((deleteRecord.worker_id == user.worker_id) || (deleteRecord.role_id == 1))
                    {
                        MessageBox.Show("Invalid selection", "Warning");
                    }
                    else
                    {
                        db.staff_acc_table.Remove(deleteRecord);
                    }

                    db.SaveChanges();

                    dataGridView2.DataSource = db.staff_acc_table.ToList();

                }

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            db.SaveChanges();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allows 0-9, backspace, and decimal
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }

            // checks to make sure only 1 decimal is allowed
            if (e.KeyChar == 46)
            {
                if ((sender as TextBox).Text.IndexOf(e.KeyChar) != -1)
                    e.Handled = true;
            }
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
