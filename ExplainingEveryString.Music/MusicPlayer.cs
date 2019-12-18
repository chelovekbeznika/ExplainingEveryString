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
        private Byte[] buffer;
        private Byte[] lengthTest;
        private Byte[] envelopeTest;
        private Byte[] sweepTest;
        private Byte[] deltaTest;

        public MusicPlayer()
        {
        }

        public void Initialize()
        {
            this.deltaSamplesLibrary = DeltaSamplesLibraryLoader.Load(@"Content/Data/Music/deltasamples.dat");
            this.sound = new DynamicSoundEffectInstance(Constants.SampleRate, AudioChannels.Mono);
            this.buffer = new NesSoundChipReplica(deltaSamplesLibrary).GetMusic(HardcodedSongs.GetTestSong(), 20);
            this.lengthTest = new NesSoundChipReplica(deltaSamplesLibrary)
                .GetMusic(HardcodedSongs.GetLengthCounterTest().Cast<ISoundDirectingSequence>().ToList(), 16);
            this.envelopeTest = new NesSoundChipReplica(deltaSamplesLibrary)
                .GetMusic(HardcodedSongs.GetEnvelopeTestLength().Cast<ISoundDirectingSequence>().ToList(), 16);
            this.sweepTest = new NesSoundChipReplica(deltaSamplesLibrary)
                .GetMusic(HardcodedSongs.GetSweepTest().Cast<ISoundDirectingSequence>().ToList(), 12);
            this.deltaTest = new NesSoundChipReplica(deltaSamplesLibrary)
                .GetMusic(HardcodedSongs.GetDeltaTest().Cast<ISoundDirectingSequence>().ToList(), 16);
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                if (sound.PendingBufferCount < 1)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.RightAlt))
                        sound.SubmitBuffer(sweepTest);
                    else if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                        sound.SubmitBuffer(envelopeTest);
                    else if (Keyboard.GetState().IsKeyDown(Keys.RightControl))
                        sound.SubmitBuffer(lengthTest);
                    else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        sound.SubmitBuffer(deltaTest);
                    else
                        sound.SubmitBuffer(buffer);
                    sound.Play();
                }
            }
        }
    }
}
