using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolLib;

namespace DDBuildHelper
{
    public partial class Form1 : Form
    {

        #region 属性
        public SynchronizationContext m_SyncContext = null;

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //int x = (2100);
            //int y = (700);
            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = (Point)new Size(x, y);
            m_SyncContext = SynchronizationContext.Current;

            //开启截屏
            GameCapture.Instance.init(this.Location, this.Size);
            GameCapture.Instance.start();
           
            //注册键盘事件
            KeyMgr.Instance.hook_KeyUp_Event += this.hook_KeyUp;

            Exclude.Instance.init();
            Exclude.Instance.start();

        }


        public struct LogMode
        {
            public string content;
            public Image<Bgr, byte> img;
        }

        public void showLogSafePost(string content, Image<Bgr, byte> img = null)
        {
            LogMode logMode = new LogMode();
            logMode.content = content;
            logMode.img = img;
            m_SyncContext.Post(showLog, logMode);
        }

        //展示日志
        int maxLog = 10;
        public void showLog(object state) {
            LogMode logMode = (LogMode)state;

            EventItem item = new EventItem(logMode.content, logMode.img);
            this.flowLayoutPanel1.Controls.Add(item);
            this.flowLayoutPanel1.Controls.SetChildIndex(item, 0);

            if (this.flowLayoutPanel1.Controls.Count > maxLog)
            {
                this.flowLayoutPanel1.Controls[this.flowLayoutPanel1.Controls.Count - 1].Dispose();
            }
            this.flowLayoutPanel1.VerticalScroll.Value = 0;
            this.flowLayoutPanel1.VerticalScroll.Value = 0;
            this.flowLayoutPanel1.Refresh();
        }

        public void showLog(string content, Image<Bgr, byte> img = null)
        {     
            EventItem item = new EventItem(content, img);
            this.flowLayoutPanel1.Controls.Add(item);
            this.flowLayoutPanel1.Controls.SetChildIndex(item, 0);

            if (this.flowLayoutPanel1.Controls.Count > maxLog)
            {
                this.flowLayoutPanel1.Controls[this.flowLayoutPanel1.Controls.Count - 1].Dispose();
            }
            this.flowLayoutPanel1.VerticalScroll.Value = 0;
            this.flowLayoutPanel1.VerticalScroll.Value = 0;
            this.flowLayoutPanel1.Refresh();
        }


        public void clearLogSafePost()
        {
            m_SyncContext.Post(clearLog, null);
        }

        public void clearLog(object state) {
            this.flowLayoutPanel1.Controls.Clear();
        }

        public void clearLog()
        {
            this.flowLayoutPanel1.Controls.Clear();
        }

        private void buttonclear_Click(object sender, EventArgs e)
        {
            clearLog();
        }


        private void buttonClose_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        void hook_KeyUp(object sender, KeyEventArgs e)
        {
            
            switch (e.KeyCode)
            {
                case Keys.F11:
                    break;
            }

            switch (e.KeyCode)
            {
                case Keys.F12:
                    Environment.Exit(0);
                    break;
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            int x = 700;
            int y = 50;
            IntPtr maindHwnd = FindWindow(null, "Form99");
            maindHwnd = (IntPtr)460908;
            //if (maindHwnd != IntPtr.Zero)
            //{
            //    MessageBox.Show("找到了！" + maindHwnd);

            //}
            //else
            //{
            //    MessageBox.Show("没有找到窗口");
            //}

          //  IntPtr lParam = (IntPtr)((y << 16) | x); // The coordinates
       //     IntPtr lParam = (IntPtr)((y) << 16) + x; // The coordinates
            IntPtr lParam = (IntPtr)(x + (y << 16)); // The coordinates
            IntPtr wParam = IntPtr.Zero; // Additional parameters for the click (e.g. Ctrl)
            const uint downCode = 0x201; // Left click down code
            const uint upCode = 0x202; // Left click up code
            SendMessage(maindHwnd, downCode, wParam, lParam); // Mouse button down
            SendMessage(maindHwnd, upCode, wParam, lParam); // Mouse button up 

        }

      
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void button2_Click(object sender, EventArgs e)
        {
            IntPtr maindHwnd = (IntPtr)263822;
            Bitmap bitmap =  CaptureWindow.GetWindowCapture(maindHwnd);
            Image<Bgr, Byte> tempGame = new Image<Bgr, byte>(bitmap);
            showLogSafePost("游戏窗口截屏",tempGame);
        }
    }







}
