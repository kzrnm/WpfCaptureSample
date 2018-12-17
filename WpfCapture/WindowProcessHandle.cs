using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using static WpfCapture.ImageConstants;
using static WpfCapture.Win32Api;

namespace WpfCapture
{
    public class WindowProcessHandle
    {
        public IntPtr Handle { get; private set; }
        public string GetWindowName()
        {
            int titleLength = GetWindowTextLength(this.Handle);
            if (0 < titleLength)
            {
                var sb = new StringBuilder(titleLength + 1);
                GetWindowText(this.Handle, sb, sb.Capacity);
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public string ProcessName { get; }

        public WindowProcessHandle(IntPtr handle)
        {
            this.Handle = handle;

            GetWindowThreadProcessId(handle, out var processId);
            using (var process = Process.GetProcessById(processId))
                ProcessName = process.ProcessName;
        }

        public bool IsActive => IsWindow(this.Handle);
        public override string ToString() => this.GetWindowName();

        public static IEnumerable<WindowProcessHandle> GetWindows()
        {
            bool EnumuWindowsProc(IntPtr hWnd, IntPtr lParam)
            {
                if (!IsWindowVisible(hWnd)) return true;

                var window = new WindowProcessHandle(hWnd);

                // 普通のアプリでWindowsNameが空のことはあまりないので除外する
                if (window.GetWindowName() == string.Empty)
                {
                    return true;
                }

                ((List<WindowProcessHandle>)((GCHandle)lParam).Target).Add(window);
                return true;
            }
            var list = new List<WindowProcessHandle>();
            var paramHandle = GCHandle.Alloc(list);
            EnumWindows(EnumuWindowsProc, (IntPtr)paramHandle);
            paramHandle.Free();
            list.Sort((wp1, wp2) => wp1.ProcessName.CompareTo(wp2.ProcessName));
            return list;
        }

        public BitmapSource GetClientBitmap(CaptureMethod method, bool onlyTargetWindow)
        {
            if (GetClientRect(this.Handle, out var rect) == 0) return null;

            var width = rect.right - rect.left;
            var height = rect.bottom - rect.top;

            if (width <= 0 || height <= 0) return null;

            var pt = new POINT { x = rect.left, y = rect.top };

            switch (method)
            {
                case CaptureMethod.Drawing:
                    return CaptureDrawing(pt, width, height);
                case CaptureMethod.Win32:
                    return CaptureWin32(pt, width, height, onlyTargetWindow);
                default:
                    throw new ArgumentException();
            }
        }

        private BitmapSource CaptureDrawing(POINT pt, int width, int height)
        {
            ClientToScreen(Handle, out pt);

            using (var img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (var g = Graphics.FromImage(img))
            {
                g.CopyFromScreen(pt.x, pt.y, 0, 0, img.Size);

                var image = Imaging.CreateBitmapSourceFromHBitmap(img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                image.Freeze();
                return image;
            }
        }

        private BitmapSource CaptureWin32(POINT pt, int width, int height, bool onlyTargetWindow)
        {
            IntPtr screenDC;
            IntPtr compatibleDC;
            IntPtr bmp;
            IntPtr oldBmp;

            var hdc = this.Handle;

            if (!onlyTargetWindow)
            {
                ClientToScreen(Handle, out pt);
                hdc = IntPtr.Zero;
            }

            screenDC = GetDC(hdc);
            try
            {
                compatibleDC = CreateCompatibleDC(screenDC);
                try
                {
                    bmp = CreateCompatibleBitmap(screenDC, width, height);
                    try
                    {
                        oldBmp = SelectObject(compatibleDC, bmp);
                        BitBlt(compatibleDC, 0, 0, width, height, screenDC, pt.x, pt.y, SRCPAINT);
                        SelectObject(compatibleDC, oldBmp);

                        var image = Imaging.CreateBitmapSourceFromHBitmap(bmp, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        image.Freeze();
                        return image;
                    }
                    finally
                    {
                        DeleteObject(bmp);
                    }
                }
                finally
                {
                    DeleteDC(compatibleDC);
                }
            }
            finally
            {
                ReleaseDC(hdc, screenDC);
            }
        }
    }
    public enum CaptureMethod
    {
        Drawing, Win32,
    }
}
