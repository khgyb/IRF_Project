using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ssp7wq_irf_project.reserve
{
    class MainPanel : Panel
    {
        public MainPanel(int width, int height)
        {
            Top = 250;
            Left = 0;
            Width = width;
            Height = height;
            BackColor = Color.LightBlue;
        }
    }
}
