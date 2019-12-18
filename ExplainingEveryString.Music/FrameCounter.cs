using ExplainingEveryString.Music.Model;
using System;

namespace ExplainingEveryString.Music
{
    internal class FrameCounter : ISoundComponent
    {
        private readonly Int32[] StepsCyclesModeSet = new Int32[] { 3729, 7457, 11185, 18641 };
        private readonly Int32[] StepsCyclesModeClear = new Int32[] { 3729, 7457, 11185, 14915 };

        internal event EventHandler QuarterFrame;
        internal event EventHandler HalfFrame;

        internal Boolean ModeFlag { get; set; }

        private Int32 currentApuCyclesValue = 0;
        private Int32 stepsEvaluated = 0;

        private Int32[] StepsCycles => ModeFlag ? StepsCyclesModeSet : StepsCyclesModeClear;

        public void MoveEmulationTowardNextSample()
        {
            Int32 apuCycles = Constants.ApuTicksBetweenSamples;
            currentApuCyclesValue += apuCycles;
            if (currentApuCyclesValue >= StepsCycles[stepsEvaluated])
            {
                stepsEvaluated += 1;
                ExecuteLastEvaluatedStep();
            }
        }

        private void ExecuteLastEvaluatedStep()
        {
            if (stepsEvaluated == 1)
            {
                QuarterFrame?.Invoke(this, EventArgs.Empty);
            }
                
            if (stepsEvaluated == 2)
            {
                QuarterFrame?.Invoke(this, EventArgs.Empty);
                HalfFrame?.Invoke(this, EventArgs.Empty);
            }

            if (stepsEvaluated == 3)
            {
                QuarterFrame?.Invoke(this, EventArgs.Empty);
            }

            if (stepsEvaluated == 4)
            {
                QuarterFrame?.Invoke(this, EventArgs.Empty);
                HalfFrame?.Invoke(this, EventArgs.Empty);
                stepsEvaluated = 0;
                currentApuCyclesValue -= StepsCycles[3];
            }
        }

        public void ProcessSoundDirectingEvent(RawSoundDirectingEvent soundEvent)
        {
            if (soundEvent.Parameter == SoundChannelParameter.FrameCounterMode)
                ModeFlag = soundEvent.Value != 0;
            else
                throw new InvalidOperationException();
        }
    }
}
