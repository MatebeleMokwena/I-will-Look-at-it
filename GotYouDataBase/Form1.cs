using System.Data.SQLite;
using Microsoft.Win32;


namespace GotYouDataBase
{
    public partial class Form1 : Form
    {

        private FileSystemWatcher? watcher;
        private Dictionary<string, long>? fSize = new();

        private readonly string configPath =Path.Combine(Application.StartupPath, "config.txt");
        public Form1()
        {
            
            InitializeComponent();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Columns.Add("Path", "Path");
            dataGridView1.Columns.Add("ChangeType", "ChangeType");
            dataGridView1.Columns.Add("ChangeTime", "ChangeTime");
            dataGridView1.Columns.Add("Size", "Size");

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //Allow app to startup during device activation
            SetStartup();

            //If folder exists then start monitoring
            if (File.Exists(configPath)) 
            {
                string savedPath = File.ReadAllText(configPath);
                if (Directory.Exists(savedPath))
                {
                    StartMonitoring(savedPath);
                    label2.Text = Path.GetFileName(savedPath) + " (Auto monitored)";
                    notifyIcon1.ShowBalloonTip(3000, "Monitoring", $"Monitoring resumed: {savedPath}", ToolTipIcon.Info);
                }
            }

           
            this.Resize += Form1_Resize;
            notifyIcon1.DoubleClick += NotifyIcon1_DoubleClick;
            notifyIcon1.Icon = SystemIcons.Application; 
            notifyIcon1.Visible = false;
            notifyIcon1.Text = "GotYouDataBase";


        }
        //when the window is minimised, hide it in the background and show it in the tray
        private void Form1_Resize(object sender, EventArgs e)/////////////////////////
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;//app visible in tray
                notifyIcon1.ShowBalloonTip(1000, "Monitoring", "GotYouDataBase is running in the background.", ToolTipIcon.Info);
            }
        }

        //When user double clicks the app icon in the tray, restore window/app
        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)/////////////////////
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;//app not visible in tray
        }

        //Start monitoring a folder for any changes
        private void StartMonitoring(string path)////////////////////////////////// 
        {
            if (watcher != null)
            {
                watcher.Changed -= OnChanged;
                watcher.Created -= OnChanged;
                watcher.Deleted -= OnChanged;
                watcher.Renamed -= OnRenamed;

                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }

            //Set up a new file watcher
            watcher = new FileSystemWatcher(path)
            {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size,
                Filter = "*.*"
            };

            
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed;
        }

        //Once app is open it will be stored in the registry which will allow the start to be part of the start up program
        private void SetStartup()/////////////////////////////
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true)) 
            {
                key?.SetValue("GotYouDataBase", Application.ExecutablePath);
            } 
            
        }

        //Will handle the monitoring of changes such as Deletion,Addition and Editing(which in this case would be the size)
        private void OnChanged(object sender, FileSystemEventArgs e)//////////////////////////////////////////// 
        {
            this.BeginInvoke((MethodInvoker)(() =>
            {

                if (e.FullPath.EndsWith(".ini", StringComparison.OrdinalIgnoreCase)) return;

                string changeType = $"{e.Name} was {e.ChangeType}";
                string timeStamp = DateTime.Now.ToString("g");
                string sizeText = "";

                // If file deleted remove the size which was being tracked from memory
                if (e.ChangeType == WatcherChangeTypes.Deleted)
                {
                    RowLimit(e.FullPath, changeType, timeStamp, "Deleted");
                    fSize?.Remove(e.FullPath); // Remove tracked size
                    return;
                }

                long fileSize;
                try
                {
                    //Try to get the size of the file
                    FileInfo fileInfo = new FileInfo(e.FullPath);
                    if (!fileInfo.Exists)
                    {
                        RowLimit(e.FullPath, changeType, timeStamp, "Not found");
                        return;
                    }

                    fileSize = fileInfo.Length;
                }
                catch
                {
                    RowLimit(e.FullPath, changeType, timeStamp, "Inaccessible");
                    return;
                }

                //Compare file size with what it was before
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

                RowLimit(e.FullPath, changeType, timeStamp, sizeText);
                dataGridView1.AutoResizeColumns();

                SaveToQlite(e.FullPath, changeType, timeStamp, sizeText);
            }));

        }

        //Handles changes like renaming
        private void OnRenamed(object sender, RenamedEventArgs e) //////////////////////////////////////
        {
            this.BeginInvoke((MethodInvoker)(() =>
            {
                string change = $"Renamed {e.OldName} =>> {e.Name}";
                string time = DateTime.Now.ToString("g");
                RowLimit(e.FullPath, change, time, "");

                SaveToQlite(e.FullPath, change, time, "");
            }));
        }

        //Save each file change to the SQLite database
        public void SaveToQlite(string path, string changeType, string time, string fileSize) //////////////////////////////////////////
        {
            //Add Monitored_Changes.db on path
            string dbPath = Path.Combine(Application.StartupPath, "Monitored_Changes.db");

            //if the database file doesn't exist, create it
            if (!File.Exists(dbPath)) 
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using var connect = new SQLiteConnection($"Data Source={dbPath}");
            connect.Open();

            //Make sure the table exists, if not create one
            var createTable = connect.CreateCommand();
            createTable.CommandText = @"CREATE TABLE IF NOT EXISTS Changes
                                    (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Path TEXT,
                                        ChangeType TEXT,
                                        ChangeTime TEXT,
                                        FileSize TEXT
                                    );";

            //Above is SQL language and below is the line that allows what is written is SQL to be properly read
            createTable.ExecuteNonQuery();

            using var transaction = connect.BeginTransaction();
            try
            {
                var insert = connect.CreateCommand();
                insert.CommandText = @"INSERT INTO Changes(Path, ChangeType, ChangeTime, FileSize)
                         VALUES($path, $changeType, $time, $size);";

                insert.Parameters.AddWithValue("$path", path);
                insert.Parameters.AddWithValue("$changeType", changeType);
                insert.Parameters.AddWithValue("$time", time);
                insert.Parameters.AddWithValue("$size", fileSize);
                insert.ExecuteNonQuery();

                transaction.Commit();//save the change to the database
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }

        private void RowLimit(params object[] items)//////////////////////////////////// 
        {
            //If items/rows in the datagridview is greater = 30 
            if(dataGridView1.Rows.Count >= 50) 
            {
                //I want you to remove the oldest item
                dataGridView1.Rows.RemoveAt(0);
            }
            //Add new item
            dataGridView1.Rows.Add(items);
        }

        //button to choose the file you want to monitor
        private void btnMon_Click(object sender, EventArgs e)/////////////////////////////////
        {
            string path;

            using (var folder = new FolderBrowserDialog())
            {
                folder.Description = "Select Folder to Monitor";
                if (folder.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(folder.SelectedPath)) return;
                path = folder.SelectedPath;
            }

            File.WriteAllText(configPath, path); // Save folder path to config
            label2.Text = Path.GetFileName(path) + " . . .";
            StartMonitoring(path);

        }

        //Export thet table data to a file
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

        //Clear the entire table
        private void btnClr_Click(object sender, EventArgs e)////////////////////
        {
            dataGridView1.Rows.Clear();
        }

    }
}
