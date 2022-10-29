using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Jakarta_Timur_2018
{
    public partial class FormEmployee : Form
    {
        private string status = null;
        private int currentSelectedRow = -1;

        public FormEmployee()
        {
            InitializeComponent();
        }

        private void FormEmployee_Load(object sender, EventArgs e)
        {
            loadDgv();
            loadComboboxData();
            enableButton(false);
            panel1.Enabled = false;
        }

        private void loadComboboxData()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            IQueryable<Job> jobs = db.Jobs;

            loadCbJob(jobs);
        }

        private void loadDgv()
        {
            dgv.Rows.Clear();
            DataClasses1DataContext db = new DataClasses1DataContext();
            IQueryable<Employee> employees = db.Employees.Where(x => x.Name.Contains(txtSearch.Text)
                || x.Email.Contains(txtSearch.Text) || x.PhoneNumber.Contains(txtSearch.Text));

            foreach (Employee employee in employees)
            {
                dgv.Rows.Add(employee.Id, employee.Password, employee.Name, employee.Email,
                    employee.PhoneNumber, employee.Address, employee.DateofBirth,
                    employee.Job.Name, employee.Salary, employee.IdJob);
            }
        }

        private void loadCbJob(IQueryable<Job> jobs)
        {
            cbJob.Items.Clear();

            cbJob.DataSource = jobs;
            cbJob.ValueMember = "Id";
            cbJob.DisplayMember = "Name";
            cbJob.SelectedValue = -1;
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgv();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            currentSelectedRow = e.RowIndex;
            if (currentSelectedRow != -1)
            {
                txtEmployeeId.Text = dgv.Rows[currentSelectedRow].Cells[0].Value.ToString();
                txtPassowrd.Text = dgv.Rows[currentSelectedRow].Cells[1].Value.ToString();
                txtConfirmPassword.Text = dgv.Rows[currentSelectedRow].Cells[1].Value.ToString();
                txtName.Text = dgv.Rows[currentSelectedRow].Cells[2].Value.ToString();
                txtEmail.Text = dgv.Rows[currentSelectedRow].Cells[3].Value.ToString();
                txtPhoneNumber.Text = dgv.Rows[currentSelectedRow].Cells[4].Value.ToString();
                txtAddress.Text = dgv.Rows[currentSelectedRow].Cells[5].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dgv.Rows[currentSelectedRow].Cells[6].Value);
                cbJob.SelectedValue = Convert.ToInt32(dgv.Rows[currentSelectedRow].Cells[9].Value.ToString());
                nmSalary.Value = Convert.ToInt32(dgv.Rows[currentSelectedRow].Cells[8].Value);
            }
        }
        private void enableButton(bool isEnable)
        {
            btnInsert.Enabled = !isEnable;
            btnUpdate.Enabled = !isEnable;
            btnDelete.Enabled = !isEnable;
            btnSave.Enabled = isEnable;
            btnCancel.Enabled = isEnable;
        }

        private bool checkAll()
        {
            DataClasses1DataContext dataClasses2DataContext = new DataClasses1DataContext();
            bool there = dataClasses2DataContext.Employees.Any(x => x.Name.Equals(txtName.Text));
            Regex regex = new Regex(@"^+.+@.+\..+$");
            Regex phone = new Regex("^[+][0-9]+$");

            if (txtConfirmPassword.Text != txtPassowrd.Text)
            {
                Support.msv("Password tidak sama!");
                return false;
            }
            else if (!phone.IsMatch(txtPhoneNumber.Text))
            {
                Support.msv("Phone Number not valid!");
                return false;
            }
            else if (!regex.IsMatch(txtEmail.Text))
            {
                Support.msv("Format Email ga Valid!");
                return false;
            }
            else if (!Support.isValid(panel1))
            {
                Support.msv("All field must be filled!");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            enableButton(true);
            status = "insert";
            txtSearch.Enabled = false;
            panel1.Enabled = true;
            Support.clear(panel1);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (currentSelectedRow != -1)
            {
                enableButton(true);
                status = "update";
                txtSearch.Enabled = false;
                panel1.Enabled = true;
            }
            else
            {
                Support.msv("Select Row or Column first please!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            status = "delete";
            DataClasses1DataContext db = new DataClasses1DataContext();
            if (currentSelectedRow != -1)
            {
                try
                {
                    if (status == "delete")
                    {
                        if (Support.msc("Are you sure want to delete this?"))
                        {
                            Employee employee = db.Employees.Where(x => x.Id.Equals(txtEmployeeId.Text)).FirstOrDefault();
                            db.Employees.DeleteOnSubmit(employee);
                            db.SubmitChanges();
                            loadDgv();
                            enableButton(false);
                            Support.clear(panel1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Support.mse(ex.Message);
                }
            }
            else
            {
                Support.msv("Select Row or Column first please!");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            if (checkAll())
            {
                if (status == "update")
                {
                    try
                    {
                        Employee employee = db.Employees.Where(x => x.Id.Equals(txtEmployeeId.Text)).FirstOrDefault();
                        employee.Password = txtPassowrd.Text;
                        employee.Name = txtName.Text;
                        employee.Email = txtEmail.Text;
                        employee.PhoneNumber = txtPhoneNumber.Text;
                        employee.Address = txtAddress.Text;
                        employee.DateofBirth = dateTimePicker1.Value;
                        employee.IdJob = Convert.ToInt32(cbJob.SelectedValue);
                        employee.Salary = Convert.ToInt32(nmSalary.Value);

                        db.SubmitChanges();
                        Support.msi("Update Success!");
                        loadDgv();
                        enableButton(false);
                        Support.clear(panel1);
                    }
                    catch (Exception ex)
                    {
                        Support.mse(ex.Message);
                    }
                }
                else if (status == "insert")
                {
                    try
                    {
                        Employee employee = new Employee();
                        employee.Password = txtPassowrd.Text;
                        employee.Name = txtName.Text;
                        employee.Email = txtEmail.Text;
                        employee.PhoneNumber = txtPhoneNumber.Text;
                        employee.Address = txtAddress.Text;
                        employee.DateofBirth = dateTimePicker1.Value;
                        employee.IdJob = Convert.ToInt32(cbJob.SelectedValue);
                        employee.Salary = Convert.ToInt32(nmSalary.Value);

                        db.Employees.InsertOnSubmit(employee);
                        db.SubmitChanges();
                        loadDgv();
                        enableButton(false);
                        Support.clear(panel1);
                    }
                    catch (Exception ex)
                    {
                        Support.mse(ex.Message);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            enableButton(false);
            Support.clear(panel1);
            panel1.Enabled = false;
            txtSearch.Enabled = true;
        }
    }
}
