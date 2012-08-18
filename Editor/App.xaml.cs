using System;

namespace Editor
{
    public partial class App
    {
        private void ApplicationStartup(object sender, System.Windows.StartupEventArgs e)
        {
            new Bootstrapper().Init();

            new MainWindow().Show();
        }
    }
}
