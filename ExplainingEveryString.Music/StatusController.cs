﻿using ExplainingEveryString.Music.Model;
using System;

namespace ExplainingEveryString.Music
{
    internal class StatusController : ISoundComponent
    {
        internal Boolean Pulse1Enabled { get; private set; } = false;
        internal Boolean Pulse2Enabled { get; private set; } = false;
        internal Boolean TriangleEnabled { get; private set; } = false;
        internal Boolean NoiseEnabled { get; private set; } = false;
        internal Boolean DeltaEnabled { get; private set; } = false;

        public void MoveEmulationTowardNextSample()
        {
        }

        public void ProcessSoundDirectingEvent(RawSoundDirectingEvent soundEvent)
        {
            switch (soundEvent.Parameter)
            {
                case SoundChannelParameter.Pulse1Enabled:
                    Pulse1Enabled = soundEvent.Value != 0;
                    break;
                case SoundChannelParameter.Pulse2Enabled:
                    Pulse2Enabled = soundEvent.Value != 0;
                    break;
                case SoundChannelParameter.TriangleEnabled:
                    TriangleEnabled = soundEvent.Value != 0;
                    break;
                case SoundChannelParameter.NoiseEnabled:
                    NoiseEnabled = soundEvent.Value != 0;
                    break;
                case SoundChannelParameter.DeltaEnabled:
                    DeltaEnabled = soundEvent.Value != 0;
                    break;
            }
        }
    }
}
