using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace 动态生成控件
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.AutoScroll = true;//让panel显示滚动条           
        }
        public  string ImagePath = Application.StartupPath +"\\image\\";
        string[,] ImageFromPath = new string[300, 300];
        int Row = 3, Cloumn = 3;
        public string AmpImagePath = string.Empty;
        /// <summary>
        /// 自动生成图片控件并加载图片，同时给图片控件添加双击事件
        /// </summary>
        /// <param name="row">生成图片控件的行数</param>
        /// <param name="cloumn">生成图片控件的列数</param>
        public void PictureControl(int row, int cloumn)
        {
            try
            {
               int ImageBoxCount = 0;
               panel1.Controls.Clear();
               for (int i = 0; i < row; i++)
                    for (int j = 0; j < cloumn; j++)
                    {
                        PictureBox p = new PictureBox();
                        p.Name = ImageBoxCount.ToString();
                        p.Size = new System.Drawing.Size(108, 216);//长宽比例是1.25:1
                        p.Top = i * 226;
                        p.Left = j * 118;
                        p.MouseDoubleClick += M;//鼠标双击事件
                        ImageBoxCount++;
                        p.BackColor = Color.Gray;
                        p.SizeMode = PictureBoxSizeMode.Zoom;
                        p.BorderStyle = BorderStyle.FixedSingle;
                        if (IsFileInUse(ImageFromPath[i, j]) == false)
                        {                           
                            Stream s = File.Open(ImageFromPath[i, j], FileMode.Open);
                            p.Image = Image.FromStream(s);
                            s.Close();
                            s.Dispose();
                        }
                        else
                        {
                            //p.Image = Image.FromFile(ImageFromPath[i, j]);
                        }
                        p.BackColor = Color.Gray;
                        this.panel1.Controls.Add(p);
                    }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void M(object sender, EventArgs e)
        {
            try
            {
                PictureBox p = sender as PictureBox;
                AmpImagePath = ImageFromPath[Convert.ToInt32(p.Name) / Cloumn,Convert.ToInt32(p.Name) % Cloumn];
                Stream s = File.Open(AmpImagePath, FileMode.Open);
                pictureBox1.Image = Image.FromStream(s);
                s.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Row = int.Parse(richTextBox1.Text);
            Cloumn = int.Parse(richTextBox2.Text);
            for (int i = 0; i < Row; i++)
                for (int j = 0; j < Cloumn; j++)
                {
                    ImageFromPath[i, j] = ImagePath + (i * Cloumn + j + 1).ToString() + ".jpg"; //保存每张图的路径
                    //Console.WriteLine(ImagePath + (i * 3 + j + 1).ToString() + ".jpg");
                }
            PictureControl(Row, Cloumn);
        }

        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                inUse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用
        }


    }
}
