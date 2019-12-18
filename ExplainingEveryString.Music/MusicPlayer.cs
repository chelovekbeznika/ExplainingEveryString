using ExplainingEveryString.Data;
using ExplainingEveryString.Music.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Music
{
    public class MusicPlayer
    {
        private DynamicSoundEffectInstance sound;
        private List<Byte[]> deltaSamplesLibrary;
        private NesSoundChipReplica soundChipReplica;
        private Byte[] currentSong;
        private Boolean isPlaying = false;

        public MusicPlayer()
        {
        }

        public void Initialize()
        {
            this.deltaSamplesLibrary = DeltaSamplesLibraryLoader.Load(@"Content/Data/Music/deltasamples.dat");
            this.soundChipReplica = new NesSoundChipReplica(deltaSamplesLibrary);
        }

        public void Update()
        {
            if (sound != null && sound.PendingBufferCount < 1 && isPlaying)
            {
                sound.SubmitBuffer(currentSong);
                sound.Play();
            }
        }

        public void Start(String songName)
        {
            Stop();
            currentSong = Load(songName);
            sound = new DynamicSoundEffectInstance(Constants.SampleRate, AudioChannels.Mono);
            sound.SubmitBuffer(currentSong);
            sound.Play();
            isPlaying = true;
        }

        public void PauseSwitch()
        {
            if (isPlaying)
            {
                if (sound.State == SoundState.Playing)
                    sound.Pause();
                else if (sound.State == SoundState.Paused)
                    sound.Resume();
            }
        }

        public void Stop()
        {
            if (isPlaying)
            {
                sound.Stop();
                isPlaying = false;
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
