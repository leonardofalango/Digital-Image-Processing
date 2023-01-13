public static class ProcessImage
{
    public static (Bitmap bmp, byte[]img) Open(string path)
    {
        Bitmap bmp = (Bitmap) Image.FromFile(path);
        byte[] img = new byte [bmp.Width * bmp.Height];
        img = Filter.ConvertImage(bmp);

        return (bmp, img);
    }

    public static List<(Bitmap bmp, byte[] img)> LoadDirectory(string directoryPath)
    {
        List<(Bitmap bmp, byte[] img)> retornoLista = new List<(Bitmap bmp, byte[] img)>();
        string[] files = Directory.GetFiles(directoryPath);
        foreach (var item in files)
        {
            var ext = string.Concat(item.Reverse().TakeWhile(c => c != '.').Reverse());
            if (ext != "jpg" && ext != "png") continue;
            retornoLista.Add(Open(item));
        }   
        return retornoLista;
    }
}