using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace formsshower
{
    public partial class Form1 : Form
    {
        String FileName = "";
        Bitmap OriginPicture; // original bitmap
        Random rnd1 = new Random();


        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loading the picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bttn_Load_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        /// <summary>
        /// Random noise adding 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bttn_Noise_Click(object sender, EventArgs e)
        {
            long total = OriginPicture.Size.Width * OriginPicture.Size.Height;
            total = (total * (long)chanse_txtbx.Value) / 100;
            Bitmap NoisePicture = new Bitmap(PictureViewer.Image);
            for (long i = 0; i < total; i++) NoisePicture.SetPixel(rnd1.Next(OriginPicture.Size.Width), rnd1.Next(OriginPicture.Size.Height), Color.FromArgb(255, rnd1.Next(255), rnd1.Next(255), rnd1.Next(255)));
            PictureViewer.Image = NoisePicture;
        }

        /// <summary>
        /// Filtering the picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private unsafe void bttn_Filter_Click(object sender, EventArgs e)
        {
            bool AutoFilter = chk_AutoFilter.Checked;
            int step = (int)txtbx_StepAuto.Value;
            int porog = (int)porog_txtbx.Value;
            if (AutoFilter)
            {
                /// Copying data from bitmap to array
                Bitmap bmp = new Bitmap(PictureViewer.Image);
                int width = bmp.Width,
                height = bmp.Height;
                byte[,,] res = new byte[3, height, width];
                BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb);
                try
                {
                    byte* curpos;
                    fixed (byte* _res = res)
                    {
                        byte* _r = _res, _g = _res + width * height, _b = _res + 2 * width * height;
                        for (int h = 0; h < height; h++)
                        {
                            curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                            for (int w = 0; w < width; w++)
                            {
                                *_b = *(curpos++); ++_b;
                                *_g = *(curpos++); ++_g;
                                *_r = *(curpos++); ++_r;
                            }
                        }
                    }
                }
                finally
                {
                    bmp.UnlockBits(bd);
                }
                byte[,,] work_res = new byte[3, height, width];
                work_res = res;
                int Prec = 3; // how much for precious when porog <25
                do
                {
                    do
                    {
                        for (int y = 1; y < width - 1; y++)
                        {
                            for (int x = 1; x < height - 1; x++)
                            {
                                int R = 0, G = 0, B = 0;
                                bool R11 = false, G11 = false, B11 = false;

                                int R_por = res[0, x, y];
                                int G_por = res[1, x, y];
                                int B_por = res[2, x, y];

                                for (int j = 0; j < 3; j++)
                                {
                                    for (int k = 0; k < 3; k++)
                                    {

                                        if ((j != 1) || (k != 1))
                                        {

                                            if (Math.Abs(res[0, x - 1 + j, y - 1 + k] - R_por) < porog) R11 = true;
                                            if (Math.Abs(res[1, x - 1 + j, y - 1 + k] - G_por) < porog) G11 = true;
                                            if (Math.Abs(res[2, x - 1 + j, y - 1 + k] - B_por) < porog) B11 = true;

                                            R += res[0, x - 1 + j, y - 1 + k];
                                            G += res[1, x - 1 + j, y - 1 + k];
                                            B += res[2, x - 1 + j, y - 1 + k];
                                        }
                                    }
                                }

                                R /= 8;
                                G /= 8;
                                B /= 8;

                                if ((R11) && (G11) && (B11))
                                {
                                    R = R_por;
                                    G = G_por;
                                    B = B_por;
                                }
                                work_res[0, x, y] = (byte)R;
                                work_res[1, x, y] = (byte)G;
                                work_res[2, x, y] = (byte)B;
//                                bmp.SetPixel(y, x, Color.FromArgb(255, R, G, B));
                            }
                        }
                        Prec++;
                    } while (Prec < 5);

                    if (AutoFilter)
                    {
                        //                        Thread trd = new Thread(delegate () { refresh_image(res); } );
                        //                       trd.Start();
//                        refresh_image(res);
                        porog -= step;
                        porog_txtbx.Value = porog;
                        porog_txtbx.Refresh();
                        res = work_res;
                    }

                    if (porog < (step))
                    {
                        chk_AutoFilter.Checked = false;
                        chk_AutoFilter.Refresh();
                        AutoFilter = false;
                        refresh_image(res);
                    }
                    if (porog < 25) Prec = 0;
                } while (AutoFilter);
            }
            else
            {
                Bitmap FilterImage = new Bitmap(PictureViewer.Image);
                Bitmap WorkImage = new Bitmap(PictureViewer.Image);
                PictureViewer.Image = FilterImage;
                int[,] R_matrix = new int[3, 3];
                int[,] G_matrix = new int[3, 3];
                int[,] B_matrix = new int[3, 3];
                for (int x = 1; x < WorkImage.Size.Width - 1; x++)
                {
                    for (int y = 1; y < WorkImage.Size.Height - 1; y++)
                    {
                        int R = 0, G = 0, B = 0;
                        bool R11 = false, G11 = false, B11 = false;
                        for (int j = 0; j < 3; j++)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                Color clr = WorkImage.GetPixel(x - 1 + j, y - 1 + k);
                                R_matrix[j, k] = clr.R;
                                G_matrix[j, k] = clr.G;
                                B_matrix[j, k] = clr.B;
                            }
                        }
                        for (int j = 0; j < 3; j++)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                if ((j != 1) || (k != 1))
                                {
                                    if (Math.Abs(R_matrix[j, k] - R_matrix[1, 1]) < porog) R11 = true;
                                    if (Math.Abs(G_matrix[j, k] - G_matrix[1, 1]) < porog) G11 = true;
                                    if (Math.Abs(B_matrix[j, k] - B_matrix[1, 1]) < porog) B11 = true;
                                }
                            }
                        }

                        if ((R11) && (G11) && (B11))
                        {
                            R = R_matrix[1, 1];
                            G = G_matrix[1, 1];
                            B = B_matrix[1, 1];
                        }
                        else
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    if ((j != 1) || (k != 1))
                                    {
                                        R += R_matrix[j, k];
                                        G += G_matrix[j, k];
                                        B += B_matrix[j, k];
                                    }
                                }
                            }
                            R /= 8;
                            G /= 8;
                            B /= 8;
                        }
                        FilterImage.SetPixel(x, y, Color.FromArgb(255, R, G, B));
                    }

                    if ((x > 3) && (x < (FilterImage.Size.Width - 3)) && ((x / (AutoFilter ? 30 : 5)) == ((double)x / (AutoFilter ? 30 : 5))))
                    {
                        int xx = x - 2;
                        for (int yy = 0; yy < FilterImage.Size.Height; yy++)
                        {
                            Color tmp_pixel = FilterImage.GetPixel(xx, yy);
                            FilterImage.SetPixel(xx, yy, Color.FromArgb(255, 255 - tmp_pixel.R, 255 - tmp_pixel.G, 255 - tmp_pixel.B));
                        }
                        PictureViewer.Refresh();
                        for (int yy = 0; yy < FilterImage.Size.Height; yy++)
                        {
                            Color tmp_pixel = FilterImage.GetPixel(xx, yy);
                            FilterImage.SetPixel(xx, yy, Color.FromArgb(255, 255 - tmp_pixel.R, 255 - tmp_pixel.G, 255 - tmp_pixel.B));
                        }
                    }
                }
                PictureViewer.Refresh();
                if (AutoFilter)
                {
                    porog -= step;
                    porog_txtbx.Value = porog;
                    porog_txtbx.Refresh();
                }
                if (porog < (step)) AutoFilter = false;
            }
        }

        public void refresh_image(byte [,,] matrix)
        {
            Bitmap bmp = new Bitmap(PictureViewer.Image);
            int heigh = bmp.Size.Height;
            int wight = bmp.Size.Width;
            for (int x = 0; x < wight; x++)
            {
                for (int y = 0; y < heigh; y++)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(255, (byte) matrix[0, y, x], (byte)matrix[1, y, x], (byte) matrix[2, y, x]));
                }
            }
 //           Invoke((MethodInvoker)delegate ()
  //          {
                PictureViewer.Image = bmp;
                PictureViewer.Refresh();
            //         });
            matrix = null;
            bmp = null;
        }

        /// <summary>
        /// To grayscale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bttn_Grey_Click(object sender, EventArgs e)
        {
            Bitmap GreyImage = new Bitmap(PictureViewer.Image);
            Bitmap WorkImage = new Bitmap(PictureViewer.Image);

            for (int x = 0; x < WorkImage.Size.Width; x++)
            {
                for (int y = 0; y < WorkImage.Size.Height; y++)
                {
                    int C = 0;
                    Color clr = WorkImage.GetPixel(x, y);
                    C += clr.R;
                    C += clr.G;
                    C += clr.B;
                    C /= 3;
                    GreyImage.SetPixel(x, y, Color.FromArgb(255, C, C, C));
                }
            }
            PictureViewer.Image = GreyImage;
        }

        private void bttn_save_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            FileName = openFileDialog.FileName;

            using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                OriginPicture = new Bitmap(fs);
            PictureViewer.Image = OriginPicture;
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            FileName = saveFileDialog.FileName;
            Bitmap bmp_out = (Bitmap)PictureViewer.Image;
            bmp_out.Save(FileName,ImageFormat.Bmp);
        }
    }
}
