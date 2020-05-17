using System;
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

    public partial class AuthForm : Form
    {
        public clients_table client;
        public worker_table worker;

        public AuthForm()
        {
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            layer1.Show();
            layer2.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            layer2.Show();
            layer1.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //sumbit
            using (PSEntity db = new PSEntity())
            {
                var clients = db.client_acc_table;
                var clientInfo = db.clients_table;
                var workerInfo = db.worker_table;
                var workers = db.staff_acc_table;

                foreach (client_acc_table item in clients)
                {
                    if(LoginText.Text.Trim() == item.username && PasswordText.Text.Trim() == item.password)
                    {
                        foreach (clients_table cli3nt in clientInfo)
                        {
                            if(item.client_id == cli3nt.client_id)
                            {
                                client = cli3nt;
                            }
                        }

                        MessageService.SuccsesfulLogin();

                        MainFormClient mainFormClient = new MainFormClient(client);
                        this.Hide();
                        mainFormClient.Show();
                    }
                }

                foreach (staff_acc_table item in workers)
                {
                    if (LoginText.Text.Trim() == item.username && PasswordText.Text.Trim() == item.password)
                    {
                        foreach (worker_table work3r in workerInfo)
                        {
                            if (work3r.worker_id == item.worker_id)
                            {


                                switch (item.role_id)
                                {
                                    case 1:
                                        {
                                            MessageService.SuccsesfulLogin();
                                            this.Hide();
                                            MainFormDir MainFormEconom = new MainFormDir(work3r);
                                            MainFormEconom.Show();
                                            break;
                                        }

                                    case 2:
                                        {
                                            MessageService.SuccsesfulLogin();
                                            this.Hide();
                                            MainFormEconom MainFormEconom = new MainFormEconom(work3r);
                                            MainFormEconom.Show();
                                            break;
                                        }

                                    case 3:
                                        {
                                            MessageService.SuccsesfulLogin();
                                            this.Hide();
                                            MainFormHH MainFormEconom = new MainFormHH(work3r);
                                            MainFormEconom.Show();
                                            break;
                                        }

                                    case 4:
                                        {
                                            MessageService.SuccsesfulLogin();
                                            this.Hide();
                                            MainFormAcc mainForm = new MainFormAcc(work3r);
                                            this.Hide();
                                            mainForm.Show();
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }

                if(client == null && worker == null)
                {
                    MessageService.FailedLogin();
                    Login.Text = PasswordText.Text = "";
                }
            }

         
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            using (PSEntity db = new PSEntity())
            {
                var clientUserBase = db.client_acc_table;
                var clientDataBase = db.clients_table;


                if (newPassword.Text.Trim() != newPasswordAgain.Text.Trim())
                {
                    MessageService.PasswordMatch();
                    newLogin.Text = newPassword.Text = newPasswordAgain.Text = "";
                }
                else
                {
                    foreach (clients_table item in clientDataBase)
                    {
                        if (Int32.Parse(id.Text.Trim()) == item.client_id)
                        {
                            foreach (client_acc_table userX in clientUserBase)
                            {
                                if (newLogin.Text.Trim() != userX.username)
                                {
                                    db.client_acc_table.Add(new client_acc_table
                                    {
                                        id = db.client_acc_table.Count() + 1,
                                        username = newLogin.Text.Trim(),
                                        password = newPassword.Text.Trim(),
                                        client_id = Int32.Parse(id.Text.Trim())
                                    });
                                    MessageService.SuccsesfulRegister();
                                   
                                }
                                else
                                {
                                    MessageService.NameIsNotVacant();
                                }
                            }
                        }
                        else
                        {
                            MessageService.FailedClientIdSearch();
                        }
                    }
                }
                db.SaveChanges();
                id.Text = newLogin.Text = newPassword.Text = newPasswordAgain.Text = "";
            }
        }

        private void id_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
