using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MouseJiggler
{
    public class MouseJigglers
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        const int KEYEVENTF_KEYUP = 0x0002;
        const byte VK_SHIFT = 0x10;

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("kernel32.dll")]
        static extern uint SetThreadExecutionState(uint esFlags);

        private const uint ES_CONTINUOUS = 0x80000000;
        private const uint ES_SYSTEM_REQUIRED = 0x00000001;
        private const uint ES_DISPLAY_REQUIRED = 0x00000002;

        public struct POINT { public int X; public int Y; }

        private Thread jiggleThread;
        private bool running = false;
        private int interval;

        public MouseJigglers(int intervalMinute = 1)
        {
            interval = intervalMinute;
        }

        public void Start()
        {
            if (running) return;

            running = true;

            SetThreadExecutionState(
                ES_CONTINUOUS |
                ES_SYSTEM_REQUIRED |
                ES_DISPLAY_REQUIRED
            );

            jiggleThread = new Thread(JiggleLoop);
            jiggleThread.IsBackground = true;
            jiggleThread.Start();
        }

        public void Stop()
        {
            running = false;
            SetThreadExecutionState(ES_CONTINUOUS);
        }

        private void JiggleLoop()
        {
            while (running)
            {
                GetCursorPos(out POINT p);

                SetCursorPos(p.X + 1, p.Y);
                Thread.Sleep(100);
                SetCursorPos(p.X, p.Y);

                keybd_event(VK_SHIFT, 0, 0, 0);
                Thread.Sleep(50);
                keybd_event(VK_SHIFT, 0, KEYEVENTF_KEYUP, 0);

                Thread.Sleep(interval * 60 * 1000);
            }
        }
    }
}