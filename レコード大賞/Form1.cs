
using System;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Lame;
using System.Diagnostics;

namespace RecordingApp
{
    public partial class MainForm : Form
    {
        private WaveInEvent waveIn;
        private LameMP3FileWriter writer;
        private Stopwatch stopwatch;
        private SaveFileDialog saveFileDialog;

        public MainForm()
        {
            InitializeComponent();
        }

        private void startRecordingButton_Click(object sender, EventArgs e)
        {
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "MP3 Files|*.mp3";
            saveFileDialog.Title = "Save an MP3 File";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                waveIn = new WaveInEvent();
                waveIn.WaveFormat = new WaveFormat(44100, 1);
                waveIn.DataAvailable += OnDataAvailable;
                writer = new LameMP3FileWriter(saveFileDialog.FileName, waveIn.WaveFormat, LAMEPreset.VBR_90);
                waveIn.StartRecording();

                stopwatch = new Stopwatch();
                stopwatch.Start();
                timer1.Start();
            }
        }

        private void stopRecordingButton_Click(object sender, EventArgs e)
        {
            waveIn.StopRecording();
            writer.Dispose();
            waveIn.Dispose();
            stopwatch.Stop();
            timer1.Stop();
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopwatch.Elapsed;
            timeLabel.Text = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        }
    }
}
