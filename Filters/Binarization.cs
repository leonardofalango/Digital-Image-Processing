public static class Bin
{
    public static Bitmap BinBmp(Bitmap bmp, int threshold = 125)
    {
        bmp = GrayFilter.FastGrayScale(bmp);

        Bitmap binBmp = new Bitmap(bmp.Width, bmp.Height);
        
        var originalData = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height),
            System.Drawing.Imaging.ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb
        );
        
        var data = binBmp.LockBits(
            new Rectangle(0, 0, binBmp.Width, binBmp.Height),
            System.Drawing.Imaging.ImageLockMode.WriteOnly,
            System.Drawing.Imaging.PixelFormat.Format24bppRgb
        );
        
        unsafe
        {
            byte* originalP = (byte*)originalData.Scan0.ToPointer();
            byte* p = (byte*)data.Scan0.ToPointer();

            for (int j = 0; j < data.Height; j++)
            {
                byte* originalL = originalP + j * originalData.Stride;
                byte* l = p + j * data.Stride;

                for (int i = 0; i < data.Width; i++, l+=3, originalL+=3)
                {
                    if (originalL[0] >= threshold)
                    {
                        l[0] = 255;
                        l[1] = 255;
                        l[2] = 255;
                    }
                    else
                    {
                        l[0] = 0;
                        l[1] = 0;
                        l[2] = 0;
                    }
                }
            }
        }
        bmp.UnlockBits(originalData);
        binBmp.UnlockBits(data);
        
        return binBmp;
    }
}