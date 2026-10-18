using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BAI4_14
{
    public class frmPatientProfiles : Form
    {
        private TextBox txtId;
        private TextBox txtName;
        private TextBox txtAge;
        private TextBox txtDiagnosis;
        private DataGridView dgvPatients;

        private BindingList<PatientModel> patients =
            new BindingList<PatientModel>();

        private BindingSource source = new BindingSource();

        public frmPatientProfiles()
        {
            Text = "HỒ SƠ BỆNH NHÂN";
            Size = new Size(850, 500);
            StartPosition = FormStartPosition.CenterScreen;

            CreateInterface();

            source.DataSource = patients;
            dgvPatients.DataSource = source;
            dgvPatients.SelectionChanged += DgvPatients_SelectionChanged;
        }

        private void CreateInterface()
        {
            Label lblId = new Label();
            lblId.Text = "Mã bệnh nhân:";
            lblId.Location = new Point(20, 25);
            lblId.Size = new Size(100, 25);
            Controls.Add(lblId);

            txtId = new TextBox();
            txtId.Location = new Point(125, 25);
            txtId.Size = new Size(180, 25);
            Controls.Add(txtId);

            Label lblName = new Label();
            lblName.Text = "Họ tên:";
            lblName.Location = new Point(20, 65);
            lblName.Size = new Size(100, 25);
            Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Location = new Point(125, 65);
            txtName.Size = new Size(180, 25);
            Controls.Add(txtName);

            Label lblAge = new Label();
            lblAge.Text = "Tuổi:";
            lblAge.Location = new Point(340, 25);
            lblAge.Size = new Size(60, 25);
            Controls.Add(lblAge);

            txtAge = new TextBox();
            txtAge.Location = new Point(405, 25);
            txtAge.Size = new Size(100, 25);
            Controls.Add(txtAge);

            Label lblDiagnosis = new Label();
            lblDiagnosis.Text = "Chẩn đoán:";
            lblDiagnosis.Location = new Point(340, 65);
            lblDiagnosis.Size = new Size(80, 25);
            Controls.Add(lblDiagnosis);

            txtDiagnosis = new TextBox();
            txtDiagnosis.Location = new Point(425, 65);
            txtDiagnosis.Size = new Size(220, 25);
            Controls.Add(txtDiagnosis);

            Button btnAdd = new Button();
            btnAdd.Text = "Thêm bệnh nhân";
            btnAdd.Location = new Point(20, 110);
            btnAdd.Size = new Size(140, 32);
            btnAdd.Click += (s, e) => AddPatient();
            Controls.Add(btnAdd);

            Button btnDelete = new Button();
            btnDelete.Text = "Xóa bệnh nhân";
            btnDelete.Location = new Point(170, 110);
            btnDelete.Size = new Size(140, 32);
            btnDelete.Click += (s, e) => DeletePatient();
            Controls.Add(btnDelete);

            Button btnClear = new Button();
            btnClear.Text = "Làm mới";
            btnClear.Location = new Point(320, 110);
            btnClear.Size = new Size(100, 32);
            btnClear.Click += (s, e) => ClearForm();
            Controls.Add(btnClear);

            dgvPatients = new DataGridView();
            dgvPatients.Location = new Point(20, 160);
            dgvPatients.Size = new Size(790, 270);
            dgvPatients.Anchor = AnchorStyles.Top |
                                 AnchorStyles.Bottom |
                                 AnchorStyles.Left |
                                 AnchorStyles.Right;

            dgvPatients.AutoGenerateColumns = false;
            dgvPatients.AllowUserToAddRows = false;
            dgvPatients.ReadOnly = true;
            dgvPatients.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvPatients.MultiSelect = false;
            dgvPatients.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvPatients.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Mã bệnh nhân",
                DataPropertyName = "PatientId"
            });

            dgvPatients.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Họ tên",
                DataPropertyName = "FullName"
            });

            dgvPatients.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tuổi",
                DataPropertyName = "Age"
            });

            dgvPatients.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Chẩn đoán",
                DataPropertyName = "Diagnosis"
            });

            Controls.Add(dgvPatients);
        }

        private void AddPatient()
        {
            int age;

            if (txtId.Text.Trim() == "" ||
                txtName.Text.Trim() == "" ||
                !int.TryParse(txtAge.Text, out age) ||
                age < 0)
            {
                MessageBox.Show(
                    "Nhập mã, họ tên và tuổi hợp lệ!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            foreach (PatientModel p in patients)
            {
                if (p.PatientId.Equals(
                    txtId.Text.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Mã bệnh nhân đã tồn tại!");
                    return;
                }
            }

            patients.Add(new PatientModel
            {
                PatientId = txtId.Text.Trim(),
                FullName = txtName.Text.Trim(),
                Age = age,
                Diagnosis = txtDiagnosis.Text.Trim()
            });

            source.ResetBindings(false);
            ClearForm();
        }

        private void DeletePatient()
        {
            PatientModel selected = source.Current as PatientModel;

            if (selected == null)
            {
                MessageBox.Show("Hãy chọn bệnh nhân cần xóa!");
                return;
            }

            if (MessageBox.Show(
                "Bạn có chắc muốn xóa bệnh nhân này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                patients.Remove(selected);
                source.ResetBindings(false);
                ClearForm();
            }
        }

        private void DgvPatients_SelectionChanged(object sender, EventArgs e)
        {
            PatientModel p = source.Current as PatientModel;

            if (p == null)
                return;

            txtId.Text = p.PatientId;
            txtName.Text = p.FullName;
            txtAge.Text = p.Age.ToString();
            txtDiagnosis.Text = p.Diagnosis;
        }

        private void ClearForm()
        {
            txtId.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtDiagnosis.Clear();
            dgvPatients.ClearSelection();
        }

        public void ReloadData()
        {
            source.ResetBindings(false);
            MessageBox.Show("Danh sách bệnh nhân đã được cập nhật.");
        }
    }

    public class PatientModel
    {
        public string PatientId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Diagnosis { get; set; }
    }
}
