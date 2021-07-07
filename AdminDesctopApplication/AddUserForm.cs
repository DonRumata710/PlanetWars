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
    public partial class AddUserForm : Form
    {
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void usernameBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {
            CheckFields();
        }

        private void CheckFields()
        {
            this.okButton.Enabled = usernameBox.Text.Length > 0 && passwordBox.Text.Length > 0;
        }

        public string username()
        {
            return this.usernameBox.Text;
        }

        public string email()
        {
            return this.emailBox.Text;
        }

        public string password()
        {
            return this.passwordBox.Text;
        }
    }
}
