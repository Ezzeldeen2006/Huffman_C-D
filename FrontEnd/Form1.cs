
using CND;

namespace CND
{

    public partial class CnD : Form
    {

        private string CLIpath;
        private string CLIName = "Huffman Compression.exe";
        private string selectedFilePath;
        private TreeNode selectedNode;
        private DateTime lastProgressUpdate = DateTime.MinValue;
        private int bufferSize = 1024;

        private List<TaskInfo> activeTasks = new List<TaskInfo>();
        private int taskCounter = 0;

        public CnD()
        {
            InitializeComponent();
            ShowuserHarddisks();

        }

        void ShowuserHarddisks()
        {
            treeView1.Nodes.Clear();
            DriveInfo[] UserDrives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in UserDrives)
            {
                if (drive.IsReady)
                {
                    TreeNode node = new TreeNode(drive.Name);
                    node.Tag = drive.RootDirectory.FullName;
                    node.Nodes.Add("just a random node so i can do the + sign");
                    treeView1.Nodes.Add(node);
                }
            }
        }

        void beforeexpanding(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;
            if (node.Nodes.Count == 1 && node.Nodes[0].Text == "just a random node so i can do the + sign")
            {
                node.Nodes.Clear();
                string path = node.Tag.ToString();

                try
                {
                    string[] dirs = Directory.GetDirectories(path);
                    foreach (string dir in dirs)
                    {
                        TreeNode dirNode = new TreeNode(Path.GetFileName(dir));
                        dirNode.Tag = dir;

                        try
                        {
                            if (Directory.EnumerateDirectories(dir).Any() || Directory.EnumerateFiles(dir).Any())
                            {
                                dirNode.Nodes.Add("just a random node so i can do the + sign");
                            }
                        }
                        catch (UnauthorizedAccessException)
                        {
                        }

                        node.Nodes.Add(dirNode);
                    }

                    string[] files = Directory.GetFiles(path);
                    foreach (string file in files)
                    {
                        TreeNode fileNode = new TreeNode(Path.GetFileName(file));
                        fileNode.Tag = file;
                        node.Nodes.Add(fileNode);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    node.Nodes.Add("Cant acces that file");
                }

            }
        }

        private void DisplayFileInformation(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            double fileSize = file.Length;
            string sizeText = FormatFileSize(fileSize);

            File_information_text.Text = $"Selected File: {filePath}\r\nFile Size: {sizeText}";
        }
        private string FormatFileSize(double fileSize)
        {
            if (fileSize >= 1024 * 1024)
                return (fileSize / (1024.0 * 1024)).ToString("0.##") + " MB";
            else if (fileSize >= 1024)
                return (fileSize / 1024.0).ToString("0.##") + " KB";
            else
                return fileSize + " bytes";
        }
        void button1_Click(object sender, EventArgs e)
        {

            DialogResult result = openFileDialog1.ShowDialog();


            if (result == DialogResult.OK)
            {
                try
                {
                    selectedFilePath = openFileDialog1.FileName;
                    DisplayFileInformation(selectedFilePath);



                }
                catch (Exception x)
                {

                    MessageBox.Show("Error: Could not process the selected file. " + x.Message, "File Error");
                }
            }

        }
        void AttachBackEnd_Click(object sender, EventArgs e)
        {

            if (ForCLI.ShowDialog() == DialogResult.OK)
            {
                string selectedCliPath = ForCLI.FileName;
                string selectedCliFileName = Path.GetFileName(selectedCliPath);


                if (selectedCliFileName.Equals(CLIName, StringComparison.OrdinalIgnoreCase))
                {
                    CLIpath = selectedCliPath;
                    MessageBox.Show("BackEnd Attached Sucessfully");


                    button1.Enabled = true;
                    treeView1.Enabled = true;
                    fileToolStripMenuItem.Enabled = true;
                    bufferSizeToolStripMenuItem.Enabled = true;
                }
                else
                {
                    CLIpath = string.Empty;
                    MessageBox.Show("Incorrect BackEnd selected. Please select (Huffman Compression.exe)");

                }
            }
        }
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = treeView1.GetNodeAt(e.X, e.Y);
                if (node != null && node.Tag != null)
                {
                    string path = node.Tag.ToString();

                   
                    if (File.Exists(path))
                    {
                        selectedFilePath = path;
                        selectedNode = node;
                        treeView1.SelectedNode = node; 
                    }
                }
            }
        }

        private void RefreshTreeNode(TreeNode parentNode) 
        {
            if (parentNode != null && parentNode.Tag != null)
            {
             
                parentNode.Nodes.Clear();
                parentNode.Nodes.Add("just a random node so i can do the + sign");
                parentNode.Expand(); 
            }
        }
        private async Task PerformCompressionAsync() //from gpt : how to compress multiple files at the same time
        {
            if (string.IsNullOrEmpty(selectedFilePath) || string.IsNullOrEmpty(CLIpath))
            {
                MessageBox.Show("Please select a file and attach the backend first.", "Error");
                return;
            }

            try
            {
                
                if (IsFileBeingProcessed(selectedFilePath))
                {
                    MessageBox.Show("This file is already being processed.", "File In Use");
                    return;
                }

              
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.FileName = Path.GetFileName(selectedFilePath) + ".ece2103";
                saveDialog.Filter = "Compressed Files (*.ece2103)|*.ece2103|All Files (*.*)|*.*";
                saveDialog.InitialDirectory = Path.GetDirectoryName(selectedFilePath);

                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                string outputFile = saveDialog.FileName;

               
                var taskCompressor = new Integration(CLIpath, bufferSize);

                var taskInfo = new TaskInfo
                {
                    Id = ++taskCounter,
                    FileName = Path.GetFileName(selectedFilePath),
                    FilePath = selectedFilePath,
                    Operation = "Compress",
                    Progress = 0,
                    Status = "Running",
                    StartTime = DateTime.Now,
                    Compressor = taskCompressor  
                };


                activeTasks.Add(taskInfo);
                UpdateTasksListView();

               
                await Task.Run(() => PerformCompression(selectedFilePath, outputFile, taskInfo));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during compression: {ex.Message}", "Compression Error");
            }
        }

        private async void file_strip_compress_Click(object sender, EventArgs e)//from gpt
        {
            await PerformCompressionAsync();
        }

        private async void rightclickcompress_Click(object sender, EventArgs e)//from gpt
        {
            if (string.IsNullOrEmpty(selectedFilePath) || !File.Exists(selectedFilePath))
            {
                MessageBox.Show("No valid file was selected or the file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }
            try
            {
                
                DisplayFileInformation(selectedFilePath);

              
                await PerformCompressionAsync();
            }
            catch (Exception x)
            {
                MessageBox.Show($"Error: Could not compress the selected file. {x.Message}", "compression Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private async Task PerformDecompressionAsync()
        {
            if (string.IsNullOrEmpty(selectedFilePath) || string.IsNullOrEmpty(CLIpath))
            {
                MessageBox.Show("Please select a file and attach the backend first.", "Error");
                return;
            }

           
            if (!selectedFilePath.EndsWith(".ece2103", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Please select a .ece2103 file to decompress.", "Invalid File Type");
                return;
            }

            try
            {
               
                if (IsFileBeingProcessed(selectedFilePath))
                {
                    MessageBox.Show("This file is already being processed.", "File In Use");
                    return;
                }

               
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.FileName = Path.GetFileNameWithoutExtension(selectedFilePath.Replace(".ece2103", ""));
                saveDialog.Filter = "All Files (*.*)|*.*";
                saveDialog.InitialDirectory = Path.GetDirectoryName(selectedFilePath);

                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                string outputFile = saveDialog.FileName;

               
                var taskDecompressor = new Integration(CLIpath, bufferSize);

             
                var taskInfo = new TaskInfo
                {
                    Id = ++taskCounter,
                    FileName = Path.GetFileName(selectedFilePath),
                    FilePath = selectedFilePath,
                    Operation = "Decompress",
                    Progress = 0,
                    Status = "Running",
                    StartTime = DateTime.Now,
                    Compressor = taskDecompressor  
                };

                activeTasks.Add(taskInfo);
                UpdateTasksListView();

                //from gpt
                await Task.Run(() => ExecuteDecompressionTask(selectedFilePath, outputFile, taskInfo));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during decompression: {ex.Message}", "Decompression Error");
            }
        }

        private async void file_strip_decompress_Click(object sender, EventArgs e)
        {
            await PerformDecompressionAsync();
        }

        private async void rightclickdecompress_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath) || !File.Exists(selectedFilePath))
            {
                MessageBox.Show("No valid file was selected or the file does not exist. Please right-click a file in the tree view.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            try
            {
               
                DisplayFileInformation(selectedFilePath);

               
                await PerformDecompressionAsync();
            }
            catch (Exception x)
            {
                MessageBox.Show($"Error: Could not decompress the selected file. {x.Message}", "Decompression Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PerformCompression(string inputFile, string outputFile, TaskInfo taskInfo)
        {

            try
            {
             
                var compressor = taskInfo.Compressor;

                compressor.ProgressUpdate += (sender, message) =>
                {
                    UpdateTaskProgressFromMessage(taskInfo, message);
                };

                bool success = compressor.CompressFile(inputFile, outputFile);

                taskInfo.Status = success ? "Completed" : "Failed";
                taskInfo.Progress = success ? 100 : 0;
                taskInfo.EndTime = DateTime.Now;

                Invoke(new Action(() => //from gpt
                {
                    UpdateTasksListView();
                    if (success)
                    {
                        RefreshTreeNode(selectedNode?.Parent);
                        ShowCompletionMessage("Compression completed successfully!", "Compression Complete");
                    }
                }));
            }
            catch (Exception ex)
            {
                taskInfo.Status = "Error";
                taskInfo.Progress = 0;
                taskInfo.EndTime = DateTime.Now;

                Invoke(new Action(() => //from gpt
                {
                    UpdateTasksListView();
                    MessageBox.Show($"Error: {ex.Message}", "Compression Error");
                }));
            }
        }

        private void ExecuteDecompressionTask(string inputFile, string outputFile, TaskInfo taskInfo) //from gpt
        {
          
            var decompressor = taskInfo.Compressor; 
            EventHandler<string> progressHandler = null; 

            try
            {
                
                progressHandler = (sender, message) =>
                {
                    
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() => UpdateTaskProgressFromMessage(taskInfo, message)));
                    }
                    else
                    {
                        UpdateTaskProgressFromMessage(taskInfo, message);
                    }
                };

                decompressor.ProgressUpdate += progressHandler;

                bool success = decompressor.DecompressFile(inputFile, outputFile);

                taskInfo.Status = success ? "Completed" : "Failed";
                taskInfo.Progress = success ? 100 : 0;
                taskInfo.EndTime = DateTime.Now;

        
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        UpdateTasksListView();
                        if (success)
                        {
                            RefreshTreeNode(selectedNode?.Parent);
                            ShowCompletionMessage("Decompression completed successfully!", "Decompression Complete");
                        }
                        else
                        {
                            ShowCompletionMessage("Decompression failed!", "Decompression Failed");
                        }
                    }));
                }
                else
                {
                    UpdateTasksListView();
                    if (success)
                    {
                        RefreshTreeNode(selectedNode?.Parent);
                        ShowCompletionMessage("Decompression completed successfully!", "Decompression Complete");
                    }
                    else
                    {
                        ShowCompletionMessage("Decompression failed!", "Decompression Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                taskInfo.Status = "Error";
                taskInfo.Progress = 0;
                taskInfo.EndTime = DateTime.Now;

             
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        UpdateTasksListView();
                        MessageBox.Show($"Error: {ex.Message}", "Decompression Error");
                    }));
                }
                else
                {
                    UpdateTasksListView();
                    MessageBox.Show($"Error: {ex.Message}", "Decompression Error");
                }
            }
            finally
            {
                // Unsubscribe and Dispose the decompressor
                if (decompressor != null && progressHandler != null)
                {
                    decompressor.ProgressUpdate -= progressHandler;
                }
                (decompressor as IDisposable)?.Dispose();
            }
        }

        private void UpdateTaskProgressFromMessage(TaskInfo taskInfo, string message) //from gpt
        {
            // Try to extract progress percentage from message
            if (message.Contains("Progress:") && message.Contains("%"))
            {
                string[] parts = message.Split('%');
                if (parts.Length > 0)
                {
                    string progressPart = parts[0];
                    string[] progressSplit = progressPart.Split(':');
                    if (progressSplit.Length > 1)
                    {
                        if (double.TryParse(progressSplit[1].Trim(), out double progress))
                        {
                            int newProgress = (int)progress;

                           
                            if (taskInfo.Progress != newProgress && DateTime.Now.Subtract(lastProgressUpdate).TotalMilliseconds > 200) 
                            {
                                taskInfo.Progress = newProgress;
                                lastProgressUpdate = DateTime.Now;

                                // Update UI on main thread
                                if (InvokeRequired)
                                {
                                    Invoke(new Action(() => UpdateTasksListView()));
                                }
                                else
                                {
                                    UpdateTasksListView();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateTasksListView()//from gpt
        {
            if (tasksListView != null)
            {
               
                var runningTasks = activeTasks.Where(t => t.Status == "Running" || t.Status == "Completed" || t.Status == "Failed").ToList();

               
                for (int i = tasksListView.Items.Count - 1; i >= 0; i--)
                {
                    var item = tasksListView.Items[i];
                    var taskInfo = item.Tag as TaskInfo;
                    if (taskInfo == null || !runningTasks.Any(t => t.Id == taskInfo.Id))
                    {
                        tasksListView.Items.RemoveAt(i);
                    }
                }

               
                foreach (var task in runningTasks)
                {
                    var existingItem = tasksListView.Items.Cast<ListViewItem>()
                        .FirstOrDefault(item => ((TaskInfo)item.Tag).Id == task.Id);

                    if (existingItem != null)
                    {
                        // Update existing item
                        existingItem.SubItems[0].Text = task.Operation;
                        existingItem.SubItems[1].Text = task.FileName;
                        existingItem.SubItems[2].Text = $"{task.Progress}%";
                        existingItem.SubItems[3].Text = CalculateETA(task);
                    }
                    else
                    {
                        // Add new item
                        var item = new ListViewItem(new[] {
                    task.Operation,
                    task.FileName,
                    $"{task.Progress}%",
                    CalculateETA(task)
                });
                        item.Tag = task;
                        tasksListView.Items.Add(item);
                    }
                }
            }

        }

        private string CalculateETA(TaskInfo task)//from gpt
        {
            if (task.Status != "Running" || task.Progress == 0)
                return task.Status == "Completed" ? "Done" : task.Status;

            TimeSpan elapsed = DateTime.Now - task.StartTime;
            double progressRatio = task.Progress / 100.0;
            TimeSpan estimatedTotal = TimeSpan.FromTicks((long)(elapsed.Ticks / progressRatio));
            TimeSpan remaining = estimatedTotal - elapsed;

            if (remaining.TotalSeconds < 0)
                return "Finishing...";

            if (remaining.TotalHours >= 1)
                return $"{remaining.Hours}h {remaining.Minutes}m";
            else if (remaining.TotalMinutes >= 1)
                return $"{remaining.Minutes}m {remaining.Seconds}s";
            else
                return $"{remaining.Seconds}s";
        }

        private void TaskUpdateTimer_Tick(object sender, EventArgs e)//from gpt
        {
            if (activeTasks.Any(t => t.Status == "Running"))
            {
                UpdateTasksListView();
            }

           
            CleanupCompletedTasks();
        }

        private bool IsFileBeingProcessed(string filePath)//from gpt
        {
            return activeTasks.Any(t => t.FilePath == filePath && t.Status == "Running"); 
        }

        private void CleanupCompletedTasks()//from gpt
        {
            var completedTasks = activeTasks.Where(t =>
                (t.Status == "Completed" || t.Status == "Failed" || t.Status == "Error") &&
                t.EndTime.HasValue &&
                DateTime.Now.Subtract(t.EndTime.Value).TotalMinutes > 5).ToList();

            foreach (var task in completedTasks)
            {
                activeTasks.Remove(task);
            }

            if (completedTasks.Any())
            {
                UpdateTasksListView();
            }
        }

        private void ShowCompletionMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void compressToolStripMenuItem_Click(object sender, EventArgs e)//from gpt
        {
            await PerformCompressionAsync();
        }

        private async void decompressToolStripMenuItem_Click(object sender, EventArgs e)//from gpt
        {
            await PerformDecompressionAsync();
        }

        private void bufferSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Buffer bufferForm = new Buffer();
            bufferForm.ShowDialog();

            if (bufferForm.IsOkClicked)
            {
                bufferSize = bufferForm.NewBufferSize;

                MessageBox.Show($"Buffer size updated to {bufferSize} bytes", "Buffer Size Changed");
            }
        }
    }
    public class TaskInfo
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; } 
        public string Operation { get; set; } 
        public string Status { get; set; }
        public int Progress { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Integration Compressor { get; set; }

    }
}

