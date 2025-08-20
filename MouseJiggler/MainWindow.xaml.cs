using System.Windows;
using System.Windows.Forms;


namespace MouseJiggler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private MouseJigglers jiggler;
        private NotifyIcon  trayIcon;

        public MainWindow()
        {
            InitializeComponent();
            jiggler = new MouseJigglers();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(IntervalBox.Text, out int interval))
            {
                if (interval < 5000)
                {
                    System.Windows.MessageBox.Show("Minimum interval is 5000 ms. Setting to 5000.");
                    interval = 5000;
                    IntervalBox.Text = interval.ToString();
                }

                jiggler = new MouseJigglers(interval);
                jiggler.Start();
            }
            else
            {
                System.Windows.MessageBox.Show("Please enter a valid number.");
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e) => jiggler.Stop();

        private void EnableTray_Click(object sender, RoutedEventArgs e)
        {
            if (trayIcon == null)
            {
                // Tray icon oluştur
                trayIcon = new NotifyIcon();
                trayIcon.Icon = System.Drawing.SystemIcons.Application;

                trayIcon.Visible = true;
                trayIcon.Text = "Mouse Jiggler";

                // Çift tıklama ile pencereyi geri getir
                trayIcon.DoubleClick += (s, args) =>
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                    this.ShowInTaskbar = true;

                    // Tray icon'u kaldır
                    trayIcon.Visible = false;
                    trayIcon.Dispose();
                    trayIcon = null;
                };

                // Pencereyi gizle
                this.Hide();
            }
            else
            {
                // Pencereyi göster
                this.Show();
                this.WindowState = WindowState.Normal;
                this.ShowInTaskbar = true;

                // Tray icon'u kaldır
                trayIcon.Visible = false;
                trayIcon.Dispose();
                trayIcon = null;
            }
        }


        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }
    }
}