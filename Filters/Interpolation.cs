public static class Interpolation
{
    public static float[] Bilinear(Bitmap bmp)
    {
        bmp = GrayFilter.FastGrayScale(bmp);
        float[] img = HistogramFilter.fastConvertFloat(bmp);
        float[] aux = img;
        float[] final = new float[img.Length];
        int width =  bmp.Width;
        int height = bmp.Height; 

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

                final[index] = (topMean + bottomMean) / 2;
            }
        }

        return final;
    }
}