using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace graphlab
{
    public partial class Form1 : Form
    {
        public string G;
        Bitmap B;
        Image image;
        int rad;
        Bitmap I;
        double e11, e12, e13, e21, e22, e23, e31, e32, e33,div;
        public Form1()
        {
            InitializeComponent();
          
           
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
      
        private void pictureBox1_Click(object sender, EventArgs e)
        {
           

        }
       

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.TIF, *.PNG, *.ICO, *.EMF, *.WMF)|*.bmp;*.jpg;*.gif; *.tif; *.png; *.ico; *.emf; *.wmf";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
            
                pictureBox1.Image = Image.FromFile(dialog.FileName);
               image= Image.FromFile(dialog.FileName);
               

                B = new Bitmap(dialog.FileName);
               
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private Bitmap resize(Bitmap my_bitmap,double val,double valY) //масштабирование изображения,первый цикл по X,второй по Y
        {



            Bitmap C = new Bitmap(Convert.ToInt32(my_bitmap.Width * val), my_bitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Bitmap H = new Bitmap(Convert.ToInt32(my_bitmap.Width * val), Convert.ToInt32(my_bitmap.Height * valY), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
       
                 
        int a = 0;
        int b = 0;
            for (int j = 0; j< my_bitmap.Height; j++)
                for (int i = 0; i< my_bitmap.Width; i++)
                {

                    if (Convert.ToInt32(i * val) - a >0)
                    {
                        for (int y = a; y < Convert.ToInt32(i * val); y++)
                        {
                            C.SetPixel(y, j, my_bitmap.GetPixel(i, j));
                        }
                    }
                 a = Convert.ToInt32(i* val);
                }
               
         
           for (int i = 0; i<C.Width; i++)
                for (int j = 0; j<C.Height; j++)
                {
                   
                    if (Convert.ToInt32(j* valY) - b > 0)
                    {
                        for (int y = b; y < Convert.ToInt32(j* valY); y++)
                        {

                           H.SetPixel(i, y, C.GetPixel(i, j));
                        }

                    }
                    b = Convert.ToInt32(j* valY);
                }


           


                    return H;
        }

        private void povorotimage(int angle) // поворот изображения
        {

            I = new Bitmap(Convert.ToInt32(Math.Sqrt(B.Height * B.Height + B.Width * B.Width)), Convert.ToInt32(Math.Sqrt(B.Height * B.Height + B.Width * B.Width)), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            double angleRadian = angle * Math.PI / 180; //переводим угол в радианты

            int z = Convert.ToInt32((Math.Sqrt(B.Height * B.Height + B.Width * B.Width) - B.Width) / 2);
            int c = Convert.ToInt32((Math.Sqrt(B.Height * B.Height + B.Width * B.Width) - B.Height) / 2);
            for (double j = 0; j < B.Height-1; j+=(1/Math.Sqrt(2)))
                for (double i = 0; i < B.Width-1; i+= (1 / Math.Sqrt(2)))
                {

                    int x = Convert.ToInt32(((i - B.Width / 2) * Math.Cos(angleRadian) - (j - B.Height / 2) * Math.Sin(angleRadian) + B.Width/2));
                    int y = Convert.ToInt32(((i - B.Width / 2) * Math.Sin(angleRadian) + (j - B.Height / 2) * Math.Cos(angleRadian) +B.Height / 2));

                    I.SetPixel(x + z, y + c, B.GetPixel(Convert.ToInt32(i), Convert.ToInt32( j)));

                }
       
            I.Save("sample.png");
            pictureBox1.Image = I;
        }
       
        

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox4.Text != "")
            {

                Bitmap my_bitmap = (Bitmap)pictureBox1.Image;

                double scale = Convert.ToDouble(textBox1.Text);
                double scaleY = Convert.ToDouble(textBox4.Text);
                Bitmap G=resize(my_bitmap,scale,scaleY);
          

                pictureBox1.Image = G;
                G.Save("sample.png");
         


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                Bitmap my_bitmap = (Bitmap)pictureBox1.Image;
                int scale = Convert.ToInt32(textBox2.Text);
                povorotimage(scale);
            }
        }
        private Tuple<double,double,double> fromRGBtoLab(Color f)
        {

            double R =Convert.ToDouble( f.R) / 255;
            double G = Convert.ToDouble(f.G) / 255;
            double B = Convert.ToDouble(f.B)/255;

            if (R > 0.04045) { R = Math.Pow(((R + 0.055) / 1.055), 2.4); }
            else R = R / 12.92;
            if (G > 0.04045) { G = Math.Pow(((G + 0.055) / 1.055), 2.4); }
            else G = G / 12.92;
            if (B > 0.04045) { B = Math.Pow(((B + 0.055) / 1.055), 2.4); }
            else B =B / 12.92;

            R = R * 100;
            G = G * 100;
            B = B * 100;

            double X = R * 0.4124 + G * 0.3576 + B * 0.1805;
            double Y =R * 0.2126 + G * 0.7152 + B * 0.0722;
           double Z = R * 0.0193 + G * 0.1192 + B * 0.9505;

            double Xq = X /95.047;
            double Yq = Y /100 ;
            double Zq = Z / 108.883;

            if (Xq > 0.008856) Xq = Math.Pow(Xq, 0.33333);
            else Xq = (7.787 * Xq) + (16 / 116);
            if (Yq > 0.008856) Yq = Math.Pow(Yq, 0.33333);
            else Yq = (7.787 * Yq) + (16 / 116);
            if (Zq > 0.008856) Zq = Math.Pow(Zq, 0.33333);
            else Zq = (7.787 * Zq) + (16 / 116);

            double L = (116 * Yq) - 16;
            double a = 500 * (Xq - Yq);
            double b = 200 * (Yq -Zq);
            


            return Tuple.Create(L,a,b);  
        }
        private Tuple<int, int, int> fromLabtoRGB(double L,double a,double b)
        {
            double var_Y = (L + 16) / 116;
      double  var_X = a / 500 + var_Y;
           double var_Z = var_Y - b / 200;

            if (Math.Pow(var_Y , 3) > 0.008856) var_Y = Math.Pow(var_Y, 3);
            else var_Y = (var_Y - 16 / 116) / (7.787);
            if (Math.Pow(var_X, 3) > 0.008856) var_X = Math.Pow(var_X, 3);
            else var_X = (var_X - 16 / 116) / (7.787);
            if (Math.Pow(var_Z, 3) > 0.008856) var_Z = Math.Pow(var_Z, 3);
            else var_Z = (var_Z - 16 / 116) / (7.787);

         double   X = var_X * 95.047;
            double Y = var_Y * 100;
            double Z = var_Z * 108.883;



            var_X = X / 100;
            var_Y = Y / 100;
            var_Z = Z / 100;

            double var_R = var_X * 3.2406 + var_Y *(-1.5372) + var_Z *(-0.4986);
            double var_G = var_X *( -0.9689) + var_Y * 1.8758 + var_Z * 0.0415;
            double var_B = var_X * 0.0557 + var_Y * -0.2040 + var_Z * 1.0570;

            if (var_R > 0.0031308) var_R = 1.055 * (Math.Pow(var_R , (1 / 2.4))) - 0.055;
            else var_R = 12.92 * var_R;
            if (var_G > 0.0031308) var_G = 1.055 * (Math.Pow(var_G, (1 / 2.4))) - 0.055;
            else var_G = 12.92 * var_G;
            if (var_B > 0.0031308) var_B = 1.055 * (Math.Pow(var_B, (1 / 2.4))) - 0.055;
            else var_B = 12.92 * var_B;

           int sR = Convert.ToInt32( Math.Round(var_R * 255));
           int sG = Convert.ToInt32(Math.Round(var_G * 255));
           int sB = Convert.ToInt32(Math.Round(var_B * 255));
            return Tuple.Create(sR, sG, sB);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
          //  Bitmap my_bitmap = (Bitmap)pictureBox1.Image;
            Bitmap my_bitmap = B;
            int w_b = my_bitmap.Width;
            int h_b = my_bitmap.Height;


            try
            {
                rad = System.Convert.ToInt32(textBox3.Text);
            }
            catch
            {
                rad = 1;
            }

            for (int x = rad + 1; x < w_b - rad; x++)
            {
                for (int y = rad + 1; y < h_b - rad; y++)
                {
                    median_filterNEW(my_bitmap, x, y);
                   

                }

            }
            pictureBox1.Image = my_bitmap;
        
        }
        private void median_filterNEW(Bitmap my_bitmap, int x, int y)//медианный фильтр в цветовой модели LAB
        {
            int n;
            double cR_, cB_, cG_;
            int k = 0;

            n = (2 * rad + 1) * (2 * rad + 1);


            double[] cR = new double[n + 1];
            double[] cB = new double[n + 1];
           double[] cG = new double[n + 1];
            



          

            for (int i = x - rad; i < x + rad + 1; i++)
            {
                for (int j = y - rad; j < y + rad + 1; j++)
                {


                    Color c = my_bitmap.GetPixel(i, j);
                    Tuple<double,double,double> f = fromRGBtoLab(c);
                    cR[k] =f.Item1;
                    cG[k] = f.Item2;
                    cB[k] = f.Item3;
                   
                    k++;

                }
            }
            
            cR = quicksort(cR, n);
            cG = quicksort(cG, n);
            cB = quicksort(cB, n);
        

            int n_ = (int)(n / 2);

            cR_ = cR[n_];
            cG_ = cG[n_];
            cB_ = cB[n_];
            Tuple<int, int, int> h =fromLabtoRGB(cR_,cG_,cB_);
            int R=h.Item1,G=h.Item2,B=h.Item3;
            for (; R > 255; R--) { }
            for (; G > 255; G--) { }
            for (; B > 255; B--) { }
            for (; R < 0; R++) { }
            for (; G < 0; G++) { }
            for (; B < 0; B++) { }

            my_bitmap.SetPixel(x, y, Color.FromArgb( R,G,B));


        }
   
        private double[] quicksort(double[] a, int n)
        {

           double tmp;
            for (int i = 0; i < n; i++)
            {
                for (int j = n - 1; j > i; j--)
                {
                    if (a[j] < a[j - 1])
                    {
                        tmp = a[j];
                        a[j] = a[j - 1];
                        a[j - 1] = tmp;
                    }
                }
            }
            return a;

        }
        private void median_filter(Bitmap my_bitmap, int x, int y)// медианный фильтр в цветовой модели RGB
        {
            int n;
            int cR_, cB_, cG_,cA_;
            int k =0;

            n = (2 * rad + 1) * (2 * rad + 1);

         
            int[] cR = new int[n + 1];
            int[] cB = new int[n + 1];
            int[] cG = new int[n + 1];
            int[] cA = new int[n + 1];



            int w_b = my_bitmap.Width;
            int h_b = my_bitmap.Height;

            for (int i = x - rad; i < x + rad + 1; i++)
            {
                for (int j = y - rad; j < y + rad + 1; j++)
                {
           
            
                    Color c = my_bitmap.GetPixel(i, j);
                    cR[k] =Convert.ToInt32(c.R);
                    cG[k] =Convert.ToInt32(c.G);
                    cB[k] =Convert.ToInt32(c.B);
                    cA[k] = Convert.ToInt32(c.A);
                    k++;
                  
                }
            }
          


         cR=   quicksort(cR, n );
           cG= quicksort(cG,  n );
            cB=quicksort(cB,  n );
            cA=quicksort(cA, n);

            int n_ = (int)(n / 2);

            cR_ = cR[n_];
            cG_ = cG[n_];
            cB_ = cB[n_];
            cA_ = cA[n_];



            my_bitmap.SetPixel(x, y,Color.FromArgb(cA_,cR_, cG_, cB_));
          

        }

        private void median_filter1(Bitmap my_bitmap, int x, int y) //медианный фильтр в RGB с размером в 5 пикселей расположенных в виде +
        {
            int cR_, cB_, cG_,cA_;
            int[] cR = new int[5];
            int[] cB = new int[5];
            int[] cG = new int[5];
            int[] cA = new int[5];
            Color c = my_bitmap.GetPixel(x, y-1);
            cR[0] = Convert.ToInt32(c.R);
            cG[0] = Convert.ToInt32(c.G);
            cB[0] = Convert.ToInt32(c.B);
            cA[0] = Convert.ToInt32(c.A);
            c = my_bitmap.GetPixel(x-1, y );
            cR[1] = Convert.ToInt32(c.R);
            cG[1] = Convert.ToInt32(c.G);
            cB[1] = Convert.ToInt32(c.B);
            cA[1] = Convert.ToInt32(c.A);
            c = my_bitmap.GetPixel(x , y);
            cR[2] = Convert.ToInt32(c.R);
            cG[2] = Convert.ToInt32(c.G);
            cB[2] = Convert.ToInt32(c.B);
            cA[2] = Convert.ToInt32(c.A);
            c = my_bitmap.GetPixel(x+1, y);
            cR[3] = Convert.ToInt32(c.R);
            cG[3] = Convert.ToInt32(c.G);
            cB[3] = Convert.ToInt32(c.B);
            cA[3] = Convert.ToInt32(c.A);
            c = my_bitmap.GetPixel(x , y+1);
            cR[4] = Convert.ToInt32(c.R);
            cG[4] = Convert.ToInt32(c.G);
            cB[4] = Convert.ToInt32(c.B);
            cA[4] = Convert.ToInt32(c.A);
            cR =quicksort(cR, 5);
           cG=quicksort(cG, 5);
          cB= quicksort(cB,5);
            cA = quicksort(cA, 5);


            cR_ = cR[2];
            cG_ = cG[2];
            cB_ = cB[2];
            cA_ = cA[2];
            my_bitmap.SetPixel(x, y, Color.FromArgb(cA_,cR_, cG_, cB_));
        }
        private int[] quicksort(int[] a, int n)
        {

            int tmp;
            for (int i = 0; i < n; i++)
            {
                for (int j = n - 1; j > i; j--)
                {
                    if (a[j] < a[j - 1])
                    {
                        tmp = a[j];
                        a[j] = a[j-1];
                        a[j-1] = tmp;
                    }
                }
            }
            return a;

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void MakeGray(Bitmap bmp) //монохромный фильтр
        {
            // Задаём формат Пикселя.
            PixelFormat pxf =PixelFormat.Format24bppRgb;

            // Получаем данные картинки.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            //Блокируем набор данных изображения в памяти
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);

            // Получаем адрес первой линии.
            IntPtr ptr = bmpData.Scan0;

            // Задаём массив из Byte и помещаем в него надор данных.
            // int numBytes = bmp.Width * bmp.Height * 3; 
            //На 3 умножаем - поскольку RGB цвет кодируется 3-мя байтами
            //Либо используем вместо Width - Stride
            int numBytes = bmpData.Stride * bmp.Height;
            int widthBytes = bmpData.Stride;
            byte[] rgbValues = new byte[numBytes];

            // Копируем значения в массив.
            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            // Перебираем пикселы по 3 байта на каждый и меняем значения
            for (int counter = 0; counter < rgbValues.Length; counter += 3)
            {

                int value = rgbValues[counter] + rgbValues[counter + 1] + rgbValues[counter + 2];
                byte color_b = 0;

                color_b = Convert.ToByte(value / 3);


                rgbValues[counter] = color_b;
                rgbValues[counter + 1] = color_b;
                rgbValues[counter + 2] = color_b;

            }
            // Копируем набор данных обратно в изображение
            Marshal.Copy(rgbValues, 0, ptr, numBytes);

            // Разблокируем набор данных изображения в памяти.
            bmp.UnlockBits(bmpData);
            pictureBox1.Image = bmp;
        }
       
        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            MakeGray(B);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Bitmap my_bitmap = (Bitmap)pictureBox1.Image;

            int w_b = my_bitmap.Width;
            int h_b = my_bitmap.Height;


            

            for (int x = 1; x < w_b -1; x++)
            {
                for (int y =  1; y < h_b - 1; y++)
                {
                  
                    if ((my_bitmap.GetPixel(x, y))!= Color.Empty)
                    {
                        median_filter1(my_bitmap, x, y);
                    }

                }

            }
            my_bitmap.Save("sample.png");
            pictureBox1.Image = my_bitmap;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Bitmap my_bitmap = B;
            int w_b = my_bitmap.Width;
            int h_b = my_bitmap.Height;
         

            try
            {
               e11 = System.Convert.ToDouble(textBox5.Text);
                e12 = System.Convert.ToDouble(textBox6.Text);
                e13 = System.Convert.ToDouble(textBox7.Text);
                e21 = System.Convert.ToDouble(textBox8.Text);
                e22 = System.Convert.ToDouble(textBox9.Text);
                e23= System.Convert.ToDouble(textBox10.Text);
                e31 = System.Convert.ToDouble(textBox11.Text);
                e32 = System.Convert.ToDouble(textBox12.Text);
                e33 = System.Convert.ToDouble(textBox13.Text);
                div = System.Convert.ToDouble(textBox14.Text);

            }
            catch
            {
                e11 = 1;
                e12 = 1;
                e13 = 1;
                e21 = 1;
                e22 = 1; 
                e23 = 1;
                e31 = 1;
                e32 = 1;
                e33 = 1;
                div = 1;

            }

            for (int x =2; x < w_b - 1; x++)
            {
                for (int y = 2; y < h_b - 1; y++)
                {
                    convolution(my_bitmap, x, y);


                }

            }
            pictureBox1.Image = my_bitmap;



        }
        private void convolution(Bitmap my_bitmap, int x, int y) //метод свёртки изображения с помощью матрицы свёртки(возможно есть погрешности)
        {
          
            double cR_, cB_, cG_;
            int k = 0;
          





            double[] cR = new double[9];
            double[] cB = new double[9];
            double[] cG = new double[9];
          
                for (int j = y - 1; j < y + 2; j++)
                {
                    for (int i = x - 1; i < x + 2; i++)
                    {


                    Color c = my_bitmap.GetPixel(i, j);
                    Tuple<double, double, double> f = fromRGBtoLab(c);
                    cR[k] = f.Item1;
                    cG[k] = f.Item2;
                    cB[k] = f.Item3;

                    k++;

                }
            }
            cR_ =  (cR[0] * e11 + cR[1] * e12 + cR[2] * e13 + cR[3] * e21 + cR[4] * e22 + cR[5] * e23 + cR[6] * e31 + cR[7] * e32 + cR[8] * e33)/div;
             cG_ = (cG[0] * e11 + cG[1] * e12 + cG[2] * e13 + cG[3] * e21 + cG[4] * e22 + cG[5] * e23 + cG[6] * e31 + cG[7] * e32 + cG[8] * e33) / div;
             cB_ = (cB[0] * e11 + cB[1] * e12 + cB[2] * e13 + cB[3] * e21 + cB[4] * e22 + cB[5] * e23 + cB[6] * e31 + cB[7] * e32 + cB[8] * e33) / div;
            
           

          
            Tuple<int, int, int> h = fromLabtoRGB(cR_, cG_, cB_);
            int R = h.Item1, G = h.Item2, B = h.Item3;
           

            my_bitmap.SetPixel(x, y, Color.FromArgb(R, G, B));


        }
    }
}
