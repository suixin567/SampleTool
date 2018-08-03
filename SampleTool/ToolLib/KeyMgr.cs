using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolLib
{

  public  class KeyMgr
    {
        private static KeyMgr instance;
        public static KeyMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new KeyMgr();
                    instance.init();
                }
                return instance;
            }
        }

        static KeyboardHook k_hook = new KeyboardHook();

        public delegate void Hook_KeyUp_Event(object sender, KeyEventArgs e);
        public Hook_KeyUp_Event hook_KeyUp_Event;

        public delegate void Hook_KeyDown_Event(object sender, KeyEventArgs e);
        public Hook_KeyDown_Event hook_KeyDown_Event;


        public delegate void Hook_KeyPress_Event(object sender, KeyPressEventArgs e);
        public Hook_KeyPress_Event hook_KeyPress_Event;

        void init() {
            k_hook.KeyDownEvent += new KeyEventHandler(hook_KeyDown);//钩住键按下
            k_hook.KeyUpEvent += new KeyEventHandler(hook_KeyUp);
            k_hook.KeyPressEvent += new KeyPressEventHandler(hook_KeyPress); 

            k_hook.Start();
        }

        void hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (hook_KeyDown_Event!=null)
            {
                hook_KeyDown_Event(sender ,e);
            }     
        }

        void hook_KeyUp(object sender, KeyEventArgs e)
        {
            if (hook_KeyUp_Event!=null)
            {
                hook_KeyUp_Event(sender, e);
            }  
        }
        void hook_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (hook_KeyPress_Event!=null)
            {
                hook_KeyPress_Event(sender, e);
            }
        }
        //public void stop() {
        //    k_hook.Stop();
        //}
    }
}
