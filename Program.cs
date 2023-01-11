using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;

ApplicationConfiguration.Initialize();

// Images in a vector
List<Bitmap> images = new List<Bitmap>();

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
    string[] files = Directory.GetFiles("Images");
    foreach (var item in files)
    {
        var ext = string.Concat(item.Reverse().TakeWhile(c => c != '.').Reverse());
        if (ext != "jpg" && ext != "png")
            continue;
        Bitmap image = (Bitmap)Image.FromFile(item);
        images.Add(image);
    }

    bmp = images[0];
    pb.Image = bmp;
};

// Functions For Drawings
int threshold = 0;
int index = 0;
int deg = 0;
Bitmap NextImage()
{
    index++;
    if (index >= images.Count)
        index = 0; 
    return images[index];
}
Bitmap PreviousImage()
{
    index--;
    if (index < 0)
        index = images.Count - 1; 
    return images[index];
}

// Getting keypresses
form.KeyDown += (o, e) =>
{
    if (e.KeyCode == Keys.Escape)
        Application.Exit();
    
    if (e.KeyCode == Keys.A)
        bmp = NextImage();
    
    if (e.KeyCode == Keys.D)
        bmp = PreviousImage();
    
    if (e.KeyCode == Keys.Up)
        bmp = HistogramFilter.DrawHistogram(bmp, pb);

    if (e.KeyCode == Keys.NumPad0)
        bmp = GrayFilter.FastGrayScale(bmp);

    if (e.KeyCode == Keys.NumPad1)
        bmp = Bin.BinBmp(bmp, threshold);

    if (e.KeyCode == Keys.Add)
    {
        threshold += 10;
        if (threshold > 255)
            threshold = 10;
        MessageBox.Show(threshold.ToString());
    }
    if (e.KeyCode == Keys.Subtract)
    {
        threshold -= 10;
        if (threshold < 0)
            threshold = 255;
        MessageBox.Show(threshold.ToString());
    }
    if (e.KeyCode == Keys.Multiply)
    {
        threshold = Algs.Otsuki(bmp);
        MessageBox.Show(threshold.ToString());
    }

    if (e.KeyCode == Keys.Enter)
    {
        bmp = ConvolutionFilter.Convolution(bmp, new float[]{-1,-2,-1,0,0,0,1,2,1});
    }

    if (e.KeyCode == Keys.Right)
    {
        deg += 10;
        bmp = Affine.Rotation(bmp, deg);
    }
    if (e.KeyCode == Keys.Left)
    {
        deg -= 10;
        bmp = Affine.Rotation(bmp, deg);
    }

    pb.Image = bmp;
    pb.Refresh();
};


// Running
Application.Run(form);