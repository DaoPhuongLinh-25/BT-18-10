
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai_4__16
{
    public partial class Form1 : Form
    {
        private Label lblBTCPrice;
        private Button btnStart;
        private Button btnPause;
        private DataGridView dgvHistoryLog;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblLastUpdate;
        private ToolStripStatusLabel lblSyncCount;

        private CancellationTokenSource cts;
        private Task monitoringTask;

        private decimal currentPrice = 65000m;
        private int syncCount = 0;
        private readonly Random random = new Random();

        public Form1()
        {
            InitializeComponent();

            this.Text = "frmCryptoStockTicker - Giam sat ty gia";
            this.Size = new Size(850, 550);
            this.StartPosition = FormStartPosition.CenterScreen;

            CreateInterface();
        }

        private void CreateInterface()
        {
            Label lblTitle = new Label();
            lblTitle.Text = "GIAM SAT TY GIA BTC/USD";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(250, 20);
            this.Controls.Add(lblTitle);

            btnStart = new Button();
            btnStart.Text = "Bat dau Giam sat";
            btnStart.Location = new Point(220, 70);
            btnStart.Size = new Size(160, 40);
            btnStart.Click += BtnStart_Click;
            this.Controls.Add(btnStart);

            btnPause = new Button();
            btnPause.Text = "Tam dung";
            btnPause.Location = new Point(400, 70);
            btnPause.Size = new Size(130, 40);
            btnPause.Enabled = false;
            btnPause.Click += BtnPause_Click;
            this.Controls.Add(btnPause);

            lblBTCPrice = new Label();
            lblBTCPrice.Text = "BTC/USD: $65,000.00";
            lblBTCPrice.Font = new Font("Arial", 20, FontStyle.Bold);
            lblBTCPrice.ForeColor = Color.Black;
            lblBTCPrice.AutoSize = true;
            lblBTCPrice.Location = new Point(250, 130);
            this.Controls.Add(lblBTCPrice);

            dgvHistoryLog = new DataGridView();
            dgvHistoryLog.Location = new Point(30, 190);
            dgvHistoryLog.Size = new Size(770, 270);
            dgvHistoryLog.AllowUserToAddRows = false;
            dgvHistoryLog.ReadOnly = true;
            dgvHistoryLog.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistoryLog.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvHistoryLog.RowHeadersVisible = false;

            dgvHistoryLog.Columns.Add("colTime", "Thoi gian");
            dgvHistoryLog.Columns.Add("colPrice", "Gia BTC/USD");
            dgvHistoryLog.Columns.Add("colChange", "Bien dong");
            dgvHistoryLog.Columns.Add("colStatus", "Trang thai");

            this.Controls.Add(dgvHistoryLog);

            statusStrip = new StatusStrip();

            lblLastUpdate = new ToolStripStatusLabel();
            lblLastUpdate.Text = "Cap nhat lan cuoi: --:--:--";

            lblSyncCount = new ToolStripStatusLabel();
            lblSyncCount.Text = "So lan dong bo: 0";

            statusStrip.Items.Add(lblLastUpdate);
            statusStrip.Items.Add(new ToolStripStatusLabel(" | "));
            statusStrip.Items.Add(lblSyncCount);

            this.Controls.Add(statusStrip);
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            if (monitoringTask != null && !monitoringTask.IsCompleted)
                return;

            cts = new CancellationTokenSource();

            btnStart.Enabled = false;
            btnPause.Enabled = true;

            CancellationToken token = cts.Token;

            monitoringTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    decimal newPrice;
                    decimal oldPrice;

                    lock (random)
                    {
                        oldPrice = currentPrice;

                        // Sinh gia moi dao dong tu -500 den +500 USD
                        decimal change = random.Next(-500, 501);
                        newPrice = oldPrice + change;

                        if (newPrice <= 0)
                            newPrice = oldPrice;

                        currentPrice = newPrice;
                    }

                    // Chuyen viec cap nhat giao dien ve UI Thread
                    UpdateInterfaceSafe(oldPrice, newPrice);

                    await Task.Delay(2000, token);
                }
            }, token);

            try
            {
                await monitoringTask;
            }
            catch (OperationCanceledException)
            {
                // Dung giam sat binh thuong
            }
            finally
            {
                btnStart.Enabled = true;
                btnPause.Enabled = false;

                cts.Dispose();
                cts = null;
            }
        }

        private void UpdateInterfaceSafe(decimal oldPrice, decimal newPrice)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateInterface(oldPrice, newPrice);
                }));
            }
            else
            {
                UpdateInterface(oldPrice, newPrice);
            }
        }

        private void UpdateInterface(decimal oldPrice, decimal newPrice)
        {
            decimal change = newPrice - oldPrice;

            lblBTCPrice.Text = "BTC/USD: $" + newPrice.ToString("N2");

            if (change > 0)
            {
                lblBTCPrice.ForeColor = Color.Green;
            }
            else if (change < 0)
            {
                lblBTCPrice.ForeColor = Color.Red;
            }
            else
            {
                lblBTCPrice.ForeColor = Color.Black;
            }

            string changeText = change > 0
                ? "+" + change.ToString("N2")
                : change.ToString("N2");

            string state = change > 0 ? "Tang"
                : change < 0 ? "Giam" : "Khong doi";

            dgvHistoryLog.Rows.Insert(
                0,
                DateTime.Now.ToString("HH:mm:ss"),
                "$" + newPrice.ToString("N2"),
                changeText,
                state
            );

            syncCount++;

            lblLastUpdate.Text =
                "Cap nhat lan cuoi: " + DateTime.Now.ToString("HH:mm:ss");

            lblSyncCount.Text = "So lan dong bo: " + syncCount;
        }

        private async void BtnPause_Click(object sender, EventArgs e)
        {
            btnPause.Enabled = false;

            if (cts != null)
            {
                cts.Cancel();
            }

            if (monitoringTask != null)
            {
                try
                {
                    await monitoringTask;
                }
                catch (OperationCanceledException)
                {
                    // Da dung
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (cts != null)
            {
                cts.Cancel();
            }

            base.OnFormClosing(e);
        }
    }
}
