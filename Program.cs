using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

ApplicationConfiguration.Initialize();

// Images in a vector
List<Bitmap> images = new List<Bitmap>();

// Processing images
Bitmap? mrInc = Image.FromFile("Images/image.jpg") as Bitmap;
images.Add(mrInc);

Bitmap? image1 = Image.FromFile("Images/image1.jpg") as Bitmap;
images.Add(image1);
// End of Processing

var form = new Form();
form.WindowState = FormWindowState.Maximized; // Size
form.FormBorderStyle = FormBorderStyle.None; // Border

// Picture Box
var pb = new PictureBox();
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

// On load
Bitmap bmp = null;
Graphics g = null;

form.Load += delegate
{
    bmp = new Bitmap(form.Width, form.Height);
    pb.Image = bmp;
    g = Graphics.FromImage(bmp);

};

// Functions For Drawings
int index = 0;
Bitmap NextImage() { index++; return images[index]; }
Bitmap PreviousImage() { index--; return images[index]; }
// Getting keypresses
form.KeyDown += (o, e) =>
{
    if (e.KeyCode == Keys.Escape)
        Application.Exit();
    
    if (e.KeyCode == Keys.Right)
        bmp = NextImage();
    
    if (e.KeyCode == Keys.Left)
        bmp = PreviousImage();
    
    if (e.KeyCode == Keys.Down)
        bmp = GrayScale();
    pb.Image = bmp;
    pb.Refresh();
};

// Filters/Functions
Bitmap GrayScale()
{
    Bitmap returnBmp = new Bitmap(bmp.Width, bmp.Height);
    for (int i = 0; i < bmp.Width; i++)
        for (int j = 0; j < bmp.Height; j++)
        {
            Color pixel = bmp.GetPixel(i,j);
            int gray = (int)(0.3 * pixel.R + 0.59 * pixel.G + 0.11 * pixel.B) / 3;
            Color newColor = Color.FromArgb(gray, gray, gray);
            returnBmp.SetPixel(i, j, newColor);
        }
    return returnBmp;
}


// Running
Application.Run(form);