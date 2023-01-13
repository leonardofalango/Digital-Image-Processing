using System.Numerics;

public static class ConvolutionFilter
{
    public static byte[] Sobel((Bitmap bmp, byte[] img) t)
    {
        byte[] result = new byte[t.img.Length];
        byte[] realResult = new byte[t.img.Length];

        float soma;
        float total;

        for (int j = 1; j < t.bmp.Height - 1; j++)
        {
            for (int i = 1; i < t.bmp.Width - 1; i++)
            {
                soma = t.img[(i + (j * t.bmp.Width)) - 1] + t.img[(i + (j * t.bmp.Width))];
                total = soma;
                soma = t.img[(i + (j * t.bmp.Width))] + t.img[(i + (j * t.bmp.Width)) + 1];
                total -= soma;
                result[i + (j * t.bmp.Width)] = (byte)total;
            }
        }

        soma = t.img[0] + t.img[1] + t.img[2];
        for (int i = 2; i < t.bmp.Width - 1; i++)
        {
            for (int j = 1; j < t.bmp.Height - 1; j++)
            {
                soma -= result[(i + (j * t.bmp.Width)) - 2];
                soma += result[(i + (j * t.bmp.Width)) + 1];

                realResult[i + (j * t.bmp.Width)] = (byte)soma;
            }
        }
        return realResult;
    }

    public static byte[] Convolution((Bitmap bmp, byte[] img) t, byte[] kernel)
    {
        int baseKernel = (int)Math.Sqrt(kernel.Length);
        byte[] result = new byte[t.img.Length];


        for (int j = 1; j < t.bmp.Height - 1; j++)
            for (int i = 1; i < t.bmp.Width - 1; i++)
                for (int m = 0; m < baseKernel; m++)
                    for (int n = 0; n < baseKernel; n++)
                        result[i + j * t.bmp.Width] = (byte)
                            (t.img[(i + n - baseKernel / 2) 
                            + (j + m - baseKernel / 2) * t.bmp.Width] 
                            * kernel[m + n * baseKernel]);

      return result;
    }

    public static byte[] RotationWithoutRed((Bitmap bmp, byte[] img) t, Matrix4x4 mat)
    {

        float[] para = new float[]
        {
            mat.M11, mat.M12, mat.M13,
            mat.M21, mat.M22, mat.M23,
            mat.M31, mat.M32, mat.M33,
        };

        byte[] returnImg = new byte[t.img.Length];

        int index;
        int newIndex;

        for (int j = 0; j < t.bmp.Height; j++)
        {
            for (int i = 0; i < t.bmp.Width; i++)
            {
                index = i + j * t.bmp.Width;

                int X1 = (int)(para[0] * i + para[1] * j + para[2]);  
                int Y1 = (int)(para[3] * i + para[4] * j + para[5]);
                
                if (X1 >= t.bmp.Width || X1 < 0 || Y1 >= t.bmp.Height || Y1 < 0)
                    continue;

                newIndex = X1 + Y1 * t.bmp.Width;
                returnImg[newIndex] = t.img[index];
            }
        }

        return returnImg;
    }

    public static byte[] Rotation((Bitmap bmp, byte[] img) t, float deg)
    {  
        Matrix4x4 matrix = translateFromSize(.5f, .5f, t.bmp) *
        rotation(deg) * 
        translateFromSize(-.5f, -.5f, t.bmp);
        return RotationWithoutRed(t, matrix);
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

    private static Matrix4x4 rotation(float degree)
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

    private static Matrix4x4 scale(float dx, float dy)
    {
        return mat(new float[]
            {
                dx, 0, 0,
                0, dy, 0,
                0, 0, 1
            }
        );
    }

    private static Matrix4x4 trasnslate(float dx, float dy)
    {
        return mat(new float[]
        {
        1, 0, dx,
        0, 1, dy,
        0, 0, 1
        });
    }

    private static Matrix4x4 translateFromSize(float dx, float dy, Bitmap bmp)
    {
        return mat(new float[]
        {
            1, 0, dx * bmp.Width,
            0, 1, dy * bmp.Height,
            0, 0, 1
        });
    }

}
