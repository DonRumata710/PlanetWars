using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace AdminDesctopApplication
{
    public partial class Form1 : Form
    {
        public Form1(string user, string pwd)
        {
            login = user;
            password = pwd;

            InitializeComponent();
            user_listTableAdapter.Connection.ConnectionString = user_listTableAdapter.Connection.ConnectionString + ";user=" + login + ";password=" + password;
            SetTimer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.user_listTableAdapter.Fill(this.planetwarsDataSet.user_list);
            connection = new global::MySql.Data.MySqlClient.MySqlConnection();
            connection.ConnectionString = global::AdminDesctopApplication.Properties.Settings.Default.planetwarsConnectionString + ";user=" + login + ";password=" + password; ;
            connection.Open();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddUserForm form = new AddUserForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(form.password());

                SHA256 sha256ToCheck = SHA256.Create();
                byte[] hash = sha256ToCheck.ComputeHash(bytes);
                string hex = Convert.ToBase64String(hash);

                user_listTableAdapter.Insert(form.username(), form.email(), hex);
                UpdateUserList();
            }
        }

        private void blockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUserStatus("Block", "call block_user(@admin, @user, @reason)");
        }

        private void unblockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeUserStatus("Unblock", "call unblock_user(@admin, @user, @reason)");
        }

        private void ChangeUserStatus(string dialogTitle, string commandStr)
        {
            try
            {
                BlockUserForm form;
                if (userTable.SelectedRows.Count > 0)
                    form = new BlockUserForm(dialogTitle, login, password, userTable.SelectedRows[0].Cells[0].Value);
                else
                    form = new BlockUserForm(dialogTitle, login, password);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(commandStr, connection))
                    {
                        command.Parameters.Add("admin", MySql.Data.MySqlClient.MySqlDbType.String).Value = login;
                        command.Parameters.Add("user", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = form.userId();
                        command.Parameters.Add("reason", MySql.Data.MySqlClient.MySqlDbType.String).Value = form.reason();
                        command.ExecuteNonQuery();

                        UpdateUserList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetTimer()
        {
            updateTimer = new DispatcherTimer();
            updateTimer.Tick += new EventHandler(OnTimedEventOn);
            updateTimer.Interval = TimeSpan.FromSeconds(10);

            updateTimer.Start();
        }

        private void OnTimedEventOn(object sender, EventArgs e)
        {
            UpdateUserList();
        }

        private void UpdateUserList()
        {
            planetwarsDataSet.user_listDataTable table = new planetwarsDataSet.user_listDataTable();
            this.user_listTableAdapter.Fill(table);
            userTable.DataSource = table;
        }

        private string login;
        private string password;

        private global::MySql.Data.MySqlClient.MySqlConnection connection;

        private DispatcherTimer updateTimer;
    }
}
