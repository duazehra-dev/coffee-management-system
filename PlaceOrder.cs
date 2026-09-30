using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coffee_Shop_ManagementSystem
{
    public partial class PlaceOrder : UserControl
    {
        int userid;

        public PlaceOrder()
        {
            InitializeComponent();
        }

        public PlaceOrder(int userid)
        {
            InitializeComponent();
            this.userid = userid;
        }

        public void ClearDataGridView()
        {
            dataGridView2.Rows.Clear();
        }

        private void LoadData()
        {
            flpMenu.Controls.Clear();
            string category = cbCategory.Text.Trim();
            using (SqlConnection conn = new SqlConnection(DBConnect.connection))
            {
                conn.Open();
                string query = "SELECT ProductName, Price, Image FROM Products WHERE Category = @category";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@category", category);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader["ProductName"].ToString();
                    int price = Convert.ToInt32(reader["Price"]);
                    byte[] imgData = reader["Image"] as byte[];
                    Panel card = CreateMenuCard(name, price, imgData);
                    flpMenu.Controls.Add(card);
                }
                conn.Close();
            }
        }

        private Panel CreateMenuCard(string name, int price, byte[] imgData)
        {
            Panel card = new Panel();
            card.Size = new Size(165, 215);
            card.BackColor = Color.FromArgb(221, 161, 94);
            card.Margin = new Padding(8);

            PictureBox pb = new PictureBox();
            pb.Size = new Size(140, 100);
            pb.Location = new Point(5, 5);
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.BackColor = Color.White;
            if (imgData != null && imgData.Length > 0)
            {
                var ms = new MemoryStream(imgData);
                pb.Image = Image.FromStream(ms);
            }

            Label lblName = new Label();
            lblName.Text = name;
            lblName.Font = new Font("Arial", 8, FontStyle.Bold);
            lblName.ForeColor = Color.FromArgb(140, 87, 51);
            lblName.Location = new Point(5, 110);
            lblName.Size = new Size(140, 20);
            lblName.TextAlign = ContentAlignment.MiddleCenter;

            Label lblPrice = new Label();
            lblPrice.Text = "Rs. " + price;
            lblPrice.Font = new Font("Arial", 8);
            lblPrice.ForeColor = Color.FromArgb(140, 87, 51);
            lblPrice.Location = new Point(5, 130);
            lblPrice.Size = new Size(140, 20);
            lblPrice.TextAlign = ContentAlignment.MiddleCenter;

            NumericUpDown nudQty = new NumericUpDown();
            nudQty.Minimum = 1;
            nudQty.Maximum = 10;
            nudQty.Value = 1;
            nudQty.Location = new Point(5, 158);
            nudQty.Size = new Size(60, 22);

            Button btnAdd = new Button();
            btnAdd.Text = "Add";
            btnAdd.BackColor = Color.FromArgb(140, 87, 51);
            btnAdd.ForeColor = Color.FromArgb(240, 204, 145);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Location = new Point(70, 155);
            btnAdd.Size = new Size(75, 28);
            btnAdd.Click += (s, e) =>
            {
                int qty = (int)nudQty.Value;
                int total = price * qty;
                string category = cbCategory.Text;
                bool found = false;
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[1].Value?.ToString() == name)
                    {
                        int existing = Convert.ToInt32(row.Cells[3].Value);
                        int newQty = existing + qty;
                        row.Cells[3].Value = newQty.ToString();
                        row.Cells[2].Value = (price * newQty).ToString();
                        found = true;
                        break;
                    }
                }
                if (!found)
                    dataGridView2.Rows.Add(category, name, total, qty);

                gTotal = CalculateGrandTotal();
                gTotallbl.Text = gTotal.ToString("N0");
            };

            card.Controls.AddRange(new Control[] { pb, lblName, lblPrice, nudQty, btnAdd });
            return card;
        }

        private void PlaceOrder_Load(object sender, EventArgs e)
        {
            cbCategory.SelectedIndex = 0;
            LoadData();
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private int CalculateGrandTotal()
        {
            int grandTotal = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                int total = Convert.ToInt32(row.Cells[2].Value);
                grandTotal += total;
            }
            return grandTotal;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (dataGridView2.Columns[e.ColumnIndex].Name == "remove")
            {
                dataGridView2.Rows.RemoveAt(e.RowIndex);
            }
        }

        static int gTotal;

        private void dataGridView2_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            gTotal = CalculateGrandTotal();
            gTotallbl.Text = gTotal.ToString("N0");
        }

        private void PlaceOrderbtn_Click(object sender, EventArgs e)
        {
            DataTable cartTable = new DataTable();
            cartTable.Columns.Add("Category");
            cartTable.Columns.Add("ProductName");
            cartTable.Columns.Add("Total");
            cartTable.Columns.Add("Quantity");

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (!row.IsNewRow)
                {
                    DataRow newRow = cartTable.NewRow();
                    for (int i = 0; i < cartTable.Columns.Count; i++)
                    {
                        newRow[i] = row.Cells[i].Value;
                    }
                    cartTable.Rows.Add(newRow);
                }
            }

            ConfirmOrder confirmorder = new ConfirmOrder(userid, gTotal, cartTable, this);
            confirmorder.ShowDialog();
        }
    }
}