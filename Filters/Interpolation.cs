public static class Interpolation
{
    public static byte[] Bilinear((Bitmap bmp, byte[] img) t)
    {
        byte[] aux = t.img;
        byte[] final = new byte[t.img.Length];
        int width =  t.bmp.Width;
        int height = t.bmp.Height; 

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                int index = i + j * height;
                if (aux[index] > 0f ||
                i == 0 || j == 0 ||
                i == width - 1 || j == height - 1
                )
                {
                    final[index] = aux[index];
                    continue;
                }

                var topLeft = (i - 1) + (j - 1) * width;
                var topRigth = (i + 1) + (j - 1) * width;
                float topMean = (topLeft + topRigth) / 2;

                var bottomLeft = (i - 1) + (j + 1) * width;
                var bottomRigth = (i + 1) + (j + 1) * width;
                float bottomMean = (bottomLeft + bottomRigth) / 2;

                final[index] = (byte)((topMean + bottomMean) / 2);
            }
        }

        return final;
    }
}