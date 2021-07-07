using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminDesctopApplication
{
    public partial class BlockUserForm : Form
    {
        public BlockUserForm(string action, string login, string password, object userId = null)
        {
            InitializeComponent();
            usernameListTableAdapter.Connection.ConnectionString = usernameListTableAdapter.Connection.ConnectionString + ";user=" + login + ";password=" + password;

            selectedUser = userId;

            Text = action + " user";
            reasonLabel.Text = action + " reason:";
        }

        public int userId()
        {
            int id;
            bool parseOK = Int32.TryParse(usernameBox.SelectedValue.ToString(), out id);
            if (!parseOK)
                return -1;
            return id;
        }
        public string reason() { return reasonBox.Text; }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void usernameBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void reasonBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void CheckFields()
        {
            this.okButton.Enabled = usernameBox.Text.Length > 0 && reasonBox.Text.Length > 0;
        }

        private void BlockUserForm_Load(object sender, EventArgs e)
        {
            this.usernameListTableAdapter.Fill(this.planetwarsDataSet.username_list);
            if (selectedUser != null)
                usernameBox.SelectedValue = selectedUser;
        }

        private object selectedUser;
    }
}
