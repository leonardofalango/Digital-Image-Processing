using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

public static class Template
{
    public static List<Point> Points = new List<Point>();

    public static void DrawTemplate(Bitmap tela, Graphics gTela,
        Point cursor, bool isDown)
    {
        try
        {

            //Canetas
            Pen CanetaVermelhaTeste = new Pen(Brushes.Red, 10f);
            Pen CanetaPreta = new Pen(Brushes.Black, 10f);
            Pen CanetaMarrom = new Pen(Brushes.SaddleBrown, 10f);


            //Pinceis
            Brush FundoVerde = Brushes.DarkGreen;
            Brush FundoPretoReconhecimento = Brushes.Black; 
            Brush FundoCinzaPreenchimento = Brushes.Gray;
            Brush YellowBrush = Brushes.Yellow;
            Brush BlueBrush = Brushes.Blue;
            

            // Menu -> options
            int quantItem = 6;
            
            int top = 0;
            int left = 0;
            int right = tela.Width;
            int bottom = tela.Height;
            int cornerMenu = right - (int)(tela.Width * 0.2f);
            int maxHeight = (bottom / 2);

            
            Rectangle Menu = new Rectangle(right, top, cornerMenu, bottom);


            //Molduras
            int padding = 10;
            int quantRow = 3;
            int quantCol = quantItem / quantRow;
            
            int widthColumn = (int)(tela.Width * 0.2f / quantCol);
            int gapColumn = widthColumn + padding;
            int heightColumn = maxHeight / quantRow;


            List<RectangleF> molds = new List<RectangleF>();
            List<RectangleF> items = new List<RectangleF>();

            for (int j = 0; j < quantRow; j++)
            {
                for (int i = 0; i < quantCol; i++)
                {
                    molds.Add(new RectangleF(
                        cornerMenu + widthColumn * i,
                        (heightColumn * j ),

                        widthColumn, heightColumn
                    ));

                    items.Add(new RectangleF(
                        cornerMenu + widthColumn * i + padding,
                        (heightColumn * j) + padding,
                        widthColumn - (padding * 2),
                        heightColumn - (padding * 2)
                    ));
                }
            }

            // Painting the items
            gTela.FillRectangles(
                YellowBrush,
                molds.ToArray()
            );
            gTela.FillRectangles(
                BlueBrush,
                items.ToArray()
            );
            
            // Menu -> Colors

            //Escritas//
            SolidBrush drawBrush = new SolidBrush(Color.AliceBlue);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            String textoDesfazer = "";
            Font fontDesfazer = new Font("Arial", 3 - (int)(0.115 * 5));
            
            //Layout//
           

            gTela.DrawString(textoDesfazer, fontDesfazer, drawBrush, new Rectangle(0,0, 200, 200), format);
            
            //Imagem Camera
            RectangleF destRect = new RectangleF();    
            RectangleF srcRect = new RectangleF(0.0F, 0.0F, tela.Width, tela.Height);
            GraphicsUnit units = GraphicsUnit.Pixel;      
            gTela.DrawImage(tela, destRect, srcRect, units);

        }
        catch
        {

        }
    }
}