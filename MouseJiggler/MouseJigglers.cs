using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MouseJiggler
{
    public class MouseJigglers
    {
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
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

            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);

            jiggleThread = new Thread(JiggleLoop) { IsBackground = true };
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
                Thread.Sleep(interval);
            }
        }
    }
}
