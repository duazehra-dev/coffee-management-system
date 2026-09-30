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

namespace Coffee_Shop_ManagementSystem
{
    public partial class DashboardAdmin : UserControl
    {
        public DashboardAdmin()
        {
            InitializeComponent();
        }

        private void DashboardAdmin_Load(object sender, EventArgs e)
        {
            string query = "";

            using (SqlConnection conn = new SqlConnection(DBConnect.connection))
            {
                conn.Open();
                query = "SELECT COUNT(*) FROM Users";
                SqlCommand custcmd = new SqlCommand(query, conn);
                try
                {
                    
                    int total = (int)custcmd.ExecuteScalar(); // ExecuteScalar returns a single value
                    TotCustlbl.Text = total.ToString();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                query = "SELECT COUNT(*) FROM Products";
                SqlCommand prodcmd = new SqlCommand(query, conn);
                try
                {
                    int total = (int)prodcmd.ExecuteScalar(); // ExecuteScalar returns a single value
                    TotProdlbl.Text = total.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

                query = "SELECT COUNT(*) FROM Orders";
                SqlCommand ordercmd = new SqlCommand(query, conn);
                try
                {
                    int total = (int)ordercmd.ExecuteScalar(); // ExecuteScalar returns a single value
                    Totorderlbl.Text = total.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

                query = "SELECT SUM(TotalAmount) AS TotalIncome FROM Orders;";
                SqlCommand incomecmd = new SqlCommand(query, conn);
                try
                {
                    var result = incomecmd.ExecuteScalar(); // ExecuteScalar returns a single value

                    if (result != DBNull.Value && result != null)
                    {
                        int totalIncome = Convert.ToInt32(result);
                        TotIncomelbl.Text = totalIncome.ToString();
                    }
                    else
                    {
                        TotIncomelbl.Text = "0";
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                conn.Close();
            }
        }

        private void ChangePassword_Click(object sender, EventArgs e)
        {
            CHPassPnl.Visible = true;
            ChangePassword.Visible = false;
        }

        private void label15_Click(object sender, EventArgs e)
        {
            CHPassPnl.Visible = false;
            ChangePassword.Visible = true;
        }

        private void txtusern_TextChanged(object sender, EventArgs e)
        {
            if (showPass.Checked)
            {
                txtConf.PasswordChar = '\0';
                txtPass.PasswordChar = '\0';
            }
            else
            {
                txtConf.PasswordChar = '*';
                txtPass.PasswordChar = '*';
            }
        }


        private void ChangePass_Click(object sender, EventArgs e)
        {
            string password = txtPass.Text.Trim();
            string Cpassword = txtConf.Text.Trim();
            if (password == "" || Cpassword == "")
            {
                MessageBox.Show("All Fields Must be Filled", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (password == Cpassword)
            {
                string query = "UPDATE Users SET Password = @password WHERE id = @id ";

                using (SqlConnection con = new SqlConnection(DBConnect.connection))
                {
                    try
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@id", "3");
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Your Password Has Been Change Successfully", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        CHPassPnl.Visible = false;
                        ChangePassword.Visible = true;
                        passwordlbl.Text = password;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR : " + ex.Message);
                    }
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("The Password and Confirm Password Need to Be Same ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
