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
    public partial class MainFormAcc : Form
    {
        private worker_table user;

        PSEntity db = new PSEntity();

        public MainFormAcc(worker_table client)
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
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DisplayPayments.Hide();
            StatDisplay.Show();
            DebtsDisplay.Hide();
            VirtualMoneyDisplay.Hide();
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
        }
    }
}
