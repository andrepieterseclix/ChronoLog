using System.ComponentModel;

namespace CLog.UI.Framework.Testing.Models
{
    public class ServicesMockSettingsModel : TestParameterModelBase
    {
        private const int _min = 0;

        private const int _max = 10000;

        private int _simulateLatencyMilliseconds;

        public ServicesMockSettingsModel()
            : base("Services Mock Settings")
        {
            SimulateLatencyMilliseconds = 200;
        }

        [DisplayName("Simulate Latency (ms)")]
        [Description("The amount of milliseconds to simulate the service call latency.")]
        public int SimulateLatencyMilliseconds
        {
            get { return _simulateLatencyMilliseconds; }
            set
            {
                if (value < _min)
                    _simulateLatencyMilliseconds = _min;
                else if (value > _max)
                    _simulateLatencyMilliseconds = _max;
                else
                    _simulateLatencyMilliseconds = value;
            }
        }
    }
}
