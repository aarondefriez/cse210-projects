using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using System.Windows.Forms;

class Program : Form
{
    public static bool debug;
    static int _width = 200;
    static int _height = 200;
    static int _desiredWdith = 500;
    static int _desiredHeight = 500;
    Grid _grid = new Grid(_width, _height);
    PictureBox newPictureBox = new PictureBox();
    System.Windows.Forms.Timer renderTimer = new System.Windows.Forms.Timer();
    Bitmap scaled = new Bitmap(_desiredWdith, _desiredHeight);
    // Input
    bool mouseDown = false;
    int mouseX;
    int mouseY;
    byte radius = 3;
    byte material = 1;
    public Program()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.AutoScaleMode = AutoScaleMode.None;
        InitializeWindow();
        
        // Set up timer
        renderTimer.Interval = 16; // ~60 FPS
        renderTimer.Tick += (s, e) => Render();
        renderTimer.Start();
    }

    public void InitializeWindow()
    {
        newPictureBox.Location = new Point(0, 0);
        newPictureBox.BackColor = Color.Black;
        newPictureBox.Visible = true;

        // Fill the form
        newPictureBox.Dock = DockStyle.Fill; 
        newPictureBox.MouseMove += Mouse_Moved;
        newPictureBox.MouseDown += Mouse_Down;
        newPictureBox.MouseUp += Mouse_Up;
        newPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

        Controls.Add(newPictureBox);
    }

    [STAThread]
    static void Main()
    {
        Application.Run(new Program());
    }
    // Handle Rendering
    public void Render()
    {
        if(!debug)
            _grid.Step();
        // Add new pixels
        if (mouseDown)
            _grid.AddPixel(mouseX, mouseY, radius, material);
        newPictureBox.Image = UpscaleImage(_grid.ToBitmap(), _desiredWdith, _desiredHeight);
    }
    // Handle Inputs
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Escape)
        {
            // Restore window
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
            return true;
        }
        if (keyData == Keys.F11)
        {
            // Restore window
            bool swap = this.FormBorderStyle == FormBorderStyle.None;
            this.FormBorderStyle = swap ? FormBorderStyle.Sizable : FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            return true;
        }
        if(keyData == Keys.Space)
        {
            _grid.Step();
            debug = true;
        }
        if (keyData == Keys.OemMinus)
        {
            // Shrink cursor
            radius--;
            radius = Math.Clamp(radius, (byte)1, (byte)20);
            return true;
        }
        if (keyData == Keys.Oemplus)
        {
            // Shrink cursor
            radius++;
            radius = Math.Clamp(radius, (byte)1, (byte)20);
            return true;
        }
        string keypressed = keyData.ToString();
        if(int.TryParse(keypressed[1].ToString(), out int key))
            material = (byte)key;
        return base.ProcessCmdKey(ref msg, keyData);
    }
    private void Mouse_Moved(object? sender, MouseEventArgs e)
    {
        mouseX =  e.X * _width / newPictureBox.Width;
        mouseY =  e.Y * _height / newPictureBox.Height;
    }
    private void Mouse_Down(object? sender, MouseEventArgs e)
    {
        mouseDown = true;
    }
    private void Mouse_Up(object? sender, MouseEventArgs e)
    {
        mouseDown = false;
    }
    
    Bitmap UpscaleImage(Bitmap original, int width, int height)
    {
        // Create a new bitmap with the desired size
        using (Graphics g = Graphics.FromImage(scaled))
        {
            g.Clear(Color.Black);
            g.InterpolationMode = InterpolationMode.NearestNeighbor; // to keep pixelized
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.DrawImage(original, 0, 0, width, height);
        }

        return scaled;
    }
}