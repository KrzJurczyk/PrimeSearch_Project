using System;
using System.Threading;
using PrimeCalculator;
using PrimeSearch.UI.Utils;

namespace PrimeSearch.UI
{
    /// <summary>
    /// ViewModel for main window
    /// </summary>
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        private readonly SieveOfEratosthenesCalculator _sieveOfEratosthenesCalculator = new SieveOfEratosthenesCalculator();
        private CancellationTokenSource _cancellationTokenSource;
        private uint _numberRange = 50000000; // 50 mln
        private int _progress;
        private string _resultText;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {
            StartCommand = new DelegateCommand(Start);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public DelegateCommand CancelCommand { get; }

        /// <summary>
        /// Performs prime number search
        /// </summary>
        private async void Start()
        {
            using (StartCommand.RunWithDisabled())
            using (_cancellationTokenSource = new CancellationTokenSource())
            {

                var progress = new ProgressFilterProxy(new Progress<double>(d => Progress = (int) (d * 100)));
                try
                {
                    var primes =
                        await _sieveOfEratosthenesCalculator.MakeSieveAsync((int) NumberRange, progress,
                            _cancellationTokenSource.Token);

                    Progress = 100;
                    ResultText = $"Znaleziono {primes.Count} liczb pierwszych";
                    PrimeNumbers.Clear();
                    PrimeNumbers.AddRange(primes,
                        ObservableCollectionEx<int>.ECollectionChangeNotificationMode.Reset);
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken == _cancellationTokenSource.Token)
                {
                    ResultText = "Operacja zatrzymana";
                    PrimeNumbers.Clear();
                }
                finally
                {
                    _cancellationTokenSource = null;
                }
            }
        }

        private void Cancel()
        {
            _cancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Max Value, where prime numbers should be searched for 
        /// </summary>
        public uint NumberRange
        {
            get { return _numberRange; }
            set
            {
                _numberRange = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Progress from 0 to 100 of current operation
        /// </summary>
        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value; 
                OnPropertyChanged();
            }
        }

        public ObservableCollectionEx<int> PrimeNumbers { get; } = new ObservableCollectionEx<int>();

        public DelegateCommand StartCommand { get; }

        public string ResultText
        {
            get { return _resultText; }
            set
            {
                _resultText = value; 
                OnPropertyChanged();
            }
        }
    }
}
