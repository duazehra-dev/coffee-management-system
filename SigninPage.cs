using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Coffee_Shop_ManagementSystem
{
    public partial class SigninPage : Form
    {
        public SigninPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            loginPage.Activate();
            
        }

        private void label6_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are You Sure You Want Exit App", "EXIT APP?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            if (showPass.Checked)
            {
                tbPassword.PasswordChar = '\0';
            }
            else
            {
                tbPassword.PasswordChar = '*';
            }
        }

        private void btnSignin_Click(object sender, EventArgs e)
        {
            string Fullname= tbfullname.Text;
            string Username = tbusername.Text;
            string Password = tbPassword.Text;


            using (SqlConnection con = new SqlConnection(DBConnect.connection))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                SqlCommand Ckcmd = new SqlCommand(query, con);
                Ckcmd.Parameters.AddWithValue("@Username", Username);
                int exists = (int)Ckcmd.ExecuteScalar();
                if(exists > 0)
                {
                    MessageBox.Show("Username Already Taken ", "ERROR", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
                else
                {
                    query = "INSERT INTO Users (FullName, Username, Password) VALUES (@FullName, @Username, @Password)";
                    SqlCommand Incmd = new SqlCommand(query, con);
                    Incmd.Parameters.AddWithValue("@FullName", Fullname.Trim());
                    Incmd.Parameters.AddWithValue("@Username", Username.Trim());
                    Incmd.Parameters.AddWithValue("@Password", Password.Trim());
                    Incmd.ExecuteNonQuery();
                    MessageBox.Show("User registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbfullname.Clear();
                    tbusername.Clear();
                    tbPassword.Clear();
                    LoginPage loginPage = new LoginPage();
                    loginPage.Show();
                    this.Close();
                }
                con.Close();
            }
            


        }
        private void SigninPage_Load(object sender, EventArgs e)
        {

        }
    }
}
