using System;

namespace MarsRovers.ObjectModels
{
    public abstract class BaseCommand
    {
        protected readonly string _rawCmdText;
        public string RawCmdText => _rawCmdText;

        protected BaseCommand(string rawTextCmd)
        {
            if (string.IsNullOrEmpty(rawTextCmd))
                throw new ArgumentNullException("Command text cannot be null or empty.");

            _rawCmdText = rawTextCmd;
        }
    }
}