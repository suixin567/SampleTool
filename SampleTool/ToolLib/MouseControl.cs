using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolLib;

namespace ToolLib
{
    public class MouseControl
    {

        public delegate void DragEvent();


        public static void Click(Point point)
        {
            int CX = /*GameCapture.GameZeroPosition.X +*/ point.X;
            int CY = /*GameCapture.GameZeroPosition.Y +*/ point.Y;
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE | MOUSEEVENTF_LEFTDOWN, CX * 65536 / GameCapture.SW, CY * 65536 / GameCapture.SH, 0, 0);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, CX * 65536 / GameCapture.SW, CY * 65536 / GameCapture.SH, 0, 0);
        }


        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        //移动鼠标 
        const int MOUSEEVENTF_MOVE = 0x0001;
        //模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        //模拟鼠标右键按下 
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //模拟鼠标右键抬起 
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //模拟鼠标中键按下 
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //模拟鼠标中键抬起 
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //标示是否采用绝对坐标 
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;






        /// <summary>
        /// 
        /// </summary>
        /// <param name="startpos"> 拖动起始位置</param>
        /// <param name="dir">方向</param>
        /// <param name="duration">持续时间</param>

        public static void Drag(Point startpos, Vector2 dir, int duration, DragEvent callback = null)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE | MOUSEEVENTF_LEFTDOWN, (int)startpos.X * 65536 / GameCapture.SW, (int)startpos.Y * 65536 / GameCapture.SH, 0, 0);
            int timeCount = 0;
            System.Windows.Forms.Timer timerFingerMove = new System.Windows.Forms.Timer();
            timerFingerMove.Interval = 1;
            timerFingerMove.Enabled = true;
            timerFingerMove.Start();
            timerFingerMove.Tick += (sen, eve) =>
            {
                if (timeCount < duration)
                {
                    timeCount++;
                    mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, ((int)startpos.X + (int)(dir.X * timeCount)) * 65536 / GameCapture.SW, ((int)startpos.Y + (int)(dir.Y * timeCount)) * 65536 / GameCapture.SH, 0, 0);
                }
                else
                {
                    ((System.Windows.Forms.Timer)sen).Stop();
                    ((System.Windows.Forms.Timer)sen).Dispose();
                    mouse_event(MOUSEEVENTF_LEFTUP, (0) * 65536 / GameCapture.SW, (0) * 65536 / GameCapture.SH, 0, 0);
                    if (callback != null) callback();
                }
            };
        }

        public static void Drag2(Vector2 startpos, Vector2 endPos, DragEvent callback = null)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE | MOUSEEVENTF_LEFTDOWN, (int)startpos.X * 65536 / GameCapture.SW, (int)startpos.Y * 65536 / GameCapture.SH, 0, 0);

            Thread.Sleep(200);



            Vector2 dir = endPos - startpos;

            Vector2 interval = new Vector2(dir.X / 100, dir.Y/100);


            int moveCount = 0;
            System.Windows.Forms.Timer timerFingerMove = new System.Windows.Forms.Timer();
            timerFingerMove.Interval = 10;
            timerFingerMove.Enabled = true;
            timerFingerMove.Start();
            timerFingerMove.Tick += (sen, eve) =>
            {
                if (moveCount < 100)
                {
                    moveCount++;
                    mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, ((int)startpos.X + (int)(interval.X * moveCount)) * 65536 / GameCapture.SW, ((int)startpos.Y + (int)(interval.Y * moveCount)) * 65536 / GameCapture.SH, 0, 0);
                }
                else
                {
                    ((System.Windows.Forms.Timer)sen).Stop();
                    ((System.Windows.Forms.Timer)sen).Dispose();

                   // mouse_event(MOUSEEVENTF_LEFTUP, ((int)endPos.X) * 65536 / GameCapture.SW, ((int)endPos.Y) * 65536 / GameCapture.SH, 0, 0);
                   // if (callback != null) callback();
                    Drag2_end(endPos, callback);
                }
            };






           

           


        }

      static void Drag2_end(Vector2 endPos, DragEvent callback = null) {

            //鼠标原地随机移动一下
            int timeCount = 0;
            System.Windows.Forms.Timer timerFingerMove = new System.Windows.Forms.Timer();
            timerFingerMove.Interval = 1;
            timerFingerMove.Enabled = true;
            timerFingerMove.Start();
            timerFingerMove.Tick += (sen, eve) =>
            {
                if (timeCount < 50)
                {
                    timeCount++;
                    Random rd = new Random();
                    int tempx = rd.Next((int)endPos.X, (int)endPos.X + 10);
                    mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, tempx * 65536 / GameCapture.SW, ((int)endPos.Y) * 65536 / GameCapture.SH, 0, 0);
                }
                else
                {
                    ((System.Windows.Forms.Timer)sen).Stop();
                    ((System.Windows.Forms.Timer)sen).Dispose();
                     mouse_event(MOUSEEVENTF_LEFTUP, ((int)endPos.X) * 65536 / GameCapture.SW, ((int)endPos.Y) * 65536 / GameCapture.SH, 0, 0);
                    if (callback != null) callback();
                }
            };
          
        }

    }
}

