using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicHotkey
{
    public sealed class KeyboardHook : IDisposable
    {
        // Registers a hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Represents the window that is used internally to get the messages.
        /// </summary>
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;

            public Window() {
                // create the handle for the window.
                this.CreateHandle(new CreateParams());
            }

            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m) {
                base.WndProc(ref m);

                // check if we got a hot key pressed.
                if (m.Msg == WM_HOTKEY) {
                    // get the keys.
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                    // invoke the event to notify the parent.
                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;

            #region IDisposable Members

            public void Dispose() {
                this.DestroyHandle();
            }

            #endregion
        }

        private Window _window = new Window();
        private int _currentId;
        private List<HotkeyRegistration> registrations = new List<HotkeyRegistration>();

        public KeyboardHook() {
            // register the event of the inner native window.
            _window.KeyPressed += delegate (object sender, KeyPressedEventArgs args)
            {
                // KeyPressed?.Invoke(this, args);
                foreach (var registration in registrations)
                {
                    if (registration.Modifier == args.Modifier && registration.Key == args.Key)
                    {
                        registration.action.Invoke();
                    }
                }
            };
        }

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public HotkeyRegistration RegisterHotKey(ModifierKeys modifier, Keys key, Action action) {
            var id = ++_currentId;

            var registration = new HotkeyRegistration(this, id, modifier, key, action);

            // register the hot key.
            if (!RegisterHotKey(_window.Handle, id, (uint)modifier, (uint)key))
                throw new InvalidOperationException("Couldn’t register the hot key.");

            registrations.Add(registration);
            return registration;
        }

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public HotkeyRegistration RegisterHotKey(Keys key, Action action)
        {
            return RegisterHotKey(0, key, action);
        }

        #region IDisposable Members

        public void Dispose()
        {
            registrations.RemoveAll(hotkey =>
            {
                UnregisterHotKey(_window.Handle, hotkey.Id);
                return true;
            });

            // dispose the inner native window.
            _window.Dispose();
        }

        #endregion

        public void Unregister(HotkeyRegistration hotkey)
        {
            UnregisterHotKey(_window.Handle, hotkey.Id);
            registrations.Remove(hotkey);
        }
    }

    public class HotkeyRegistration
    {
        public readonly int Id;
        public readonly ModifierKeys Modifier;
        public readonly Keys Key;
        private readonly KeyboardHook hook;
        internal readonly Action action;

        internal HotkeyRegistration(KeyboardHook hook, int id, ModifierKeys modifier, Keys key, Action action) {
            this.hook = hook;
            this.Modifier = modifier;
            this.Key = key;
            this.Id = id;
            this.action = action;
        }

        public void Unregister() {
            hook.Unregister(this);
        }
    }

    /// <summary>
    /// Event Args for the event that is fired after the hot key has been pressed.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        private ModifierKeys _modifier;
        private Keys _key;

        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key) {
            _modifier = modifier;
            _key = key;
        }

        public ModifierKeys Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }

    /// <summary>
    /// The enumeration of possible modifiers.
    /// </summary>
    [Flags]
    public enum ModifierKeys : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }
}
