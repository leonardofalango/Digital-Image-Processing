public static class Rotate
{
    public static Bitmap Rotation(Bitmap bmp, float[] para)
    {
        Bitmap returnBmp = new Bitmap(bmp.Width, bmp.Height);
        var grayBmp = GrayFilter.FastGrayScale(bmp);
        float[] img = HistogramFilter.fastConvertFloat(grayBmp);
        float[] returnImg = new float[img.Length];

        int index;
        int newIndex;

        for (int j = 0; j < bmp.Height; j++)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                index = i + j * bmp.Width;

                int X1 = (int)(para[0] * i + para[1] * j + para[2]);  
                int Y1 = (int)(para[3] * i + para[4] * j + para[5]);
                
                if (X1 >= bmp.Width || X1 < 0 || Y1 >= bmp.Height || Y1 < 0)
                    continue;

                newIndex = X1 + Y1 * bmp.Width;
                returnImg[newIndex] = img[index];
            }
        }

        index = 0;
        var data = returnBmp.LockBits(
            new Rectangle(0, 0, returnBmp.Width, returnBmp.Height),
            System.Drawing.Imaging.ImageLockMode.WriteOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb
        );

        unsafe
        {
            byte* p = (byte*)data.Scan0.ToPointer();

            for (int j = 0; j < data.Height; j++)
            {
                byte* originL = p + j * data.Stride;
                byte* l = p + j * data.Stride;

                for (int i = 0; i < data.Width; i++, l+=3, index++)
                {
                    int r = (int)(returnImg[index] * 255);
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    l[0] = (byte)r;
                    l[1] = (byte)r;
                    l[2] = (byte)r;
                }
            }
        }
        returnBmp.UnlockBits(data);
        return returnBmp;

    }
}