using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ToolLib;

namespace DDBuildHelper
{
   public class Exclude
    {

        private static Exclude instance;
        public static Exclude Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Exclude();
                }
                return instance;
            }
        }

        #region 属性
        Image<Bgr, byte> game;
        Image<Bgr, byte> tar;
        Timer timerExcludeOKButton;//排除确定按钮
        #endregion


        public void init() {
            timerExcludeOKButton = new System.Windows.Forms.Timer();
            timerExcludeOKButton.Interval = 6000;
            timerExcludeOKButton.Enabled = true;
            timerExcludeOKButton.Tick += new EventHandler(timerExcludeOKButtonCallBack);
            timerExcludeOKButton.Stop();
        }

        public void start() {
            tar = new Image<Bgr, byte>(SampleTool.Properties.Resources.yzm);
            timerExcludeOKButton.Start();
        }


        public void stop() {
            timerExcludeOKButton.Stop();
        }

        //确定 teamviewer 和其他的一些 干扰排除
        public void timerExcludeOKButtonCallBack(object sender, EventArgs e)
        {
          
            Point btnPos = new Point();
            double result = MatchTemplate(ref btnPos);
            //Debug.Print("排除确定按钮"+ result);
            //处理验证码***********************************           
            btnPos = new Point();
            result = MatchTemplate(ref btnPos);
            if (result > 0.78)
            {
              //  MessageBox.Show("发现了验证码"+result);
                TipSound.play(Application.StartupPath + @"\thank.wav");
            }
            else {
                //   MessageBox.Show(result.ToString());
              
            }
        }



        double MatchTemplate(ref Point tarPos)
        {
            Rectangle rec = new Rectangle(0, 0, GameCapture.Instance.game.Width, GameCapture.Instance.game.Height);
            game = GameCapture.Instance.game.Copy(rec);
            Image<Gray, float> result = new Image<Gray, float>(game.Width, game.Height);
            result = game.MatchTemplate(tar, TemplateMatchingType.CcorrNormed);
            double min = 0;
            double max = 0;
            Point maxp = new Point(0, 0);
            Point minp = new Point(0, 0);
            CvInvoke.MinMaxLoc(result, ref min, ref max, ref minp, ref maxp);//从结果图中取数据？？
         //   CvInvoke.Rectangle(game, new Rectangle(maxp, new Size(tar.Width, tar.Height)), new MCvScalar(0, 0, 255), 3);//在原图上画矩形  
            //GameCapture.Instance.ShowEvent("", GameCapture.Instance.game);
            //GameCapture.Instance.ShowEvent("", tar);
            //GameCapture.Instance.ShowEvent("", game);
            tarPos = maxp;
            return max;
        }

    }
}
