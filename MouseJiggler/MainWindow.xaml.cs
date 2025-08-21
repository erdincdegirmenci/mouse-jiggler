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
        private NotifyIcon  notifyIcon;

        public MainWindow()
        {
            InitializeComponent();
            jiggler = new MouseJigglers();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(IntervalBox.Text, out int interval))
            {
                if (interval < 1)
                {
                    System.Windows.MessageBox.Show("Minimum interval is 1 minute. Setting to 1 minute.");
                    interval = 1;
                    IntervalBox.Text = interval.ToString();
                }
                int intervalMs = interval * 60000;
                jiggler = new MouseJigglers(intervalMs);
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
            if (notifyIcon == null)
            {
                notifyIcon = new NotifyIcon();
                notifyIcon.Icon = new System.Drawing.Icon("Resources\\mousejiggler.ico");

                notifyIcon.Visible = true;
                notifyIcon.Text = "Mouse Jiggler";

                notifyIcon.DoubleClick += (s, args) =>
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                    this.ShowInTaskbar = true;

                    notifyIcon.Visible = false;
                    notifyIcon.Dispose();
                    notifyIcon = null;
                };

                this.Hide();
            }
            else
            {
                this.Show();
                this.WindowState = WindowState.Normal;
                this.ShowInTaskbar = true;

                notifyIcon.Visible = false;
                notifyIcon.Dispose();
                notifyIcon = null;
            }
        }


        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            notifyIcon?.Dispose();
            base.OnClosing(e);
            System.Windows.Application.Current.Shutdown();
        }
    }
}