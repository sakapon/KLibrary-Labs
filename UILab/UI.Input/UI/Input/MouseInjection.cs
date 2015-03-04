using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KLibrary.Labs.UI.Input
{
    public static class MouseInjection
    {
        public const int WheelDeltaUnit = (int)NativeKeyboardMouse.MouseData_WHEEL_DELTA;

        static readonly Int32Rect AllScreensBounds = ScreenHelper.AllScreensBounds;

        public static void Move(Point point)
        {
            SendMouseInput(ToMouseInputForMove(point));
        }

        // Remarks: Depends on "Pointer Speed".
        public static void MoveDelta(Vector delta)
        {
            var mi = new MOUSEINPUT
            {
                dx = (int)delta.X,
                dy = (int)delta.Y,
                dwFlags = MOUSEEVENTF.MOVE,
            };
            SendMouseInput(mi);
        }

        public static void ButtonDown(MouseButtonType button)
        {
            SendMouseInput(ToMouseInputForButtonDown(button));
        }

        public static void ButtonUp(MouseButtonType button)
        {
            SendMouseInput(ToMouseInputForButtonUp(button));
        }

        public static void Click()
        {
            Click(MouseButtonType.Left);
        }

        public static void Click(Point point)
        {
            Click(point, MouseButtonType.Left);
        }

        public static void Click(MouseButtonType button)
        {
            var mi_down = ToMouseInputForButtonDown(button);
            var mi_up = ToMouseInputForButtonUp(button);
            NativeKeyboardMouse.SendInput(new[] { ToInput(mi_down), ToInput(mi_up) });
        }

        public static void Click(Point point, MouseButtonType button)
        {
            var mi_move = ToMouseInputForMove(point);
            var mi_down = ToMouseInputForButtonDown(button);
            var mi_up = ToMouseInputForButtonUp(button);
            NativeKeyboardMouse.SendInput(new[] { ToInput(mi_move), ToInput(mi_down), ToInput(mi_up) });
        }

        public static void Wheel(int delta)
        {
            var mi = new MOUSEINPUT
            {
                mouseData = (uint)delta,
                dwFlags = MOUSEEVENTF.WHEEL,
            };
            SendMouseInput(mi);
        }

        public static void WheelHorizontally(int delta)
        {
            var mi = new MOUSEINPUT
            {
                mouseData = (uint)delta,
                dwFlags = MOUSEEVENTF.HWHEEL,
            };
            SendMouseInput(mi);
        }

        public static void WheelUp()
        {
            Wheel(WheelDeltaUnit);
        }

        public static void WheelDown()
        {
            Wheel(-WheelDeltaUnit);
        }

        public static void WheelLeft()
        {
            WheelHorizontally(-WheelDeltaUnit);
        }

        public static void WheelRight()
        {
            WheelHorizontally(WheelDeltaUnit);
        }

        static void SendMouseInput(MOUSEINPUT mi)
        {
            NativeKeyboardMouse.SendInput(new[] { ToInput(mi) });
        }

        static readonly Func<MOUSEINPUT, INPUT> ToInput = mi => new INPUT
        {
            type = INPUT_TYPE.MOUSE,
            mi = mi,
        };

        static readonly Func<Point, MOUSEINPUT> ToMouseInputForMove = point => new MOUSEINPUT
        {
            dx = (int)((point.X - AllScreensBounds.X) * 0x10000 / AllScreensBounds.Width),
            dy = (int)((point.Y - AllScreensBounds.Y) * 0x10000 / AllScreensBounds.Height),
            dwFlags = MOUSEEVENTF.MOVE | MOUSEEVENTF.VIRTUALDESK | MOUSEEVENTF.ABSOLUTE,
        };

        static MOUSEINPUT ToMouseInputForButtonDown(MouseButtonType button)
        {
            var mi = new MOUSEINPUT();

            switch (button)
            {
                case MouseButtonType.Left:
                    mi.dwFlags = MOUSEEVENTF.LEFTDOWN;
                    break;
                case MouseButtonType.Right:
                    mi.dwFlags = MOUSEEVENTF.RIGHTDOWN;
                    break;
                case MouseButtonType.Middle:
                    mi.dwFlags = MOUSEEVENTF.MIDDLEDOWN;
                    break;
                case MouseButtonType.XButton1:
                    mi.mouseData = NativeKeyboardMouse.MouseData_XBUTTON1;
                    mi.dwFlags = MOUSEEVENTF.XDOWN;
                    break;
                case MouseButtonType.XButton2:
                    mi.mouseData = NativeKeyboardMouse.MouseData_XBUTTON2;
                    mi.dwFlags = MOUSEEVENTF.XDOWN;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return mi;
        }

        static MOUSEINPUT ToMouseInputForButtonUp(MouseButtonType button)
        {
            var mi = new MOUSEINPUT();

            switch (button)
            {
                case MouseButtonType.Left:
                    mi.dwFlags = MOUSEEVENTF.LEFTUP;
                    break;
                case MouseButtonType.Right:
                    mi.dwFlags = MOUSEEVENTF.RIGHTUP;
                    break;
                case MouseButtonType.Middle:
                    mi.dwFlags = MOUSEEVENTF.MIDDLEUP;
                    break;
                case MouseButtonType.XButton1:
                    mi.mouseData = NativeKeyboardMouse.MouseData_XBUTTON1;
                    mi.dwFlags = MOUSEEVENTF.XUP;
                    break;
                case MouseButtonType.XButton2:
                    mi.mouseData = NativeKeyboardMouse.MouseData_XBUTTON2;
                    mi.dwFlags = MOUSEEVENTF.XUP;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return mi;
        }
    }

    public enum MouseButtonType
    {
        Left,
        Right,
        Middle,
        XButton1,
        XButton2,
    }
}
