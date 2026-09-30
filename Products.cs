using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Coffee_Shop_ManagementSystem
{
    public partial class Products : UserControl
    {
        public Products()
        {
            InitializeComponent();

        }
        private void LoadData()
        {
            comboBox1.SelectedIndex = 0;
            using (SqlConnection conn = new SqlConnection(DBConnect.connection))
            {
                conn.Open();
                string query = "Select * FROM Products";
                SqlCommand cmd = new SqlCommand(query, conn);


                var reader = cmd.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["Id"].ToString(),
                        reader["Category"].ToString(),
                        reader["ProductName"].ToString(),
                        reader["Price"].ToString(),
                        reader["Quantity"].ToString(),
                        reader["Image"] as byte[]
);
                }
                conn.Close();
            }
        }
        private void Products_Load(object sender, EventArgs e)
        {
            LoadData();

        }
        static int getid;
        private byte[] selectedImageBytes = null;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex<0)
            {
                return;
            }
            string query = "";
            getid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Id"].Value);
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Confirm", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(DBConnect.connection))
                    {
                        query = "DELETE FROM Products WHERE Id = @Id";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Id", getid);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
                
            }
            else if(dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                string category = dataGridView1.Rows[e.RowIndex].Cells["Category"].Value.ToString();
                string productname = dataGridView1.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
                string price = dataGridView1.Rows[e.RowIndex].Cells["Price"].Value.ToString();
                string quantity = dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();

                cbCategory.SelectedIndex = -1;
                cbCategory.SelectedText = category;
                tbProductName.Text = productname;
                tbPrice.Text = price;
                tbQuantity.Text = quantity;

                byte[] imgData = dataGridView1.Rows[e.RowIndex].Cells["ImageCol"].Value as byte[];
                if (imgData != null && imgData.Length > 0)
                {
                    var ms = new MemoryStream(imgData);
                    pbProductImage.Image = Image.FromStream(ms);
                    selectedImageBytes = imgData;
                }

                btnSave.Text = "UPDATE";
                Insertbtn.Visible = true;
            }
        }


        

        private void tbfullname_TextChanged(object sender, EventArgs e)
        {
            string SearchData = tbSearch.Text;
            using (SqlConnection conn = new SqlConnection(DBConnect.connection))
            {
                String query = "Select Id, Category , ProductName ,Price ,Quantity From Products WHERE ";
                conn.Open();
                if (comboBox1.SelectedIndex == 0)
                {
                    query = "Select Id, Category , ProductName ,Price ,Quantity From Products " +
                        "WHERE Category LIKE @Search " +
                        "OR ProductName LIKE @Search";
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    query += "Category LIKE @Search";
                }
                else if(comboBox1.SelectedIndex == 2)
                {
                    query += " ProductName LIKE @Search";
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Search", "%" + SearchData + "%");
                var reader = cmd.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader["Id"].ToString(),
                        reader["Category"].ToString(),
                        reader["ProductName"].ToString(),
                        reader["Price"].ToString(),
                        reader["Quantity"].ToString()
                    );
                }
                conn.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string category = cbCategory.Text.ToString();
            string productname =tbProductName.Text;
            int price = int .Parse(tbPrice.Text);
            int quantity = int.Parse(tbQuantity.Text);

            using (SqlConnection conn = new SqlConnection(DBConnect.connection))
            {
                SqlCommand cmd = null;
                string query = "";

                if (btnSave.Text == "UPDATE")
                {
                    query = "UPDATE Products SET Category=@category, ProductName=@productname, Price=@price, Quantity=@quantity, Image=@image WHERE Id = @id";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", getid);
                }
                else if (btnSave.Text == "ADD")
                {
                    query = "INSERT INTO Products (Category, ProductName, Price, Quantity, Image) VALUES (@category, @productname, @price, @quantity, @image)"; cmd = new SqlCommand(query, conn);
                }

                if (cmd != null)
                {
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@productname", productname);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    SqlParameter imgParam = new SqlParameter("@image", SqlDbType.VarBinary, -1);
                    imgParam.Value = (object)selectedImageBytes ?? DBNull.Value;
                    cmd.Parameters.Add(imgParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    cbCategory.SelectedIndex = -1;
                    cbCategory.Text = "";
                    tbProductName.Clear();
                    tbPrice.Clear();
                    tbQuantity.Clear();
                    pbProductImage.Image = null;
                    selectedImageBytes = null;

                    MessageBox.Show("Product Added Successfully","SUCCESS",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else 
                {
                    MessageBox.Show("cmd is null", "Error", MessageBoxButtons.OK);
                }
            }
            LoadData();

        }

        private void Clearbtn_Click(object sender, EventArgs e)
        {
            btnSave.Text = "ADD";
            cbCategory.SelectedIndex = -1;
            cbCategory.Text = "";
            tbProductName.Clear();
            tbPrice.Clear();
            tbQuantity.Clear();
            pbProductImage.Image = null;
            selectedImageBytes = null;

        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                selectedImageBytes = File.ReadAllBytes(ofd.FileName);
                pbProductImage.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void Insertbtn_Click(object sender, EventArgs e)
        {
            btnSave.Text = "ADD";
            Insertbtn.Visible = false;
            cbCategory.SelectedIndex = -1;
            cbCategory.Text = "";
            tbProductName.Clear();
            tbPrice.Clear();
            tbQuantity.Clear();
            pbProductImage.Image = null;
selectedImageBytes = null;
        }

        
    }
}
