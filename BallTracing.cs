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
	private double delta = 0.1;
	private const double refresh_rate = 30.0; // how many timers the frame is repaineted
	private const double update_rate = 100.0; // how many times the coordinates of the dot is updated each second
	private const double time_converter = 1000.0; // number of milliseconds per second

	Point center = new Point(800, 400);
	private int x;
	private int y;
	private int previous_x = 0;
	private int previous_y = 0;
	private double t = 0.0;
	private const double center_x = 800.0;
	private const double center_y = 400.0;
	private const double dot_radius = 5.0;
	private const double scale_factor = 40.0; // for the actual graph

	private System.Drawing.Graphics pointer_to_graphic_surface;
	private System.Drawing.Bitmap pointer_to_bitmap_in_memory;
	private const double scalefactor = 40.0; // for the scale of s and y axis

	private Pen bic = new Pen(Color.Black, 1);
	private Pen spiral_pen = new Pen(Color.Red, 3);
	private SolidBrush ballBrush = new SolidBrush(Color.White);

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

		pointer_to_bitmap_in_memory = new Bitmap(1600, 800, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
		pointer_to_graphic_surface = Graphics.FromImage(pointer_to_bitmap_in_memory);
		initialize_bitmap();

		animation_clock.Enabled = false;
		animation_clock.Elapsed += new ElapsedEventHandler(update_position);
		refresh_clock.Enabled = false; 
		refresh_clock.Elapsed += new ElapsedEventHandler(update_graphics);

		DoubleBuffered = true;

		CenterToScreen();
	}

	protected void initialize_bitmap()
	{	Font numeric_label_font = new System.Drawing.Font("Arial", 8, FontStyle.Regular);
		Brush numeric_label_brush = new SolidBrush(System.Drawing.Color.Black);
		double numeric_label = 0.0;
		pointer_to_graphic_surface.Clear(System.Drawing.Color.White);
		
		//draw x axis and y axis
		bic.DashStyle = DashStyle.Solid;
		bic.Width = 1;
		pointer_to_graphic_surface.DrawLine(bic, 0, 400, 1600, 400);
		pointer_to_graphic_surface.DrawLine(bic, 800, 0, 800, 800);

		//draw labels along the axis using a loop
		while(numeric_label * scalefactor * 2.0 < 1600.0)
		{	pointer_to_graphic_surface.DrawString(String.Format("{0:0.0}", numeric_label), numeric_label_font, numeric_label_brush,
								800 + (int)System.Math.Round(numeric_label*scalefactor-10.0), 402);
			if(numeric_label != 0.0)
			{	double numeric_label_negative = numeric_label * (-1.0);
				pointer_to_graphic_surface.DrawString(String.Format("{0:0.0}", numeric_label_negative), numeric_label_font, numeric_label_brush,
								800 - (int)System.Math.Round(numeric_label*scalefactor+10.0), 402);
			}
			numeric_label = numeric_label + 1.0;
		}
		numeric_label = 1.0;
		while(numeric_label * scalefactor * 2.0 < 800.0)
		{	pointer_to_graphic_surface.DrawString(String.Format("{0:0.0}", numeric_label), numeric_label_font, numeric_label_brush,
								802, 400 - (int)System.Math.Round(numeric_label*scalefactor+1.0));
			double numeric_label_negative = numeric_label * (-1.0);
			pointer_to_graphic_surface.DrawString(String.Format("{0:0.0}", numeric_label_negative), numeric_label_font, numeric_label_brush,
								802, 400 + (int)System.Math.Round(numeric_label*scalefactor-1.0));
			numeric_label = numeric_label + 1.0;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{	Graphics board = e.Graphics;
		board.DrawImage(pointer_to_bitmap_in_memory, 0, 0, 1600, 800);
/*
		board.FillRectangle(Brushes.LightYellow, 0, 800, 1600, 100);
		Pen line_pen = new Pen(Color.Black, 1);
		board.DrawLine(line_pen, 0, 400, 1600, 400);
		board.DrawLine(line_pen, 800, 0, 800, 800);
*/
		base.OnPaint(e);
	}

	// x=r*cos(t), y=r*sin(t), r=sqrt(t)
	protected void update_position(System.Object sender, ElapsedEventArgs even)
	{	t = t + delta;
		double r = System.Math.Sqrt(t);
		double x_math = r * System.Math.Cos(t);
		double y_math = r * System.Math.Sin(t);
		double x_scaled = x_math * scale_factor;
		double y_scaled = y_math * scale_factor;
		double x_csharp = x_scaled + 800;
		double y_csharp = y_scaled + 400;
		double x_corner = x_csharp - dot_radius;
		double y_corner = y_csharp - dot_radius;

		x = (int)System.Math.Round(x_corner);
		y = (int)System.Math.Round(y_corner);
	}

	protected void update_graphics(System.Object sender, ElapsedEventArgs even)
	{	if(x>0 && x<=1600)
		{	if(y>0 && y<= 800)
			{	pointer_to_graphic_surface.FillEllipse(ballBrush, x, y, 2, 2);
				if(previous_x != 0 && previous_y != 0)
					pointer_to_graphic_surface.DrawLine(spiral_pen, previous_x, previous_y, x, y);
				previous_x = x;
				previous_y = y;
			}
			else
			{	refresh_clock.Enabled = false;
				animation_clock.Enabled = false;
			}
		}
		else
		{	refresh_clock.Enabled = false;
			animation_clock.Enabled = false;
		}

		Invalidate();
	}

	protected void go_click(Object sender, EventArgs events)
	{	System.Console.WriteLine("you've clicked on the go button.");
		animation_clock.Enabled = true;
		refresh_clock.Enabled = true;
		ballBrush.Color = Color.Red;
	}

	protected void pause_click(Object sender, EventArgs events)
	{	if(pause.Text == "Pause")
		{	System.Console.WriteLine("you've clicked on the pause button.");
			animation_clock.Enabled = false;
			refresh_clock.Enabled = false;
			pause.Text = "Resume";
		}
		else
		{	System.Console.WriteLine("you've clicked on the resume button.");
			animation_clock.Enabled = true;
			refresh_clock.Enabled = true;
			pause.Text = "Pause";
		}
	}

	protected void exit_click(Object sender, EventArgs events)
	{	System.Console.WriteLine("you've clicked the exit button, the program will now shut down.");
		Close();
	}
}