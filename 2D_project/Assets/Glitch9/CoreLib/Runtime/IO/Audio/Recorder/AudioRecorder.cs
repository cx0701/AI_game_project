using UnityEngine;

namespace Glitch9.CoreLib.IO.Audio
{
    public class AudioRecorder : AudioRecorderBase
    {
        public AudioRecorder(
            SampleRate sampleRate = SampleRate.Hz16000,
            int recordingLength = 30,
            string microphoneDeviceName = null,
            ILogger logger = null) : base(sampleRate, recordingLength, microphoneDeviceName, logger)
        {
        }

        public AudioClip StopRecording(bool playRecording = false)
        {
            if (!IsRecording)
            {
                _logger.Warning(Messages.kRecordingNotStarted);
                return null;
            }

            // Stop the recording
            Microphone.End(null);
            _logger.Info(Messages.kRecordingStopped);

            if (RecordedClip != null)
            {
                RecordedClip.TrimSilence();
                if (playRecording) PlayRecording();
                return RecordedClip;
            }

            return null;
        }
    }
}