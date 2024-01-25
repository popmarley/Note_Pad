using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Not_Defteri
{
    public partial class Password : Form
    {
        private TextBox txtPassword;
        private Button btnSubmit;

        public string Passwords { get; private set; }

        public Password()
        {
            InitializeComponent();

            txtPassword = new TextBox();
            txtPassword.PasswordChar = '*';
            txtPassword.Location = new Point(10, 10);
            txtPassword.Size = new Size(200, 20);
            this.Controls.Add(txtPassword);

            btnSubmit = new Button();
            btnSubmit.Text = "Onayla";
            btnSubmit.Location = new Point(75, 40);
            btnSubmit.Click += btnSubmit_Click;
            this.Controls.Add(btnSubmit);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.Passwords = txtPassword.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
