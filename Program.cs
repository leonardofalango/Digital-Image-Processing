using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;

ApplicationConfiguration.Initialize();

// Images in a vector
List<(Bitmap, byte[])> images = new List<(Bitmap, byte[])>();

var form = new Form();
form.WindowState = FormWindowState.Maximized; // Size

// Picture Box
var pb = new PictureBox();
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

// On load
(Bitmap bmp, byte[] img) t = (null, new byte[0]);
Graphics g = null;

form.Load += delegate
{
    images = ProcessImage.LoadDirectory("Images");
    t = images[0];
    pb.Image = t.bmp;
};

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
    


    pb.Image = t.bmp;
    pb.Refresh();
};


// Running
Application.Run(form);