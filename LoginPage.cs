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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Coffee_Shop_ManagementSystem
{

    public partial class LoginPage : Form
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        int userid;
        string fullname;
        public bool emptyfields()
        {
            if(txtusern.Text=="" || txtPass.Text=="")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtusern.Text.Trim();
            string password = txtPass.Text.Trim();
            if (emptyfields())
            {
                MessageBox.Show("All fields are required to filled", "ERROR Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {        using (SqlConnection con = new SqlConnection(DBConnect.connection))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                    SqlCommand Ckcmd = new SqlCommand(query, con);
                    Ckcmd.Parameters.AddWithValue("@Username", username);
                    Ckcmd.Parameters.AddWithValue("@Password", password);
                    int exists = (int)Ckcmd.ExecuteScalar();
                    if (exists==1 && username=="Admin")
                    {
                        MessageBox.Show("LOGIN SUCCESSFULLY ", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        AdminPortal adminPortal = new AdminPortal();
                        adminPortal.Show();
                    }
                    
                    else if (exists == 1)
                    {
                        MessageBox.Show("LOGIN SUCCESSFULLY ", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        query = "SELECT id , FullName FROM Users WHERE Username = @Username AND Password = @Password";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        var reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            userid = reader.GetInt32(0);
                            fullname = reader.GetString(1);

                        }
                        CustomerPortal customerPortal = new CustomerPortal(userid,username,password,fullname);
                        customerPortal.Show();
                        this.Hide();


                    }
                    else
                    {

                        MessageBox.Show("Invalid Username Or Password", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    
                    
                    con.Close();
                }

            }
            txtusern.Clear();
            txtPass.Clear();
        }
        private void label6_Click(object sender, EventArgs e)
        {
            DialogResult dr =  MessageBox.Show("Are You Sure You Want Exit App", "EXIT APP?", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(showPass.Checked)
            {
                txtPass.PasswordChar = '\0';
            }
            else
            {
                txtPass.PasswordChar = '*';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            SigninPage signinPage = new SigninPage();
            signinPage.Show();
            signinPage.Activate();
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {

        }

    }
}
