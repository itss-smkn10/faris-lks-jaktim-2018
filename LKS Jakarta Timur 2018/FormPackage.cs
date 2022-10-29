using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Jakarta_Timur_2018
{
    public partial class FormPackage : Form
    {
        private int currentSelectedRow = -1;
        private string status = null;
        public FormPackage()
        {
            InitializeComponent();
        }

        private void FormPackage_Load(object sender, EventArgs e)
        {
            loadDgv();
            loadCbService();
            panel1.Enabled = false;
            enableButton(false);
        }

        private void loadDgv()
        {
            dgv.Rows.Clear();
            DataClasses1DataContext db = new DataClasses1DataContext();
            IQueryable<Package> packages = db.Packages.Where(p => p.Service.Name.Contains(txtSearch.Text)
                || p.TotalUnit.ToString().Contains(txtSearch.Text) || p.Price.ToString().Contains(txtSearch.Text));

            foreach (Package p in packages)
            {
                dgv.Rows.Add(p.Id, p.Service.Name, p.TotalUnit, p.Price, p.IdService);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgv();
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

        private void enableButton(bool v)
        {
            btnInsert.Enabled = !v;
            btnUpdate.Enabled = !v;
            btnDelete.Enabled = !v;
            btnSave.Enabled = v;
            btnCancel.Enabled = v;
        }
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            currentSelectedRow = e.RowIndex;
            if (currentSelectedRow != -1)
            {
                txtPackageId.Text = dgv.Rows[currentSelectedRow].Cells[0].Value.ToString();
                cbService.SelectedValue = Convert.ToInt32(dgv.Rows[currentSelectedRow].Cells[4].Value.ToString());
                nmTotalUnit.Value = Convert.ToDecimal(dgv.Rows[currentSelectedRow].Cells[2].Value.ToString());
                nmPrice.Value = Convert.ToInt32(dgv.Rows[currentSelectedRow].Cells[3].Value);
            }
        }

        private bool checkAll()
        {
            if (!Support.isValid(panel1))
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
            panel1.Enabled = true;
            Support.clear(panel1);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (currentSelectedRow != -1)
            {
                enableButton(true);
                status = "update";
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
                            Package package = db.Packages.Where(x => x.Id.Equals(txtPackageId.Text)).FirstOrDefault();
                            db.Packages.DeleteOnSubmit(package);
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
                        Package package = db.Packages.Where(x => x.Id.Equals(txtPackageId.Text)).FirstOrDefault();
                        package.IdService = Convert.ToInt32(cbService.SelectedValue);
                        package.TotalUnit = Convert.ToInt32(nmTotalUnit.Value);
                        package.Price = Convert.ToInt32(nmPrice.Value);

                        db.SubmitChanges();
                        Support.msi("Update success!");
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
                        Package package = new Package();
                        package.IdService = Convert.ToInt32(cbService.SelectedValue);
                        package.TotalUnit = Convert.ToInt32(nmTotalUnit.Value);
                        package.Price = Convert.ToInt32(nmPrice.Value);

                        db.Packages.InsertOnSubmit(package);
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
        }
    }
}
