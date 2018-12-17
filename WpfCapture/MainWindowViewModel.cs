using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Imaging;

namespace WpfCapture
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public string ProcessName { set; get; } = string.Empty;
        public string WindowName { set; get; } = string.Empty;
        public bool UseOnlyTargetWindow { set; get; }

        private readonly static PropertyChangedEventArgs imageChangedEventArgs = new PropertyChangedEventArgs(nameof(Image));
        private BitmapSource _Image;
        public BitmapSource Image
        {
            set
            {
                if (this._Image == value) return;

                this._Image = value;
                PropertyChanged?.Invoke(this, imageChangedEventArgs);
            }
            get => _Image;
        }

        private readonly static PropertyChangedEventArgs captureTimeTextChangedEventArgs = new PropertyChangedEventArgs(nameof(CaptureTimeText));
        private string _CaptureTimeText;
        public string CaptureTimeText
        {
            set
            {
                if (this._CaptureTimeText == value) return;

                this._CaptureTimeText = value;
                PropertyChanged?.Invoke(this, captureTimeTextChangedEventArgs);
            }
            get => _CaptureTimeText;
        }

        public RelayCommand DrawingCaptureCommand { get; }
        public RelayCommand Win32CaptureCommand { get; }

        private void Capture(CaptureMethod method, bool onlyTargetWindow = false)
        {
            var sw = new Stopwatch();
            var target = this.GetTargetWindow();
            sw.Start();
            var bitmap = target?.GetClientBitmap(method, onlyTargetWindow);
            sw.Stop();
            if (bitmap != null)
            {
                this.Image = bitmap;
            }
            this.CaptureTimeText = $"キャプチャにかかった時間: {sw.ElapsedMilliseconds} ms";
        }

        private WindowProcessHandle GetTargetWindow()
            => WindowProcessHandle.GetWindows()
                .FirstOrDefault(w =>
                w.ProcessName.IndexOf(this.ProcessName, StringComparison.OrdinalIgnoreCase) >= 0
                && w.GetWindowName().IndexOf(this.WindowName, StringComparison.OrdinalIgnoreCase) >= 0);

        public MainWindowViewModel()
        {
            this.DrawingCaptureCommand = new RelayCommand(() => this.Capture(CaptureMethod.Drawing));
            this.Win32CaptureCommand = new RelayCommand(() => this.Capture(CaptureMethod.Win32, this.UseOnlyTargetWindow));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
