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
using System.Windows.Forms.DataVisualization.Charting;

namespace PService
{
    public partial class MainFormDir: Form
    {
        private worker_table user;

        PSEntity db = new PSEntity();

        public MainFormDir(worker_table client)
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

        private void button11_Click(object sender, EventArgs e)
        {
            chart1.Show();
            switch (comboBox4.SelectedIndex + 1)
            {
                case 1:
                    {
                       
                        if (chart1.Series[0].Points != null)
                        {
                            chart1.Series[0].Points.Clear();
                        }
                        chart1.Series[0].LegendText = "Sales";
                        chart1.Series["Series1"].Color = Color.Green;
                        for (int i = 0; i < Misc.GetTodayMonth(); i++)
                        {
                            var sum = db.service_payments.Where(o => o.date == i).Select(o => o.price).Sum();
                            chart1.Series[0].Points.AddXY(i, sum);
                        }
                        break;
                    }
                case 2:
                    {
                       
                        if (chart1.Series[0].Points != null)
                        {
                            chart1.Series[0].Points.Clear();
                        }
                        chart1.Series[0].LegendText = "Lost profit";
                        var data = (from a in db.active_debts
                                    join b in db.debt_table on a.debt_id equals b.id
                                    select new EasyDataRepresentation
                                    {
                                        Debt = b.debt.Value,
                                        Month = b.month_id.Value
                                    }
                                    ).ToList();
                        chart1.Series["Series1"].Color = Color.Red;

                        for (int i = 0; i < data.Count(); i++)
                        {
                            var sum = data.Select(o => o.Debt).Sum();
                            chart1.Series[0].Points.AddXY(i, sum);
                        }
                        break;
                    }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex + 1 != 9 && comboBox2.SelectedIndex + 1 != 13) {
                var data = (from a in db.service_payments.Where(a => a.service_id == comboBox1.SelectedIndex + 1 && a.date == comboBox2.SelectedIndex + 1)
                            join b in db.clients_table on a.client_id equals b.client_id
                            join c in db.service_type on a.service_id equals c.service_id
                            join d in db.months_2020_table on a.date equals d.month_id
                            select new EasyPaymentsRepresentation
                            {
                                Client_name = b.name,
                                Service_name = c.name,
                                Date_name = d.name,
                                Price = (decimal)a.price,
                                Usage = (decimal)a.usage
                            }).ToList();

                dataGridView1.DataSource = data;
            }
            else if(comboBox1.SelectedIndex + 1 == 9 && comboBox2.SelectedIndex + 1 != 13)
            {
                var data = (from a in db.service_payments.Where(a => a.date == comboBox2.SelectedIndex + 1)
                            join b in db.clients_table on a.client_id equals b.client_id
                            join c in db.service_type on a.service_id equals c.service_id
                            join d in db.months_2020_table on a.date equals d.month_id
                            select new EasyPaymentsRepresentation
                            {
                                Client_name = b.name,
                                Service_name = c.name,
                                Date_name = d.name,
                                Price = (decimal)a.price,
                                Usage = (decimal)a.usage
                            }).ToList();

                dataGridView1.DataSource = data;
            }

            else if(comboBox1.SelectedIndex + 1 == 9 && comboBox2.SelectedIndex + 1 == 13)
            {
                var data = (from a in db.service_payments
                            join b in db.clients_table on a.client_id equals b.client_id
                            join c in db.service_type on a.service_id equals c.service_id
                            join d in db.months_2020_table on a.date equals d.month_id
                            select new EasyPaymentsRepresentation
                            {
                                Client_name = b.name,
                                Service_name = c.name,
                                Date_name = d.name,
                                Price = (decimal)a.price,
                                Usage = (decimal)a.usage
                            }).ToList();

                dataGridView1.DataSource = data;
            }
            else if(comboBox1.SelectedIndex + 1 != 9 && comboBox2.SelectedIndex + 1 == 13)
            {
                var data = (from a in db.service_payments.Where(a => a.service_id == comboBox1.SelectedIndex + 1)
                            join b in db.clients_table on a.client_id equals b.client_id
                            join c in db.service_type on a.service_id equals c.service_id
                            join d in db.months_2020_table on a.date equals d.month_id
                            select new EasyPaymentsRepresentation
                            {
                                Client_name = b.name,
                                Service_name = c.name,
                                Date_name = d.name,
                                Price = (decimal)a.price,
                                Usage = (decimal)a.usage
                            }).ToList();

                dataGridView1.DataSource = data;
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisplayPayments.Show();
            StatDisplay.Hide();
            DebtsDisplay.Hide();
            VirtualMoneyDisplay.Hide();
            WorkersAdditor.Hide();
            CleintsAdditor.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DisplayPayments.Hide();
            StatDisplay.Show();
            DebtsDisplay.Hide();
            VirtualMoneyDisplay.Hide();
            WorkersAdditor.Hide();
            CleintsAdditor.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(comboBox6.SelectedIndex < 1)
            {
                if (comboBox3.SelectedIndex + 1 != 9 && comboBox5.SelectedIndex + 1 != 13)
                {
                    var data = (from a in db.debt_table.Where(a => a.service_id == comboBox3.SelectedIndex + 1 && a.month_id == comboBox5.SelectedIndex + 1)
                                join b in db.clients_table on a.client_id equals b.client_id
                                join c in db.service_type on a.service_id equals c.service_id
                                join d in db.months_2020_table on a.month_id equals d.month_id
                                select new EasyDebtRecordsRepresentation
                                {
                                    Client_name = b.name,
                                    Service_name = c.name,
                                    Date_name = d.name,
                                    Price = (decimal)a.debt
                                }
                                )
                           .ToList();
                    dataGridView2.DataSource = data;
                }
                else if (comboBox3.SelectedIndex + 1 == 9 && comboBox5.SelectedIndex + 1 != 13)
                {
                    var data = (from a in db.debt_table.Where(a => a.month_id == comboBox5.SelectedIndex + 1)
                                join b in db.clients_table on a.client_id equals b.client_id
                                join c in db.service_type on a.service_id equals c.service_id
                                join d in db.months_2020_table on a.month_id equals d.month_id
                                select new EasyDebtRecordsRepresentation
                                {
                                    Client_name = b.name,
                                    Service_name = c.name,
                                    Date_name = d.name,
                                    Price = (decimal)a.debt
                                }
                               )
                          .ToList();
                    dataGridView2.DataSource = data;
                }
                else if (comboBox3.SelectedIndex + 1 == 9 && comboBox5.SelectedIndex + 1 == 13)
                {
                    var data = (from a in db.debt_table
                                join b in db.clients_table on a.client_id equals b.client_id
                                join c in db.service_type on a.service_id equals c.service_id
                                join d in db.months_2020_table on a.month_id equals d.month_id
                                select new EasyDebtRecordsRepresentation
                                {
                                    Client_name = b.name,
                                    Service_name = c.name,
                                    Date_name = d.name,
                                    Price = (decimal)a.debt
                                }
                                )
                           .ToList();
                    dataGridView2.DataSource = data;
                }
                else if (comboBox3.SelectedIndex + 1 != 9 && comboBox5.SelectedIndex + 1 == 13)
                {
                    var data = (from a in db.debt_table.Where(a => a.service_id == comboBox3.SelectedIndex + 1)
                                join b in db.clients_table on a.client_id equals b.client_id
                                join c in db.service_type on a.service_id equals c.service_id
                                join d in db.months_2020_table on a.month_id equals d.month_id
                                select new EasyDebtRecordsRepresentation
                                {
                                    Client_name = b.name,
                                    Service_name = c.name,
                                    Date_name = d.name,
                                    Price = (decimal)a.debt
                                }
                               )
                          .ToList();
                    dataGridView2.DataSource = data;
                }
            }
            else
            {
                if (comboBox3.SelectedIndex + 1 != 9 && comboBox5.SelectedIndex + 1 != 13)
                {
                    var data = (from o in db.active_debts
                                
                                join a in db.debt_table.Where(a => a.service_id == comboBox3.SelectedIndex + 1 && a.month_id == comboBox5.SelectedIndex + 1) on o.debt_id equals a.id
                                join b in db.clients_table on a.client_id equals b.client_id
                                join c in db.service_type on a.service_id equals c.service_id
                                join d in db.months_2020_table on a.month_id equals d.month_id
                                select new EasyDebtRecordsRepresentation
                                {
                                    Client_name = b.name,
                                    Service_name = c.name,
                                    Date_name = d.name,
                                    Price = (decimal)a.debt
                                }
                                )
                           .ToList();
                    dataGridView2.DataSource = data;
                }
                else if (comboBox3.SelectedIndex + 1 == 9 && comboBox5.SelectedIndex + 1 != 13)
                {
                    var data = (from o in db.active_debts
                                join a in db.debt_table.Where(a => a.month_id == comboBox5.SelectedIndex + 1) on o.debt_id equals a.id
                                join b in db.clients_table on a.client_id equals b.client_id
                                join c in db.service_type on a.service_id equals c.service_id
                                join d in db.months_2020_table on a.month_id equals d.month_id
                                select new EasyDebtRecordsRepresentation
                                {
                                    Client_name = b.name,
                                    Service_name = c.name,
                                    Date_name = d.name,
                                    Price = (decimal)a.debt
                                }
                               )
                          .ToList();
                    dataGridView2.DataSource = data;
                }
                else if (comboBox3.SelectedIndex + 1 == 9 && comboBox5.SelectedIndex + 1 == 13)
                {
                    var data = (from o in db.active_debts
                                join a in db.debt_table on o.debt_id equals a.id
                                join b in db.clients_table on a.client_id equals b.client_id
                                join c in db.service_type on a.service_id equals c.service_id
                                join d in db.months_2020_table on a.month_id equals d.month_id
                                select new EasyDebtRecordsRepresentation
                                {
                                    Client_name = b.name,
                                    Service_name = c.name,
                                    Date_name = d.name,
                                    Price = (decimal)a.debt
                                }
                                )
                           .ToList();
                    dataGridView2.DataSource = data;
                }
                else if (comboBox3.SelectedIndex + 1 != 9 && comboBox5.SelectedIndex + 1 == 13)
                {
                    var data = (from o in db.active_debts
                                join a in db.debt_table.Where(a => a.service_id == comboBox3.SelectedIndex + 1) on o.debt_id equals a.id
                                join b in db.clients_table on a.client_id equals b.client_id
                                join c in db.service_type on a.service_id equals c.service_id
                                join d in db.months_2020_table on a.month_id equals d.month_id
                                select new EasyDebtRecordsRepresentation
                                {
                                    Client_name = b.name,
                                    Service_name = c.name,
                                    Date_name = d.name,
                                    Price = (decimal)a.debt
                                }
                               )
                          .ToList();
                    dataGridView2.DataSource = data;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DebtsDisplay.Show();
            DisplayPayments.Hide();
            StatDisplay.Hide();
            VirtualMoneyDisplay.Hide();
            CleintsAdditor.Hide();
            WorkersAdditor.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox8.SelectedIndex < 9)
            {
                var data = (from o in db.virtual_money.Where(o => o.service_id == comboBox8.SelectedIndex + 1)
                            join a in db.clients_table on o.client_id equals a.client_id
                            join b in db.service_type on o.service_id equals b.service_id
                            select new EasyVirtualMoneyRepresentation
                            {
                                Client_name = a.name,
                                Service_name = b.name,
                                Price = (decimal)o.money
                            }
                              )
                         .ToList();
                dataGridView3.DataSource = data;
            }
            else
            {
                var data = (from o in db.virtual_money
                            join a in db.clients_table on o.client_id equals a.client_id
                            join b in db.service_type on o.service_id equals b.service_id
                            select new EasyVirtualMoneyRepresentation
                            {
                                Client_name = a.name,
                                Service_name = b.name,
                                Price = (decimal)o.money
                            }
                             )
                        .ToList();
                dataGridView3.DataSource = data;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            VirtualMoneyDisplay.Show();
            DebtsDisplay.Hide();
            DisplayPayments.Hide();
            StatDisplay.Hide();
            CleintsAdditor.Hide();
            WorkersAdditor.Hide();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            dataGridView4.DataSource = db.worker_table.ToList();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure?", "Sample text",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)

                {

                    int delId = (int)item.Cells[0].Value;
                    var deleteRecord = db.worker_table.First(b => b.worker_id == delId);    // get the record. will throw exception if not found.
                    db.worker_table.Remove(deleteRecord);

                    db.SaveChanges();

                    dataGridView1.DataSource = db.worker_table.ToList();

                }

            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            db.SaveChanges();
            MessageService.SuccsesfulChange();
        }

        private void button13_Click(object sender, EventArgs e)
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

        private void button9_Click(object sender, EventArgs e)
        {
            WorkersAdditor.Show();
            VirtualMoneyDisplay.Hide();
            DebtsDisplay.Hide();
            DisplayPayments.Hide();
            StatDisplay.Hide();
            CleintsAdditor.Hide();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            dataGridView5.DataSource = db.clients_table.ToList();
        }

        private void button17_Click(object sender, EventArgs e)
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

        private void button18_Click(object sender, EventArgs e)
        {
            db.SaveChanges();
        }

        private void button10_Click(object sender, EventArgs e)
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

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

        private void button12_Click(object sender, EventArgs e)
        {
            CleintsAdditor.Show();
            WorkersAdditor.Hide();
            VirtualMoneyDisplay.Hide();
            DebtsDisplay.Hide();
            DisplayPayments.Hide();
            StatDisplay.Hide();
        }
    }
}
