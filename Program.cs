using System.Drawing;
using System.Windows.Forms;

ApplicationConfiguration.Initialize();

var form = new Form();
form.WindowState = FormWindowState.Maximized; // Size
form.FormBorderStyle = FormBorderStyle.None; // Border

// Picture Box
var pb = new PictureBox();
pb.Dock = DockStyle.Fill;
form.Controls.Add(pb);

// Getting keypresses
form.KeyDown += (o, e) =>
{
    if (e.KeyCode == Keys.Escape)
        Application.Exit();
};

// Running
Application.Run(form);