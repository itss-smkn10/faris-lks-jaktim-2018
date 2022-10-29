using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Jakarta_Timur_2018
{
    public partial class TransactionDepositForm : Form
    {
        private int customerId = -1;
        private DateTime? estimationDateTime = null;
        private DataGridViewRowCollection data;
        public TransactionDepositForm()
        {
            InitializeComponent();
        }

        private void TransactionDepositForm_Load(object sender, EventArgs e)
        {
            loadCbService();
        }
        
        private void loadCbService()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            IQueryable<Service> services = db.Services;

            loadService(services);
        }

        private void loadService(IQueryable<Service> services)
        {
            cbService.Items.Clear();

            cbService.DataSource = services;
            cbService.ValueMember = "Id";
            cbService.DisplayMember = "Name";
            cbService.SelectedIndex = -1;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dgv.Rows.Add(cbService.Text, "", txtPricePerUnit.Text, nmTotalUnit.Value, 
                    int.Parse(txtPricePerUnit.Text) * nmTotalUnit.Value);
                generateTotal();
                estimationTime();
            }
            catch (Exception ex)
            {
                Support.mse(ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormAdd add = new FormAdd();
            add.Show();
        }

        private void txtPhoneNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Support.msi("Enter Pressed!");

                DataClasses1DataContext db = new DataClasses1DataContext();
                Customer cus = db.Customers.Where(x => x.PhoneNumber.Equals(txtPhoneNumber.Text)).FirstOrDefault();
                if (cus != null)
                {
                    addressChange.Text = cus.Address;
                    nameChange.Text = cus.Name;
                    customerId = cus.Id;
                }
                else
                {
                    Support.mse("There's no customer!");
                }
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 5)
            {
                dgv.Rows.RemoveAt(e.RowIndex);
                generateTotal();
            }
        }

        int segundo = 0;
        DateTime dt = new DateTime();

        private void currentTime_Click(object sender, EventArgs e)
        {
            DateTime dayLeft = DateTime.Parse("1/1/2012 12:00:01 AM");
            DateTime startDate = DateTime.Now;

            TimeSpan t = dayLeft - startDate;
            string countDown = string.Format("{0} Days, {1} Hours, {2} Minutes, {3} Seconds till launch.", 
                t.Days, t.Hours, t.Seconds);
        }

        private void generateTotal()
        {
            int a = 0;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                a += Convert.ToInt32(dgv.Rows[i].Cells[4].Value);
            }

            totalPay.Text = $"Total Pay: {a.ToString("N")}";
        }

        private void estimationTime()
        {
            int a = 0;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                a += Convert.ToInt32(dgv.Rows[i].Cells[3].Value) * Convert.ToInt32(dgv.Rows[i].Cells[8].Value);
            }

            TimeSpan result = TimeSpan.FromHours(a);
            string fromTimeString = result.ToString();
            estimationDateTime = DateTime.Now + result;

            estimationPay.Text = $"Estimation Time: {fromTimeString}";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            segundo++;
            currentTime.Text = dt.AddSeconds(segundo).ToString("HH:mm:ss");
            currentTime.Text = $"Current time : {DateTime.Now.ToString("dd-MMM-yy HH:mm:ss")}";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (customerId == -1)
                {
                    Support.mse("Data is invalid");
                }
                else
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();

                    HeaderDeposit headerDeposit = new HeaderDeposit();
                    headerDeposit.IdCustomer = customerId;
                    headerDeposit.TransactionDatetime = DateTime.Now;
                    headerDeposit.IdEmployee = DataStorage.EmployeeId;
                    headerDeposit.CompleteEstimationDatetime = estimationDateTime;

                    db.HeaderDeposits.InsertOnSubmit(headerDeposit);
                    db.SubmitChanges();
                    Support.msi("Insert Success!");

                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        DetailDeposit detailDeposit = new DetailDeposit();
                        Service service = new Service();

                        detailDeposit.IdDeposit = headerDeposit.Id;
                        detailDeposit.IdService = service.Id;

                        detailDeposit.PriceUnit = int.Parse(txtPricePerUnit.Text);
                        detailDeposit.TotalUnit = double.Parse(data[i].Cells[3].Value.ToString());
                        detailDeposit.CompleteDatetime = null;
                        
                        db.DetailDeposits.InsertOnSubmit(detailDeposit);
                        db.SubmitChanges();
                        Support.msi("Insert Success!");
                    }
                }
            }
            catch (Exception ex)
            {
                Support.mse(ex.Message);
            } 
        }
    }
}
