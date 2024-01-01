using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.BeanMachine.Debug
{
    [Serializable]
    public class DebugFile
    {
        public List<string> MonitoredValues;
        public List<string> MonitoredValuesNames;

        public List<string> LoggedValues;
        public List<string> LoggedValuesTimes;
    }
}
