using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coffee_Shop_ManagementSystem
{
    public partial class AdminPortal : Form
    {
        public AdminPortal()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SidebarTimer.Start();
        }
        bool sidebarExpand=false;
        private void SidebarTimer_Tick(object sender, EventArgs e)
        {
            if(sidebarExpand)
            {
                SideBar.Visible=false;
                label5.Visible = false;
                Dclick = false;
                Pclick = false;
                Cclick = false;
                ResetButtonColors();
                Container.Controls.Clear();
                sidebarExpand =false;
                Container.Visible = false;
                pictureBox3.Visible=true;
                pictureBox3.BringToFront();
                SidebarTimer.Stop();
            }
            else
            {
                SideBar.Visible = true;
                label5.Visible = true;
                sidebarExpand = true;
                Container.Visible = true;
                pictureBox3.Visible = false;
                pictureBox3.SendToBack();
                SidebarTimer.Stop();
            }

        }
        private void label6_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are You Sure You Want Exit App", "EXIT APP?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        static bool Logbtn = false;
        private void button3_Click(object sender, EventArgs e)
        {
            Logbtn = true;
            this.Hide();
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
        }
        private void Dashboardbtn_MouseHover(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.BackColor = Color.FromArgb(240, 204, 145);
                btn.ForeColor = Color.FromArgb(140, 87, 51);
            }
        }
        private void Dashboardbtn_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            if ((btn == Dashboardbtn && !Dclick) ||
                (btn == ProductBtn && !Pclick) ||
                (btn == CustomersBtn && !Cclick) ||
                (btn == Orderbtn && !Oclick) ||
                (btn == button3 && !Logbtn)) // button3 is Logout
            {
                btn.BackColor = Color.FromArgb(140, 87, 51);
                btn.ForeColor = Color.FromArgb(240, 204, 145);
            }
        }

       
        static bool Dclick=false;
        static bool Pclick = false;
        static bool Cclick = false;
        static bool Oclick = false;

        Color defaultBackColor = Color.FromArgb(140, 87, 51);
        Color defaultForeColor = Color.FromArgb(240, 204, 145);
        Color activeBackColor = Color.FromArgb(240, 204, 145);
        Color activeForeColor = Color.FromArgb(140, 87, 51);

        private void ResetButtonColors()
        {
            Dashboardbtn.BackColor = defaultBackColor;
            Dashboardbtn.ForeColor = defaultForeColor;

            ProductBtn.BackColor = defaultBackColor;
            ProductBtn.ForeColor = defaultForeColor;

            CustomersBtn.BackColor = defaultBackColor;
            CustomersBtn.ForeColor = defaultForeColor;

            Orderbtn.BackColor = defaultBackColor;
            Orderbtn.ForeColor = defaultForeColor;
        }
        private void Dashboardbtn_Click(object sender, EventArgs e)
        {
            Dclick = true;
            Pclick = false;
            Cclick = false;
            Oclick = false;
            ResetButtonColors();
            Dashboardbtn.BackColor = activeBackColor;
            Dashboardbtn.ForeColor = activeForeColor;
            Container.Controls.Clear();
            DashboardAdmin dashboard = new DashboardAdmin();
            dashboard.Dock = DockStyle.Fill;
            Container.Controls.Add(dashboard);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Pclick = true; Dclick = false; Cclick = false; Oclick = false; 
            ResetButtonColors();
            ProductBtn.BackColor = activeBackColor;
            ProductBtn.ForeColor = activeForeColor;
            Container.Controls.Clear();
            Products products = new Products();
            products.Dock = DockStyle.Fill;
            Container.Controls.Add(products);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cclick = true; Pclick = false; Dclick = false; Oclick = false ;
            ResetButtonColors();
            CustomersBtn.BackColor = activeBackColor;
            CustomersBtn.ForeColor = activeForeColor;
            Container.Controls.Clear();
            Customers customers = new Customers();
            customers.Dock = DockStyle.Fill;
            Container.Controls.Add(customers);
        }
        private void Orderbtn_Click(object sender, EventArgs e)
        {
            Oclick = true; Cclick = false; Pclick = false; Dclick = false;
            ResetButtonColors();
            Orderbtn.BackColor = activeBackColor;
            Orderbtn.ForeColor = activeForeColor;
            Container.Controls.Clear();
            Orders orders = new Orders();
            orders.Dock = DockStyle.Fill;
            Container.Controls.Add(orders);
        }
        private void AdminPortal_Load(object sender, EventArgs e)
        {
            pictureBox3.BringToFront();
            
        }

        
    }
}
