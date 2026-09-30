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
    public partial class DashboardCustomer : UserControl
    {
        public DashboardCustomer()
        {
            InitializeComponent();
        }
        int userid;
        public DashboardCustomer(int userid,string username,string password,string fullname)
        {
            InitializeComponent();
            this.userid= userid;
            Idlbl.Text = userid.ToString();
            usernamelbl.Text = username;
            passwordlbl.Text = password;
            fullnamelbl.Text = fullname;

            string query = "SELECT OrderId, OrderDate, Phone , Address,TotalAmount FROM Orders WHERE UserId = @UserId ";

            using (SqlConnection con = new SqlConnection(DBConnect.connection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userid);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
         
                dataGridView1.DataSource = table; 
                dataGridView1.Columns["OrderId"].Width = 80;
                dataGridView1.Columns["OrderDate"].Width = 150;
                dataGridView1.Columns["Phone"].Width = 150;
                dataGridView1.Columns["Address"].Width = 190;
                con.Close();
            }

        }

        private void DashboardCustomer_Load(object sender, EventArgs e)
        {

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "Details")
            {
                int orderId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["OrderId"].Value);
                ShowDetails showDetails = new ShowDetails(orderId);
                showDetails.ShowDialog();
            }
        }

        private void ChangePass_Click(object sender, EventArgs e)
        {
            string password = txtPass.Text.Trim();
            string Cpassword = txtConf.Text.Trim();
            if(password == "" || Cpassword == "")
            {
                MessageBox.Show("All Fields Must be Filled", "ERROR",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(password == Cpassword)
            {
                string query = "UPDATE Users SET Password = @password WHERE id = @id ";

                using (SqlConnection con = new SqlConnection(DBConnect.connection))
                {
                    try 
                    { 
                         con.Open();
                         SqlCommand cmd = new SqlCommand(query, con);
                         cmd.Parameters.AddWithValue("@password", password);
                         cmd.Parameters.AddWithValue("@id", userid);
                         cmd.ExecuteNonQuery();
                         MessageBox.Show("Your Password Has Been Change Successfully","SUCCESS",MessageBoxButtons.OK,MessageBoxIcon.Information);


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
