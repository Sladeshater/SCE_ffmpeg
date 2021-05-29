using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Screencast_eitor_ffmpeg
{
    public partial class Form1 : Form
    {
        string ffmpegLocation = @"C:\Users\velon\Desktop\FFWORKSPACE\ffmpeg\bin\";
        string workFolder = @"C:\Users\velon\Desktop\FFWORKSPACE\";
        private void Fprobe(string filename)
        {
            filename = "\"" + filename + "\"";
            File.WriteAllText(workFolder + "fpp.cmd", "\"" + ffmpegLocation + "ffprobe.exe" + "\" " + filename + " -hide_banner > " + "\"" + workFolder + "fpt.txt" + "\"" + " 2>&1");
            Process fppRun = new Process();
            fppRun.StartInfo.FileName = workFolder + "fpp.cmd";
            fppRun.StartInfo.UseShellExecute = false;
            fppRun.StartInfo.CreateNoWindow = true;
            fppRun.Start();
            Thread.Sleep(1000);
            File.Delete(workFolder + "fpp.cmd");
        }
        private void Thumbnails(string filename, int seconds)
        {
            filename = "\"" + filename + "\"";
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            File.WriteAllText(workFolder + "th" + seconds.ToString() + ".cmd",
                "\"" + ffmpegLocation + "ffmpeg.exe" + "\" -y -i " + filename +
                " -vframes 1 -an -s 416x234 -ss " + t.ToString() +
                " \"" + workFolder + "th" + seconds.ToString() + ".png" + "\"");
            Process fppRun = new Process();
            fppRun.StartInfo.FileName = workFolder + "th" + seconds.ToString() + ".cmd";
            fppRun.StartInfo.UseShellExecute = false;
            fppRun.StartInfo.CreateNoWindow = true;
            fppRun.Start();
            Thread.Sleep(3000);
            //File.Delete(workFolder + "th" + seconds.ToString() + ".cmd");
        }
        private string[] FPParse()
        {
            string fileText = File.ReadAllText(workFolder + "fpt.txt");
            string[] fileLines = fileText.Split('\n')[3].Split(',')[0].Split(':');
            string[] timeString = new string[3];
            timeString[0] = string.Concat(fileLines[1].Where(c => !char.IsWhiteSpace(c)));
            timeString[1] = string.Concat(fileLines[2].Where(c => !char.IsWhiteSpace(c)));
            timeString[2] = string.Concat(fileLines[3].Where(c => !char.IsWhiteSpace(c)));
            File.Delete(workFolder + "fpt.txt");
            return timeString;
        }
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Video files(*.mkv)|*.mkv|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName; 
            textBox1.Text = filename;
            Fprobe(filename);
            string[] timeString = FPParse();
            int[] timeInt = new int[3] { int.Parse(timeString[0]), int.Parse(timeString[1]), (int)double.Parse(timeString[2]) };
            int overalSeconds = timeInt[2] + 60 * timeInt[1] + 3600 * timeInt[0];
            trackBar1.Maximum = overalSeconds;
            trackBar2.Maximum = overalSeconds;
            EH.Text = timeString[0];
            EM.Text = timeString[1];
            ES.Text = timeInt[2].ToString();
            trackBar2.Value = trackBar2.Maximum;
            int[] ThSec = new int[overalSeconds + 1];
            //for (int i = 0; i < overalSeconds; i ++)
            //{
            //    ThSec[i] = i;
            //}
            //foreach (int Sec in ThSec)
            //{
            //    Thumbnails(filename, Sec);
            //}

            MessageBox.Show("Файл открыт");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            File.WriteAllText(filename, textBox1.Text);
            MessageBox.Show("Файл сохранен");
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            TimeSpan t = TimeSpan.FromSeconds(trackBar1.Value);
            SH.Text = t.Hours.ToString();
            SM.Text = t.Minutes.ToString();
            SS.Text = t.Seconds.ToString();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            TimeSpan t = TimeSpan.FromSeconds(trackBar2.Value);
            EH.Text = t.Hours.ToString();
            EM.Text = t.Minutes.ToString();
            ES.Text = t.Seconds.ToString();
        }
    }
}
