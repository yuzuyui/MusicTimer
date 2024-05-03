using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WaveOutEvent waveOut;
        private AudioFileReader audioFile;
        private float volume = 1.0f;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            S_Volume.Value = 0.8f;
            volume = 0.8f;
        }

        private void Btn_Pickup_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "ファイル選択ダイアログ";
                openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3|All Files (*.*)|*.*";
                openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;

                //ファイル選択ダイアログを開く
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Tb_MusicFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            if (Tb_MusicFilePath.Text == "" || File.Exists(Tb_MusicFilePath.Text) == false)
            {
                Lbl_Now.Content = "ファイルを選択してください。";
            }

            Lbl_Now.Content = "Start";
            if (waveOut != null)
            {
                // すでに再生中の場合は停止してリソースを解放
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    waveOut.Stop();
                    waveOut.Dispose();
                    audioFile.Dispose();
                }
            }

            //string filePath = System.Environment.CurrentDirectory;
            // 音楽ファイルを読み込む
            audioFile = new AudioFileReader(Tb_MusicFilePath.Text);

            // loopする
            LoopStream loop = new LoopStream(audioFile);

            // WaveOutEventを作成
            waveOut = new WaveOutEvent();

            // 音楽ファイルをWaveOutEventに設定
            waveOut.Init(loop);
            waveOut.Volume = volume;

            // 再生開始
            waveOut.Play();
        }
        private void Btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            Lbl_Now.Content = "Stop";
            // WaveOutEventが初期化されていて再生中の場合は停止
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Stop();
            }
        }

        private void S_Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            volume = (float)(S_Volume.Value / 100);
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Volume = volume;
            }
        }
    }
}
