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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

            }

        private void button1_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            string hash = Support.GenerateMD5(txtPassword.Text);
            Employee emp = db.Employees.Where(x => x.Email.Equals(txtEmail.Text) 
                && x.Password.Equals(hash)).FirstOrDefault();

            if (emp != null)
            {
                DataStorage.EmployeeId = emp.Id;

                FormMain form = new FormMain();
                form.Show();
                this.Hide();
            }
            else
            {
                Support.msv("Please Try Again, Your Data is not Valid!");
            }
        }
    }
}
