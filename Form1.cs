//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//using OpenCvSharp;
//using OpenCvSharp.Extensions;

//namespace openCV
//{
//    public partial class Form1 : Form
//    {
//        public Form1()
//        {
//            InitializeComponent();
//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                OpenFileDialog dlg = new OpenFileDialog();
//                if (dlg.ShowDialog() == DialogResult.OK)
//                {
//                    Mat mat = Cv2.ImRead(dlg.FileName);

//                    if (pictureBox1.Image != null)
//                    {
//                        pictureBox1.Image.Dispose();
//                    }

//                    // Mat to Bitmap 1
//                    //Bitmap bitmap = new Bitmap(mat.ToMemoryStream());
//                    //pictureBox1.Image = bitmap;

//                    // Mat to Bitmap 2
//                    //Bitmap bitmap = (Bitmap)Image.FromStream(mat.ToMemoryStream());
//                    //pictureBox1.Image = bitmap;

//                    // Mat to Bitmap 3
//                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(mat.ToBytes());
//                    pictureBox1.Image = new Bitmap(memoryStream);
//                }
//            }
//            catch (Exception exc)
//            {
//                MessageBox.Show(exc.Message);
//            }
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
using System.Threading;
using OpenCvSharp;
using OpenCvSharp.Extensions;
 
namespace openCV
{
    public partial class Form1 : Form
    {
        bool isCameraOn;

        Thread thread;
        Mat mat;
        VideoCapture videoCapture;

        public Form1()
        {
            InitializeComponent();

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            button1.Text = "Start";
            isCameraOn = false;
        }

        private void CameraCallback()
        {
            mat = new Mat();
            videoCapture = new VideoCapture(0);

            if (!videoCapture.IsOpened())
            {
                Text = "Camera open failed!";
                return;
            }

            while (true)
            {
                videoCapture.Read(mat);

                if (!mat.Empty())
                {
                    pictureBox1.Image = BitmapConverter.ToBitmap(mat);

                    //System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(mat.ToBytes());
                    //pictureBox1.Image = new Bitmap(memoryStream);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isCameraOn == false)
            {
                thread = new Thread(new ThreadStart(CameraCallback));

                thread.Start();
                isCameraOn = true;
                button1.Text = "Stop";
            }
            else
            {
                if (videoCapture.IsOpened())
                {
                    thread.Abort();
                    videoCapture.Release();
                    mat.Release();
                }
                isCameraOn = false;
                button1.Text = "Start";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null && thread.IsAlive && videoCapture.IsOpened())
            {
                thread.Abort();
                videoCapture.Release();
                mat.Release();
            }
        }
    }
}