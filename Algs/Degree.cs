public static class Degree
{
    public static float[] rotation(float degree)
    {
        float radian = degree / 180 * MathF.PI;
        float cos = MathF.Cos(radian);
        float sin = MathF.Sin(radian);
        return new float[]
        {
            cos, -sin, 0,
            sin, cos, 0,
            0,     0, 1
        };
    }

    public static float[] scale(float dx, float dy)
    {
        return new float[]
        {
            dx, 0, 0,
            0, dy, 0,
            0, 0, 1
        };
    }

    public static float[] trasnslate(float dx, float dy)
    {
        return new float[]
        {
        1, 0, dx,
        0, 1, dy,
        0, 0, 1
        };
    }

    public static float[] translateFromSize(float dx, float dy, Bitmap bmp)
    {
        return new float[]
        {
            1, 0, dx * bmp.Width,
            0, 1, dy * bmp.Height,
            0, 0, 1
        };
    }
}