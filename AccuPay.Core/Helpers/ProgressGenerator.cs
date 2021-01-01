using AccuPay.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AccuPay.Core.Helpers
{
    public class ProgressGenerator : IProgressGenerator
    {
        public IReadOnlyCollection<IResult> Results { get; private set; }

        private readonly int _total;

        private int _finished;

        public ProgressGenerator(int total)
        {
            _total = total;
        }

        public int Progress
        {
            get
            {
                if (_finished == 0)
                    return 0;

                //using decimal does not update the progress threading
                return Convert.ToInt32(Math.Floor(_finished / (double)_total * 100));
            }
        }

        public string CurrentMessage { get; private set; }

        protected void SetResults(IEnumerable<IResult> results)
        {
            Results = results.ToList();
        }

        public void SetCurrentMessage(string currentMessage)
        {
            CurrentMessage = currentMessage;
        }

        public void IncreaseProgress(int incrementCount = 1)
        {
            Interlocked.Add(ref _finished, incrementCount);
        }

        public void IncreaseProgress(string currentMessage, int incrementCount = 1)
        {
            SetCurrentMessage(currentMessage);
            Interlocked.Add(ref _finished, incrementCount);
        }

        public interface IResult
        {
            bool IsSuccess { get; }

            bool IsError { get; }
        }
    }
}
