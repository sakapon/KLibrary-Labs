using System;
using System.Runtime.InteropServices;

namespace KLibrary.Labs.UI.Input
{
    static class NativeKeyboardMouse
    {
        public const uint MouseData_XBUTTON1 = 0x0001;
        public const uint MouseData_XBUTTON2 = 0x0002;
        public const uint MouseData_WHEEL_DELTA = 120;

        static readonly int SizeOf_INPUT = Marshal.SizeOf(typeof(INPUT));

        public static uint SendInput(INPUT[] inputs)
        {
            // SendInput function
            // http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
            return NativeMethods.SendInput((uint)inputs.Length, inputs, SizeOf_INPUT);
        }

        static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    struct INPUT
    {
        [FieldOffset(0)]
        public INPUT_TYPE type;
        [FieldOffset(4)]
        public MOUSEINPUT mi;
        [FieldOffset(4)]
        public KEYBDINPUT ki;
        [FieldOffset(4)]
        public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public MOUSEEVENTF dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public KEYEVENTF dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    enum INPUT_TYPE : uint
    {
        MOUSE = 0,
        KEYBOARD = 1,
        HARDWARE = 2,
    }

    [Flags]
    enum MOUSEEVENTF : uint
    {
        MOVE = 0x0001,
        LEFTDOWN = 0x0002,
        LEFTUP = 0x0004,
        RIGHTDOWN = 0x0008,
        RIGHTUP = 0x0010,
        MIDDLEDOWN = 0x0020,
        MIDDLEUP = 0x0040,
        XDOWN = 0x0080,
        XUP = 0x0100,
        WHEEL = 0x0800,
        HWHEEL = 0x01000,
        MOVE_NOCOALESCE = 0x2000,
        VIRTUALDESK = 0x4000,
        ABSOLUTE = 0x8000,
    }

    [Flags]
    enum KEYEVENTF : uint
    {
        //KEYDOWN = 0x0000,
        EXTENDEDKEY = 0x0001,
        KEYUP = 0x0002,
        UNICODE = 0x0004,
        SCANCODE = 0x0008,
    }
}
