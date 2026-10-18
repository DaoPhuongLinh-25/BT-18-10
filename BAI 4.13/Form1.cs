
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BAI4_13
{
    public partial class Form1 : Form
    {
        private TextBox txtBookId, txtBookName, txtAuthor;
        private TextBox txtCategory, txtQuantity, txtPrice;
        private PictureBox picBookCover;
        private Button btnAdd, btnDelete, btnSearch, btnRefresh, btnLoadCover;
        private DataGridView dgvBooks;

        private BindingList<BookModel> books = new BindingList<BookModel>();
        private BindingSource bindingSource = new BindingSource();

        private ToolStripStatusLabel lblCount;
        private ToolStripStatusLabel lblTotalValue;
        private ToolStripStatusLabel lblTime;

        private System.Windows.Forms.Timer timer;
        private string currentCoverPath = "";

        public Form1()
        {
            InitializeComponent();

            // Xoa cac control cu do Designer tao ra
            Controls.Clear();

            CreateInterface();

            bindingSource.DataSource = books;
            dgvBooks.DataSource = bindingSource;

            books.ListChanged += Books_ListChanged;
            dgvBooks.SelectionChanged += DgvBooks_SelectionChanged;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            GenerateBookId();
            UpdateStatus();
        }

        private void CreateInterface()
        {
            Text = "QUAN LY SACH NHA SACH";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1100, 750);
            MinimumSize = new Size(950, 650);

            // MENU
            MenuStrip menu = new MenuStrip();
            menu.Dock = DockStyle.Top;

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            ToolStripMenuItem openMenu = new ToolStripMenuItem("Mo file CSV");
            ToolStripMenuItem saveMenu = new ToolStripMenuItem("Luu file CSV");
            ToolStripMenuItem exitMenu = new ToolStripMenuItem("Thoat");

            openMenu.Click += (s, e) => OpenCsv();
            saveMenu.Click += (s, e) => SaveCsv();
            exitMenu.Click += (s, e) => Close();

            fileMenu.DropDownItems.Add(openMenu);
            fileMenu.DropDownItems.Add(saveMenu);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add(exitMenu);

            ToolStripMenuItem editMenu = new ToolStripMenuItem("Edit");
            ToolStripMenuItem addMenu = new ToolStripMenuItem("Them");
            ToolStripMenuItem deleteMenu = new ToolStripMenuItem("Xoa");
            ToolStripMenuItem searchMenu = new ToolStripMenuItem("Tim kiem");

            addMenu.Click += (s, e) => AddBook();
            deleteMenu.Click += (s, e) => DeleteBook();
            searchMenu.Click += (s, e) => SearchBook();

            editMenu.DropDownItems.Add(addMenu);
            editMenu.DropDownItems.Add(deleteMenu);
            editMenu.DropDownItems.Add(searchMenu);

            menu.Items.Add(fileMenu);
            menu.Items.Add(editMenu);

            // TOOL STRIP
            ToolStrip tool = new ToolStrip();
            tool.Dock = DockStyle.Top;
            tool.GripStyle = ToolStripGripStyle.Hidden;

            ToolStripButton toolOpen = new ToolStripButton("Mo file");
            toolOpen.Image = SystemIcons.Application.ToBitmap();
            toolOpen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            toolOpen.Click += (s, e) => OpenCsv();

            ToolStripButton toolSave = new ToolStripButton("Luu file");
            toolSave.Image = SystemIcons.Information.ToBitmap();
            toolSave.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            toolSave.Click += (s, e) => SaveCsv();

            ToolStripButton toolAdd = new ToolStripButton("Them moi");
            toolAdd.Image = SystemIcons.Asterisk.ToBitmap();
            toolAdd.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            toolAdd.Click += (s, e) => AddBook();

            tool.Items.Add(toolOpen);
            tool.Items.Add(toolSave);
            tool.Items.Add(toolAdd);

            // Them tool truoc, menu sau de Menu nam tren ToolStrip
            Controls.Add(tool);
            Controls.Add(menu);
            MainMenuStrip = menu;

            // STATUS STRIP
            StatusStrip status = new StatusStrip();

            lblCount = new ToolStripStatusLabel();
            lblTotalValue = new ToolStripStatusLabel();
            lblTime = new ToolStripStatusLabel();

            status.Items.Add(lblCount);
            status.Items.Add(new ToolStripStatusLabel(" | "));
            status.Items.Add(lblTotalValue);
            status.Items.Add(new ToolStripStatusLabel(" | "));
            status.Items.Add(lblTime);

            Controls.Add(status);

            // KHU VUC THONG TIN
            GroupBox grpInfo = new GroupBox();
            grpInfo.Text = "Thong tin sach";
            grpInfo.Location = new Point(15, 75);
            grpInfo.Size = new Size(1050, 245);
            grpInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left |
                             AnchorStyles.Right;
            Controls.Add(grpInfo);

            AddLabel(grpInfo, "Ma sach:", 20, 30);
            txtBookId = AddTextBox(grpInfo, 115, 27);
            txtBookId.ReadOnly = true;

            AddLabel(grpInfo, "Ten sach:", 20, 70);
            txtBookName = AddTextBox(grpInfo, 115, 67);

            AddLabel(grpInfo, "Tac gia:", 20, 110);
            txtAuthor = AddTextBox(grpInfo, 115, 107);

            AddLabel(grpInfo, "The loai:", 20, 150);
            txtCategory = AddTextBox(grpInfo, 115, 147);

            AddLabel(grpInfo, "So luong:", 410, 30);
            txtQuantity = AddTextBox(grpInfo, 500, 27);

            AddLabel(grpInfo, "Don gia:", 410, 70);
            txtPrice = AddTextBox(grpInfo, 500, 67);

            // CAC NUT NAM RIENG, KHONG DE LEN ANH
            btnAdd = AddButton("Them moi", 410, 120, grpInfo);
            btnDelete = AddButton("Xoa sach", 530, 120, grpInfo);
            btnSearch = AddButton("Tim kiem", 650, 120, grpInfo);
            btnRefresh = AddButton("Lam moi", 770, 120, grpInfo);

            btnAdd.Click += (s, e) => AddBook();
            btnDelete.Click += (s, e) => DeleteBook();
            btnSearch.Click += (s, e) => SearchBook();
            btnRefresh.Click += (s, e) => ClearForm();

            // ANH BIA DUOC DAT BEN PHAI
            picBookCover = new PictureBox();
            picBookCover.Location = new Point(875, 25);
            picBookCover.Size = new Size(140, 140);
            picBookCover.BorderStyle = BorderStyle.FixedSingle;
            picBookCover.SizeMode = PictureBoxSizeMode.Zoom;
            grpInfo.Controls.Add(picBookCover);

            btnLoadCover = AddButton("Tai anh bia", 875, 175, grpInfo);
            btnLoadCover.Size = new Size(140, 30);
            btnLoadCover.Click += (s, e) => LoadCover();

            // DATAGRIDVIEW
            dgvBooks = new DataGridView();
            dgvBooks.Name = "dgvBooks";
            dgvBooks.Location = new Point(15, 330);
            dgvBooks.Size = new Size(1050, 350);
            dgvBooks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                              AnchorStyles.Left | AnchorStyles.Right;

            dgvBooks.AutoGenerateColumns = false;
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.AllowUserToDeleteRows = false;
            dgvBooks.ReadOnly = true;
            dgvBooks.MultiSelect = false;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBooks.RowHeadersVisible = false;

            ConfigureGrid();
            Controls.Add(dgvBooks);
        }

        private void AddLabel(Control parent, string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = new Point(x, y);
            label.Size = new Size(90, 25);
            label.TextAlign = ContentAlignment.MiddleLeft;
            parent.Controls.Add(label);
        }

        private TextBox AddTextBox(Control parent, int x, int y)
        {
            TextBox txt = new TextBox();
            txt.Location = new Point(x, y);
            txt.Size = new Size(240, 25);
            parent.Controls.Add(txt);
            return txt;
        }

        private Button AddButton(string text, int x, int y, Control parent)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(110, 30);
            parent.Controls.Add(btn);
            return btn;
        }

        private void ConfigureGrid()
        {
            dgvBooks.Columns.Clear();

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Ma sach",
                DataPropertyName = "BookId",
                Name = "colBookId"
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Ten sach",
                DataPropertyName = "BookName",
                Name = "colBookName"
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tac gia",
                DataPropertyName = "Author",
                Name = "colAuthor"
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "The loai",
                DataPropertyName = "Category",
                Name = "colCategory"
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "So luong",
                DataPropertyName = "Quantity",
                Name = "colQuantity"
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Don gia",
                DataPropertyName = "Price",
                Name = "colPrice",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0"
                }
            });

            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Anh bia",
                DataPropertyName = "CoverPath",
                Name = "colCoverPath"
            });
        }

        private void GenerateBookId()
        {
            int max = 0;

            foreach (BookModel book in books)
            {
                int number;

                if (book.BookId != null && book.BookId.StartsWith("S") &&
                    int.TryParse(book.BookId.Substring(1), out number))
                {
                    if (number > max)
                        max = number;
                }
            }

            txtBookId.Text = "S" + (max + 1).ToString("D3");
        }

        private bool ValidateInput()
        {
            int quantity;
            decimal price;

            if (txtBookName.Text.Trim() == "")
            {
                MessageBox.Show("Ten sach khong duoc de trong!");
                txtBookName.Focus();
                return false;
            }

            if (txtAuthor.Text.Trim() == "")
            {
                MessageBox.Show("Tac gia khong duoc de trong!");
                txtAuthor.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out quantity) ||
                quantity < 0)
            {
                MessageBox.Show("So luong phai la so nguyen khong am!");
                txtQuantity.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(),
                NumberStyles.Number, CultureInfo.CurrentCulture, out price) ||
                price <= 0)
            {
                MessageBox.Show("Don gia phai la so duong!");
                txtPrice.Focus();
                return false;
            }

            return true;
        }

        private BookModel GetBookFromForm()
        {
            return new BookModel
            {
                BookId = txtBookId.Text,
                BookName = txtBookName.Text.Trim(),
                Author = txtAuthor.Text.Trim(),
                Category = txtCategory.Text.Trim(),
                Quantity = int.Parse(txtQuantity.Text.Trim()),
                Price = decimal.Parse(txtPrice.Text.Trim(),
                    CultureInfo.CurrentCulture),
                CoverPath = currentCoverPath
            };
        }

        private void AddBook()
        {
            if (!ValidateInput())
                return;

            books.Add(GetBookFromForm());
            bindingSource.ResetBindings(false);

            MessageBox.Show("Them sach thanh cong!");
            ClearForm();
        }

        private void DeleteBook()
        {
            BookModel selected = bindingSource.Current as BookModel;

            if (selected == null)
            {
                MessageBox.Show("Hay chon sach can xoa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Ban co chac muon xoa sach nay khong?",
                "Xac nhan xoa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                books.Remove(selected);
                bindingSource.ResetBindings(false);
                ClearForm();
            }
        }

        private void SearchBook()
        {
            string keyword = Microsoft.VisualBasic.Interaction.InputBox(
                "Nhap ten hoac ma sach can tim:",
                "Tim kiem sach", "");

            if (keyword.Trim() == "")
                return;

            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].BookId.IndexOf(keyword,
                        StringComparison.OrdinalIgnoreCase) >= 0 ||
                    books[i].BookName.IndexOf(keyword,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    bindingSource.Position = i;
                    return;
                }
            }

            MessageBox.Show("Khong tim thay sach!");
        }

        private void ClearForm()
        {
            txtBookName.Clear();
            txtAuthor.Clear();
            txtCategory.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();

            currentCoverPath = "";

            Image oldImage = picBookCover.Image;
            picBookCover.Image = null;

            if (oldImage != null)
                oldImage.Dispose();

            GenerateBookId();
            dgvBooks.ClearSelection();
        }

        private void LoadCover()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Chon anh bia sach";
                dialog.Filter =
                    "Anh (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (Image temp = Image.FromFile(dialog.FileName))
                        {
                            Image oldImage = picBookCover.Image;
                            picBookCover.Image = new Bitmap(temp);

                            if (oldImage != null)
                                oldImage.Dispose();
                        }

                        currentCoverPath = dialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Khong mo duoc anh: " + ex.Message);
                    }
                }
            }
        }

        private void SaveCsv()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Luu danh sach sach";
                dialog.Filter = "CSV (*.csv)|*.csv";
                dialog.FileName = "DanhSachSach.csv";
                dialog.DefaultExt = "csv";
                dialog.AddExtension = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (StreamWriter writer = new StreamWriter(
                        dialog.FileName, false, Encoding.UTF8))
                    {
                        writer.WriteLine(
                            "BookId,BookName,Author,Category,Quantity,Price,CoverPath");

                        foreach (BookModel book in books)
                        {
                            string[] fields =
                            {
                                book.BookId,
                                book.BookName,
                                book.Author,
                                book.Category,
                                book.Quantity.ToString(),
                                book.Price.ToString(CultureInfo.InvariantCulture),
                                book.CoverPath ?? ""
                            };

                            writer.WriteLine(ToCsvLine(fields));
                        }
                    }

                    MessageBox.Show("Luu file CSV thanh cong!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Loi luu file: " + ex.Message);
                }
            }
        }

        private void OpenCsv()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Mo danh sach sach";
                dialog.Filter = "CSV (*.csv)|*.csv";

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    List<BookModel> imported = new List<BookModel>();

                    using (StreamReader reader = new StreamReader(
                        dialog.FileName, Encoding.UTF8, true))
                    {
                        string line;
                        bool firstLine = true;

                        while ((line = reader.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line))
                                continue;

                            if (firstLine)
                            {
                                firstLine = false;

                                if (line.TrimStart('\uFEFF').StartsWith(
                                    "BookId,", StringComparison.OrdinalIgnoreCase))
                                    continue;
                            }

                            string[] fields = ParseCsvLine(line);

                            if (fields.Length < 7)
                                throw new Exception("Dong CSV khong du 7 cot.");

                            int quantity;
                            decimal price;

                            if (!int.TryParse(fields[4], out quantity) ||
                                !decimal.TryParse(fields[5], NumberStyles.Number,
                                    CultureInfo.InvariantCulture, out price))
                                throw new Exception(
                                    "So luong hoac don gia khong hop le.");

                            imported.Add(new BookModel
                            {
                                BookId = fields[0],
                                BookName = fields[1],
                                Author = fields[2],
                                Category = fields[3],
                                Quantity = quantity,
                                Price = price,
                                CoverPath = fields[6]
                            });
                        }
                    }

                    books.RaiseListChangedEvents = false;
                    books.Clear();

                    foreach (BookModel book in imported)
                        books.Add(book);

                    books.RaiseListChangedEvents = true;
                    bindingSource.ResetBindings(false);

                    ClearForm();
                    UpdateStatus();

                    MessageBox.Show("Mo file CSV thanh cong!");
                }
                catch (Exception ex)
                {
                    books.RaiseListChangedEvents = true;
                    bindingSource.ResetBindings(false);
                    MessageBox.Show("Loi doc CSV: " + ex.Message);
                }
            }
        }

        private string ToCsvLine(string[] fields)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < fields.Length; i++)
            {
                if (i > 0)
                    result.Append(",");

                string value = (fields[i] ?? "").Replace("\"", "\"\"");
                result.Append("\"" + value + "\"");
            }

            return result.ToString();
        }

        private string[] ParseCsvLine(string line)
        {
            List<string> fields = new List<string>();
            StringBuilder value = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length &&
                        line[i + 1] == '"')
                    {
                        value.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(value.ToString());
                    value.Clear();
                }
                else
                {
                    value.Append(c);
                }
            }

            if (inQuotes)
                throw new Exception("CSV co dau ngoac kep chua dong.");

            fields.Add(value.ToString());
            return fields.ToArray();
        }

        private void Books_ListChanged(object sender, ListChangedEventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (lblCount == null || lblTotalValue == null)
                return;

            decimal total = 0;

            foreach (BookModel book in books)
                total += book.TotalValue;

            lblCount.Text = "Tong so dau sach: " + books.Count;
            lblTotalValue.Text =
                "Tong gia tri kho: " + total.ToString("N0") + " VND";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTime.Text = "Thoi gian: " +
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void DgvBooks_SelectionChanged(object sender, EventArgs e)
        {
            BookModel selected = bindingSource.Current as BookModel;

            if (selected == null)
                return;

            txtBookId.Text = selected.BookId;
            txtBookName.Text = selected.BookName;
            txtAuthor.Text = selected.Author;
            txtCategory.Text = selected.Category;
            txtQuantity.Text = selected.Quantity.ToString();
            txtPrice.Text = selected.Price.ToString("0.##");
            currentCoverPath = selected.CoverPath ?? "";

            Image oldImage = picBookCover.Image;
            picBookCover.Image = null;

            if (oldImage != null)
                oldImage.Dispose();

            if (!string.IsNullOrWhiteSpace(currentCoverPath) &&
                File.Exists(currentCoverPath))
            {
                try
                {
                    using (Image temp = Image.FromFile(currentCoverPath))
                        picBookCover.Image = new Bitmap(temp);
                }
                catch
                {
                    picBookCover.Image = null;
                }
            }
        }
    }
}
