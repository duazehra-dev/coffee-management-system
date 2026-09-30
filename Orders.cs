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
    public partial class Orders : UserControl
    {
        public Orders()
        {
            InitializeComponent();

            string query = "SELECT * FROM Orders";

            using (SqlConnection con = new SqlConnection(DBConnect.connection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table; 
                dataGridView1.Columns["OrderId"].Width = 70;
                dataGridView1.Columns["UserId"].Width = 70;
                dataGridView1.Columns["OrderDate"].Width = 130;
                dataGridView1.Columns["Phone"].Width = 125;
                dataGridView1.Columns["Address"].Width = 185;
                con.Close();
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
    }
}
