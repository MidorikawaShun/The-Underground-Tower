using System.Windows;
using TheUndergroundTower.Options;

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
            Sound.ChangeSoundVolume();
        }

        private void MusicVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Sound.TempMusicVolume = MusicVolume.Value/100;
            Sound.ChangeSoundVolume();
        }

        private void SoundVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Sound.TempSfxVolume = SoundVolume.Value/100;
            Sound.ChangeSoundVolume();
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
