using System;

namespace PrimeSearch.UI.Utils
{
    public class ProgressFilterProxy : IProgress<double>
    {
        private readonly IProgress<double> _progressImplementation;
        private readonly double _threshold;

        private double? _lastProgress;

        public ProgressFilterProxy(IProgress<double> progressImplementation, double threshold = 0.01)
        {
            _progressImplementation = progressImplementation;
            _threshold = threshold;
        }

        public void Report(double value)
        {
            if (_lastProgress != null && Math.Abs(_lastProgress.Value - value) < _threshold)
                return;

            _progressImplementation.Report(value);
            _lastProgress = value;
        }
    }
}
