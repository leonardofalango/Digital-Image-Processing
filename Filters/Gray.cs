public static class GrayFilter
{
    public static Bitmap GrayScale(Bitmap bmp )
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

    public static Bitmap FastGrayScale(Bitmap bmp)
    {
        Bitmap returnBmp = new Bitmap(bmp.Width, bmp.Height);
        
        // Locking the original data
        var originData = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            System.Drawing.Imaging.ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb
        );
        // Locking the return data
        var data = returnBmp.LockBits(
            new Rectangle(0, 0, returnBmp.Width, returnBmp.Height),
            System.Drawing.Imaging.ImageLockMode.ReadWrite,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb
        );

        unsafe
        {
            byte* originP = (byte*)originData.Scan0.ToPointer();
            byte* p = (byte*)data.Scan0.ToPointer();

            for (int j = 0; j < originData.Height; j++)
            {
                byte* originL = originP + j * originData.Stride;
                byte* l = p + j * data.Stride;

                for (int i = 0; i < originData.Width; i++, l+=3, originL += 3)
                {
                    // originL[0] = original Blue pixel
                    // l[0] = returnBmp Blue pixel
                    byte gray = (byte)((30 * originL[2] + 59 * originL[1] + 11 * originL[0]) / 100);
                    l[0] = gray;
                    l[1] = gray;
                    l[2] = gray;
                }
            }
        }
        bmp.UnlockBits(originData);
        returnBmp.UnlockBits(data);
        return returnBmp;
    }

    public static float[] ConvertImage(Bitmap bmp)
    {
        float[] img = new float[bmp.Width * bmp.Height];
        
        int index = 0;
        var data = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            System.Drawing.Imaging.ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb
        );
        unsafe
        {
            byte* p = (byte*)data.Scan0.ToPointer();

            for (int j = 0; j < bmp.Height; j++)
            {
                byte* l = p + j * data.Stride;
                for (int i = 0; i < bmp.Width; i++, l+=3, index++)
                {
                    img[index] = l[0] * 0.11f + l[1] * 0.59f + l[2] * 0.3f;
                }
            }
        }
        return img;
    }

}