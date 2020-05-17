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
    public partial class MainFormClient : Form
    {
        private clients_table user;

        public MainFormClient(clients_table client)
        {
            InitializeComponent();
            user = client;
            AboutMeTextSetter();
            DebtNotifierShow();
            FillDataGrid3();
            FillComboBox2();
        }

        private void DebtNotifierShow()
        {
            if (dataGridView3 == null || dataGridView3.Rows.Count == 0)
            {
                debtNotify.Hide();

            }
            else
            {
                debtNotify.Show();

            }
        }

        private void FillDataGrid3()
        {
            using(PSEntity db = new PSEntity())
            {
                var data = (from a in db.active_debts
                            join b in db.debt_table.Where(b => b.client_id == user.client_id) on a.debt_id equals b.id
                            join c in db.service_type on b.service_id equals c.service_id
                            join d in db.months_2020_table on b.month_id equals d.month_id
                            select new ActiveDebtsView
                            {
                                Debt_id = b.id,
                                Client_id = (int) b.client_id,
                                Service_id = c.name,
                                Debt = (decimal) b.debt,
                                Month_id = d.name
                             }).ToList();


                dataGridView3.DataSource = data;
            }

        }

        private void FillComboBox2()
        {
            using (PSEntity db = new PSEntity())
            {
                comboBox2.DataSource = db.service_type.ToList();
            }
        }

        private void AboutMeTextSetter()
        {
            label12.Text = user.client_id.ToString();
            label13.Text = user.name;
            label14.Text = user.phone_number.ToString();
            label15.Text = user.street;
            label16.Text = user.house;
            
            if(user.flat != null)
            {
                label17.Text = user.flat;
            }
            
            label18.Text = user.dwelling_space.ToString();
            label19.Text = user.persons.ToString();
        }

        private void MainFormClient_Load(object sender, EventArgs e)
        {

                
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a, b, c, d, f, g;
            if (ServicePayRequests.MainServiceDataProvider(int.Parse(comboBox1.SelectedIndex.ToString()), user.client_id, Misc.GetTodayMonth(), (decimal)user.dwelling_space))
            {
                LablingService.GetTextAboutServiceWithMetrics(int.Parse(comboBox1.SelectedIndex.ToString()), user.client_id, Misc.GetTodayMonth(), out a, out b, out c, out d, out f, out g);
            }
            else
            {
                LablingService.SetTextForStaticService(int.Parse(comboBox1.SelectedIndex.ToString()),user.client_id, Misc.GetTodayMonth(), (decimal) user.dwelling_space, out a, out b,out c, out d, out f, out g);
            }

            label25.Text = a;
            label27.Text = b;
            label28.Text = c;
            label32.Text = f;

            TariffBox.Show();
            UsageBox.Show();
            ModBox.Show();
            PaymentBox.Show();

            if (d == "-1")
            {
                OverBox.Hide();
            }
            else
            {
                OverBox.Show();
                label30.Text = d;
            }

            if(g == "-1")
            {
                DebtBox.Hide();
            }
            else
            {
                DebtBox.Show();
                label34.Text = g;
            }

            label32.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PaymentHandler.PaymentStateMachine(int.Parse(comboBox1.SelectedIndex.ToString()), user.client_id, Misc.GetTodayMonth(),
                decimal.Parse(MoneyInstertText.Text.ToString()), decimal.Parse(label30.Text.ToString()), decimal.Parse(label32.Text.ToString()),
                PaymentHandler.GetPaymentState(decimal.Parse(label30.Text.ToString()), decimal.Parse(label34.Text.ToString())), decimal.Parse(label32.Text.ToString()));

            UpdateForm();
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

        private void button3_Click(object sender, EventArgs e)
        {
            PayMentCreation.Hide();
            ActiveDebts.Hide();
            PayMentLog.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PayMentCreation.Show();
            ActiveDebts.Hide();
            PayMentLog.Hide();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

                using (PSEntity db = new PSEntity())
            {
                dataGridView2.DataSource = db.service_payments.Where(x => x.service_id == comboBox2.SelectedIndex + 1).ToList();
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            using (PSEntity db = new PSEntity())
            {
                dataGridView2.DataSource = db.service_payments.ToList();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            dataGridView2.Show();
        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.Show();
            chart1.Show();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chart1.Series[0].Points != null)
            {
                chart1.Series[0].Points.Clear();
            }
            using (PSEntity db = new PSEntity())
            {
                var dataCChart = db.metrics_archive.Where(x => x.service_id == comboBox2.SelectedIndex + 1 && x.client_id == user.client_id).ToList();
                dataGridView1.DataSource = dataCChart;
               foreach (var item in dataCChart)
               {
                    chart1.Series[0].Points.Add((double)item.usage);
                }
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AboutMe.Show();
            ServiceUsageData.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AboutMe.Hide();
            ServiceUsageData.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PayMentCreation.Hide();
            PayMentLog.Hide();
            ActiveDebts.Show();
            if (dataGridView3 == null || dataGridView3.Rows.Count == 0)
            {
                NoneDebt.Show();
                debtNotify.Hide();
                
            }
            else
            {
                DebtExtists.Show();
            }

            
        }
        private void UpdateForm()
        {
            FillDataGrid3();
            DebtNotifierShow();
        }

        private void MoneyInstertText_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
