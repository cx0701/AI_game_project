using System;

namespace Glitch9.AIDevKit.Google
{
    public class StopCandidateException : Exception
    {
        public StopCandidateException(Candidate candidate) : base(candidate.GetErrorMessage())
        {
        }
    }
}