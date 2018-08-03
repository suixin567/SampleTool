using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolLib
{
   public class Keybd
    {

        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        public static void button1_Click()
        {
        //    textBox1.Focus();
            keybd_event(Keys.A, 0, 2, 0);
        }
    }
}
