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
    public partial class CustomerPortal : Form
    {
        public CustomerPortal()
        {
            InitializeComponent();
        }
        string username,password,fullname;
        int userid;
        public CustomerPortal(int userid, string username,string password,string fullname)
        {
            InitializeComponent();
            this.username = username;
            this.userid = userid;
            this.password=password;
            this.fullname = fullname;
        }

        private void CustomerPortal_Load(object sender, EventArgs e)
        {
            label2.Text= username;
            pictureBox3.BringToFront();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SidebarTimer.Start();
        }
        bool sidebarExpand = false;
        private void SidebarTimer_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                SideBar.Visible = false;
                label5.Visible = false;
                sidebarExpand = false;
                Container.Visible = false;
                pictureBox3.Visible = true;
                pictureBox3.BringToFront();
                Container.Controls.Clear();
                SidebarTimer.Stop();
            }
            else
            {
                SideBar.Visible = true;
                label5.Visible= true;
                sidebarExpand = true;
                Container.Visible = true;
                pictureBox3.Visible = false;
                pictureBox3.SendToBack();
                SidebarTimer.Stop();
            }

        }
        Color defaultBackColor = Color.FromArgb(140, 87, 51);
        Color defaultForeColor = Color.FromArgb(240, 204, 145);
        Color activeBackColor = Color.FromArgb(240, 204, 145);
        Color activeForeColor = Color.FromArgb(140, 87, 51);


        static bool Dclick = false;
        static bool Pclick = false;
        private void ResetButtonColors()
        {
            Dashboardbtn.BackColor = defaultBackColor;
            Dashboardbtn.ForeColor = defaultForeColor;

            PlaceOrderbtn.BackColor = defaultBackColor;
            PlaceOrderbtn.ForeColor = defaultForeColor;

            
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

            if ((btn == Dashboardbtn && !Dclick || btn == PlaceOrderbtn && !Pclick))
            {
                btn.BackColor = Color.FromArgb(140, 87, 51);
                btn.ForeColor = Color.FromArgb(240, 204, 145);
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
        }

        private void Dashboardbtn_Click(object sender, EventArgs e)
        {
            Dclick = true;
            Pclick = false;
            Container.Controls.Clear();
            ResetButtonColors();
            Dashboardbtn.BackColor = activeBackColor;
            Dashboardbtn.ForeColor = activeForeColor;
            DashboardCustomer dashboard = new DashboardCustomer(userid,username,password,fullname);
            dashboard.Dock = DockStyle.None;
            Container.Controls.Add(dashboard);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dclick = false;
            Pclick = true;
            Container.Controls.Clear();
            ResetButtonColors();
            PlaceOrderbtn.BackColor = activeBackColor;
            PlaceOrderbtn.ForeColor = activeForeColor;
            PlaceOrder placeorder = new PlaceOrder(userid);
            placeorder.Dock = DockStyle.Fill;
            Container.Controls.Add(placeorder);
           
        }
    }
}
