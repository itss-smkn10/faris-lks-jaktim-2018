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
    public partial class FormService : Form
    {
        private string status = null;
        private int currentSelectedRow = -1;
        public FormService()
        {
            InitializeComponent();
        }

        private void FormService_Load(object sender, EventArgs e)
        {
            loadDgv();
            loadCbCategory();
            loadCbUnit();
            panel1.Enabled = false;
            enableButton(false);
        }

        private void loadDgv()
        {
            dgv.Rows.Clear();
            DataClasses1DataContext db = new DataClasses1DataContext();
            IQueryable<Service> services = db.Services.Where(x => x.Name.Contains(txtSearch.Text)
                || x.Category.Name.Contains(txtSearch.Text) || x.Unit.Name.Contains(txtSearch.Text)
                || x.PriceUnit.ToString().Contains(txtSearch.Text));

            foreach (Service service in services)
            {
                dgv.Rows.Add(service.Id, service.Name, service.Category.Name, service.Unit.Name,
                    service.PriceUnit, service.EstimationDuration, service.IdCategory, service.IdUnit);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgv();
        }

        private void loadCbCategory()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            IQueryable<Category> categories = db.Categories;

            loadCategory(categories);
        }

        private void loadCbUnit()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            IQueryable<Unit> units = db.Units;

            loadUnit(units);
        }
        private void loadCategory(IQueryable<Category> categories)
        {
            cbCategory.Items.Clear();

            cbCategory.DataSource = categories;
            cbCategory.ValueMember = "Id";
            cbCategory.DisplayMember = "Name";
            cbCategory.SelectedValue = -1;
        }

        private void loadUnit(IQueryable<Unit> units)
        {
            cbUnit.Items.Clear();

            cbUnit.DataSource = units;
            cbUnit.ValueMember = "Id";
            cbUnit.DisplayMember = "Name";
            cbUnit.SelectedValue = -1;
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            currentSelectedRow = e.RowIndex;
            if (currentSelectedRow != -1)
            {
                txtServiceId.Text = dgv.Rows[currentSelectedRow].Cells[0].Value.ToString();
                txtServiceName.Text = dgv.Rows[currentSelectedRow].Cells[1].Value.ToString();
                cbCategory.SelectedValue = Convert.ToInt32(dgv.Rows[currentSelectedRow].Cells[6].Value.ToString());
                cbUnit.SelectedValue = Convert.ToInt32(dgv.Rows[currentSelectedRow].Cells[7].Value.ToString());
                nmPrice.Value = Convert.ToInt32(dgv.Rows[currentSelectedRow].Cells[4].Value);
                nmEstDur.Value = Convert.ToInt32(dgv.Rows[currentSelectedRow].Cells[5].Value);
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
                txtSearch.Enabled = false;
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
                            Service service = db.Services.Where(x => x.Id.Equals(txtServiceId.Text)).FirstOrDefault();
                            db.Services.DeleteOnSubmit(service);
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
                        Service service = db.Services.Where(x => x.Id.Equals(txtServiceId.Text)).FirstOrDefault();
                        service.Name = txtServiceName.Text;
                        service.IdCategory = Convert.ToInt32(cbCategory.SelectedValue);
                        service.IdUnit = Convert.ToInt32(cbUnit.SelectedValue);
                        service.PriceUnit = Convert.ToInt32(nmPrice.Value);
                        service.EstimationDuration = Convert.ToInt32(nmEstDur.Value);

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
                        Service service = new Service();
                        service.Name = txtServiceName.Text;
                        service.IdCategory = Convert.ToInt32(cbCategory.SelectedValue);
                        service.IdUnit = Convert.ToInt32(cbUnit.SelectedValue);
                        service.PriceUnit = Convert.ToInt32(nmPrice.Value);
                        service.EstimationDuration = Convert.ToInt32(nmEstDur.Value);

                        db.Services.InsertOnSubmit(service);
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
