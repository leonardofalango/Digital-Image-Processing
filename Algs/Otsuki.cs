public static class Algs
{
    public static int Otsuki(Bitmap bmp, float db=0.5f)
    {
        var img = HistogramFilter.fastConvertFloat(bmp);
        var histogram = HistogramFilter.Histogram(bmp);
        int N = bmp.Width * bmp.Height;
        int threshold = 0;

        float Ex0 = 0;
        float Ex1 = img.Average();
        float Dx0 = 0;
        float Dx1 = img.Sum(x => x*x);
        int N0 = 0;
        int N1 = img.Length;

        float minStddev = float.PositiveInfinity;

        for (int i = 0; i < histogram.Length; i++)
        {
            float value = db * (2 * i + 1) / 2;
            float s = histogram[i] * value;

            if (N0 == 0 && histogram[i] == 0)
                continue;

            Ex0 = (Ex0 * N0 + s) / (N0 + histogram[i]);
            Ex1 = (Ex1 * N1 - s) / (N1 - histogram[i]);

            N0 += histogram[i];
            N1 -= histogram[i];

            Dx0 += value * value * histogram[i];
            Dx1 -= value * value * histogram[i];

            float stddev =
                Dx0 - N0 * Ex0 * Ex0 + 
                Dx1 - N1 * Ex1 * Ex1;
            
            if (float.IsInfinity(stddev) ||
                float.IsNaN(stddev))
                continue;
            
            if (stddev < minStddev)
            {
                minStddev = stddev;
                threshold = i;
            }
        }
        return threshold;
    }
}