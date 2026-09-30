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
    public partial class Customers : UserControl
    {
        public Customers()
        {
            InitializeComponent();
        }
        private void LoadUserData()
        {
            using (SqlConnection conn = new SqlConnection(DBConnect.connection))
            {
                conn.Open();
                string query = "SELECT * FROM Users";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["Id"].ToString(),
                        reader["FullName"].ToString(),
                        reader["Username"].ToString(),
                        reader["Password"].ToString()
                    );
                }

                conn.Close();
            }
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }
    }
}
