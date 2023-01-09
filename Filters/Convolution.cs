public static class ConvolutionFilter
{
    public static Bitmap Sobel(Bitmap bmp)
    {
        Bitmap returnBmp = new Bitmap(bmp.Width, bmp.Height);
        var grayBmp = GrayFilter.FastGrayScale(bmp);
        float[] img = HistogramFilter.fastConvertFloat(grayBmp);

        float[] result = new float[img.Length];
        float[] realResult = new float[img.Length];

        float soma;
        float total;

        for (int j = 1; j < bmp.Height - 1; j++)
        {
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                soma = img[(i + (j * bmp.Width)) - 1] + img[(i + (j * bmp.Width))];
                total = soma;
                soma = img[(i + (j * bmp.Width))] + img[(i + (j * bmp.Width)) + 1];
                total -= soma;
                result[i + (j * bmp.Width)] = total;
            }
        }

        for (int i = 1; i < bmp.Width - 1; i++)
        {
            for (int j = 1; j < bmp.Height - 1; j++)
            {
                soma = result[(i + (j * bmp.Width)) - 1] + result[(i + (j * bmp.Width))] + result[(i + (j * bmp.Width))] + 1;
                total = soma;
                soma = result[(i + (j * bmp.Width))];
                total -= soma;
                realResult[i + (j * bmp.Width)] = total;
            }
        }


        int index = 0;
        // Locking the return data
        var data = returnBmp.LockBits(
            new Rectangle(0, 0, returnBmp.Width, returnBmp.Height),
            System.Drawing.Imaging.ImageLockMode.ReadWrite,
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
                    int r = (int)(realResult[index] * 255);
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


    public static Bitmap FastSobel(Bitmap bmp)
    {
        Bitmap returnBmp = new Bitmap(bmp.Width, bmp.Height);
        var grayBmp = GrayFilter.FastGrayScale(bmp);
        float[] img = HistogramFilter.fastConvertFloat(grayBmp);

        float[] result = new float[img.Length];
        float[] realResult = new float[img.Length];

        float soma;
        float total;
        
        soma = img[0] - img[2];
        for (int j = 1; j < bmp.Height - 1; j++)
        {
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                soma -= result[(i + (j * bmp.Width)) - 1];
                soma += result[(i + (j * bmp.Width)) + 1]; 
            }
        }

        soma = img[0] + img[1] + img[2];
        for (int i = 2; i < bmp.Width - 1; i++)
        {
            for (int j = 1; j < bmp.Height - 1; j++)
            {
                soma -= result[(i + (j * bmp.Width)) - 2];
                soma += result[(i + (j * bmp.Width)) + 1];

                realResult[i + (j * bmp.Width)] = soma;
            }
        }


        int index = 0;
        // Locking the return data
        var data = returnBmp.LockBits(
            new Rectangle(0, 0, returnBmp.Width, returnBmp.Height),
            System.Drawing.Imaging.ImageLockMode.ReadWrite,
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
                    int r = (int)(result[index] * 255);
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











    // public static Bitmap Convolution(Bitmap bmp, float[] kernel)
    // {
    //     Bitmap returnbmp = new Bitmap(bmp.Width, bmp.Height);
    //     var grayBmp = GrayFilter.FastGrayScale(bmp);
    //     float[] img = HistogramFilter.fastConvertFloat(grayBmp);

    //     float[] parcialSum = new float[img.Length];

    //     // float[] parcial = {img[0], img[1], img[2]};
    //     // int sum = (int)parcial.Sum(e => e);
    //     float sum = img[0] + img[1] + img[2];

    //     for (int j = 1; j < bmp.Height - 1; j++)
    //     {
    //         for (int i = 1; i < bmp.Width - 1; i++)
    //         {
    //             sum -= img[(i * (j * bmp.Width)) - 1];
    //             sum += img[(i + (j * bmp.Width)) + 1];
                
    //             parcialSum[i + (j * bmp.Width)] = sum;
    //         }
    //         sum = 0;
    //     }

    //     int color;

        
    //     return returnbmp;
    // }
}