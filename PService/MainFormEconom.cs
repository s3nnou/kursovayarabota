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
    public partial class MainFormEconom : Form
    {
        private worker_table user;

        PSEntity db = new PSEntity();

        public MainFormEconom(worker_table client)
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

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (PSEntity db = new PSEntity())
            {
                var tarifQuery = from o in db.service_type
                                 select o;
                dataGridView1.DataSource = tarifQuery.ToList();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

           dataGridView1.DataSource = db.service_type.ToList();
        }

        private void button4_Click_3(object sender, EventArgs e)
        {
            db.SaveChanges();
            MessageService.SuccsesfulChange();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            clients_table client = new clients_table();

            var countOfRows = db.clients_table.Count();
            var lastRow = db.clients_table.OrderBy(c => 1 == 1).Skip(countOfRows - 1).FirstOrDefault();

            client.client_id = lastRow.client_id + 1;
            client.name = textBox2.Text.Trim();
            client.phone_number = int.Parse(textBox3.Text.Trim());
            client.street = textBox4.Text.Trim();
            client.flat = textBox5.Text.Trim();
            client.house = textBox6.Text.Trim();
            client.persons = int.Parse(textBox7.Text.Trim());
            client.dwelling_space = double.Parse(textBox8.Text.Trim());
            db.clients_table.Add(client);
            db.SaveChanges();

            dataGridView2.DataSource = db.clients_table.ToList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = db.clients_table.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure?", "Sample text",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in this.dataGridView2.SelectedRows)

                {

                    int delId = (int)item.Cells[0].Value;
                    var deleteRecord = db.clients_table.First(b => b.client_id == delId);    // get the record. will throw exception if not found.
                    db.clients_table.Remove(deleteRecord);

                    db.SaveChanges();

                    dataGridView2.DataSource = db.clients_table.ToList();

                }

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            db.SaveChanges();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            TariffEditor.Show();
            CleintsAdditor.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TariffEditor.Hide();
            CleintsAdditor.Show();
        }
    }
}
