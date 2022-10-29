using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Jakarta_Timur_2018
{
    internal class Support
    {
        public static void clear(Control c)
        {
            foreach(Control a in c.Controls)
            {
                if (a is TextBox)
                    ((TextBox)a).Clear();

                if (a is ComboBox)
                    ((ComboBox)a).SelectedIndex = -1;

                if (a is PictureBox)
                    ((PictureBox)a).Image = null;

                if (a is NumericUpDown)
                    ((NumericUpDown)a).Value = 0;

                if (a is DateTimePicker)
                    ((DateTimePicker)a).Value = DateTime.Now;
            }
        }
        public static void msi(string text)
        {
            MessageBox.Show(text, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        public static void mse(string text)
        {
            MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void msv(string text)
        {
            MessageBox.Show(text, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool msc(string text)
        {
            return MessageBox.Show(text, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public static string GenerateMD5(string myString)
        {
            return string.Join("", MD5.Create().ComputeHash(
                Encoding.ASCII.GetBytes(myString)).Select(s => s.ToString("x2")));
        }
        public static bool isValid(Control c)
        {
            bool isValid = true;

            foreach (Control a in c.Controls)
            {
                if (a is TextBox && a.Name != "txtEmployeeId")
                {
                    if (a.Text == "")
                    {
                        isValid = false;
                    }
                }
                else if (a is ComboBox)
                {
                    if (int.Parse(((ComboBox)a).SelectedIndex.ToString()) == -1)
                    {
                        isValid = false;
                    }
                }
                else if (a is NumericUpDown)
                {
                    if (((NumericUpDown)a).Value == 0)
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
    }
}
