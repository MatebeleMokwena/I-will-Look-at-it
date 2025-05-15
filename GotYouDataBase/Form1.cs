using System.Data.SQLite;

namespace GotYouDataBase
{
    public partial class Form1 : Form
    {

        private FileSystemWatcher? watcher;
        private Dictionary<string, long>? fSize = new();

        public Form1()
        {
            InitializeComponent();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Columns.Add("Path", "Path");
            dataGridView1.Columns.Add("ChangeType", "ChangeType");
            dataGridView1.Columns.Add("Time", "Time");
            dataGridView1.Columns.Add("Size", "Size");

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)//////////////////////////////////////////// 
        {
            this.BeginInvoke((MethodInvoker)(() =>
            {
                if (e.FullPath.EndsWith(".ini", StringComparison.OrdinalIgnoreCase)) return;

                string changeType = $"{e.Name} was {e.ChangeType}";
                string timeStamp = DateTime.Now.ToString("g");
                string sizeText = "";

                // Handle Deleted
                if (e.ChangeType == WatcherChangeTypes.Deleted)
                {
                    dataGridView1.Rows.Add(e.FullPath, changeType, timeStamp, "Deleted");
                    fSize?.Remove(e.FullPath); // Remove tracked size
                    return;
                }

                long fileSize;
                try
                {
                    FileInfo fileInfo = new FileInfo(e.FullPath);
                    if (!fileInfo.Exists)
                    {
                        dataGridView1.Rows.Add(e.FullPath, changeType, timeStamp, "Not found");
                        return;
                    }

                    fileSize = fileInfo.Length;
                }
                catch
                {
                    dataGridView1.Rows.Add(e.FullPath, changeType, timeStamp, "Inaccessible");
                    return;
                }

                if (fSize != null && fSize.TryGetValue(e.FullPath, out long oldSize))
                {
                    if (oldSize != fileSize)
                    {
                        sizeText = $"{(oldSize / 1024 + 1)} KB => {(fileSize / 1024 + 1)} KB";
                    }
                    else
                    {
                        sizeText = $"{fileSize / 1024 + 1} KB";
                    }
                }
                else
                {
                    sizeText = $"{fileSize / 1024 + 1} KB";
                }

                fSize[e.FullPath] = fileSize;

                dataGridView1.Rows.Add(e.FullPath, changeType, timeStamp, sizeText);
                dataGridView1.AutoResizeColumns();
            }));

        }

        private void OnRenamed(object sender, RenamedEventArgs e) //////////////////////////////////////
        {
            this.BeginInvoke((MethodInvoker)(() =>
            {
                string change = $"Renamed {e.OldName} =>> {e.Name}";
                string time = DateTime.Now.ToString("g");
                dataGridView1.Rows.Add(e.FullPath, change, time, "");
            }));
        }
        public void SaveToQlite() //////////////////////////////////////////
        {
            string dbPath = Path.Combine(Application.StartupPath, "Monitored_Changes.db");
            using var connect = new SQLiteConnection($"Data Source={dbPath}");

            connect.Open();

            var createTable = connect.CreateCommand();
            createTable.CommandText =
                @"CREATE TABLE IF NOT EXISTS Changes
        (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Path TEXT,
            ChangeType TEXT,
            Time TEXT,
            FileSize TEXT
        );";
            createTable.ExecuteNonQuery();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                var insert = connect.CreateCommand();
                insert.CommandText =
                    @"
              INSERT INTO Changes(Path, ChangeType, Time, FileSize)
              VALUES($path, $changeType, $time, $size);";

                insert.Parameters.AddWithValue("$path", row.Cells[0].Value?.ToString());
                insert.Parameters.AddWithValue("$changeType", row.Cells[1].Value?.ToString());
                insert.Parameters.AddWithValue("$time", row.Cells[2].Value?.ToString());
                insert.Parameters.AddWithValue("$size", row.Cells[3].Value?.ToString());

                insert.ExecuteNonQuery();
            }
            MessageBox.Show("Saved to SQLite DB", "Database", MessageBoxButtons.OK);
        }


        private void btnMon_Click(object sender, EventArgs e)/////////////////////////////////
        {
            //String to hold the path being monitered
            String path;

            using (var folder = new FolderBrowserDialog())
            {
                folder.Description = "Select File to Monitor";
                DialogResult result = folder.ShowDialog();// dispaly explore folder

                //If user clicks cancel or doesn't click ok 
                if (result != DialogResult.OK || string.IsNullOrWhiteSpace(folder.SelectedPath)) return;

                path = folder.SelectedPath;

            }
            label2.Text = Path.GetFileName(path) + ". . .";

            //create a new instance of a watchere that also tracks subdirectories
            watcher = new FileSystemWatcher(path);
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;

            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size;

            //Read any type of file
            watcher.Filter = "*.*";

            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed;

        }

        private void btnExp_Click(object sender, EventArgs e)/////////////////////////////////////////////
        {
            using (SaveFileDialog export = new())
            {
                export.Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt;";

                if (export.ShowDialog() != DialogResult.OK) return;

                using StreamWriter write = new(export.FileName);
                write.WriteLine("Path,Change Type,Time,Size");

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    var cells = row.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value?.ToString()?.Replace(",", " ") ?? "");
                    string line = string.Join(",", cells);
                    write.WriteLine(line);
                }

                MessageBox.Show("Export complete", "Done", MessageBoxButtons.OK);
            }
        }



        private void btnSave_Click_1(object sender, EventArgs e)//////////////////////////////////////////
        {
            SaveToQlite();
        }

        private void btnFil_Click(object sender, EventArgs e)/////////////////////////////////////////
        {
            dataGridView1.Rows.Clear();

            var conditions = new List<string>();

            if (chkCre.Checked)
                conditions.Add("ChangeType LIKE '%Created%'");
            if (chkDel.Checked)
                conditions.Add("ChangeType LIKE '%Deleted%'");
            if (chkRe.Checked)
                conditions.Add("ChangeType LIKE '%Renamed%'");
            if (chkCh.Checked)
                conditions.Add("ChangeType LIKE '%Changed%'");

            string whereClause = conditions.Count > 0 ? "WHERE " + string.Join(" OR ", conditions) : "";

            string dbPath = Path.Combine(Application.StartupPath, "Monitored_Changes.db");
            using var connection = new SQLiteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT Path, ChangeType, Time, FileSize FROM Changes {whereClause}";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                dataGridView1.Rows.Add(
                    reader["Path"].ToString(),
                    reader["ChangeType"].ToString(),
                    reader["Time"].ToString(),
                    reader["FileSize"].ToString()
                );
            }
        }
        private void btnSrch_Click(object sender, EventArgs e)/////////////////////////
        {
            dataGridView1.Rows.Clear();
        
            string search = txtSearch.Text.Trim().ToLower();
        
            if (string.IsNullOrEmpty(search))
            {
                MessageBox.Show("Please enter a file name.");
                return;
            }
        
            var conditions = new List<string>();
        
            if (chkCh.Checked)
                conditions.Add("ChangeType LIKE '%Changed%'");
            if (chkCre.Checked)
                conditions.Add("ChangeType LIKE '%Created%'");
            if (chkRe.Checked)
                conditions.Add("ChangeType LIKE '%Renamed%'");
            if (chkDel.Checked)
                conditions.Add("ChangeType LIKE '%Deleted%'");
        
            string filterClause = conditions.Count > 0 ? $"AND ({string.Join(" OR ", conditions)})" : "";
        
            string dbPath = Path.Combine(Application.StartupPath, "Monitored_Changes.db");
        
            using var connection = new SQLiteConnection($"Data Source={dbPath}");
            connection.Open();
        
            var command = connection.CreateCommand();
            command.CommandText = $@"
        SELECT Path, ChangeType, Time, FileSize 
        FROM Changes 
        WHERE (Path LIKE @search OR ChangeType LIKE @search)
        {filterClause};";
        
            command.Parameters.AddWithValue("@search", $"%{search}%");
        
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                dataGridView1.Rows.Add(
                    reader["Path"].ToString(),
                    reader["ChangeType"].ToString(),
                    reader["Time"].ToString(),
                    reader["FileSize"].ToString()
                );
            }
                 
               
        }
        private void btnClr_Click(object sender, EventArgs e)////////////////////////
        {
            dataGridView1.Rows.Clear();
        }
    }
}
