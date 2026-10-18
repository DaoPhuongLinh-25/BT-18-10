using BAI4_12;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BAI4_12
{
    public partial class Form1 : Form
    {
        private TextBox txtMemberId;
        private TextBox txtFullName;
        private TextBox txtPhone;
        private ComboBox cboPackage;
        private DateTimePicker dtpStartDate;
        private TextBox txtFee;

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        private DataGridView dgvMembers;
        private ErrorProvider errorProvider;

        private BindingList<MemberModel> members =
            new BindingList<MemberModel>();

        private BindingSource bindingSource = new BindingSource();

        public Form1()
        {
            InitializeComponent();
            CreateInterface();
            ConfigureGrid();

            bindingSource.DataSource = members;
            dgvMembers.DataSource = bindingSource;

            dgvMembers.SelectionChanged += DgvMembers_SelectionChanged;

            GenerateMemberId();
        }

        private void CreateInterface()
        {
            this.Text = "QUẢN LÝ DỊCH VỤ PHÒNG GYM & HỘI VIÊN";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(850, 550);

            errorProvider = new ErrorProvider();
            errorProvider.ContainerControl = this;

            Label lblTitle = new Label();
            lblTitle.Text = "QUẢN LÝ HỘI VIÊN GYM";
            lblTitle.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 50;
            this.Controls.Add(lblTitle);

            GroupBox grpInfo = new GroupBox();
            grpInfo.Text = "Thông tin hội viên";
            grpInfo.Location = new Point(15, 60);
            grpInfo.Size = new Size(950, 205);
            grpInfo.Anchor = AnchorStyles.Top |
                             AnchorStyles.Left |
                             AnchorStyles.Right;
            this.Controls.Add(grpInfo);

            AddLabel(grpInfo, "Mã hội viên:", 20, 30);
            txtMemberId = AddTextBox(grpInfo, 130, 27);
            txtMemberId.ReadOnly = true;

            AddLabel(grpInfo, "Họ và tên:", 20, 70);
            txtFullName = AddTextBox(grpInfo, 130, 67);

            AddLabel(grpInfo, "Số điện thoại:", 20, 110);
            txtPhone = AddTextBox(grpInfo, 130, 107);

            AddLabel(grpInfo, "Gói tập:", 490, 30);
            cboPackage = new ComboBox();
            cboPackage.Location = new Point(600, 27);
            cboPackage.Size = new Size(250, 25);
            cboPackage.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPackage.Items.AddRange(new object[]
            {
                "Theo tháng",
                "6 tháng",
                "1 năm"
            });
            cboPackage.SelectedIndex = 0;
            grpInfo.Controls.Add(cboPackage);

            AddLabel(grpInfo, "Ngày đăng ký:", 490, 70);
            dtpStartDate = new DateTimePicker();
            dtpStartDate.Location = new Point(600, 67);
            dtpStartDate.Size = new Size(250, 25);
            dtpStartDate.Format = DateTimePickerFormat.Short;
            grpInfo.Controls.Add(dtpStartDate);

            AddLabel(grpInfo, "Phí dịch vụ:", 490, 110);
            txtFee = AddTextBox(grpInfo, 600, 107);
            txtFee.Text = "300000";

            btnAdd = AddButton("Thêm mới", 20, 155, grpInfo);
            btnEdit = AddButton("Sửa thông tin", 150, 155, grpInfo);
            btnDelete = AddButton("Xóa hội viên", 300, 155, grpInfo);
            btnRefresh = AddButton("Làm mới", 450, 155, grpInfo);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;

            dgvMembers = new DataGridView();
            dgvMembers.Name = "dgvMembers";
            dgvMembers.Location = new Point(15, 280);
            dgvMembers.Size = new Size(950, 315);
            dgvMembers.Anchor = AnchorStyles.Top |
                                AnchorStyles.Bottom |
                                AnchorStyles.Left |
                                AnchorStyles.Right;
            dgvMembers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvMembers.MultiSelect = false;
            dgvMembers.ReadOnly = true;
            dgvMembers.AllowUserToAddRows = false;
            dgvMembers.AllowUserToDeleteRows = false;
            dgvMembers.AutoGenerateColumns = false;
            dgvMembers.RowHeadersVisible = false;
            dgvMembers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            this.Controls.Add(dgvMembers);
        }

        private void AddLabel(Control parent, string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = new Point(x, y);
            label.Size = new Size(105, 25);
            label.TextAlign = ContentAlignment.MiddleLeft;
            parent.Controls.Add(label);
        }

        private TextBox AddTextBox(Control parent, int x, int y)
        {
            TextBox txt = new TextBox();
            txt.Location = new Point(x, y);
            txt.Size = new Size(250, 25);
            parent.Controls.Add(txt);
            return txt;
        }

        private Button AddButton(
            string text, int x, int y, Control parent)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(120, 30);
            parent.Controls.Add(btn);
            return btn;
        }

        private void ConfigureGrid()
        {
            dgvMembers.AutoGenerateColumns = false;
            dgvMembers.Columns.Clear();

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Mã HV",
                DataPropertyName = "MemberId",
                Name = "colMemberId"
            });

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tên hội viên",
                DataPropertyName = "FullName",
                Name = "colFullName"
            });

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "SĐT",
                DataPropertyName = "Phone",
                Name = "colPhone"
            });

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Gói tập",
                DataPropertyName = "Package",
                Name = "colPackage"
            });

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Ngày ĐK",
                DataPropertyName = "StartDate",
                Name = "colStartDate",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy"
                }
            });

            dgvMembers.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Phí dịch vụ",
                DataPropertyName = "Fee",
                Name = "colFee",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });
        }

        private void GenerateMemberId()
        {
            int maxId = 0;

            foreach (MemberModel member in members)
            {
                if (member.MemberId.StartsWith("HV"))
                {
                    int number;
                    if (int.TryParse(member.MemberId.Substring(2), out number))
                    {
                        if (number > maxId)
                            maxId = number;
                    }
                }
            }

            txtMemberId.Text = "HV" + (maxId + 1).ToString("D3");
        }

        private bool ValidateInput()
        {
            errorProvider.Clear();
            bool valid = true;

            string name = txtFullName.Text.Trim();

            if (name == "")
            {
                errorProvider.SetError(txtFullName,
                    "Họ tên không được để trống.");
                valid = false;
            }
            else if (!Regex.IsMatch(name,
                @"^[\p{L}\s]+$"))
            {
                errorProvider.SetError(txtFullName,
                    "Họ tên chỉ được chứa chữ cái và khoảng trắng.");
                valid = false;
            }

            if (!Regex.IsMatch(txtPhone.Text.Trim(), @"^\d{10}$"))
            {
                errorProvider.SetError(txtPhone,
                    "Số điện thoại phải gồm đúng 10 chữ số.");
                valid = false;
            }

            decimal fee;
            if (!decimal.TryParse(txtFee.Text.Trim(), out fee) || fee <= 0
                || fee != decimal.Truncate(fee))
            {
                errorProvider.SetError(txtFee,
                    "Phí dịch vụ phải là số nguyên dương.");
                valid = false;
            }

            if (!valid)
                MessageBox.Show(
                    "Vui lòng kiểm tra lại thông tin!",
                    "Dữ liệu không hợp lệ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

            return valid;
        }

        private MemberModel GetMemberFromControls()
        {
            return new MemberModel
            {
                MemberId = txtMemberId.Text,
                FullName = txtFullName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Package = cboPackage.Text,
                StartDate = dtpStartDate.Value.Date,
                Fee = decimal.Parse(txtFee.Text.Trim())
            };
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            MemberModel member = GetMemberFromControls();
            members.Add(member);

            bindingSource.ResetBindings(false);
            dgvMembers.ClearSelection();

            int index = members.Count - 1;
            if (index >= 0)
            {
                dgvMembers.Rows[index].Selected = true;
                bindingSource.Position = index;
            }

            MessageBox.Show("Thêm hội viên thành công!");
            ClearForm();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvMembers.CurrentRow == null ||
                bindingSource.Current == null)
            {
                MessageBox.Show("Vui lòng chọn hội viên cần sửa!");
                return;
            }

            if (!ValidateInput())
                return;

            MemberModel selected =
                bindingSource.Current as MemberModel;

            if (selected == null)
                return;

            MemberModel edited = GetMemberFromControls();

            selected.FullName = edited.FullName;
            selected.Phone = edited.Phone;
            selected.Package = edited.Package;
            selected.StartDate = edited.StartDate;
            selected.Fee = edited.Fee;

            bindingSource.ResetCurrentItem();

            MessageBox.Show("Cập nhật thông tin thành công!");
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            MemberModel selected =
                bindingSource.Current as MemberModel;

            if (selected == null)
            {
                MessageBox.Show("Vui lòng chọn hội viên cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa hội viên " +
                selected.FullName + " không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                members.Remove(selected);
                bindingSource.ResetBindings(false);
                ClearForm();
                MessageBox.Show("Đã xóa hội viên!");
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            errorProvider.Clear();
            dgvMembers.ClearSelection();

            txtFullName.Clear();
            txtPhone.Clear();
            cboPackage.SelectedIndex = 0;
            dtpStartDate.Value = DateTime.Today;
            txtFee.Text = "300000";

            GenerateMemberId();
        }

        private void DgvMembers_SelectionChanged(
            object sender, EventArgs e)
        {
            MemberModel selected =
                bindingSource.Current as MemberModel;

            if (selected == null)
                return;

            txtMemberId.Text = selected.MemberId;
            txtFullName.Text = selected.FullName;
            txtPhone.Text = selected.Phone;
            cboPackage.SelectedItem = selected.Package;
            dtpStartDate.Value = selected.StartDate;
            txtFee.Text = selected.Fee.ToString("0");
        }
    }
}
