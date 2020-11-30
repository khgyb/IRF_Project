﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ssp7wq_irf_project.reserve
{
    class Seat : Button
    {
		private int _status;

		public int Status
		{
			get { return _status; }
			set 
			{
				_status = value;
				if (_status<1)
				{
					_status = 3;
				}
				if (_status>3)
				{
					_status = 1;
				}

				if (_status==1)
				{
					BackColor = Color.Green;
				}
				else if (_status==2)
				{
					BackColor = Color.Yellow;
				}
				else if (_status==3)
				{
					BackColor = Color.Red;
				}
			}
		}

		public Seat()
		{
			Width = 20;
			Height = Width;
		}

	}
}