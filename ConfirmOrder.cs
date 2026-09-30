using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coffee_Shop_ManagementSystem
{
    public partial class ConfirmOrder : Form
    {
        public ConfirmOrder()
        {
            InitializeComponent();
        }
        int GrandTotal,userid;
        PlaceOrder control_caller;
      
        public ConfirmOrder(int userid,int GrandTotal,DataTable cartTable,PlaceOrder caller)
        {
            InitializeComponent();
            this.GrandTotal = GrandTotal;
            dataGridView1.DataSource = cartTable;
            this.userid = userid;
            control_caller = caller;
        }
        private void SaveOrder(int userid, int GrandTotal, string phone, string address, DataGridView cartGrid)
        {
            using (SqlConnection conn = new SqlConnection(DBConnect.connection))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string query1 = "INSERT INTO Orders (UserId, Phone, Address, TotalAmount) OUTPUT INSERTED.OrderId VALUES (@UserId, @Phone, @Address, @TotalAmount)";
                    SqlCommand cmd1 = new SqlCommand(query1, conn, transaction);
                    cmd1.Parameters.AddWithValue("@UserId", userid);
                    cmd1.Parameters.AddWithValue("@Phone", phone);
                    cmd1.Parameters.AddWithValue("@Address", address);
                    cmd1.Parameters.AddWithValue("@TotalAmount", GrandTotal);

                    int orderid=(int)cmd1.ExecuteScalar();


                    foreach (DataGridViewRow row in cartGrid.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string product = row.Cells["ProductName"].Value.ToString();
                            int qty = Convert.ToInt32(row.Cells["Quantity"].Value);
                            int price = Convert.ToInt32(row.Cells["Total"].Value);

                            string query2 = "INSERT INTO OrderItems (OrderId, ProductName, Quantity, Price) VALUES (@OrderId, @ProductName, @Quantity, @Price)";
                            SqlCommand cmd2 = new SqlCommand(query2, conn, transaction);
                            cmd2.Parameters.AddWithValue("@OrderId", orderid);
                            cmd2.Parameters.AddWithValue("@ProductName", product);
                            cmd2.Parameters.AddWithValue("@Quantity", qty);
                            cmd2.Parameters.AddWithValue("@Price", price);
                            cmd2.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                     DialogResult dr = MessageBox.Show("Order placed successfully!\nDo You Want To Print Receipt ","SUCCESS",MessageBoxButtons.YesNo);
                    if(dr == DialogResult.Yes)
                    {

                        DGVPrinter printer = new DGVPrinter();

                        printer.Title = "COFFEE SHOP Receipt";
                        printer.SubTitle = $"Customer ID : {userid}\nDate: {DateTime.Now.ToShortDateString()}   Time: {DateTime.Now.ToShortTimeString()}";
                        printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

                        

                        // Optional: Resize page (for smaller print)
                        printer.printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Receipt", 280, 400);

                        

                        printer.PageNumbers = false;
                        printer.PageNumberInHeader = false;
                        printer.PorportionalColumns = true;
                        printer.HeaderCellAlignment = StringAlignment.Near;
                        

                        printer.Footer = $"Total Amount: Rs. {GrandTotal}\nThankyou For Ordering ";  // Optional
                        printer.FooterSpacing = 30;

                        // Print Preview (or use .PrintDataGridView to print directly)
                        printer.PrintPreviewDataGridView(dataGridView1);
                    }
                    this.Close();
                    dataGridView1.DataSource = null;
                    control_caller.ClearDataGridView();
                    
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error saving order: " + ex.Message);
                }
            }
        }

        private void ConfirmOrder_Load(object sender, EventArgs e)
        {
            gTotallbl.Text = GrandTotal.ToString("N0");
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PlaceOrderbtn_Click(object sender, EventArgs e)
        {
            string address=tbAddress.Text;
            string phone =tbContact.Text;
            

            using (SqlConnection con = new SqlConnection(DBConnect.connection))
            {
                con.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string productName = row.Cells["ProductName"].Value.ToString();
                        int orderedQty = Convert.ToInt32(row.Cells["Quantity"].Value);

                        // Reduce quantity from Products table
                        string query = "UPDATE Products SET Quantity = Quantity - @quantity WHERE ProductName = @ProductName";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@quantity", orderedQty);
                            cmd.Parameters.AddWithValue("@ProductName", productName);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            if(address!= "" && phone!= "")
                SaveOrder(userid, GrandTotal, phone, address, dataGridView1);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
