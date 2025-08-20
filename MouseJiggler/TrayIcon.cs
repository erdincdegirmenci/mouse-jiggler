using System.Drawing;
using System.Windows;
using System.Windows.Forms;


namespace MouseJiggler
{
    public class TrayIcon
    {
        private NotifyIcon notifyIcon;
        private MouseJigglers jiggler;
        private MainWindow window;

        public TrayIcon(MainWindow mw, MouseJigglers mj)
        {
            window = mw;
            jiggler = mj;

            notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application,
                Visible = true,
                Text = "Mouse Jiggler WPF"
            };

            var context = new ContextMenuStrip();
            context.Items.Add("Aç/Kapat", null, (s, e) => ToggleWindow());
            context.Items.Add("Başlat Jiggle", null, (s, e) => jiggler.Start());
            context.Items.Add("Durdur Jiggle", null, (s, e) => jiggler.Stop());
            context.Items.Add("Çıkış", null, (s, e) => Exit());

            notifyIcon.ContextMenuStrip = context;
            notifyIcon.DoubleClick += (s, e) => ToggleWindow();
        }

        private void ToggleWindow()
        {
            if (window.Visibility == Visibility.Visible)
                window.Hide();
            else
                window.Show();
        }

        private void Exit()
        {
            jiggler.Stop();
            notifyIcon.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }
    }
}
