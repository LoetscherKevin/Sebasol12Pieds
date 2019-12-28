using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public interface IWaterHeater
    {
        double InputTemperature { get; }
        double OutputTemperature { get; }
        double Flow { get; }
        double Power { get; }
    }
}
