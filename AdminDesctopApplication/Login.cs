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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public string login { get; set; }
        public string password { get; set; }

        private void okButton_Click(object sender, EventArgs e)
        {
            login = loginBox.Text;
            password = passwordBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void loginBox_TextChanged(object sender, EventArgs e)
        {
            okButton.Enabled = loginBox.Text.Length > 0;
        }
    }
}
