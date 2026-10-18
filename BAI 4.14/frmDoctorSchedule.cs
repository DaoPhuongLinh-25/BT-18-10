using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace BAI4_14
{
    public class frmDoctorSchedule : Form
    {
        private DataGridView dgvSchedule;

        public frmDoctorSchedule()
        {
            Text = "LỊCH TRỰC BÁC SĨ";
            Size = new Size(750, 420);
            StartPosition = FormStartPosition.CenterScreen;

            CreateInterface();
        }

        private void CreateInterface()
        {
            Label title = new Label();
            title.Text = "DANH SÁCH LỊCH TRỰC";
            title.Font = new Font("Arial", 14, FontStyle.Bold);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(20, 15);
            title.Size = new Size(690, 35);
            Controls.Add(title);

            dgvSchedule = new DataGridView();
            dgvSchedule.Location = new Point(20, 65);
            dgvSchedule.Size = new Size(690, 280);
            dgvSchedule.Anchor = AnchorStyles.Top |
                                 AnchorStyles.Bottom |
                                 AnchorStyles.Left |
                                 AnchorStyles.Right;

            dgvSchedule.AllowUserToAddRows = false;
            dgvSchedule.ReadOnly = true;
            dgvSchedule.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvSchedule.Columns.Add("DoctorId", "Mã bác sĩ");
            dgvSchedule.Columns.Add("DoctorName", "Tên bác sĩ");
            dgvSchedule.Columns.Add("Department", "Khoa");
            dgvSchedule.Columns.Add("DutyDate", "Ngày trực");
            dgvSchedule.Columns.Add("Shift", "Ca trực");

            dgvSchedule.Rows.Add(
                "BS001", "Nguyễn Văn An", "Nội tổng hợp",
                DateTime.Today.ToString("dd/MM/yyyy"), "Sáng");

            dgvSchedule.Rows.Add(
                "BS002", "Trần Thị Bình", "Ngoại khoa",
                DateTime.Today.ToString("dd/MM/yyyy"), "Chiều");

            dgvSchedule.Rows.Add(
                "BS003", "Lê Văn Cường", "Cấp cứu",
                DateTime.Today.ToString("dd/MM/yyyy"), "Đêm");

            Controls.Add(dgvSchedule);
        }
    }
}
