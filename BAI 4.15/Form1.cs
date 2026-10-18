
using Bai_4_15;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai_4_14
{
    public partial class Form1 : Form
    {
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private ComboBox cboBranch;
        private Button btnStatistics;
        private Button btnCancel;
        private ProgressBar pgbLoading;
        private Label lblProcessingStatus;
        private DataGridView dgvRevenueData;

        private CancellationTokenSource cts;
        private BindingSource bindingSource = new BindingSource();

        public Form1()
        {

            Text = "THONG KE DOANH THU";
            Size = new Size(1050, 650);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(850, 500);

            CreateInterface();
        }

        private void CreateInterface()
        {
            GroupBox grpCriteria = new GroupBox();
            grpCriteria.Text = "Tieu chi thong ke";
            grpCriteria.Location = new Point(15, 15);
            grpCriteria.Size = new Size(1000, 145);
            grpCriteria.Anchor = AnchorStyles.Top |
                                 AnchorStyles.Left |
                                 AnchorStyles.Right;
            Controls.Add(grpCriteria);

            Label lblFrom = new Label();
            lblFrom.Text = "Tu ngay:";
            lblFrom.Location = new Point(20, 30);
            lblFrom.AutoSize = true;
            grpCriteria.Controls.Add(lblFrom);

            dtpFromDate = new DateTimePicker();
            dtpFromDate.Format = DateTimePickerFormat.Short;
            dtpFromDate.Value = DateTime.Today.AddDays(-7);
            dtpFromDate.Location = new Point(100, 25);
            dtpFromDate.Size = new Size(150, 25);
            grpCriteria.Controls.Add(dtpFromDate);

            Label lblTo = new Label();
            lblTo.Text = "Den ngay:";
            lblTo.Location = new Point(280, 30);
            lblTo.AutoSize = true;
            grpCriteria.Controls.Add(lblTo);

            dtpToDate = new DateTimePicker();
            dtpToDate.Format = DateTimePickerFormat.Short;
            dtpToDate.Value = DateTime.Today;
            dtpToDate.Location = new Point(360, 25);
            dtpToDate.Size = new Size(150, 25);
            grpCriteria.Controls.Add(dtpToDate);

            Label lblBranch = new Label();
            lblBranch.Text = "Chi nhanh:";
            lblBranch.Location = new Point(540, 30);
            lblBranch.AutoSize = true;
            grpCriteria.Controls.Add(lblBranch);

            cboBranch = new ComboBox();
            cboBranch.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBranch.Location = new Point(620, 25);
            cboBranch.Size = new Size(200, 25);
            cboBranch.Items.AddRange(new object[]
            {
                "Tat ca chi nhanh",
                "Chi nhanh Ha Noi",
                "Chi nhanh TP.HCM",
                "Chi nhanh Da Nang"
            });
            cboBranch.SelectedIndex = 0;
            grpCriteria.Controls.Add(cboBranch);

            btnStatistics = new Button();
            btnStatistics.Text = "Thong Ke Doanh Thu";
            btnStatistics.Location = new Point(100, 80);
            btnStatistics.Size = new Size(190, 38);
            btnStatistics.Click += async (s, e) =>
                await RunStatisticsAsync();
            grpCriteria.Controls.Add(btnStatistics);

            btnCancel = new Button();
            btnCancel.Text = "Huy Thao Tac";
            btnCancel.Location = new Point(310, 80);
            btnCancel.Size = new Size(150, 38);
            btnCancel.Enabled = false;
            btnCancel.Click += BtnCancel_Click;
            grpCriteria.Controls.Add(btnCancel);

            GroupBox grpStatus = new GroupBox();
            grpStatus.Text = "Trang thai xu ly";
            grpStatus.Location = new Point(15, 170);
            grpStatus.Size = new Size(1000, 90);
            grpStatus.Anchor = AnchorStyles.Top |
                               AnchorStyles.Left |
                               AnchorStyles.Right;
            Controls.Add(grpStatus);

            pgbLoading = new ProgressBar();
            pgbLoading.Location = new Point(20, 25);
            pgbLoading.Size = new Size(950, 25);
            pgbLoading.Minimum = 0;
            pgbLoading.Maximum = 100;
            pgbLoading.Anchor = AnchorStyles.Top |
                                AnchorStyles.Left |
                                AnchorStyles.Right;
            grpStatus.Controls.Add(pgbLoading);

            lblProcessingStatus = new Label();
            lblProcessingStatus.Text = "San sang thong ke.";
            lblProcessingStatus.Location = new Point(20, 55);
            lblProcessingStatus.Size = new Size(950, 25);
            grpStatus.Controls.Add(lblProcessingStatus);

            Label lblResult = new Label();
            lblResult.Text = "CHI TIET GIAO DICH DOANH THU";
            lblResult.Font = new Font("Arial", 11, FontStyle.Bold);
            lblResult.Location = new Point(15, 275);
            lblResult.Size = new Size(500, 25);
            Controls.Add(lblResult);

            dgvRevenueData = new DataGridView();
            dgvRevenueData.Location = new Point(15, 305);
            dgvRevenueData.Size = new Size(1000, 290);
            dgvRevenueData.Anchor = AnchorStyles.Top |
                                    AnchorStyles.Bottom |
                                    AnchorStyles.Left |
                                    AnchorStyles.Right;
            dgvRevenueData.AutoGenerateColumns = false;
            dgvRevenueData.AllowUserToAddRows = false;
            dgvRevenueData.ReadOnly = true;
            dgvRevenueData.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvRevenueData.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvRevenueData.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Ma giao dich",
                DataPropertyName = "TransactionId"
            });

            dgvRevenueData.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Ngay giao dich",
                DataPropertyName = "TransactionDate",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy"
                }
            });

            dgvRevenueData.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Chi nhanh",
                DataPropertyName = "Branch"
            });

            dgvRevenueData.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Noi dung",
                DataPropertyName = "Description"
            });

            dgvRevenueData.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Doanh thu (VND)",
                DataPropertyName = "Revenue",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0"
                }
            });

            dgvRevenueData.DataSource = bindingSource;
            Controls.Add(dgvRevenueData);
        }

        private async Task RunStatisticsAsync()
        {
            if (dtpFromDate.Value.Date > dtpToDate.Value.Date)
            {
                MessageBox.Show("Tu ngay khong duoc lon hon den ngay!");
                return;
            }

            if (cts != null)
                return;

            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            btnStatistics.Enabled = false;
            btnCancel.Enabled = true;
            pgbLoading.Value = 0;
            lblProcessingStatus.Text = "Dang thong ke...";
            bindingSource.DataSource = new List<RevenueModel>();

            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date;
            string selectedBranch = cboBranch.SelectedItem.ToString();

            try
            {
                List<RevenueModel> result = await Task.Run(async () =>
                {
                    List<RevenueModel> allData =
                        new List<RevenueModel>();

                    Random random = new Random();

                    string[] branches =
                    {
                        "Chi nhanh Ha Noi",
                        "Chi nhanh TP.HCM",
                        "Chi nhanh Da Nang"
                    };

                    // Tao 50 giao dich tong cong
                    for (int i = 1; i <= 50; i++)
                    {
                        token.ThrowIfCancellationRequested();

                        // Gia lap truy xuat va xu ly trong 5 giay
                        await Task.Delay(100, token);

                        string branch = branches[(i - 1) % branches.Length];

                        double range = (toDate - fromDate).TotalDays;
                        DateTime date = fromDate.AddDays(
                            random.Next((int)range + 1));

                        allData.Add(new RevenueModel
                        {
                            TransactionId = "GD" + i.ToString("D4"),
                            TransactionDate = date,
                            Branch = branch,
                            Description = "Giao dich ban hang " + i,
                            Revenue = random.Next(100000, 5000000)
                        });

                        int progress = i * 100 / 50;

                        this.Invoke((MethodInvoker)delegate
                        {
                            if (!IsDisposed)
                            {
                                pgbLoading.Value = progress;
                                lblProcessingStatus.Text =
                                    "Dang xu ly... " + progress + "%";
                            }
                        });
                    }

                    // LOC CHI NHANH TREN DU LIEU DA TAO
                    List<RevenueModel> filtered =
                        new List<RevenueModel>();

                    foreach (RevenueModel item in allData)
                    {
                        if (selectedBranch == "Tat ca chi nhanh" ||
                            item.Branch == selectedBranch)
                        {
                            filtered.Add(item);
                        }
                    }

                    return filtered;
                }, token);

                bindingSource.DataSource = result;
                dgvRevenueData.DataSource = bindingSource;

                pgbLoading.Value = 100;
                lblProcessingStatus.Text =
                    "Hoan tat! So giao dich: " + result.Count;

                MessageBox.Show(
                    "Thong ke thanh cong!\nSo giao dich: " +
                    result.Count,
                    "Ket qua",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                pgbLoading.Value = 0;
                lblProcessingStatus.Text = "Da huy thao tac!";
                bindingSource.DataSource = new List<RevenueModel>();

                MessageBox.Show("Da huy thao tac!");
            }
            catch (Exception ex)
            {
                lblProcessingStatus.Text = "Thong ke that bai.";
                MessageBox.Show("Loi: " + ex.Message);
            }
            finally
            {
                btnStatistics.Enabled = true;
                btnCancel.Enabled = false;

                if (cts != null)
                {
                    cts.Dispose();
                    cts = null;
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (cts != null)
            {
                cts.Cancel();
                btnCancel.Enabled = false;
                lblProcessingStatus.Text = "Dang huy thao tac...";
            }
        }
    }
}
