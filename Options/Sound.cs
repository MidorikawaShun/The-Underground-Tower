using System;
using System.IO;
using System.Windows.Media;
using static WpfApp1.GameProperties.Definitions;
using static WpfApp1.Utilities;

namespace TheUndergroundTower.Options
{
    /// <summary>
    /// Used for playing sound in the game.
    /// </summary>
    public static class Sound
    {
        /// <summary>
        /// We have two types of media players: music and special effects.
        /// MusicPlayer is static because we can only play one music track at any given time.
        /// Sfx players are *not* static because many effects can play at once.
        /// </summary>
        public enum EnumMediaPlayers
        {
            MusicPlayer = 0,
            SfxPlayer = 1
        }

        private static System.Windows.Media.MediaPlayer MusicPlayer;
        private static double MasterVolume = 0.5;
        private static double MusicVolume = 1;
        private static double SfxVolume = 1;

        /*TempVolumes are for when we want to change the actual volumes and need to 
         save the previous status.*/

        public static double TempMasterVolume { get; set; }
        public static double TempMusicVolume { get; set; }
        public static double TempSfxVolume { get; set; }

        /*Variables for if we initialized the mediaplayers or changed volumes.*/
        private static bool Initialized = false;
        private static bool VolumeChanged = false;

        /// <summary>
        /// The method that needs to be called first before you use other Sound class methods.
        /// </summary>
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
        /// Determines which sound and which mediaplayer to play the sound on.
        /// </summary>
        /// <param name="sfx">The EnumSoundFile you want to play. The Enum contains a file path.</param>
        public static void PlaySound(EnumSoundFiles sound, EnumMediaPlayers musicPlayer)
        {
            InitializePlayer();
            string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SOUND_FILES[(int)sound]);
            if (musicPlayer == EnumMediaPlayers.MusicPlayer)
                PlaySound(soundPath, MusicPlayer);
            if (musicPlayer == EnumMediaPlayers.SfxPlayer)
                PlaySound(soundPath, new MediaPlayer() { Volume = SfxVolume * MasterVolume });
        }

        /// <summary>
        /// Play music or a sound effect.
        /// </summary>
        /// <param name="path">The path of the sound effect / music that we want played.</param>
        /// <param name="mediaPlayer">The mediaplayer we want to play it on</param>
        private static void PlaySound(string path, MediaPlayer mediaPlayer)
        {
            bool openedFile = false;
            try
            {
                //Try to open the sound file
                mediaPlayer.Open(new Uri(path));
                openedFile = true;
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex, "An error has occured while trying to open the sound effect file in path: " + path);
            }
            if (openedFile) //File opened successfully
            {
                try
                {
                    //Initializes the mediaplayer's position
                    mediaPlayer.Position = new TimeSpan(0, 0, 0);
                    mediaPlayer.Play();
                }
                catch (Exception ex)
                {
                    ErrorLog.Log(ex, "An error has occured while attempting to play the sound effect file in path: " + path);
                }
            }
            //Destroy Sfx Mediaplayers so they don't take additional memory
            if (mediaPlayer != MusicPlayer)
                mediaPlayer = null;
        }

        /// <summary>
        /// Stops the music player's track.
        /// </summary>
        public static void StopMusic()
        {
            MusicPlayer.Stop();
        }

        /// <summary>
        /// Changes the volume.
        /// </summary>
        public static void ChangeSoundVolume()
        {
            MasterVolume = TempMasterVolume;
            MusicVolume = TempMusicVolume;
            SfxVolume = TempSfxVolume;
            MusicPlayer.Volume = MasterVolume * MusicVolume;
            VolumeChanged = true;
            InitializePlayer();
        }

        /// <summary>
        /// Resets temporary volumes for the options menu.
        /// </summary>
        public static void ResetTempVolumes()
        {
            TempMasterVolume = MasterVolume;
            TempMusicVolume = MusicVolume;
            TempSfxVolume = SfxVolume;
        }

        /// <summary>
        /// An event that resets the music position to the beginning.
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">The event</param>
        private static void RestartMusic(object sender, EventArgs e)
        {
            MusicPlayer.Position = new TimeSpan(0);
        }

    }
}

