using System.Numerics;

public static class Affine
{
    public static Bitmap RotationWithoutRed(Bitmap bmp, Matrix4x4 mat)
    {

        float[] para = new float[]
        {
            mat.M11, mat.M12, mat.M13,
            mat.M21, mat.M22, mat.M23,
            mat.M31, mat.M32, mat.M33,
        };

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

    public static Bitmap Rotation(Bitmap bmp, float deg)
    {  
        Matrix4x4 matrix = translateFromSize(.5f, .5f, bmp) *
        rotation(deg) * 
        translateFromSize(-.5f, -.5f, bmp);
        return RotationWithoutRed(bmp, matrix);
    }

    private static Matrix4x4 mat(float[] arr)
    {
        return new Matrix4x4(
        arr[0], arr[1], arr[2], 0,
        arr[3], arr[4], arr[5], 0,
        arr[6], arr[7], arr[8], 0,
        0,      0,      0,      1
    );
    }

    public static Matrix4x4 rotation(float degree)
    {
        float radian = degree / 180 * MathF.PI;
        float cos = MathF.Cos(radian);
        float sin = MathF.Sin(radian);
        return mat(new float[]
        {
            cos, -sin, 0,
            sin, cos, 0,
            0,     0, 1
        });
    }

    public static Matrix4x4 scale(float dx, float dy)
    {
        return mat(new float[]
            {
                dx, 0, 0,
                0, dy, 0,
                0, 0, 1
            }
        );
    }

    public static Matrix4x4 trasnslate(float dx, float dy)
    {
        return mat(new float[]
        {
        1, 0, dx,
        0, 1, dy,
        0, 0, 1
        });
    }

    public static Matrix4x4 translateFromSize(float dx, float dy, Bitmap bmp)
    {
        return mat(new float[]
        {
            1, 0, dx * bmp.Width,
            0, 1, dy * bmp.Height,
            0, 0, 1
        });
    }
}