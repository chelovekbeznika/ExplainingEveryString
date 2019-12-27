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
        private Single Volume { get { return volume; } set { volume = value; if (sound != null) sound.Volume = value; } }
        private List<Byte[]> deltaSamplesLibrary;
        private NesSoundChipReplica soundChipReplica;
        private List<Byte[]> songParts;
        private Int32 songPartsPlaying;
        private String nowPlaying = null;

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
                List<Byte[]> newSongParts = soundChipReplica.GetGeneratedParts();
                if (newSongParts != null)
                    songParts.AddRange(newSongParts);

                while (songParts.Count > songPartsPlaying)
                {
                    sound.SubmitBuffer(songParts[songPartsPlaying]);
                    songPartsPlaying += 1;
                }

                if (sound.PendingBufferCount < 1)
                {
                    foreach (Byte[] buffer in songParts)
                        sound.SubmitBuffer(buffer);
                    songPartsPlaying = songParts.Count;
                    sound.Play();
                }
            }
        }

        public void Start(String songName)
        {
            Stop();
            songParts = new List<Byte[]>
            {
                Load(songName)
            };
            sound = new DynamicSoundEffectInstance(Constants.SampleRate, AudioChannels.Mono) { Volume = Volume };
            sound.SubmitBuffer(songParts[0]);
            songPartsPlaying = 1;
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
                songPartsPlaying = 0;
                soundChipReplica = new NesSoundChipReplica(deltaSamplesLibrary);
                nowPlaying = null;
            }
        }

        private Byte[] Load(String songName)
        {
            String fileName = $"Content/Data/Music/{songName}.dat";
            SongSpecification song = JsonDataAccessor.Instance.Load<SongSpecification>(fileName);
            return soundChipReplica.StartMusicGeneration(song);
        }
    }
}
