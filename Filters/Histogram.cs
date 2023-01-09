public static class HistogramFilter
{
    public static int[] Histogram(Bitmap bmp, float db = 0.05f)
    {
        var grayBmp = GrayFilter.FastGrayScale(bmp);
        float[] grays = fastConvertFloat(grayBmp);
        int histogramLen = (int)(1 / db) + 1;
        int[] histogram = new int[histogramLen];

        for (int i = 0; i < grays.Length; i++)
            histogram[(int)(grays[i] / db)]++;
        
        return histogram;
    }

    public static Bitmap DrawHistogram(Bitmap bmp, PictureBox pb)
    {
        int[] hist = Histogram(bmp);

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

    private static float[] convertFloat(Bitmap bmp)
    {
        float[] returnFloat = new float[bmp.Width * bmp.Height];
        int l = 0;
        for (int j = 0; j < bmp.Height; j++)
            for (int i = 0; i < bmp.Width; i++, l++)
                returnFloat[l] = bmp.GetPixel(i, j).R / 255f;
        return returnFloat;
    }

    public static float[] fastConvertFloat(Bitmap bmp)
    {
        float[] arr = new float[bmp.Width * bmp.Height];
        int index = 0;
        
        var data = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            System.Drawing.Imaging.ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb
        );
        unsafe
        {
            byte* p = (byte*)data.Scan0.ToPointer();
            
            // column loop
            for (int j = 0; j < data.Height; j++)
            {
                byte* l = p + j * data.Stride;
                // row loop
                for (int i = 0; i < data.Width; i++, l+=3, index++)
                    arr[index] = l[0] / 255f;
            }
        }
        bmp.UnlockBits(data);

        return arr;
    }
}