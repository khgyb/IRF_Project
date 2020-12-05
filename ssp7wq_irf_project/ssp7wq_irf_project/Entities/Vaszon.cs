using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ssp7wq_irf_project.Entities
{
    class Vaszon : PictureBox
    {
        public Vaszon(int width)
        {
            Height = 10;
            Width = width;
            BackColor = Color.Black;
            Left = 20;
            Top = 50;
        }
    }
}
