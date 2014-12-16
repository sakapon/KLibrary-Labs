using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KLibrary.Labs.UI.Input
{
    public static class KeyboardInjection
    {
        // Modifiers will be ignored.
        public static void KeyDown(Keys key)
        {
            SendKeyboardInput(ToKeyboardInputForKeyDown(key));
        }

        // Modifiers will be ignored.
        public static void KeyUp(Keys key)
        {
            SendKeyboardInput(ToKeyboardInputForKeyUp(key));
        }

        public static void StrokeKey(Keys key)
        {
            if ((key & Keys.Modifiers) == Keys.None)
            {
                var ki_down = ToKeyboardInputForKeyDown(key);
                var ki_up = ToKeyboardInputForKeyUp(key);
                NativeKeyboardMouse.SendInput(new[] { ToInput(ki_down), ToInput(ki_up) });
            }
            else
            {
                var kis = new List<KEYBDINPUT>
                {
                    ToKeyboardInputForKeyDown(key),
                    ToKeyboardInputForKeyUp(key),
                };

                if (key.HasFlag(Keys.Shift))
                {
                    kis.Insert(0, ToKeyboardInputForKeyDown(Keys.ShiftKey));
                    kis.Add(ToKeyboardInputForKeyUp(Keys.ShiftKey));
                }
                if (key.HasFlag(Keys.Control))
                {
                    kis.Insert(0, ToKeyboardInputForKeyDown(Keys.ControlKey));
                    kis.Add(ToKeyboardInputForKeyUp(Keys.ControlKey));
                }
                if (key.HasFlag(Keys.Alt))
                {
                    kis.Insert(0, ToKeyboardInputForKeyDown(Keys.Menu));
                    kis.Add(ToKeyboardInputForKeyUp(Keys.Menu));
                }
                NativeKeyboardMouse.SendInput(kis.Select(ToInput).ToArray());
            }
        }

        public static void SendScanCode(ushort scanCode)
        {
            var ki = new KEYBDINPUT
            {
                wScan = scanCode,
                dwFlags = KEYEVENTF.SCANCODE,
            };
            SendKeyboardInput(ki);
        }

        public static void EnterCharacter(char c)
        {
            NativeKeyboardMouse.SendInput(ToInputsForUnicode(c));
        }

        public static void EnterCharacters(string text)
        {
            var inputs = text.SelectMany(ToInputsForUnicode).ToArray();
            NativeKeyboardMouse.SendInput(inputs);
        }

        static void SendKeyboardInput(KEYBDINPUT ki)
        {
            NativeKeyboardMouse.SendInput(new[] { ToInput(ki) });
        }

        static readonly Func<KEYBDINPUT, INPUT> ToInput = ki => new INPUT
        {
            type = INPUT_TYPE.KEYBOARD,
            ki = ki,
        };

        // Modifiers will be ignored.
        static readonly Func<Keys, KEYBDINPUT> ToKeyboardInputForKeyDown = key => new KEYBDINPUT
        {
            wVk = (ushort)key,
        };

        // Modifiers will be ignored.
        static readonly Func<Keys, KEYBDINPUT> ToKeyboardInputForKeyUp = key => new KEYBDINPUT
        {
            wVk = (ushort)key,
            dwFlags = KEYEVENTF.KEYUP,
        };

        static INPUT[] ToInputsForUnicode(char c)
        {
            var ki_down = new KEYBDINPUT
            {
                wScan = c,
                dwFlags = KEYEVENTF.UNICODE,
            };
            var ki_up = new KEYBDINPUT
            {
                wScan = c,
                dwFlags = KEYEVENTF.UNICODE | KEYEVENTF.KEYUP,
            };
            return new[] { ToInput(ki_down), ToInput(ki_up) };
        }
    }
}
