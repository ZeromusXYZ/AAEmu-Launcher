using AAEmu.Launcher.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AAEmu.LauncherExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AAEmuLauncherBase.RegisterLaunchers();
        }

        private AAEmuLauncherBase CreateLauncherByName(string configName)
        {
            AAEmuLauncherBase res = null;
            foreach (var l in AAEmuLauncherBase.AllLaunchers)
            {
                if (l.ConfigName == configName)
                {
                    res = Activator.CreateInstance(l.LauncherClass) as AAEmuLauncherBase;
                    break;
                }
            }
            return res;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            var l = CreateLauncherByName("trino_1_2");
            l.GameExeFilePath = "C:\\AAEmu\\Working\\bin32\\archeage.exe";
            l.UserName = tUserName.Text;
            l.SetPassword(tPassword.Text);
            l.HShieldArgs = "+acpxmk";
            l.Locale = "en_us";

            l.InitializeForLaunch();
            l.Launch();
            l.FinalizeLaunch();
        }
    }
}
