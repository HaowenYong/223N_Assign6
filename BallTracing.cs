using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Timers;

public class BallTracing : Form
{	Button go = new Button();
	Button pause = new Button();
	Button exit = new Button();
	
	// timers and timers-related variables
	private static System.Timers.Timer animation_clock = new System.Timers.Timer();
	private static System.Timers.Timer refresh_clock = new System.Timers.Timer();
	private const double delta = 1.0;
	private const double refresh_rate = 30.0; // how many timers the frame is repaineted
	private const double update_rate = 100.0; // how many times the coordinates of the dot is updated each second
	private const double time_converter = 1000.0; // number of milliseconds per second

	public BallTracing()
	{	Size = new Size(1600, 900);
		Text = "Assignment 6";

		go.Text = "Go";
		go.Size = new Size(85, 25);
		go.Location = new Point(100, 820);
		pause.Text = "Pause";
		pause.Size = new Size(85, 25);
		pause.Location = new Point(200, 820);
		exit.Text = "Exit";
		exit.Size = new Size(85, 25);
		exit.Location = new Point(1400, 820);

		go.Click += new EventHandler(go_click);
		pause.Click += new EventHandler(pause_click);
		exit.Click += new EventHandler(exit_click);

		Controls.Add(go);
		Controls.Add(pause);
		Controls.Add(exit);
	}

	protected override void OnPaint(PaintEventArgs e)
	{	Graphics board = e.Graphics;
		board.FillRectangle(Brushes.LightYellow, 0, 800, 1600, 100);
		

		base.OnPaint(e);
	}

	protected void go_click(Object sender, EventArgs events)
	{	System.Console.WriteLine("you've clicked on the go button.");
	}

	protected void pause_click(Object sender, EventArgs events)
	{	if(pause.Text == "Pause")
		{	System.Console.WriteLine("you've clicked on the pause button.");
			pause.Text = "Resume";
		}
		else
		{	System.Console.WriteLine("you've clicked on the resume button.");
			pause.Text = "Pause";
		}
	}

	protected void exit_click(Object sender, EventArgs events)
	{	System.Console.WriteLine("you've clicked the exit button, the program will not shut down.");
		Close();
	}
}
