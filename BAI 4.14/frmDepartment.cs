using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace BAI4_14
{
    public class frmDepartment : Form
    {
        private DataGridView dgvDepartments;

        public frmDepartment()
        {
            Text = "DANH MỤC KHOA / PHÒNG";
            Size = new Size(650, 400);
            StartPosition = FormStartPosition.CenterScreen;

            CreateInterface();
        }

        private void CreateInterface()
        {
            Label title = new Label();
            title.Text = "DANH MỤC KHOA / PHÒNG";
            title.Font = new Font("Arial", 14, FontStyle.Bold);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(20, 15);
            title.Size = new Size(590, 35);
            Controls.Add(title);

            dgvDepartments = new DataGridView();
            dgvDepartments.Location = new Point(20, 65);
            dgvDepartments.Size = new Size(590, 250);
            dgvDepartments.Anchor = AnchorStyles.Top |
                                    AnchorStyles.Bottom |
                                    AnchorStyles.Left |
                                    AnchorStyles.Right;

            dgvDepartments.AllowUserToAddRows = false;
            dgvDepartments.ReadOnly = true;
            dgvDepartments.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvDepartments.Columns.Add("DepartmentId", "Mã khoa");
            dgvDepartments.Columns.Add("DepartmentName", "Tên khoa / phòng");
            dgvDepartments.Columns.Add("Location", "Vị trí");

            dgvDepartments.Rows.Add("K001", "Nội tổng hợp", "Tầng 1");
            dgvDepartments.Rows.Add("K002", "Ngoại khoa", "Tầng 2");
            dgvDepartments.Rows.Add("K003", "Cấp cứu", "Tầng 1");
            dgvDepartments.Rows.Add("K004", "Nhi khoa", "Tầng 3");

            Controls.Add(dgvDepartments);
        }
    }
}
