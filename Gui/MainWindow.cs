using System;
using System.Drawing;
using System.Windows.Forms;

using Catodo.Core;

namespace Catodo.Gui {
    public class MainWindow: Form {
        public MainWindow() : base()
        {
            this.todoList = new TodoList();

            this.Build();
            this.edTask.Focus();
        }

        private void BuildIcons()
        {
            this.binIcon = new Bitmap(
                System.Reflection.Assembly.GetEntryAssembly().
                GetManifestResourceStream( "catodo.Res.bin.png" )
            );

            this.diaryIcon = new Bitmap(
                System.Reflection.Assembly.GetEntryAssembly().
                GetManifestResourceStream( "catodo.Res.diary.png" )
            );

            this.upIcon = new Bitmap(
                System.Reflection.Assembly.GetEntryAssembly().
                GetManifestResourceStream( "catodo.Res.up.png" )
            );

            this.downIcon = new Bitmap(
                System.Reflection.Assembly.GetEntryAssembly().
                GetManifestResourceStream( "catodo.Res.down.png" )
            );
        }

        private void BuildAddTaskPanel()
        {
            this.pnlAddTask = new Panel();
            this.pnlAddTask.SuspendLayout();
            this.pnlAddTask.Padding = new Padding( 5 );
            this.pnlAddTask.Dock = DockStyle.Top;

            var imgList = new ImageList();
            imgList.Images.Add( this.diaryIcon );
            imgList.ImageSize = new Size( 16, 16 );

            this.edTask = new TextBox();
            this.edTask.Dock = DockStyle.Fill;
            this.btAdd = new Button();
            this.btAdd.ImageList = imgList;
            this.btAdd.ImageIndex = 0;
            this.btAdd.Dock = DockStyle.Right;
            this.btAdd.Click += (sender, e) => this.DoAddTask();

            this.pnlAddTask.Controls.Add( this.edTask );
            this.pnlAddTask.Controls.Add( this.btAdd );
            this.pnlAddTask.ResumeLayout( false );
        }

        private void BuildTaskListPanel()
        {
            this.pnlTaskList = new Panel();
            this.pnlTaskList.SuspendLayout();
            this.pnlTaskList.Padding = new Padding( 5 );
            this.pnlTaskList.Dock = DockStyle.Fill;

            // Create listbox
            this.lbTasks = new ListBox();
            this.lbTasks.Dock = DockStyle.Fill;
            this.pnlTaskList.Controls.Add( this.lbTasks );

            // Add it
            this.pnlTaskList.ResumeLayout( false );
        }

        private void BuildButtonsPanel()
        {
            this.pnlButtons = new TableLayoutPanel();
            this.pnlButtons.SuspendLayout();
            this.pnlButtons.Dock = DockStyle.Right;
            this.pnlButtons.GrowStyle = TableLayoutPanelGrowStyle.AddRows;

            var imgList = new ImageList();
            imgList.Images.AddRange( new Image[]{ 
                this.binIcon, this.upIcon, this.downIcon
            });
            imgList.ImageSize = new Size( 16, 16 );

            this.btRemove = new Button();
            this.btRemove.Dock = DockStyle.Top;
            this.btRemove.ImageList = imgList;
            this.btRemove.ImageIndex = 0;
            this.btRemove.Click += (sender, e) => this.DoRemoveTask();

            this.btUp = new Button();
            this.btUp.Dock = DockStyle.Top;
            this.btUp.ImageList = imgList;
            this.btUp.ImageIndex = 1;
            this.btUp.Click += (sender, e) => this.DoMoveUp();

            this.btDown = new Button();
            this.btDown.Dock = DockStyle.Top;
            this.btDown.ImageList = imgList;
            this.btDown.ImageIndex = 2;
            this.btDown.Click += (sender, e) => this.DoMoveDown();

            this.pnlButtons.MaximumSize = new Size( 96, int.MaxValue );
            this.pnlButtons.Controls.Add( this.btRemove );
            this.pnlButtons.Controls.Add( this.btUp );
            this.pnlButtons.Controls.Add( this.btDown );

            this.pnlButtons.ResumeLayout( false );
        }

        private void Build()
        {
            this.BuildIcons();
            this.BuildAddTaskPanel();
            this.BuildButtonsPanel();
            this.BuildTaskListPanel();

            // Add it all
            this.Controls.Add( this.pnlTaskList );
            this.Controls.Add( this.pnlButtons );
            this.Controls.Add( this.pnlAddTask );

            // Sizes
            this.pnlAddTask.MaximumSize = new Size( int.MaxValue, this.edTask.Height * 2);
            this.btAdd.MaximumSize = new Size( 32, this.edTask.Height );
            this.pnlButtons.MinimumSize = new Size( this.btRemove.Width, this.lbTasks.Height );

            // Polish
            this.Text = AppInfo.Name;
            this.MinimumSize = new Size( 320, 240 );
			this.Icon = Icon.FromHandle( this.diaryIcon.GetHicon() );
        }

        /// <summary>
        /// Adds the task in the edit box to the task list.
        /// </summary>
        public void DoAddTask()
        {
            string strTask = this.edTask.Text.Trim();

            if ( strTask.Length > 0 ) {
                this.todoList.Add( strTask );

                this.edTask.Text = "";
                this.UpdateTaskList();
                this.edTask.Focus();
            }

            return;
        }

        /// <summary>
        /// Updates the view of the task list.
        /// </summary>
        public void UpdateTaskList()
        {
            this.lbTasks.Enabled = false;
            this.lbTasks.Items.Clear();

            foreach(string task in this.todoList) {
                this.lbTasks.Items.Add( task );
            }

            this.lbTasks.Enabled = true;
            this.lbTasks.SelectedIndex = this.lbTasks.Items.Count - 1;
        }

        public void DoRemoveTask()
        {
            int i = this.lbTasks.SelectedIndex;

            if ( i >= 0 ) {
                this.todoList.RemoveAt( i );

                this.UpdateTaskList();

                if ( this.lbTasks.Items.Count > 0 ) {
                    this.lbTasks.SelectedIndex = Math.Max( i - 1, 0 );
                }
            }

            return;
        }

        public void DoMoveUp()
        {
            int i = this.lbTasks.SelectedIndex;

            if ( i > 0
              && i < this.lbTasks.Items.Count )
            {
                this.todoList.MoveUp( i );
                this.UpdateTaskList();
                this.lbTasks.SelectedIndex = i - 1;
            }

            return;
        }

        public void DoMoveDown()
        {
            int i = this.lbTasks.SelectedIndex;

            if ( i >= 0
              && i < this.lbTasks.Items.Count )
            {
                this.todoList.MoveDown( i );
                this.UpdateTaskList();
                this.lbTasks.SelectedIndex = i + 1;
            }

            return;
        }
            
        private Panel pnlTaskList;
        private Panel pnlAddTask;
        private TableLayoutPanel pnlButtons;
        private TextBox edTask;
        private Button btAdd;
        private Button btRemove;
        private Button btUp;
        private Button btDown;
        private ListBox lbTasks;
        private Bitmap binIcon;
        private Bitmap diaryIcon;
        private Bitmap upIcon;
        private Bitmap downIcon;

        private TodoList todoList;
    }
}

