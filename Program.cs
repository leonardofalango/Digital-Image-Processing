using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;

ApplicationConfiguration.Initialize();

// Images in a vector
List<(Bitmap, byte[])> images = new List<(Bitmap, byte[])>();

var form = new Form();
form.WindowState = FormWindowState.Maximized; // Size

// Tick/Timer
System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
tm.Interval = 20;


// Picture Box
var pb = new PictureBox();
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

// On load
(Bitmap bmp, byte[] img) t = (null, new byte[0]);
Graphics g = null;
Point cursor = Point.Empty;
Bitmap bmp = null;

form.Load += delegate
{
    images = ProcessImage.LoadDirectory("Images");
    tm.Start();
    t = images[0];
    bmp = new Bitmap(pb.Width, pb.Height);
    pb.Image = bmp; 
    g = Graphics.FromImage(bmp);
};

// Mouse Controls
pb.MouseMove += (o, e) =>
{
    cursor = e.Location;
};
bool isDown = false;

// Functions For Drawings
int threshold = 110;
int index = 0;
int deg = 0;
Bitmap NextImage()
{
    index++;
    if (index >= images.Count)
        index = 0; 
    return images[index].Item1;
}
Bitmap PreviousImage()
{
    index--;
    if (index < 0)
        index = images.Count - 1; 
    return images[index].Item1;
}

// Getting keypresses
form.KeyDown += (o, e) =>
{
    if (e.KeyCode == Keys.Escape)
        Application.Exit();
    
    if (e.KeyCode == Keys.A)
        t.bmp = NextImage();
    
    if (e.KeyCode == Keys.D)
        t.bmp = PreviousImage();
    
    pb.Refresh();
};

// Running
tm.Tick += (o, e) =>
{
    Template.DrawTemplate(
        bmp,
        g,
        cursor,
        isDown
    );
    pb.Image = bmp;
    pb.Refresh();
};

Application.Run(form);