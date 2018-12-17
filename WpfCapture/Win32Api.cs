using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WpfCapture
{
    public static class Win32Api
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern int GetClientRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hwnd, out POINT lpPoint);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);


        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(int eventMin, int eventMax, IntPtr hmodWinEventProc, WinEventProc lpfnWinEventProc, int idProcess, int idThread, int dwflags);
        [DllImport("user32.dll")]
        public static extern int UnhookWinEvent(IntPtr hWinEventHook);
        public delegate void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);


        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);


        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);



        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public RGBQUAD bmiColors_1;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
    }

    public static class ImageConstants
    {
        public const int SRCCOPY = 0x00CC0020;
        public const int SRCPAINT = 0x00EE0086; /* dest = source OR dest           */
        public const int SRCAND = 0x008800C6; /* dest = source AND dest          */
        public const int SRCINVERT = 0x00660046; /* dest = source XOR dest          */
        public const int SRCERASE = 0x00440328; /* dest = source AND (NOT dest )   */
        public const int NOTSRCCOPY = 0x00330008; /* dest = (NOT source)             */
        public const int NOTSRCERASE = 0x001100A6; /* dest = (NOT src) AND (NOT dest) */
        public const int MERGECOPY = 0x00C000CA; /* dest = (source AND pattern)     */
        public const int MERGEPAINT = 0x00BB0226; /* dest = (NOT source) OR dest     */
        public const int PATCOPY = 0x00F00021; /* dest = pattern                  */
        public const int PATPAINT = 0x00FB0A09; /* dest = DPSnoo                   */
        public const int PATINVERT = 0x005A0049; /* dest = pattern XOR dest         */
        public const int DSTINVERT = 0x00550009; /* dest = (NOT dest)               */
        public const int BLACKNESS = 0x00000042; /* dest = BLACK                    */
        public const int WHITENESS = 0x00FF0062; /* dest = WHITE                    */
        public const int CAPTUREBLT = 0x40000000; /* Include layered windows */
    }

    public static class EventConstants
    {
        public const int EVENT_MIN = 0x00000001;
        public const int EVENT_SYSTEM_SOUND = 0x00000001;
        public const int EVENT_SYSTEM_ALERT = 0x00000002;
        public const int EVENT_SYSTEM_FOREGROUND = 0x00000003;
        public const int EVENT_SYSTEM_MENUSTART = 0x00000004;
        public const int EVENT_SYSTEM_MENUEND = 0x00000005;
        public const int EVENT_SYSTEM_MENUPOPUPSTART = 0x00000006;
        public const int EVENT_SYSTEM_MENUPOPUPEND = 0x00000007;
        public const int EVENT_SYSTEM_CAPTURESTART = 0x00000008;
        public const int EVENT_SYSTEM_CAPTUREEND = 0x00000009;
        public const int EVENT_SYSTEM_MOVESIZESTART = 0x0000000a;
        public const int EVENT_SYSTEM_MOVESIZEEND = 0x0000000b;
        public const int EVENT_SYSTEM_CONTEXTHELPSTART = 0x0000000c;
        public const int EVENT_SYSTEM_CONTEXTHELPEND = 0x0000000d;
        public const int EVENT_SYSTEM_DRAGDROPSTART = 0x0000000e;
        public const int EVENT_SYSTEM_DRAGDROPEND = 0x0000000f;
        public const int EVENT_SYSTEM_DIALOGSTART = 0x00000010;
        public const int EVENT_SYSTEM_DIALOGEND = 0x00000011;
        public const int EVENT_SYSTEM_SCROLLINGSTART = 0x00000012;
        public const int EVENT_SYSTEM_SCROLLINGEND = 0x00000013;
        public const int EVENT_SYSTEM_SWITCHSTART = 0x00000014;
        public const int EVENT_SYSTEM_SWITCHEND = 0x00000015;
        public const int EVENT_SYSTEM_MINIMIZESTART = 0x00000016;
        public const int EVENT_SYSTEM_MINIMIZEEND = 0x00000017;
        public const int EVENT_SYSTEM_DESKTOPSWITCH = 0x00000020;

        public const int EVENT_OBJECT_CREATE = 0x00008000;
        public const int EVENT_OBJECT_DESTROY = 0x00008001;
        public const int EVENT_OBJECT_SHOW = 0x00008002;
        public const int EVENT_OBJECT_HIDE = 0x00008003;
        public const int EVENT_OBJECT_REORDER = 0x00008004;
        public const int EVENT_OBJECT_FOCUS = 0x00008005;
        public const int EVENT_OBJECT_SELECTION = 0x00008006;
        public const int EVENT_OBJECT_SELECTIONADD = 0x00008007;
        public const int EVENT_OBJECT_SELECTIONREMOVE = 0x00008008;
        public const int EVENT_OBJECT_SELECTIONWITHIN = 0x00008009;
        public const int EVENT_OBJECT_STATECHANGE = 0x0000800a;
        public const int EVENT_OBJECT_LOCATIONCHANGE = 0x0000800b;
        public const int EVENT_OBJECT_NAMECHANGE = 0x0000800c;
        public const int EVENT_OBJECT_DESCRIPTIONCHANGE = 0x0000800d;
        public const int EVENT_OBJECT_VALUECHANGE = 0x0000800e;
        public const int EVENT_OBJECT_PARENTCHANGE = 0x0000800f;
        public const int EVENT_OBJECT_HELPCHANGE = 0x00008010;
        public const int EVENT_OBJECT_DEFACTIONCHANGE = 0x00008011;
        public const int EVENT_OBJECT_ACCELERATORCHANGE = 0x00008012;
        public const int EVENT_OBJECT_INVOKED = 0x00008013;
        public const int EVENT_OBJECT_TEXTSELECTIONCHANGED = 0x00008014;
        public const int EVENT_OBJECT_CONTENTSCROLLED = 0x00008015;

        public const int EVENT_CONSOLE_CARET = 0x00004001;
        public const int EVENT_CONSOLE_UPDATE_REGION = 0x00004002;
        public const int EVENT_CONSOLE_UPDATE_SIMPLE = 0x00004003;
        public const int EVENT_CONSOLE_UPDATE_SCROLL = 0x00004004;
        public const int EVENT_CONSOLE_LAYOUT = 0x00004005;
        public const int EVENT_CONSOLE_START_APPLICATION = 0x00004006;
        public const int EVENT_CONSOLE_END_APPLICATION = 0x00004007;

        public const int EVENT_MAX = 0x7fffffff;

        public const uint OBJID_WINDOW = 0x00000000;
        public const uint OBJID_SYSMENU = 0xFFFFFFFF;
        public const uint OBJID_TITLEBAR = 0xFFFFFFFE;
        public const uint OBJID_MENU = 0xFFFFFFFD;
        public const uint OBJID_CLIENT = 0xFFFFFFFC;
        public const uint OBJID_VSCROLL = 0xFFFFFFFB;
        public const uint OBJID_HSCROLL = 0xFFFFFFFA;
        public const uint OBJID_SIZEGRIP = 0xFFFFFFF9;
        public const uint OBJID_CARET = 0xFFFFFFF8;
        public const uint OBJID_CURSOR = 0xFFFFFFF7;
        public const uint OBJID_ALERT = 0xFFFFFFF6;
        public const uint OBJID_SOUND = 0xFFFFFFF5;

        public const int WINEVENT_INCONTEXT = 4;
        public const int WINEVENT_OUTOFCONTEXT = 0;
        public const int WINEVENT_SKIPOWNPROCESS = 2;
        public const int WINEVENT_SKIPOWNTHREAD = 1;
    }
}

