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
using System.Windows.Shapes;
using TheUndergroundTower.Options;
using static WpfApp1.Options;

namespace TheUndergroundTower.Windows
{
    /// <summary>
    /// Interaction logic for windowOptions.xaml
    /// </summary>
    public partial class windowOptions : Window
    {
        public windowOptions()
        {
            InitializeComponent();
            MasterVolume.Value = Sound.TempMasterVolume*100;
            MusicVolume.Value = Sound.TempMusicVolume*100;
            SoundVolume.Value = Sound.TempSfxVolume*100;
            ShowDialog();
        }

        private void MasterVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Sound.TempMasterVolume = MasterVolume.Value/100;
        }

        private void MusicVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Sound.TempMusicVolume = MusicVolume.Value/100;
        }

        private void SoundVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Sound.TempSfxVolume = SoundVolume.Value/100;
        }


        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Sound.ChangeSoundVolume();
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Sound.ResetTempVolumes();
            Close();
        }
        
    }
}
