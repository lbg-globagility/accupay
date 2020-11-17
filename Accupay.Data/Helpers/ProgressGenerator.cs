using AccuPay.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Helpers
{
    public class ProgressGenerator : IProgressGenerator
    {
        public IReadOnlyCollection<IResult> Results { get; private set; }

        protected readonly int _total;

        protected int _finished;

        protected string _currentMessage;

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

        public string CurrentMessage => _currentMessage;

        protected void SetResults(IEnumerable<IResult> results)
        {
            Results = results.ToList();
        }

        protected void SetCurrentMessage(string currentMessage)
        {
            _currentMessage = currentMessage;
        }

        public interface IResult
        {
            bool IsSuccess { get; }

            bool IsError { get; }
        }
    }
}