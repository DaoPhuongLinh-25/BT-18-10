
using BAI4_14;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bai_4_14
{
    public partial class Form1 : Form
    {
        private ToolStripStatusLabel lblDoctor;
        private ToolStripStatusLabel lblTime;
        private ToolStripProgressBar progressBar;

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer progressTimer;

        public Form1()
        {
            InitializeComponent();

            IsMdiContainer = true;
            Text = "HE THONG QUAN LY BENH VIEN";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;

            CreateMenu();
            CreateStatusBar();
        }

        private void CreateMenu()
        {
            MenuStrip menu = new MenuStrip();

            // MENU HE THONG
            ToolStripMenuItem systemMenu =
                new ToolStripMenuItem("He thong");

            ToolStripMenuItem logout =
                new ToolStripMenuItem("Dang xuat");

            ToolStripMenuItem reload =
                new ToolStripMenuItem("Nap lai du lieu");

            ToolStripMenuItem exit =
                new ToolStripMenuItem("Thoat");

            logout.Click += (s, e) =>
            {
                MessageBox.Show("Da dang xuat!");
            };

            reload.Click += (s, e) =>
            {
                foreach (Form child in MdiChildren)
                {
                    if (child is frmPatientProfiles)
                    {
                        ((frmPatientProfiles)child).ReloadData();
                    }
                }

                MessageBox.Show("Da nap lai du lieu!");
            };

            exit.Click += (s, e) => Close();

            systemMenu.DropDownItems.Add(logout);
            systemMenu.DropDownItems.Add(reload);
            systemMenu.DropDownItems.Add(new ToolStripSeparator());
            systemMenu.DropDownItems.Add(exit);

            // MENU QUAN LY
            ToolStripMenuItem manageMenu =
                new ToolStripMenuItem("Quan ly");

            ToolStripMenuItem patients =
                new ToolStripMenuItem("Ho so benh nhan");

            ToolStripMenuItem doctors =
                new ToolStripMenuItem("Lich truc bac si");

            ToolStripMenuItem departments =
                new ToolStripMenuItem("Danh muc Khoa/Phong");

            patients.Click += (s, e) =>
                OpenChild<frmPatientProfiles>();

            doctors.Click += (s, e) =>
                OpenChild<frmDoctorSchedule>();

            departments.Click += (s, e) =>
                OpenChild<frmDepartment>();

            manageMenu.DropDownItems.Add(patients);
            manageMenu.DropDownItems.Add(doctors);
            manageMenu.DropDownItems.Add(departments);

            // MENU CUA SO
            ToolStripMenuItem windowMenu =
                new ToolStripMenuItem("Cua so");

            ToolStripMenuItem cascade =
                new ToolStripMenuItem("Xep tang");

            ToolStripMenuItem horizontal =
                new ToolStripMenuItem("Sap xep ngang");

            ToolStripMenuItem vertical =
                new ToolStripMenuItem("Sap xep doc");

            ToolStripMenuItem closeAll =
                new ToolStripMenuItem("Dong tat ca");

            cascade.Click += (s, e) =>
                LayoutMdi(MdiLayout.Cascade);

            horizontal.Click += (s, e) =>
                LayoutMdi(MdiLayout.TileHorizontal);

            vertical.Click += (s, e) =>
                LayoutMdi(MdiLayout.TileVertical);

            closeAll.Click += (s, e) =>
            {
                foreach (Form child in MdiChildren)
                {
                    child.Close();
                }
            };

            windowMenu.DropDownItems.Add(cascade);
            windowMenu.DropDownItems.Add(horizontal);
            windowMenu.DropDownItems.Add(vertical);
            windowMenu.DropDownItems.Add(new ToolStripSeparator());
            windowMenu.DropDownItems.Add(closeAll);

            menu.Items.Add(systemMenu);
            menu.Items.Add(manageMenu);
            menu.Items.Add(windowMenu);

            MainMenuStrip = menu;
            Controls.Add(menu);
        }

        // KHONG MO TRUNG FORM CON
        private void OpenChild<T>() where T : Form, new()
        {
            foreach (Form child in MdiChildren)
            {
                if (child is T)
                {
                    if (child.WindowState == FormWindowState.Minimized)
                    {
                        child.WindowState = FormWindowState.Normal;
                    }

                    child.Activate();
                    return;
                }
            }

            T form = new T();
            form.MdiParent = this;
            form.Show();
        }

        private void CreateStatusBar()
        {
            StatusStrip status = new StatusStrip();

            lblDoctor = new ToolStripStatusLabel();
            lblDoctor.Text = "Bac si truc: Nguyen Van An";

            progressBar = new ToolStripProgressBar();
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
            progressBar.Size = new Size(150, 16);

            lblTime = new ToolStripStatusLabel();
            lblTime.Text = DateTime.Now.ToString(
                "dd/MM/yyyy HH:mm:ss");

            status.Items.Add(lblDoctor);
            status.Items.Add(new ToolStripStatusLabel(" | Tien trinh: "));
            status.Items.Add(progressBar);
            status.Items.Add(new ToolStripStatusLabel(" | "));
            status.Items.Add(lblTime);

            Controls.Add(status);

            // CAP NHAT THOI GIAN
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;

            timer.Tick += (s, e) =>
            {
                lblTime.Text = DateTime.Now.ToString(
                    "dd/MM/yyyy HH:mm:ss");
            };

            timer.Start();

            // THANH TIEN TRINH MINH HOA
            progressTimer = new System.Windows.Forms.Timer();
            progressTimer.Interval = 100;

            progressTimer.Tick += (s, e) =>
            {
                if (progressBar.Value < 100)
                {
                    progressBar.Value += 5;
                }
                else
                {
                    progressTimer.Stop();
                }
            };

            progressTimer.Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            if (progressTimer != null)
            {
                progressTimer.Stop();
                progressTimer.Dispose();
            }

            base.OnFormClosed(e);
        }
    }
}
