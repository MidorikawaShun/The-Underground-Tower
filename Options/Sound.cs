using System;
using System.IO;
using System.Windows.Media;
using static WpfApp1.Definitions;
using static WpfApp1.Options;

namespace TheUndergroundTower.Options
{
    public static class Sound
    {

        public enum EnumMediaPlayers
        {
            MusicPlayer = 0,
            SfxPlayer = 1
        }

        private static System.Windows.Media.MediaPlayer MusicPlayer;
        private static double MasterVolume = 0.5;
        private static double MusicVolume = 1;
        private static double SfxVolume = 1;

        public static double TempMasterVolume { get; set; }
        public static double TempMusicVolume { get; set; }
        public static double TempSfxVolume { get; set; }

        private static bool Initialized = false;
        private static bool VolumeChanged = false;

        private static void InitializePlayer()
        {
            if (!Initialized)
            {
                MusicPlayer = new MediaPlayer();
                MusicPlayer.MediaEnded += RestartMusic;
                TempMasterVolume = MasterVolume;
                TempMusicVolume = MusicVolume;
                TempSfxVolume = SfxVolume;
                MusicPlayer.Volume = MusicVolume;
                Initialized = true;
            }
            if (VolumeChanged)
            {
                VolumeChanged = false;
            }
        }

        /// <summary>
        /// Play music or a sound effect.
        /// </summary>
        /// <param name="sfx">The EnumSoundFile you want to play. The Enum contains a file path.</param>
        public static void PlaySound(EnumSoundFiles sound, EnumMediaPlayers player)
        {
            InitializePlayer();
            string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SOUND_FILES[(int)sound]);
            if (player == EnumMediaPlayers.MusicPlayer)
                PlaySound(soundPath, MusicPlayer);
            if (player == EnumMediaPlayers.SfxPlayer)
                PlaySound(soundPath, new MediaPlayer() { Volume = SfxVolume * MasterVolume });


        }

        private static void PlaySound(string path, MediaPlayer player)
        {
            bool openedFile = false;
            try
            {
                player.Open(new Uri(path));
                openedFile = true;
            }
            catch (Exception ex)
            {
                WpfApp1.Options.ErrorLog.Log(ex, "An error has occured while trying to open the sound effect file in path: " + path);
            }
            if (openedFile)
            {
                try
                {
                    player.Position = new TimeSpan(0, 3, 10);
                    player.Play();
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to play the sound effect file in path: " + path);
                }
            }
            if (player != MusicPlayer)
                player = null;
        }

        public static void StopMusic(EnumMediaPlayers player)
        {
            MusicPlayer.Stop();
        }

        public static void ChangeSoundVolume()
        {
            MasterVolume = TempMasterVolume;
            MusicVolume = TempMusicVolume;
            SfxVolume = TempSfxVolume;
            MusicPlayer.Volume = MasterVolume * MusicVolume;
            VolumeChanged = true;
            InitializePlayer();
        }

        public static void ResetTempVolumes()
        {
            TempMasterVolume = MasterVolume;
            TempMusicVolume = MusicVolume;
            TempSfxVolume = SfxVolume;
        }

        private static void RestartMusic(object sender, EventArgs e)
        {
            MusicPlayer.Position = new TimeSpan(0);
        }

    }
}

