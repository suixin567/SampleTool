using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolLib
{
    public class GameCapture
    {

        #region 属性
        public static int SW = Screen.PrimaryScreen.Bounds.Width;
        public static int SH = Screen.PrimaryScreen.Bounds.Height;
        //public const int GW = ConstDefine.GW;
        //public const int GH = ConstDefine.GH;
       // public static Point GameZeroPosition;//主窗体来赋值
      //  public static int CenterX = 0;//game中央位置
      //  public static int CenterY = 0;

        public Image<Bgr, Byte> game = null;
        System.Timers.Timer timer = new System.Timers.Timer(ConstDefine.GameCaptureRefreshRate);

     //   Point m_mianFormLocation;
      //  Size m_mianFormSize;
        #endregion


        private static GameCapture instance;
        public static GameCapture Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameCapture();
                }
                return instance;
            }
        }


        public GameCapture()
        {
            if (instance != null)
            {
                throw new ArgumentException("不能创建多个captureGame!");
            }
        }


        //初始化
        public void init(Point mianFormLocation, Size mianFormSize)
        {
         //   m_mianFormLocation = mianFormLocation;
        //    m_mianFormSize = mianFormSize;

      //      GameZeroPosition = new Point(mianFormLocation.X + mianFormSize.Width, mianFormLocation.Y);
      //      CenterX = GameZeroPosition.X + GW / 2;
       //     CenterY = GameZeroPosition.Y + GH / 2;

            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_TimesUp);
            timer.AutoReset = true;
            timer.Enabled = true; //是否触发Elapsed事件
            timer.Stop();
            Debug.Print("屏幕尺寸： " + SW+" , "+SH);
        }

        public void start()
        {
            timer.Start();
        }
        public void stop()
        {
            timer.Stop();
        }

        //刷新截屏，外部类可以调用。
        public void Timer_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            //截图
            Bitmap CatchBmp = new Bitmap(SW, SH);
            Graphics g = Graphics.FromImage(CatchBmp);
            //截游戏窗口图
            Point p1 = new Point(0, 0);// m_mianFormLocation.X + m_mianFormSize.Width, m_mianFormLocation.Y);
            Point p2 = new Point(0, 0);
            g.CopyFromScreen(p1, p2, new Size(SW, SH));
            Image<Bgr, Byte> tempGame = new Image<Bgr, byte>(CatchBmp);
            game = tempGame.Clone();
            g.Dispose();
            CatchBmp.Dispose();
            tempGame.Dispose();
            // isMoveCanCopy = true;
           // Debug.Print("打印啊");
        }
    }
}
