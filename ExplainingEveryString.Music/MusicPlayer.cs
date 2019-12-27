using ExplainingEveryString.Data;
using ExplainingEveryString.Music.Model;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Music
{
    public class MusicPlayer
    {
        private DynamicSoundEffectInstance sound;
        private Single volume;
        private List<Byte[]> deltaSamplesLibrary;
        private NesSoundChipReplica soundChipReplica;
        private Byte[] currentSong;
        private String nowPlaying = null;

        public MusicPlayer()
        {
        }

        public void Initialize(Single volume)
        {
            this.volume = volume;
            this.deltaSamplesLibrary = DeltaSamplesLibraryLoader.Load(@"Content/Data/Music/deltasamples.dat");
            this.soundChipReplica = new NesSoundChipReplica(deltaSamplesLibrary);
        }

        public void Update()
        {
            if (sound != null && sound.PendingBufferCount < 1 && nowPlaying != null)
            {
                sound.SubmitBuffer(currentSong);
                sound.Play();
            }
        }

        public void Start(String songName)
        {
            Stop();
            currentSong = Load(songName);
            sound = new DynamicSoundEffectInstance(Constants.SampleRate, AudioChannels.Mono) { Volume = volume };
            sound.SubmitBuffer(currentSong);
            sound.Play();
            nowPlaying = songName;
        }

        public void TryPause()
        {
            if (nowPlaying != null)
                sound.Pause();
        }

        public void TryResume()
        {
            if (nowPlaying != null)
                sound.Resume();
        }

        public void Stop()
        {
            if (nowPlaying != null)
            {
                sound.Stop();
                nowPlaying = null;
            }
        }

        private Byte[] Load(String songName)
        {
            String fileName = $"Content/Data/Music/{songName}.dat";
            SongSpecification song = JsonDataAccessor.Instance.Load<SongSpecification>(fileName);
            return soundChipReplica.GetMusic(song);
        }
    }
}
