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
            jiggleThread = new Thread(JiggleLoop) { IsBackground = true };
            jiggleThread.Start();
        }

        public void Stop()
        {
            running = false;
        }

        private void JiggleLoop()
        {
            while (running)
            {
                GetCursorPos(out POINT p);
                SetCursorPos(p.X + 1, p.Y);
                Thread.Sleep(interval);
                SetCursorPos(p.X, p.Y);
                Thread.Sleep(interval);
            }
        }
    }
}
