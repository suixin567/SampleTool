using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DDBuildHelper
{
    public partial class EventItem : UserControl
    {
        public string m_content;
        public string m_time;

        public EventItem()
        {
            InitializeComponent();
        }

        public EventItem(string content, Image<Bgr, byte> img)
        {
            InitializeComponent();
            m_content = content;
            m_time = System.DateTime.Now.ToString();
            this.label1.Text = content + "    " + m_time;
            this.imageBox1.Image = img;
            if (img == null)
            {
                this.Size = new Size(label1.Width, label1.Height + 7);
            }
            else {
                this.imageBox1.Size = img.Size;
                if (label1.Width > imageBox1.Width)
                {
                    this.Size = new Size(label1.Width, label1.Height + 7 + imageBox1.Height);
                }
                else {
                    this.Size = new Size(imageBox1.Width, label1.Height + 7 + imageBox1.Height);
                }                
            }
        }

        private void EventItem_Load(object sender, EventArgs e)
        {

        }
    }
}
