using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Jakarta_Timur_2018
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin login = new FormLogin();
            login.Show();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Support.msc("Do you want to close this windows?"))
            {
                this.Close();
            }
        }

        private void emloyeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEmployee emp = new FormEmployee();
            emp.MdiParent = this;
            emp.Show();
        }

        private void serviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormService ser = new FormService();
            ser.MdiParent = this;
            ser.Show();
        }

        private void packageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPackage pack = new FormPackage();
            pack.MdiParent = this;
            pack.Show();
        }
    }
}
