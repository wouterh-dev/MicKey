using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace MicHotkey
{
    public partial class MainForm : Form
    {
        private CoreAudioController audioController = new CoreAudioController();
        private KeyboardHook hook = new KeyboardHook();
        private bool wantMute;
        private HotkeyRegistration _hotkeyRegistration;
        private bool updateHotkey;
        private ContextMenu contextMenu;
        private MenuItem menuItem1;
        private SoundPlayer sound;
        private bool firstStart;
        private bool enableSound;

        public MainForm()
        {
            // Hacky workaround to start window as hidden...
            Opacity = 0;
            ShowInTaskbar = false;
            firstStart = true;

            InitializeComponent();
            InitializeState();
        }

        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);

            if (firstStart)
            {
                // Restore state from hacky workaround...
                Visible = false;
                Opacity = 100;
                ShowInTaskbar = true;
                firstStart = false;
            }
        }

        private void setHotkey(ModifierKeys modifierKeys, Keys key)
        {
            List<string> txt = new List<string>();
            txt.Add("Hotkey:");

            foreach (ModifierKeys mod in Enum.GetValues(typeof(ModifierKeys)))
            {
                if ((modifierKeys & mod) == mod)
                {
                    txt.Add(mod.ToString());
                }
            }

            txt.Add(key.ToString());
            label_hotkey.Text = string.Join(" ", txt);

            if (_hotkeyRegistration == null || _hotkeyRegistration.Key != key || _hotkeyRegistration.Modifier != modifierKeys)
            {
                _hotkeyRegistration?.Unregister();
                try
                {
                    _hotkeyRegistration = hook.RegisterHotKey(modifierKeys, key, Hook_KeyPress);
                }
                catch (InvalidOperationException ex)
                {
                    label_hotkey.Text = "Could not register key";
                }
            }

            if (Properties.Settings.Default.hotkeyModifiers != modifierKeys || Properties.Settings.Default.hotkeyKey != key)
            {
                Properties.Settings.Default.hotkeyModifiers = modifierKeys;
                Properties.Settings.Default.hotkeyKey = key;
                Properties.Settings.Default.Save();
            }
        }

        private async void Hook_KeyPress()
        {
            await toggleMute();
        }

        private async void InitializeState()
        {
            setHotkey(Properties.Settings.Default.hotkeyModifiers, Properties.Settings.Default.hotkeyKey);

            contextMenu = new ContextMenu();
            menuItem1 = new MenuItem("Exit", (sender, args) => Dispose());
            contextMenu.MenuItems.Add(menuItem1);
            notifyIcon.ContextMenu = contextMenu;

            wantMute = await IsMuted();
            await SetMute(wantMute);
            timer_update.Enabled = true;

            notifyIcon.Visible = true;
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            await toggleMute();
        }

        private async Task toggleMute()
        {
            var devices = (await audioController.GetDevicesAsync(DeviceType.Capture, DeviceState.Active)).ToList();
            wantMute = !await IsMuted(devices);
            await SetMute(wantMute, devices);

            sound?.Stop();
            if (enableSound)
            {
                var soundFile = wantMute ? Properties.Resources.beep300 : Properties.Resources.beep750;
                sound = new SoundPlayer(soundFile);
                sound.Play();
            }
            else
            {
                sound = null;
            }
        }

        private async Task<bool> SetMute(bool mute, IEnumerable<CoreAudioDevice> devices = null)
        {
            if (devices == null)
            {
                devices = await audioController.GetDevicesAsync(DeviceType.Capture, DeviceState.Active);
            }

            var changed = false;

            devices.Where(d => d.IsDefaultCommunicationsDevice || d.IsDefaultDevice).ToList()
                .ForEach(async device =>
                {
                    if (device.IsMuted == mute) return;
                    changed = true;
                    await device.SetMuteAsync(mute);
                });

            BackColor = mute ? Color.Red : Color.Green;
            var Icon = mute ? Properties.Resources.red : Properties.Resources.green;

            notifyIcon.Icon = Icon;
            TaskbarManager.Instance.SetOverlayIcon(Handle, Icon, mute ? "Muted" : "Unmuted");

            return changed;
        }

        private async Task<bool> IsMuted(IEnumerable<CoreAudioDevice> devices = null)
        {
            if (devices == null)
            {
                devices = await audioController.GetDevicesAsync(DeviceType.Capture, DeviceState.Active);
            }

            var firstDevice = devices.FirstOrDefault(d => d.IsDefaultCommunicationsDevice);
            return firstDevice != null && firstDevice.IsMuted;
        }

        private async void Timer_update_Tick(object sender, EventArgs e)
        {
            bool changed = await SetMute(wantMute);

            if (changed)
            {
                notifyIcon.BalloonTipText = $"Other application {(wantMute ? "enabled" : "disabled")} mic!";
                notifyIcon.ShowBalloonTip(5000);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && notifyIcon.Visible)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void doShow()
        {
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            doShow();
        }

        private void notifyIcon_Click(object sender, EventArgs e) {
            doShow();
        }

        private void button2_KeyUp(object sender, KeyEventArgs e)
        {
            if (!updateHotkey) return;
            updateHotkey = false;

            ModifierKeys modifierKeys = 0;

            if (e.Control)
            {
                modifierKeys |= MicHotkey.ModifierKeys.Control;
            }

            if (e.Alt)
            {
                modifierKeys |= MicHotkey.ModifierKeys.Alt;
            }

            if (e.Shift)
            {
                modifierKeys |= MicHotkey.ModifierKeys.Shift;
            }

            if ((e.Modifiers & Keys.LWin) == Keys.LWin || (e.Modifiers & Keys.RWin) == Keys.RWin)
            {
                modifierKeys |= MicHotkey.ModifierKeys.Win;
            }

            setHotkey(modifierKeys, e.KeyCode);
        }

        private void button_hotkey_Click(object sender, EventArgs e)
        {
            if (updateHotkey)
            {
                updateHotkey = false;
                setHotkey(_hotkeyRegistration.Modifier, _hotkeyRegistration.Key);
            }
            else
            {
                updateHotkey = true;
                label_hotkey.Text = "Press key...";
            }
        }

        private void button_hotkey_Leave(object sender, EventArgs e)
        {
            updateHotkey = false;
            setHotkey(_hotkeyRegistration.Modifier, _hotkeyRegistration.Key);
        }
    }
}