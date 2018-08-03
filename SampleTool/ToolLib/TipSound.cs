using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolLib
{
    public class TipSound
    {
      
        static System.Media.SoundPlayer sp;

        public static void play(string path)
        {
            //播放声音
            if (sp!=null)
            {
                sp.Stop();
            }         
            sp = new System.Media.SoundPlayer(path);
            sp.Play();
        }
        public static void stop()
        {
            sp.Stop();
        }
    }
}
