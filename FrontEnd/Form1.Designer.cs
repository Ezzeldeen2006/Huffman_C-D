namespace CND
{
    partial class CnD
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            file_strip_compress = new ToolStripMenuItem();
            file_strip_decompress = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            cLIToolStripMenuItem = new ToolStripMenuItem();
            bufferSizeToolStripMenuItem = new ToolStripMenuItem();
            splitContainer1 = new SplitContainer();
            treeView1 = new TreeView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            rightclickcompress = new ToolStripMenuItem();
            rightclickdecompress = new ToolStripMenuItem();
            currentTaskProgressBar = new ProgressBar();
            button1 = new Button();
            groupBox2 = new GroupBox();
            tasksListView = new ListView();
            T = new ColumnHeader();
            Name = new ColumnHeader();
            prog = new ColumnHeader();
            eta = new ColumnHeader();
            groupbox1 = new GroupBox();
            File_information_text = new TextBox();
            currentTaskLabel = new Label();
            openFileDialog1 = new OpenFileDialog();
            ForCLI = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            taskUpdateTimer = new System.Windows.Forms.Timer(components);
            messageTimer = new System.Windows.Forms.Timer(components);
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupbox1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1136, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { file_strip_compress, file_strip_decompress });
            fileToolStripMenuItem.Enabled = false;
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // file_strip_compress
            // 
            file_strip_compress.Name = "file_strip_compress";
            file_strip_compress.Size = new Size(139, 22);
            file_strip_compress.Text = "Compress";
            file_strip_compress.Click += file_strip_compress_Click;
            // 
            // file_strip_decompress
            // 
            file_strip_decompress.Name = "file_strip_decompress";
            file_strip_decompress.Size = new Size(139, 22);
            file_strip_decompress.Text = "Decompress";
            file_strip_decompress.Click += file_strip_decompress_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cLIToolStripMenuItem, bufferSizeToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // cLIToolStripMenuItem
            // 
            cLIToolStripMenuItem.Name = "cLIToolStripMenuItem";
            cLIToolStripMenuItem.Size = new Size(129, 22);
            cLIToolStripMenuItem.Text = "CLI";
            cLIToolStripMenuItem.Click += AttachBackEnd_Click;
            // 
            // bufferSizeToolStripMenuItem
            // 
            bufferSizeToolStripMenuItem.Enabled = false;
            bufferSizeToolStripMenuItem.Name = "bufferSizeToolStripMenuItem";
            bufferSizeToolStripMenuItem.Size = new Size(129, 22);
            bufferSizeToolStripMenuItem.Text = "Buffer Size";
            bufferSizeToolStripMenuItem.Click += bufferSizeToolStripMenuItem_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = SystemColors.ScrollBar;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView1);
            splitContainer1.Panel1.Controls.Add(currentTaskProgressBar);
            splitContainer1.Panel1.Controls.Add(button1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(groupBox2);
            splitContainer1.Panel2.Controls.Add(groupbox1);
            splitContainer1.Size = new Size(1136, 558);
            splitContainer1.SplitterDistance = 377;
            splitContainer1.TabIndex = 1;
            // 
            // treeView1
            // 
            treeView1.BackColor = Color.Gray;
            treeView1.ContextMenuStrip = contextMenuStrip1;
            treeView1.Dock = DockStyle.Fill;
            treeView1.Enabled = false;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(377, 502);
            treeView1.TabIndex = 1;
            treeView1.BeforeExpand += beforeexpanding;
            treeView1.MouseDown += treeView1_MouseDown;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { rightclickcompress, rightclickdecompress });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(140, 48);
            // 
            // rightclickcompress
            // 
            rightclickcompress.Name = "rightclickcompress";
            rightclickcompress.Size = new Size(139, 22);
            rightclickcompress.Text = "Compress";
            rightclickcompress.Click += rightclickcompress_Click;
            // 
            // rightclickdecompress
            // 
            rightclickdecompress.Name = "rightclickdecompress";
            rightclickdecompress.Size = new Size(139, 22);
            rightclickdecompress.Text = "Decompress";
            rightclickdecompress.Click += rightclickdecompress_Click;
            // 
            // currentTaskProgressBar
            // 
            currentTaskProgressBar.Location = new Point(196, 235);
            currentTaskProgressBar.Name = "currentTaskProgressBar";
            currentTaskProgressBar.Size = new Size(100, 23);
            currentTaskProgressBar.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.Info;
            button1.Dock = DockStyle.Bottom;
            button1.Enabled = false;
            button1.ForeColor = SystemColors.ControlText;
            button1.Location = new Point(0, 502);
            button1.Name = "button1";
            button1.Size = new Size(377, 56);
            button1.TabIndex = 0;
            button1.Text = "Browse for files";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = SystemColors.ScrollBar;
            groupBox2.Controls.Add(tasksListView);
            groupBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(2, 193);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(741, 362);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Tasks";
            // 
            // tasksListView
            // 
            tasksListView.Columns.AddRange(new ColumnHeader[] { T, Name, prog, eta });
            tasksListView.FullRowSelect = true;
            tasksListView.GridLines = true;
            tasksListView.Location = new Point(6, 28);
            tasksListView.Name = "tasksListView";
            tasksListView.Size = new Size(735, 337);
            tasksListView.TabIndex = 2;
            tasksListView.UseCompatibleStateImageBehavior = false;
            tasksListView.View = View.Details;
            // 
            // T
            // 
            T.Text = "Task";
            T.Width = 200;
            // 
            // Name
            // 
            Name.Text = "File Name";
            Name.Width = 200;
            // 
            // prog
            // 
            prog.Text = "Progress";
            prog.Width = 200;
            // 
            // eta
            // 
            eta.Text = "ETA";
            eta.Width = 200;
            // 
            // groupbox1
            // 
            groupbox1.Controls.Add(File_information_text);
            groupbox1.Controls.Add(currentTaskLabel);
            groupbox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupbox1.Location = new Point(2, 3);
            groupbox1.Name = "groupbox1";
            groupbox1.Size = new Size(741, 184);
            groupbox1.TabIndex = 1;
            groupbox1.TabStop = false;
            groupbox1.Text = "Selcted File Information";
            // 
            // File_information_text
            // 
            File_information_text.Dock = DockStyle.Fill;
            File_information_text.Font = new Font("Arial", 11.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            File_information_text.Location = new Point(3, 25);
            File_information_text.Multiline = true;
            File_information_text.Name = "File_information_text";
            File_information_text.ReadOnly = true;
            File_information_text.Size = new Size(735, 156);
            File_information_text.TabIndex = 0;
            // 
            // currentTaskLabel
            // 
            currentTaskLabel.AutoSize = true;
            currentTaskLabel.Location = new Point(383, 88);
            currentTaskLabel.Name = "currentTaskLabel";
            currentTaskLabel.Size = new Size(52, 21);
            currentTaskLabel.TabIndex = 0;
            currentTaskLabel.Text = "label1";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt|Image Files (*.jpeg;*.jpg;*.png;*.gif;*.bmp)|*.jpeg;*.jpg;*.png;*.gif;*.bmp";
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Title = "This PC";
            // 
            // ForCLI
            // 
            ForCLI.FileName = "Select Backend.exe";
            ForCLI.Filter = "Executable Files (*.exe)|*.exe";
            ForCLI.InitialDirectory = "C:\\";
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.InitialDirectory = "D:\\";
            // 
            // taskUpdateTimer
            // 
            taskUpdateTimer.Enabled = true;
            taskUpdateTimer.Tick += TaskUpdateTimer_Tick;
            // 
            // CnD
            // 
            AccessibleName = "";
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1136, 582);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Text = "C&D";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupbox1.ResumeLayout(false);
            groupbox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem file_strip_compress;
        private ToolStripMenuItem file_strip_decompress;
        private SplitContainer splitContainer1;
        private TreeView treeView1;
        private Button button1;
        private GroupBox groupBox2;
        private GroupBox groupbox1;
        private TextBox File_information_text;
        private OpenFileDialog openFileDialog1;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem cLIToolStripMenuItem;
        private ToolStripMenuItem bufferSizeToolStripMenuItem;
        private OpenFileDialog ForCLI;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem rightclickcompress;
        private ToolStripMenuItem rightclickdecompress;
        private SaveFileDialog saveFileDialog1;
        private ProgressBar currentTaskProgressBar;
        private Label currentTaskLabel;
        private ListView tasksListView;
        private ColumnHeader T;
        private ColumnHeader Name;
        private ColumnHeader prog;
        private ColumnHeader eta;
        private System.Windows.Forms.Timer taskUpdateTimer;
        private System.Windows.Forms.Timer messageTimer;
    }
}
