using System;
using System.Data.OleDb;
using System.Windows.Forms;


namespace Opp2
{
    public partial class password : Form
    {
        private const string Username = "admin";
        private const string Password = "admin123";
        string link = "Provider=Microsoft.ACE.OLEB8.12.0;Data Source=\"C:\\Users\\IPI\\Destop\\opp2 1\\opp2\\opp2\\opp2\\OPP.accdb\"";
        public password()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool AccountExists(string username, string password)
        {
            bool exists = false;
            try
            {
                string query = "SELECT * From Accounts WHERE [Username] = @user and [Password] = @pass";
                using (OleDbConnection conn = new OleDbConnection(link))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.Parameters.AddWithValue("@pass", Password);
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            return exists = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return exists;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string enteredUsername = user_tbx.Text;
            string enteredPassword = password_tbx.Text;
            bool exists = AccountExists(enteredUsername, enteredPassword);
            if (exists)
            {
                menu form = new menu(user_tbx.Text, link);
                form.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or Password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SIGN_UP_FORM fORM = new SIGN_UP_FORM(link);
            fORM.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void user_tbx_TextChanged(object sender, EventArgs e)
        {

        }

        private void password_tbx_TextChanged(object sender, EventArgs e)
        {

        }

    }

}