using OpenCL.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Environment = System.Environment;

namespace formsshower
{
    public partial class Form1 : Form
    {
        String FileName = "";
        Bitmap OriginPicture; // original bitmap
        Random rnd1 = new Random();
        DataTable dataTable;
        byte[] dataToDisk;
        float[,] contrast_resulter;
        private Context _context;
        private Device _device;
        private Kernel kernel;
        private Kernel kernel2;

        public Form1()
        {
            InitializeComponent();
            dataTable = new DataTable();
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Values");
            dataTable.Columns.Add("Size");
            dataTable.Rows.Add("1.идентификатор типа файла", "BM", "2");
            dataTable.Rows.Add("2.размер файла в байтах", "", "4");
            dataTable.Rows.Add("3.размер заголовка в байтах", "", "2");
            dataTable.Rows.Add("4.размер растра в байтах", "", "4");
            dataTable.Rows.Add("5.смещение растровых данных от начала файла в байтах", "", "2");
            dataTable.Rows.Add("6.ширина изображения в пикселях", "", "4");
            dataTable.Rows.Add("7.высота изображения в пикселях", "", "4");
            dataTable.Rows.Add("8.размер изображения в пикселях", "", "4");
            dataTable.Rows.Add("9.глубина цвета", "", "1");
            dataTable.Rows.Add("10.количество различных цветов на изображении", "", "4");
            dataTable.Rows.Add("11.комментарий", "", "16");
            dataTable.Rows.Add("12.версия файла", "", "2");
            dataTable.Rows.Add("13.тип сжатия", "", "1");
            dataTable.Rows.Add("14.автор формата", "", "20");
            dataTable.Rows.Add("15.название программы, создающей файлы данного формата", "", "8");
            //this.SetupOCL();
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

        private void CheckErr(ErrorCode err, string name)
        {
            if (err != ErrorCode.Success)
            {
                MessageBox.Show("ERROR: " + name + " (" + err.ToString() + ")");
            }
        }

        private void ContextNotify(string errInfo, byte[] data, IntPtr cb, IntPtr userData)
        {
            MessageBox.Show("OpenCL Notification: " + errInfo);
        }

        private void SetupOCL()
        {
            ErrorCode error;
            Platform[] platforms = Cl.GetPlatformIDs(out error);
            List<Device> devicesList = new List<Device>();
            CheckErr(error, "Cl.GetPlatformIDs");

            foreach (Platform platform in platforms)
            {
                string platformName = Cl.GetPlatformInfo(platform, PlatformInfo.Name, out error).ToString();
                Console.WriteLine("Platform: " + platformName);
                CheckErr(error, "Cl.GetPlatformInfo");

                //We will be looking only for GPU devices
                foreach (Device device in Cl.GetDeviceIDs(platform, DeviceType.All, out error))
                {
                    CheckErr(error, "Cl.GetDeviceIDs");
                    MessageBox.Show("Device: " + device.ToString());
                    devicesList.Add(device);
                }
            }
            if (devicesList.Count <= 0)
            {
                MessageBox.Show("No devices found.");
                return;
            }
            _device = devicesList[0];
            if (Cl.GetDeviceInfo(_device, DeviceInfo.ImageSupport, out error).CastTo<Bool>() == Bool.False)
            {
                MessageBox.Show("No image support.");
                return;
            }

            _context = Cl.CreateContext(null, 1, new[] { _device }, ContextNotify, IntPtr.Zero, out error); //Second parameter is amount of devices
            CheckErr(error, "Cl.CreateContext");

            string programSource =
                @"
__kernel void AnisoDiff2D
                (
                    __global uchar * res,
                    int x,
                    int y,
                    int height, 
                    int width,
                    int porog,
                    __global int out
                )
                {
                 int retval =0;
                 int iJob = get_global_id(0);

            //Check boundary conditions
            if (iJob >= 3*height*width) return; 
            int R = 0, G = 0, B = 0;
            bool R11 = false, G11 = false, B11 = false;

            int R_por = res[x * 3 + y * 3 * width];
            int G_por = res[1 + x * 3 + y * 3 * width];
            int B_por = res[2 + x * 3 + y * 3 * width];

            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {

                    if ((j != 1) || (k != 1))
                    {
                        int corr = (x - 1 + j) * 3 + (y - 1 + k) * 3 * width;
                        if (abs(res[corr] - R_por) < porog) R11 = true;
                        if (abs(res[1 + corr] - G_por) < porog) G11 = true;
                        if (abs(res[2 + corr] - B_por) < porog) B11 = true;

                        R += res[corr];
                        G += res[1 + corr];
                        B += res[2 + corr];
                    }
                }
            }
            
            R = R >> 3;
            G = G >> 3;
            B = B >> 3;

            if ((R11) && (G11) && (B11))
            {
                R = R_por;
                G = G_por;
                B = B_por;
            }
            retval = 255; retval=retval<<8;
            retval += R;retval=retval<<8;
            retval += G;retval=retval<<8;
            retval += B;
            out[iJob] = retval;
            }
";
            programSource = programSource.Replace(Environment.NewLine, "");
            using (OpenCL.Net.Program program = Cl.CreateProgramWithSource(_context, 1, new[] { programSource }, null, out error))
            {
                CheckErr(error, "Cl.CreateProgramWithSource");

                //Compile kernel source
                error = Cl.BuildProgram(program, 1, new[] { _device }, string.Empty, null, IntPtr.Zero);
                CheckErr(error, "Cl.BuildProgram");

                //Check for any compilation errors
                if
                (
                    Cl.GetProgramBuildInfo
                    (
                        program,
                        _device,
                        ProgramBuildInfo.Status,
                        out error
                    ).CastTo<BuildStatus>() != BuildStatus.Success
                )
                {
                    CheckErr(error, "Cl.GetProgramBuildInfo");
                    Console.WriteLine("Cl.GetProgramBuildInfo != Success");
                    Console.WriteLine(Cl.GetProgramBuildInfo(program, _device, ProgramBuildInfo.Log, out error));
                    return;
                }

                //Create the required kernel (entry function)
                kernel = Cl.CreateKernel(program, "GPUAntiNoise", out error);
                kernel2 = Cl.CreateKernel(program, "GPUAntiNoise", out error);
                CheckErr(error, "Cl.CreateKernel");
            }
        }



        /// <summary>
        /// Random noise adding 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private unsafe void bttn_Noise_Click(object sender, EventArgs e)
        {
            int total = this.OriginPicture.Size.Width * this.OriginPicture.Size.Height;
            double chanse = ((double)chanse_txtbx.Value);

            Bitmap NoisePicture = new Bitmap(PictureViewer.Image);

            const int bytesPerPixel = 4;
            byte[] pict_bytes = pic2arr(NoisePicture, bytesPerPixel);
            int tst = 0;
            rnd1 = new Random();
            ParallelLoopResult parr_res =
            Parallel.For(0, total, (i, loopState) =>
            {
                if ((((tst / (double)total)) * 100) < chanse)
                {

                    double next_rnd = rnd1.Next(0, 100);
                    if (next_rnd <= chanse)
                    {
                        byte R = (byte)rnd1.Next(0, 255);
                        byte G = (byte)rnd1.Next(0, 255);
                        byte B = (byte)rnd1.Next(0, 255);
                        int nxt = i;// rnd1.Next(0, total);
                                    //int nxt = i;
                        pict_bytes[nxt * bytesPerPixel] = R;
                        pict_bytes[nxt * bytesPerPixel + 1] = G;
                        pict_bytes[nxt * bytesPerPixel + 2] = B;
                        pict_bytes[nxt * bytesPerPixel + 3] = 255;
                        tst++;
                        if (tst % 100 == 0) rnd1 = new Random();
                    }
                }
            });
            if (parr_res.IsCompleted)
            {
                arr2pic(NoisePicture, bytesPerPixel, pict_bytes);
                PictureViewer.Image = NoisePicture;
            }
        }

        private byte[] pic2arr(Bitmap NP, int bytesPerPixel)
        {
            BitmapData bits = null;
            Rectangle rect = new Rectangle(0, 0, NP.Width, NP.Height);
            byte[] pict_bytes = new byte[NP.Height * NP.Width * bytesPerPixel];
            int cur_pic_bt = 0;
            try
            {
                bits = NP.LockBits(rect, ImageLockMode.ReadWrite, NP.PixelFormat);
                unsafe
                {
                    // указатель на Pixel не имеет смысла, лэйаут структур
                    // в C# не определён
                    // используем указатель на байт
                    byte* start = (byte*)bits.Scan0;
                    // цикл должен быть до H
                    for (int y = 0; y < NP.Height; y++)
                    {
                        // указатель на начало строки
                        // поскольку Stride в байтах, мы вычисляем верно
                        // а вот арифметика с Pixel* была бы здесь неверна
                        byte* row = start + y * bits.Stride;
                        for (int x = 0; x < NP.Width; x++)
                        {
                            // указатель на текущий пиксель
                            // опять-таки, арифметика с Pixel* была бы неверна
                            byte* pixel = row + x * bytesPerPixel;
                            for (int d = 0; d < bytesPerPixel; d++)
                            {
                                pict_bytes[cur_pic_bt] = pixel[d];
                                cur_pic_bt++;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (bits != null)
                    NP.UnlockBits(bits);
            }
            return pict_bytes;
        }

        private unsafe Bitmap arr2pic(Bitmap NP, int bytesPerPixel, byte[] pict_bytes)
        {
            BitmapData bits = null;
            Rectangle rect = new Rectangle(0, 0, NP.Width, NP.Height);
            int cur_pic_bt = 0;
            try
            {
                bits = NP.LockBits(rect, ImageLockMode.ReadWrite, NP.PixelFormat);

                {
                    // указатель на Pixel не имеет смысла, лэйаут структур
                    // в C# не определён
                    // используем указатель на байт
                    byte* start = (byte*)bits.Scan0;
                    // цикл должен быть до H
                    for (int y = 0; y < NP.Height; y++)
                    {
                        // указатель на начало строки
                        // поскольку Stride в байтах, мы вычисляем верно
                        // а вот арифметика с Pixel* была бы здесь неверна
                        byte* row = start + y * bits.Stride;
                        for (int x = 0; x < NP.Width; x++)
                        {
                            // указатель на текущий пиксель
                            // опять-таки, арифметика с Pixel* была бы неверна
                            byte* pixel = row + x * bytesPerPixel;
                            for (int d = 0; d < bytesPerPixel; d++)
                            {
                                pixel[d] = pict_bytes[cur_pic_bt];
                                cur_pic_bt++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (bits != null)
                    NP.UnlockBits(bits);
            }
            return NP;

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
                Stopwatch sw = new Stopwatch();
                sw.Start();
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
                byte[] arr1D = this.Arr3Dto1D(res, height, width);
                int Prec = 3; // how much for precious when porog <25
                int stop_auto = Convert.ToInt32(Stop_StepAuto.Value);
                do
                {
                    do
                    {
                        arr1D = this.Arr3Dto1D(res, height, width);
                        int sz = arr1D.Length * sizeof(byte);
                        fixed (byte* _arr1D = arr1D)
                        {
                            Parallel.For(1, height - 1, y =>
                            {
                                for (int x = 1; x < (width - 1); x++)
                                {
                                    Color result = this.AutoAntiNoise(arr1D, x, y, height, width, porog);
                                    /* int intPtrSize = 0;
                                     intPtrSize = Marshal.SizeOf(typeof(IntPtr));
                                     ErrorCode error;

                                     IMem inp_buff = Cl.CreateBuffer(_context, MemFlags.CopyHostPtr, sz, out error);
                                     IMem x_buff = Cl.CreateBuffer(_context, MemFlags.ReadOnly, sizeof(Int32), out error);
                                     IMem y_buff = Cl.CreateBuffer(_context, MemFlags.ReadOnly, sizeof(Int32), out error);
                                     IMem height_buff = Cl.CreateBuffer(_context, MemFlags.ReadOnly, sizeof(Int32), out error);
                                     IMem width_buff = Cl.CreateBuffer(_context, MemFlags.ReadOnly, sizeof(Int32), out error);
                                     IMem porog_buff = Cl.CreateBuffer(_context, MemFlags.ReadOnly, sizeof(Int32), out error);
                                     IMem out_buff = Cl.CreateBuffer(_context, MemFlags.WriteOnly, sizeof(Int32), out error);
                                     CheckErr(error, "424.Cl.CreateBuffer");

                                     uint iArg = 0;
                                     error = Cl.SetKernelArg(kernel, iArg++, (IntPtr)intPtrSize, inp_buff);
                                     error = Cl.SetKernelArg(kernel, iArg++, (IntPtr)intPtrSize, x_buff);
                                     error = Cl.SetKernelArg(kernel, iArg++, (IntPtr)intPtrSize, y_buff);
                                     error = Cl.SetKernelArg(kernel, iArg++, (IntPtr)intPtrSize, height_buff);
                                     error = Cl.SetKernelArg(kernel, iArg++, (IntPtr)intPtrSize, width_buff);
                                     error = Cl.SetKernelArg(kernel, iArg++, (IntPtr)intPtrSize, porog_buff);
                                     error = Cl.SetKernelArg(kernel, iArg++, (IntPtr)intPtrSize, out_buff);
                                     CheckErr(error, "434.Cl.SetKernelArg");

                                     CommandQueue cmdQueue = Cl.CreateCommandQueue(_context, _device, (CommandQueueProperties)0, out error);
                                     CheckErr(error, "Cl.CreateCommandQueue");


                                     Cl.ReleaseMemObject(inp_buff);
                                     Cl.ReleaseMemObject(x_buff);
                                     Cl.ReleaseMemObject(y_buff);
                                     Cl.ReleaseMemObject(height_buff);
                                     Cl.ReleaseMemObject(width_buff);
                                     Cl.ReleaseMemObject(porog_buff);
                                     Cl.ReleaseMemObject(out_buff);
                                     CheckErr(error, "447.Cl.ReleaseMemObject");
                                     */
                                    work_res[0, y, x] = result.R;
                                    work_res[1, y, x] = result.G;
                                    work_res[2, y, x] = result.B;
                                    //                                bmp.SetPixel(y, x, Color.FromArgb(255, R, G, B));
                                }
                            });
                        }
                        Prec++;
                    } while (Prec < 5);

                    if (AutoFilter)
                    {
                        try
                        {
                            Thread trd = new Thread(delegate () { refresh_image(res); });
                            trd.Start();

                            porog -= step;
                            porog_txtbx.Value = porog;
                            porog_txtbx.Refresh();
                            res = work_res;
                        }
                        catch
                        {
                        }
                    }

                    if (porog < (step))
                    {
                        chk_AutoFilter.Checked = false;
                        chk_AutoFilter.Refresh();
                        AutoFilter = false;
                    }
                    if (porog < 100) Prec = 0;
                    if (porog < stop_auto)
                    {
                        chk_AutoFilter.Checked = false;
                        chk_AutoFilter.Refresh();
                        AutoFilter = false;
                    }
                } while (AutoFilter);
                sw.Stop();
                this.sw_label.Text = "Worked by " + sw.ElapsedMilliseconds.ToString() + " msec.";
            }
            else
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Bitmap FilterImage = new Bitmap(PictureViewer.Image);
                Bitmap WorkImage = new Bitmap(PictureViewer.Image);
                int WIW = WorkImage.Size.Width;
                int WIH = WorkImage.Size.Height;
                PictureViewer.Image = FilterImage;

                byte[,,] WI_bt_arr = BitmapToByteRgb(WorkImage);
                byte[,,] FI_bt_arr = new byte[3, FilterImage.Height, FilterImage.Width];

                for (int x = 1; x < WIW - 1; x++)
                {
                    Parallel.For(1, WIH - 1, y =>
                   {
                       int R = 0, G = 0, B = 0;
                       bool R11 = false, G11 = false, B11 = false;
                       byte[,] R_matrix = new byte[3, 3];
                       byte[,] G_matrix = new byte[3, 3];
                       byte[,] B_matrix = new byte[3, 3];
                       for (int j = 0; j < 3; j++)
                       {
                           for (int k = 0; k < 3; k++)
                           {
                               //Color clr = WI_clr_array[x - 1 + j, y - 1 + k];
                               R_matrix[k, j] = WI_bt_arr[0, y - 1 + k, x - 1 + j];// clr.R;
                               G_matrix[k, j] = WI_bt_arr[1, y - 1 + k, x - 1 + j];// clr.G;
                               B_matrix[k, j] = WI_bt_arr[2, y - 1 + k, x - 1 + j];// clr.B;
                           }
                       }

                       for (int j = 0; j < 3; j++)
                       {
                           for (int k = 0; k < 3; k++)
                           {
                               if ((j != 1) || (k != 1))
                               {
                                   if (Math.Abs(R_matrix[k, j] - R_matrix[1, 1]) < porog) R11 = true;
                                   if (Math.Abs(G_matrix[k, j] - G_matrix[1, 1]) < porog) G11 = true;
                                   if (Math.Abs(B_matrix[k, j] - B_matrix[1, 1]) < porog) B11 = true;
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
                                       R += R_matrix[k, j];
                                       G += G_matrix[k, j];
                                       B += B_matrix[k, j];
                                   }
                               }
                           }
                           /*  R /= 8;
                             G /= 8;
                             B /= 8;*/
                           R = R >> 3;
                           G = G >> 3;
                           B = B >> 3;
                       }
                       FI_bt_arr[0, y, x] = (byte)R;
                       FI_bt_arr[1, y, x] = (byte)G;
                       FI_bt_arr[2, y, x] = (byte)B;
                   });

                    /*
                    // рисовалка линии обработки
                    if ((x > 5) && (x < (FilterImage.Size.Width - 3)) && ((x / (AutoFilter ? 30 : 5)) == ((double)x / (AutoFilter ? 30 : 5))))
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
                    }*/
                }

                FilterImage = ByteToBitmapRgb(FilterImage, FI_bt_arr);
                PictureViewer.Refresh();
                if (AutoFilter)
                {
                    porog -= step;
                    porog_txtbx.Value = porog;
                    porog_txtbx.Refresh();
                }
                if (porog < (step)) AutoFilter = false;
                sw.Stop();
                this.sw_label.Text = "Worked by " + sw.ElapsedMilliseconds.ToString() + " msec.";
            }
        }

        public byte[] Arr3Dto1D(byte[,,] source, int height, int width)
        {
            byte[] result = new byte[3 * height * width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        result[z + x * 3 + y * 3 * width] = source[z, y, x];
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Antinoise for auto
        /// </summary>
        /// <param name="res"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="porog"></param>
        /// <returns></returns>
        public unsafe Color AutoAntiNoise(byte[] res, int x, int y, int height, int width, int porog)
        {
            Color retval = new Color();
            int R = 0, G = 0, B = 0;
            bool R11 = false, G11 = false, B11 = false;

            int R_por = res[x * 3 + y * 3 * width];
            int G_por = res[1 + x * 3 + y * 3 * width];
            int B_por = res[2 + x * 3 + y * 3 * width];

            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if ((j != 1) || (k != 1))
                    {
                        int corr = (x - 1 + j) * 3 + (y - 1 + k) * 3 * width;
                        if (Math.Abs(res[corr] - R_por) < porog) R11 = true;
                        if (Math.Abs(res[1 + corr] - G_por) < porog) G11 = true;
                        if (Math.Abs(res[2 + corr] - B_por) < porog) B11 = true;

                        R += res[corr];
                        G += res[1 + corr];
                        B += res[2 + corr];
                    }
                }
            }
            R = R >> 3;
            G = G >> 3;
            B = B >> 3;

            if ((R11) && (G11) && (B11))
            {
                R = R_por;
                G = G_por;
                B = B_por;
            }
            retval = Color.FromArgb(255, R, G, B);
            return retval;
        }


        public static unsafe byte[,,] BitmapToByteRgb(Bitmap bmp)
        {
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
            return res;
        }

        public static unsafe Bitmap ByteToBitmapRgb(Bitmap bmp, byte[,,] ress)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[,,] res = ress;
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite,
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
                            *(curpos++) = *_b; ++_b;
                            *(curpos++) = *_g; ++_g;
                            *(curpos++) = *_r; ++_r;
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return bmp;
        }

        /// <summary>
        /// Array to BMP
        /// </summary>
        /// <param name="matrix"></param>
        public void refresh_image(byte[,,] matrix)
        {
            try
            {
                Bitmap bmp = new Bitmap(PictureViewer.Image);
                bmp = ByteToBitmapRgb(bmp, matrix);
                if (PictureViewer.InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate ()
                    {
                        PictureViewer.Image = bmp;
                        PictureViewer.Refresh();
                    });
                }
                else
                {
                    PictureViewer.Image = bmp;
                    PictureViewer.Refresh();
                }
            }
            catch
            { }
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
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    OriginPicture = new Bitmap(fs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            PictureViewer.Image = OriginPicture;
            Bitmap editedPicture = new Bitmap(PictureViewer.Image);
            PictureViewer.Image = editedPicture;
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            FileName = saveFileDialog.FileName;

            try
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        Bitmap bmp = (Bitmap)this.PictureViewer.Image;
                        bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void filtr_Menu_item_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem ddi_source = sender as ToolStripMenuItem;
            foreach (ToolStripDropDownItem ddi in this.FilterToolStripMenuItem.DropDownItems)
            {
                ToolStripMenuItem tsmi = ddi as ToolStripMenuItem;
                if (ddi_source.Name == ddi.Name)
                {
                    tsmi.Checked = true;
                    this.Text = "Обработка фото. " + tsmi.Text;
                }
                else
                {
                    tsmi.Checked = false;
                }
            }
            if (ddi_source.Name == LF_filtr_Menu_item.Name)
            {
                Filter_tabs.SelectedTab = LF_filter_tab;
            }

            if (ddi_source.Name == Median_vzvesh_Menu_Item.Name)
            {
                Filter_tabs.SelectedTab = Median_vzvesh_filter_tab;
            }

            if (ddi_source.Name == Median_Menu_Item.Name)
            {
                Filter_tabs.SelectedTab = Median_filter_tab;
            }
        }

        // медианная взвешенная фильтрация
        private void median_filter_bttn_Click(object sender, EventArgs e)
        {
            Bitmap FilterImage = new Bitmap(PictureViewer.Image);
            Bitmap WorkImage = new Bitmap(PictureViewer.Image);
            byte[,,] WorkImageArray;
            WorkImageArray = BitmapToByteRgb(WorkImage);
            PictureViewer.Image = FilterImage;
            int array_size = 0;
            foreach (DataGridViewRow dgvr in weigh_dgv.Rows)
            {
                for (int i = 0; i < 3; i++) array_size += Convert.ToInt32(dgvr.Cells[i].Value);
            }

            byte[] median_arr_R = new byte[array_size];
            byte[] median_arr_G = new byte[array_size];
            byte[] median_arr_B = new byte[array_size];

            int median_center = array_size / 2 + 1;
            int WIWidth = WorkImage.Size.Width;
            int WIHeight = WorkImage.Size.Height;
            for (int x = 1; x < WIWidth - 1; x++)
            {
                for (int y = 1; y < WIHeight - 1; y++)
                {
                    int med_arr_pos = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            int arr_count = Convert.ToInt32(weigh_dgv.Rows[k].Cells[j].Value);
                            //Color clr = WorkImage.GetPixel(x - 1 + j, y - 1 + k);
                            Color clr = GetColorFromArr(WorkImageArray, x - 1 + j, y - 1 + k);

                            for (int i = 0; i < arr_count; i++)
                            {
                                median_arr_R[med_arr_pos] = clr.R;
                                median_arr_G[med_arr_pos] = clr.G;
                                median_arr_B[med_arr_pos] = clr.B;
                                med_arr_pos++;
                            }

                        }
                    }

                    Array.Sort(median_arr_R);
                    Array.Sort(median_arr_G);
                    Array.Sort(median_arr_B);


                    FilterImage.SetPixel(x, y, Color.FromArgb(255, median_arr_R[median_center], median_arr_G[median_center], median_arr_B[median_center]));

                }

                // рисовалка линии обработки

                if ((x > 3) && (x < (FilterImage.Size.Width - 3)))
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
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++) weigh_dgv.Rows.Add(1, 1, 1);

        }

        // ограничим только цифры
        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                { e.Handled = true; }
            }
        }

        private void weigh_dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (TextBox)e.Control;
            tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
        }

        private void header_bttn_Click(object sender, EventArgs e)
        {
            byte[] reads;
            if (FileName != "")
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                reads = new byte[54];
                fs.Read(reads, 0, 54);
                fs.Close();
                Headers_dgv.AllowUserToAddRows = false;
                Headers_dgv.AllowUserToDeleteRows = false;
                Headers_dgv.ReadOnly = true;
                int rows_count = Headers_dgv.Rows.Count;
                for (int i = 0; i < rows_count; i++) Headers_dgv.Rows.RemoveAt(0);

                DataGridViewTextBoxColumn dgtbx = new DataGridViewTextBoxColumn();
                dgtbx.Name = Headers_dgv.Columns[0].Name;
                dgtbx.HeaderText = Headers_dgv.Columns[0].HeaderText;
                Headers_dgv.Columns.RemoveAt(0);
                Headers_dgv.Columns.Insert(0, dgtbx);
                Headers_dgv.Rows.Add("bfType", Convert.ToChar(reads[0]).ToString() + Convert.ToChar(reads[1]).ToString());
                Headers_dgv.Rows.Add("bfSize", BitConverter.ToInt32(reads, 2));
                Headers_dgv.Rows.Add("bfReserved1", BitConverter.ToInt16(reads, 6));
                Headers_dgv.Rows.Add("bfReserved2", BitConverter.ToInt16(reads, 8));
                Headers_dgv.Rows.Add("bfOffBits", BitConverter.ToInt32(reads, 10));
                Headers_dgv.Rows.Add("biSize", BitConverter.ToInt32(reads, 14));
                Headers_dgv.Rows.Add("biWidth", BitConverter.ToInt32(reads, 18));
                Headers_dgv.Rows.Add("biHeight", BitConverter.ToInt32(reads, 22));
                Headers_dgv.Rows.Add("biPlanes", BitConverter.ToInt16(reads, 26));
                Headers_dgv.Rows.Add("biBitCount", BitConverter.ToInt16(reads, 28));
                Headers_dgv.Rows.Add("biCompression", BitConverter.ToInt32(reads, 30));
                Headers_dgv.Rows.Add("biSizeImage", BitConverter.ToInt32(reads, 34));
                Headers_dgv.Rows.Add("biXPelsPerMeter", BitConverter.ToInt32(reads, 38));
                Headers_dgv.Rows.Add("biYPelsPerMeter", BitConverter.ToInt32(reads, 42));
                Headers_dgv.Rows.Add("biClrUsed", BitConverter.ToInt32(reads, 46));
                Headers_dgv.Rows.Add("biClrImportant", BitConverter.ToInt32(reads, 50));
            }
            else
            {
                Headers_dgv.AllowUserToAddRows = false;
                Headers_dgv.AllowUserToDeleteRows = false;
                Headers_dgv.ReadOnly = true;
                int rows_count = Headers_dgv.Rows.Count;
                for (int i = 0; i < rows_count; i++) Headers_dgv.Rows.RemoveAt(0);

                DataGridViewTextBoxColumn dgtbx = new DataGridViewTextBoxColumn();
                dgtbx.Name = Headers_dgv.Columns[0].Name;
                dgtbx.HeaderText = Headers_dgv.Columns[0].HeaderText;
                Headers_dgv.Columns.RemoveAt(0);
                Headers_dgv.Columns.Insert(0, dgtbx);
                Headers_dgv.Rows.Add("bfType", "");
                Headers_dgv.Rows.Add("bfSize", "");
                Headers_dgv.Rows.Add("bfReserved1", "");
                Headers_dgv.Rows.Add("bfReserved2", "");
                Headers_dgv.Rows.Add("bfOffBits", "");
                Headers_dgv.Rows.Add("biSize", "");
                Headers_dgv.Rows.Add("biWidth", "");
                Headers_dgv.Rows.Add("biHeight", "");
                Headers_dgv.Rows.Add("biPlanes", "");
                Headers_dgv.Rows.Add("biBitCount", "");
                Headers_dgv.Rows.Add("biCompression", "");
                Headers_dgv.Rows.Add("biSizeImage", "");
                Headers_dgv.Rows.Add("biXPelsPerMeter", "");
                Headers_dgv.Rows.Add("biYPelsPerMeter", "");
                Headers_dgv.Rows.Add("biClrUsed", "");
                Headers_dgv.Rows.Add("biClrImportant", "");
            }

        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int head_count = Headers_dgv.Rows.Count - 1;
            Bitmap bmp = new Bitmap(PictureViewer.Image);
            int bpp = 24;//Image.GetPixelFormatSize(bmp.PixelFormat);
            int rastrSize = (bmp.Height * bmp.Width) * bpp / 8;
            int headerSize = getHeaderSize();
            int sizeOfData = headerSize + rastrSize;
            int imgSize = bmp.Height * bmp.Width;
            int uniqclrs = getUniqClrCount(imgSize, bmp);
            dataToDisk = new byte[sizeOfData];
            int dataPos = 0;
            for (int i = 0; i < head_count; i++)
            {
                DataGridViewComboBoxCell dgvcbc = Headers_dgv.Rows[i].Cells[0] as DataGridViewComboBoxCell;
                string str_val = dgvcbc.FormattedValue.ToString();


                if (str_val == "2.размер файла в байтах")
                {
                    Headers_dgv[1, i].Value = headerSize + rastrSize;
                }

                if (str_val == "1.идентификатор типа файла")
                {
                    //Headers_dgv[1, i].Value = "BM";
                }

                if (str_val == "3.размер заголовка в байтах")
                {
                    Headers_dgv[1, i].Value = headerSize;
                }

                if (str_val == "4.размер растра в байтах")
                {
                    Headers_dgv[1, i].Value = rastrSize;
                }

                if (str_val == "5.смещение растровых данных от начала файла в байтах")
                {
                    Headers_dgv[1, i].Value = headerSize + 1;
                }

                if (str_val == "6.ширина изображения в пикселях")
                {
                    Headers_dgv[1, i].Value = bmp.Width;
                }


                if (str_val == "7.высота изображения в пикселях")
                {
                    Headers_dgv[1, i].Value = bmp.Height;
                }

                if (str_val == "8.размер изображения в пикселях")
                {
                    Headers_dgv[1, i].Value = imgSize;
                }

                if (str_val == "9.глубина цвета")
                {
                    Headers_dgv[1, i].Value = bpp;
                }

                if (str_val == "10.количество различных цветов на изображении")
                {
                    Headers_dgv[1, i].Value = uniqclrs;
                }


                string indx = Headers_dgv[0, i].Value.ToString();
                DataRow[] res = dataTable.Select("Name = '" + indx + "'");
                int byteSize = Convert.ToInt32(res[0]["Size"]);
                if (byteSize == 1) dataToDisk[dataPos] = Convert.ToByte(Headers_dgv[1, i].Value.ToString());
                if (byteSize == 2)
                {
                    Int16 sht16 = 0;
                    if (Int16.TryParse(Headers_dgv[1, i].Value.ToString(), out sht16))
                    {
                        byte[] dataInt16 = BitConverter.GetBytes(sht16);
                        for (int pos = 0; pos < byteSize; pos++)
                        {
                            dataToDisk[dataPos + pos] = dataInt16[pos];
                        }
                    }
                    else
                    {
                        string text = Headers_dgv[1, i].Value.ToString();
                        for (int pos = 0; pos < byteSize; pos++)
                        {
                            dataToDisk[dataPos + pos] = (byte)text.ElementAt(pos);
                        }
                    }
                }
                if (byteSize == 4)
                {
                    byte[] dataInt32 = BitConverter.GetBytes((Int32)Headers_dgv[1, i].Value);
                    for (int pos = 0; pos < byteSize; pos++)
                    {
                        dataToDisk[dataPos + pos] = dataInt32[pos];
                    }
                }
                if (byteSize > 4)
                {
                    string text = Headers_dgv[1, i].Value.ToString();
                    for (int pos = 0; pos < byteSize; pos++)
                    {
                        dataToDisk[dataPos + pos] = (byte)text.ElementAt(pos);
                    }
                }
                dataPos += byteSize;
            }

            Color clr;

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    clr = bmp.GetPixel(x, y);
                    dataToDisk[dataPos] = clr.B;
                    dataPos++;
                    dataToDisk[dataPos] = clr.G;
                    dataPos++;
                    dataToDisk[dataPos] = clr.R;
                    dataPos++;
                    /*                    dataToDisk[dataPos] = clr.A;
                                        dataPos++;*/
                }
            }
            saveFileAsDialog.ShowDialog();

        }

        private void custom_header_bttn_Click(object sender, EventArgs e)
        {
            int rows_count = Headers_dgv.Rows.Count;
            if (((rows_count > 1) && (Headers_dgv.AllowUserToAddRows == true)) || (Headers_dgv.AllowUserToAddRows == false))
            {
                for (int i = 0; i < rows_count; i++) Headers_dgv.Rows.RemoveAt(0);
                Headers_dgv.AllowUserToAddRows = true;
                Headers_dgv.AllowUserToDeleteRows = true;
                DataGridViewComboBoxColumn dgcbx = new DataGridViewComboBoxColumn();
                dgcbx.DataSource = dataTable;
                dgcbx.DataPropertyName = "Name";
                dgcbx.DisplayMember = "Name";
                dgcbx.ValueMember = "Name";
                dgcbx.Name = Headers_dgv.Columns[0].Name;
                dgcbx.HeaderText = Headers_dgv.Columns[0].HeaderText;
                Headers_dgv.Columns.RemoveAt(0);
                Headers_dgv.Columns.Insert(0, dgcbx);
                Headers_dgv.ReadOnly = false;
            }
        }

        private void Headers_dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 1) && (e.RowIndex >= 0))
            {
                string str_value = Headers_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (Headers_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "1.идентификатор типа файла") MessageBox.Show("Это менять нельзя");
                if ((str_value == "2.размер файла в байтах") || (str_value == "4.размер растра в байтах") || (str_value == "6.ширина изображения в пикселях")
                    || (str_value == "7.высота изображения в пикселях") || (str_value == "7.высота изображения в пикселях") || (str_value == "8.размер изображения в пикселях")
                    || (str_value == "10.количество различных цветов на изображении"))
                {

                }
            }

            if ((e.ColumnIndex == 0) && (e.RowIndex >= 0))
            {
                try
                {
                    Bitmap bmp = new Bitmap(PictureViewer.Image);
                    int rows_count = Headers_dgv.Rows.Count;
                    int bpp = Image.GetPixelFormatSize(bmp.PixelFormat);
                    int rastrSize = (bmp.Height * bmp.Width) * bpp / 8;
                    int headerSize = getHeaderSize();
                    int pictureSize = bmp.Height * bmp.Width;
                    int uniqclrs = getUniqClrCount(pictureSize, bmp);
                    for (int i = 0; i < (rows_count - 1); i++)
                    {
                        DataGridViewComboBoxCell dgvcbc = Headers_dgv.Rows[i].Cells[0] as DataGridViewComboBoxCell;
                        string str_val = dgvcbc.FormattedValue.ToString();


                        if (str_val == "2.размер файла в байтах")
                        {
                            Headers_dgv[1, i].Value = headerSize + rastrSize;
                        }

                        if (str_val == "1.идентификатор типа файла")
                        {
                            Headers_dgv[1, i].Value = "BM";
                        }

                        if (str_val == "3.размер заголовка в байтах")
                        {
                            Headers_dgv[1, i].Value = headerSize;
                        }

                        if (str_val == "4.размер растра в байтах")
                        {
                            Headers_dgv[1, i].Value = rastrSize;
                        }

                        if (str_val == "5.смещение растровых данных от начала файла в байтах")
                        {
                            Headers_dgv[1, i].Value = headerSize + 1;
                        }

                        if (str_val == "6.ширина изображения в пикселях")
                        {
                            Headers_dgv[1, i].Value = bmp.Width;
                        }


                        if (str_val == "7.высота изображения в пикселях")
                        {
                            Headers_dgv[1, i].Value = bmp.Height;
                        }

                        if (str_val == "8.размер изображения в пикселях")
                        {
                            Headers_dgv[1, i].Value = pictureSize;
                        }

                        if (str_val == "9.глубина цвета")
                        {
                            Headers_dgv[1, i].Value = bpp;
                        }

                        if (str_val == "10.количество различных цветов на изображении")
                        {
                            Headers_dgv[1, i].Value = uniqclrs;
                        }

                    }
                }
                catch
                {

                }
            }
        }

        private int getHeaderSize()
        {
            int result = 0;
            int rows_count = Headers_dgv.Rows.Count;
            for (int i = 0; i < (rows_count - 1); i++)
            {
                string indx = Headers_dgv[0, i].Value.ToString();
                DataRow[] res = dataTable.Select("Name = '" + indx + "'");
                result += Convert.ToInt32(res[0]["Size"]);
                Headers_dgv[2, i].Value = res[0]["Size"];
            }
            return result;
        }

        private int getUniqClrCount(int arraySize, Bitmap bmp)
        {
            int result = 0;
            int[] array = new int[arraySize];
            for (int a = 0; a < array.Length; a++) array[a] = 0;
            int arr_pos = 0;
            Color clr;

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    clr = bmp.GetPixel(x, y);
                    array[arr_pos] += clr.B;
                    array[arr_pos] = array[arr_pos] << 8;
                    array[arr_pos] += clr.G;
                    array[arr_pos] = array[arr_pos] << 8;
                    array[arr_pos] += clr.R;
                    array[arr_pos] = array[arr_pos] << 8;
                    array[arr_pos] += clr.A;
                    arr_pos++;
                }
            }
            result = array.GroupBy(p => p).Count();
            return result;
        }

        private void Headers_dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }

        private void saveFileAsDialog_FileOk(object sender, CancelEventArgs e)
        {
            FileName = saveFileAsDialog.FileName;
            FileStream str = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            str.Write(dataToDisk, 0, dataToDisk.Length);
            str.Close();
        }

        private void menuStrip_Click(object sender, EventArgs e)
        {
            Headers_dgv.CurrentCell = null;
        }

        /// <summary>
        /// Медианный фильтр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Median_filter_bttn_Click_1(object sender, EventArgs e)
        {
            Bitmap FilterImage = new Bitmap(PictureViewer.Image);
            Bitmap WorkImage = new Bitmap(PictureViewer.Image);
            byte[,,] WorkImageArray;
            WorkImageArray = BitmapToByteRgb(WorkImage);
            PictureViewer.Image = FilterImage;
            int X_lengh = (int)Columns_nums.Value;
            int Y_lengh = (int)Rows_nums.Value;
            int array_size = X_lengh * Y_lengh;


            byte[] median_arr_R = new byte[array_size];
            byte[] median_arr_G = new byte[array_size];
            byte[] median_arr_B = new byte[array_size];

            int median_center = array_size / 2 + 1;
            for (int x = ((X_lengh + 1) / 2); x < WorkImage.Size.Width - ((X_lengh + 1) / 2); x++)
            {
                for (int y = ((Y_lengh + 1) / 2); y < WorkImage.Size.Height - ((Y_lengh + 1) / 2); y++)
                {
                    int med_arr_pos = 0;
                    Parallel.For(0, X_lengh, j =>
                   {
                       for (int k = 0; k < Y_lengh; k++)
                       {
                           //Color clr = WorkImage.GetPixel(x - ((X_lengh + 1) / 2) + j, y - ((Y_lengh + 1) / 2) + k);
                           Color clr = GetColorFromArr(WorkImageArray, x - ((X_lengh + 1) / 2) + j, y - ((Y_lengh + 1) / 2) + k);
                           median_arr_R[med_arr_pos] = clr.R;
                           median_arr_G[med_arr_pos] = clr.G;
                           median_arr_B[med_arr_pos] = clr.B;
                           med_arr_pos++;
                       }
                   });

                    Array.Sort(median_arr_R);
                    Array.Sort(median_arr_G);
                    Array.Sort(median_arr_B);

                    FilterImage.SetPixel(x, y, Color.FromArgb(255, median_arr_R[median_center], median_arr_G[median_center], median_arr_B[median_center]));
                }

                // рисовалка линии обработки
                if ((x > 3) && (x < (FilterImage.Size.Width - 3)))
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
        }

        private void LoadSpecificToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileAsDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                FileName = openFileAsDialog.FileName;
                byte[] reads;

                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    int fslength = (int)fs.Length;
                    reads = new byte[fslength];
                    fs.Read(reads, 0, fslength);
                }
                int pic_heigh = 0;
                int pic_widght = 0;
                int header_size = getHeaderSize();
                int pic_size = reads.Length - header_size;
                foreach (DataGridViewRow dgvr in Headers_dgv.Rows) dgvr.Cells[1].Value = "";
                int rows_count = Headers_dgv.Rows.Count;
                int curr_pos = 0;
                for (int i = 0; i < rows_count; i++)
                {
                    DataGridViewComboBoxCell dgvcbc = Headers_dgv.Rows[i].Cells[0] as DataGridViewComboBoxCell;
                    string str_val = dgvcbc.FormattedValue.ToString();
                    int size_byte = Convert.ToInt32(Headers_dgv.Rows[i].Cells[2].Value);

                    if (str_val == "2.размер файла в байтах")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToInt32(reads, curr_pos);
                    }

                    if (str_val == "1.идентификатор типа файла")
                    {
                        Headers_dgv[1, i].Value = "" + (char)reads[curr_pos] + (char)reads[curr_pos + 1];
                    }

                    if (str_val == "3.размер заголовка в байтах")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToInt16(reads, curr_pos);
                    }

                    if (str_val == "4.размер растра в байтах")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToInt32(reads, curr_pos);
                    }

                    if (str_val == "5.смещение растровых данных от начала файла в байтах")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToInt16(reads, curr_pos);
                    }

                    if (str_val == "6.ширина изображения в пикселях")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToInt32(reads, curr_pos);
                        pic_widght = BitConverter.ToInt32(reads, curr_pos);
                    }


                    if (str_val == "7.высота изображения в пикселях")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToInt32(reads, curr_pos); ;
                        pic_heigh = BitConverter.ToInt32(reads, curr_pos);
                    }

                    if (str_val == "8.размер изображения в пикселях")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToInt32(reads, curr_pos);
                    }

                    if (str_val == "9.глубина цвета")
                    {
                        Headers_dgv[1, i].Value = reads[curr_pos];
                    }

                    if (str_val == "10.количество различных цветов на изображении")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToInt32(reads, curr_pos);
                    }

                    if (str_val == "11.комментарий")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToString(reads, curr_pos, size_byte);
                    }
                    if (str_val == "12.версия файла")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToString(reads, curr_pos, size_byte);
                    }

                    if (str_val == "13.тип сжатия")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToString(reads, curr_pos, size_byte);
                    }

                    if (str_val == "14.автор формата")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToString(reads, curr_pos, size_byte);
                    }

                    if (str_val == "15.название программы, создающей файлы данного формата")
                    {
                        Headers_dgv[1, i].Value = BitConverter.ToString(reads, curr_pos, size_byte);
                    }

                    curr_pos += size_byte;
                }

                if (pic_widght == 0) pic_widght = (pic_size / 3) / pic_heigh;
                if (pic_heigh == 0) pic_heigh = (pic_size / 3) / pic_heigh;

                Color clr;
                Bitmap bmp = new Bitmap(pic_widght, pic_heigh);
                PictureViewer.Image = bmp;
                for (int y = 0; y < pic_heigh; y++)
                {
                    for (int x = 0; x < pic_widght; x++)
                    {
                        clr = Color.FromArgb(255, reads[curr_pos + 2], reads[curr_pos + 1], reads[curr_pos]);
                        curr_pos += 3;
                        bmp.SetPixel(x, y, clr);
                    }
                }
            }
        }

        private void Sobel_contrast_menu_item_Click(object sender, EventArgs e)
        {
            Filter_tabs.SelectedTab = Sobel_kontrast_tab;
            this.Text = "Обработка фото. Контрастирование Собела";
        }

        // фильтр Собела (Контраст)
        private void Sobel_bttn_Click(object sender, EventArgs e)
        {
            Bitmap FilterImage = new Bitmap(PictureViewer.Image);
            Bitmap WorkImage = new Bitmap(PictureViewer.Image);
            int WIH = WorkImage.Size.Height;
            int WIW = WorkImage.Size.Width;
            PictureViewer.Image = FilterImage;
            byte[,,] WI_bt_arr = BitmapToByteRgb(WorkImage);
            byte[,,] FI_bt_arr = BitmapToByteRgb(FilterImage);
            this.contrast_resulter = new float[WorkImage.Width, WorkImage.Height];
            Parallel.For(1, WIW - 1, x =>
           {
               Parallel.For(1, WIH - 1, y =>
             {
                 Color clr;
                 float X = 0;
                 float temp1 = 0;
                 float temp2 = 0;
                 float Y = 0;
                 clr = GetColorFromArr(WI_bt_arr, x - 1, y - 1);
                 temp1 += clr.GetBrightness();
                 clr = GetColorFromArr(WI_bt_arr, x - 1, y);
                 temp1 += (2 * clr.GetBrightness());
                 clr = GetColorFromArr(WI_bt_arr, x - 1, y + 1);
                 temp1 += clr.GetBrightness();

                 clr = GetColorFromArr(WI_bt_arr, x + 1, y - 1);
                 temp2 += clr.GetBrightness();
                 clr = GetColorFromArr(WI_bt_arr, x + 1, y);
                 temp2 += (2 * clr.GetBrightness());
                 clr = GetColorFromArr(WI_bt_arr, x + 1, y + 1);
                 temp2 += clr.GetBrightness();

                 X = temp1 - temp2;

                 temp1 = 0;
                 temp2 = 0;

                 clr = GetColorFromArr(WI_bt_arr, x - 1, y - 1);
                 temp1 += clr.GetBrightness();
                 clr = GetColorFromArr(WI_bt_arr, x, y - 1);
                 temp1 += (2 * clr.GetBrightness());
                 clr = GetColorFromArr(WI_bt_arr, x + 1, y - 1);
                 temp1 += clr.GetBrightness();

                 clr = GetColorFromArr(WI_bt_arr, x - 1, y + 1);
                 temp2 += clr.GetBrightness();
                 clr = GetColorFromArr(WI_bt_arr, x, y + 1);
                 temp2 += (2 * clr.GetBrightness());
                 clr = GetColorFromArr(WI_bt_arr, x + 1, y + 1);
                 temp2 += clr.GetBrightness();

                 Y = temp1 - temp2;

                 float result = 0;

                 if (qudr_sobel_radio.Checked) result = (float)Math.Sqrt(X * X + Y * Y);
                 if (modul_sobel_radio.Checked) result = Math.Abs(X) + Math.Abs(Y);
                 this.contrast_resulter[x, y] = result;
                 int res_int = (int)(result * (float)Sobel_mnoj.Value);
                 if (res_int > 255) res_int = 255;
                 if (this.invert_chkbx.Checked) res_int = 255 - res_int;
                 FI_bt_arr[0, y, x] = FI_bt_arr[1, y, x] = FI_bt_arr[2, y, x] = (byte)res_int;
             });

               if (x % 100 == 0)
               {
                   try
                   {
                       ByteToBitmapRgb(FilterImage, FI_bt_arr);
                       PictureViewer.Refresh();
                   }
                   catch
                   { }
               }

           });
            try
            {
                FilterImage = ByteToBitmapRgb(FilterImage, FI_bt_arr);
                PictureViewer.Image = null;
                PictureViewer.Image = FilterImage;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private Color GetColorFromArr(byte[,,] array, int x, int y)
        {
            Color ret_val = Color.FromArgb(255, array[0, y, x], array[1, y, x], array[2, y, x]);
            return ret_val;
        }

        private void Sobel_mnoj_ValueChanged(object sender, EventArgs e)
        {
            if (this.contrast_resulter != null)
            {
                Bitmap FilterImage = new Bitmap(PictureViewer.Image);
                Bitmap WorkImage = new Bitmap(PictureViewer.Image);
                byte[,,] WI_bt_arr = BitmapToByteRgb(WorkImage);
                byte[,,] FI_bt_arr = BitmapToByteRgb(FilterImage);
                int WIH = WorkImage.Size.Height;
                int WIW = WorkImage.Size.Width;
                PictureViewer.Image = FilterImage;
                int painting = 0;
                ParallelLoopResult par_res_main = Parallel.For(1, WIW - 1, x =>
                {
                    ParallelLoopResult par_res = Parallel.For(1, WIH - 1, y =>
                    {
                        float result = 0;
                        result = this.contrast_resulter[x, y];
                        int res_int = (int)(result * (float)Sobel_mnoj.Value);
                        if (res_int > 255) res_int = 255;
                        if (this.invert_chkbx.Checked) res_int = 255 - res_int;
                        FI_bt_arr[0, y, x] = FI_bt_arr[1, y, x] = FI_bt_arr[2, y, x] = (byte)res_int;
                    });

                    if ((x % 100 == 0) && (par_res.IsCompleted) && (painting == 0))
                    {
                        painting++;
                        try
                        {
                            if (painting == 1) ByteToBitmapRgb(FilterImage, FI_bt_arr);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        painting--;
                    }

                });
                if (par_res_main.IsCompleted)
                {
                    try
                    {
                        FilterImage = ByteToBitmapRgb(FilterImage, FI_bt_arr);
                        this.PictureViewer.Image = FilterImage;
                        this.PictureViewer.Update();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void Uolis_contrast_menu_item_Click(object sender, EventArgs e)
        {
            Filter_tabs.SelectedTab = Uoles_kontrast_tab;
            this.Text = "Обработка фото. Контрастирование Уолиса";
        }

        //Фильтр Уолиса (Контраст)
        private void Uolis_start_bttn_Click(object sender, EventArgs e)
        {
            Bitmap FilterImage = new Bitmap(PictureViewer.Image);
            Bitmap WorkImage = new Bitmap(PictureViewer.Image);
            PictureViewer.Image = FilterImage;
            //this.contrast_resulter = new float[WorkImage.Width , WorkImage.Height];
            for (int x = 1; x < WorkImage.Size.Width - 1; x++)
            {
                for (int y = 1; y < WorkImage.Size.Height - 1; y++)
                {
                    Color clr;
                    float temp1 = 0;
                    float temp2 = 0;
                    clr = WorkImage.GetPixel(x - 1, y);
                    temp1 = clr.GetBrightness();
                    clr = WorkImage.GetPixel(x, y - 1);
                    temp1 *= (clr.GetBrightness());
                    clr = WorkImage.GetPixel(x + 1, y);
                    temp1 *= clr.GetBrightness();
                    clr = WorkImage.GetPixel(x, y + 1);
                    temp1 *= clr.GetBrightness();


                    clr = WorkImage.GetPixel(x, y);
                    temp2 = clr.GetBrightness();

                    double result = Math.Log(Math.Pow(temp2, 4) / temp1);
                    double porog = (double)this.Uolis_porog.Value;
                    int res_int = 255;
                    if (result < porog) res_int = 0;
                    //res_int = (int)result>255? 255: (int)result < 0 ? 0 : (int)result;
                    if (this.Uolis_invert_chkbx.Checked) res_int = 255 - res_int;
                    FilterImage.SetPixel(x, y, Color.FromArgb(255, res_int, res_int, res_int));
                }
                if (x % 10 == 0) PictureViewer.Refresh();
            }
            PictureViewer.Refresh();
        }
    }
}
