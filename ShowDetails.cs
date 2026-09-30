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
    public partial class ShowDetails : Form
    {
        public ShowDetails()
        {
            InitializeComponent();
        }
        public ShowDetails(int orderId)
        {
            InitializeComponent();
            string connection = "Data Source=localhost\\SQLEXPRESS01;Initial Catalog=Cafe;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            using (SqlConnection con = new SqlConnection(connection))
            {
                string query = "SELECT ProductName, Quantity, Price FROM OrderItems WHERE OrderId = @OrderId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@OrderId", orderId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table;


                dataGridView1.Columns["ProductName"].Width = 200;
                dataGridView1.Columns["Quantity"].Width = 150;
                dataGridView1.Columns["Price"].Width = 150;
            }
        }

        private void ShowDetails_Load(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
