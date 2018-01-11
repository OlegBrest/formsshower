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
using System.Windows.Forms;

namespace formsshower
{
    public partial class Form1 : Form
    {
        String FileName = "";
        Bitmap OriginPicture; // original bitmap
        Random rnd1 = new Random();
        DataTable dataTable;
        byte[] dataToDisk;
        float[] contrast_resulter;
        /*
        struct BitmapFileHeader
        {
            uint bfType;
            uint bfSize;
            uint bfReserved1;
            uint bfReserved2;
            uint bfOffBits;
        }

        struct BitmapInfoHeader
        {
            int biSize;
            long biWidth;
            long biHeight;
            Int16 biPlanes;
            Int16 biBitCount;
            uint biCompression;
            uint biSizeImage;
            long biXpelsPerMeter;
            long biYpelsPerMeter;
            uint biClrUsed;
            uint biClrImportant;
        }

        struct RGBQUAD
        {
            byte rgbBlue;
            byte rgbGreen;
            byte rgbRed;
            byte rgbReserved;
        }

        struct BitmapInfo
        {
            BitmapInfoHeader bmiHeader;
            RGBQUAD bmiColors;
        }
        */

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
                int stop_auto = Convert.ToInt32(Stop_StepAuto.Value);
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
                        Thread trd = new Thread(delegate () { refresh_image(res); });
                        trd.Start();

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
                    }
                    if (porog < 25) Prec = 0;
                    if (porog < stop_auto)
                    {
                        chk_AutoFilter.Checked = false;
                        chk_AutoFilter.Refresh();
                        AutoFilter = false;
                    }
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


                    // рисовалка линии обработки
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

        /// <summary>
        /// Array to BMP
        /// </summary>
        /// <param name="matrix"></param>
        public void refresh_image(byte[,,] matrix)
        {
            Bitmap bmp = new Bitmap(PictureViewer.Image);
            int heigh = bmp.Size.Height;
            int wight = bmp.Size.Width;
            for (int x = 0; x < wight; x++)
            {
                for (int y = 0; y < heigh; y++)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(255, (byte)matrix[0, y, x], (byte)matrix[1, y, x], (byte)matrix[2, y, x]));
                }
            }
            Invoke((MethodInvoker)delegate ()
            {
                PictureViewer.Image = bmp;
                PictureViewer.Refresh();
            });
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

            using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                OriginPicture = new Bitmap(fs);

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
                        bmp.Save(memory, ImageFormat.Bmp);
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
            for (int x = 1; x < WorkImage.Size.Width - 1; x++)
            {
                for (int y = 1; y < WorkImage.Size.Height - 1; y++)
                {
                    int med_arr_pos = 0;
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            int arr_count = Convert.ToInt32(weigh_dgv.Rows[k].Cells[j].Value);
                            Color clr = WorkImage.GetPixel(x - 1 + j, y - 1 + k);

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
                        /*
                        dataTable.Rows.Add("11.комментарий", "", "16");
                        dataTable.Rows.Add("12.версия файла", "", "2");
                        dataTable.Rows.Add("13.тип сжатия", "", "1");
                        dataTable.Rows.Add("14.автор формата", "", "20");
                        dataTable.Rows.Add("15.название программы, создающей файлы данного формата", "", "8");
                        */
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
            //MessageBox.Show(sender.ToString());
            /* if (e.Control. == 1)
             {
                 string str_value = Headers_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                 //             if (Headers_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == "1.идентификатор типа файла") MessageBox.Show ("Это менять нельзя");
                 if ((str_value == "2.размер файла в байтах") || (str_value == "4.размер растра в байтах") || (str_value == "6.ширина изображения в пикселях")
                     || (str_value == "7.высота изображения в пикселях") || (str_value == "7.высота изображения в пикселях") || (str_value == "8.размер изображения в пикселях")
                     || (str_value == "10.количество различных цветов на изображении"))*/
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

        private void Median_filter_bttn_Click_1(object sender, EventArgs e)
        {
            Bitmap FilterImage = new Bitmap(PictureViewer.Image);
            Bitmap WorkImage = new Bitmap(PictureViewer.Image);
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
                    for (int j = 0; j < X_lengh; j++)
                    {
                        for (int k = 0; k < Y_lengh; k++)
                        {
                            Color clr = WorkImage.GetPixel(x - ((X_lengh + 1) / 2) + j, y - ((Y_lengh + 1) / 2) + k);
                            median_arr_R[med_arr_pos] = clr.R;
                            median_arr_G[med_arr_pos] = clr.G;
                            median_arr_B[med_arr_pos] = clr.B;
                            med_arr_pos++;
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
        }

        private void Sobel_bttn_Click(object sender, EventArgs e)
        {
            Bitmap FilterImage = new Bitmap(PictureViewer.Image);
            Bitmap WorkImage = new Bitmap(PictureViewer.Image);
            PictureViewer.Image = FilterImage;
            contrast_resulter = new float[WorkImage.Width * WorkImage.Height];
            int cur_pos = 0;
            for (int x = 1; x < WorkImage.Size.Width - 1; x++)
            {
                for (int y = 1; y < WorkImage.Size.Height - 1; y++)
                {
                    Color clr;
                    float X = 0;
                    float temp1 = 0;
                    float temp2 = 0;
                    float Y = 0;
                    clr = WorkImage.GetPixel(x - 1 , y - 1);
                    temp1 += clr.GetBrightness();
                    clr = WorkImage.GetPixel(x - 1, y);
                    temp1 += (2 * clr.GetBrightness());
                    clr = WorkImage.GetPixel(x - 1, y + 1);
                    temp1 += clr.GetBrightness();

                    clr = WorkImage.GetPixel(x + 1, y - 1);
                    temp2 += clr.GetBrightness();
                    clr = WorkImage.GetPixel(x + 1, y);
                    temp2 += (2 * clr.GetBrightness());
                    clr = WorkImage.GetPixel(x + 1, y + 1);
                    temp2 += clr.GetBrightness();

                    X = temp1 - temp2;

                    temp1 = 0;
                    temp2 = 0;

                    clr = WorkImage.GetPixel(x - 1, y - 1);
                    temp1 += clr.GetBrightness();
                    clr = WorkImage.GetPixel(x , y - 1);
                    temp1 += (2 * clr.GetBrightness());
                    clr = WorkImage.GetPixel(x + 1, y - 1);
                    temp1 += clr.GetBrightness();

                    clr = WorkImage.GetPixel(x - 1, y + 1);
                    temp2 += clr.GetBrightness();
                    clr = WorkImage.GetPixel(x , y + 1);
                    temp2 += (2 * clr.GetBrightness());
                    clr = WorkImage.GetPixel(x + 1, y + 1);
                    temp2 += clr.GetBrightness();

                    Y = temp1 - temp2;

                    float result = 0;

                    if (qudr_sobel_radio.Checked) result = (float) Math.Sqrt(X * X + Y * Y);
                    if (modul_sobel_radio.Checked) result = (float)Math.Abs(X) + Math.Abs(Y);
                    contrast_resulter[cur_pos] = result;
                    int res_int = (int) (result * (float)Sobel_mnoj.Value);
                    if (res_int > 255) res_int = 255;
                    FilterImage.SetPixel(x, y, Color.FromArgb(255, res_int, res_int, res_int));
                    cur_pos++;
                }
                if (x % 10 == 0) PictureViewer.Refresh();
            }
            PictureViewer.Refresh();
        }

        private void Sobel_mnoj_ValueChanged(object sender, EventArgs e)
        {
            Bitmap FilterImage = new Bitmap(PictureViewer.Image);
            Bitmap WorkImage = new Bitmap(PictureViewer.Image);
            PictureViewer.Image = FilterImage;
            int cur_pos = 0;
            for (int x = 1; x < WorkImage.Size.Width - 1; x++)
            {
                for (int y = 1; y < WorkImage.Size.Height - 1; y++)
                {
                    float result = 0;
                    result = contrast_resulter[cur_pos];
                    int res_int = (int)(result * (float)Sobel_mnoj.Value);
                    if (res_int > 255) res_int = 255;
                    FilterImage.SetPixel(x, y, Color.FromArgb(255, res_int, res_int, res_int));
                    cur_pos++;
                }
                if (x % 10 == 0) PictureViewer.Refresh();
            }
            PictureViewer.Refresh();
        }
    }
}
