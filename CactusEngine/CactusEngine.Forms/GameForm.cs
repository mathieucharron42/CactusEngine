using GameEngine;
using System;
using System.Windows.Forms;

using System.Drawing;
using System.Runtime.InteropServices;

namespace GameEngine
{
	public partial class GameForm : Form
	{
		public GameForm()
		{
			InitializeComponent();
		}

		public Panel GetViewport()
		{
			return viewport;
		}
	}
}
