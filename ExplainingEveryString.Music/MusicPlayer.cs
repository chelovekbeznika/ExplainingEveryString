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
        private List<Byte[]> songParts;
        private Int32 songPartsPlaying;
        private String nowPlaying = null;

        public Single Volume 
        { 
            get 
            { 
                return volume; 
            } 
            set 
            { 
                volume = value; 
                if (sound != null) 
                    sound.Volume = value; 
            } 
        }

        public MusicPlayer()
        {
        }

        public void Initialize(Single volume)
        {
            this.Volume = volume;
            this.deltaSamplesLibrary = DeltaSamplesLibraryLoader.Load(@"Content/Data/Music/deltasamples.dat");
            this.soundChipReplica = new NesSoundChipReplica(deltaSamplesLibrary);
        }

        public void Update()
        {
            if (sound != null && nowPlaying != null)
            {
                var newSongParts = soundChipReplica.GetGeneratedParts();
                if (newSongParts != null)
                    songParts.AddRange(newSongParts);

                while (songParts.Count > songPartsPlaying)
                {
                    sound.SubmitBuffer(songParts[songPartsPlaying]);
                    songPartsPlaying += 1;
                }

                if (sound.PendingBufferCount < 1)
                {
                    foreach (var buffer in songParts)
                        sound.SubmitBuffer(buffer);
                    songPartsPlaying = songParts.Count;
                    sound.Play();
                }
            }
        }

        public void Play(String songName, Boolean forceRestart)
        {
            if (forceRestart || songName != nowPlaying)
            {
                Stop();
                songParts = new List<Byte[]> { Load(songName) };
                sound = new DynamicSoundEffectInstance(Constants.SampleRate, AudioChannels.Mono) { Volume = Volume };
                sound.SubmitBuffer(songParts[0]);
                songPartsPlaying = 1;
                sound.Play();
                nowPlaying = songName;
            }
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
                songPartsPlaying = 0;
                soundChipReplica.StopMusicGeneration();
                soundChipReplica = new NesSoundChipReplica(deltaSamplesLibrary);
                nowPlaying = null;
            }
        }

        private Byte[] Load(String songName)
        {
            var fileName = $"Content/Data/Music/{songName}.dat";
            var song = JsonDataAccessor.Instance.Load<SongSpecification>(fileName);
            return soundChipReplica.StartMusicGeneration(song);
        }
    }
}
