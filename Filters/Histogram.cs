public static class HistogramFilter
{
    public static int[] Histogram((Bitmap bmp, byte[] img) t, float db = 0.05f)
    {
        int histogramLen = (int)(1 / db) + 1;
        int[] histogram = new int[histogramLen];

        for (int i = 0; i < t.img.Length; i++)
            histogram[(int)(t.img[i] / db)]++;
        
        return histogram;
    }

    public static Bitmap DrawHistogram((Bitmap bmp, byte[] img) t, PictureBox pb)
    {
        int[] hist = Histogram(t);

        Bitmap returnBmp = new Bitmap(pb.Width, pb.Height);
        var g = Graphics.FromImage(returnBmp);
        float margin = 16;

        int max = hist.Max();
        float barlen = (returnBmp.Width - 2 * margin) / hist.Length;
        float r = (returnBmp.Height - 2 * margin) / max;

        for (int i = 0; i < hist.Length; i++)
        {
            float bar = hist[i] * r;
            g.FillRectangle(Brushes.Black, 
                margin + i * barlen,
                returnBmp.Height - margin - bar, 
                barlen,
                bar);
            g.DrawRectangle(Pens.DarkBlue, 
                margin + i * barlen,
                returnBmp.Height - margin - bar, 
                barlen,
                bar);
        }

        return returnBmp; 
    }

}