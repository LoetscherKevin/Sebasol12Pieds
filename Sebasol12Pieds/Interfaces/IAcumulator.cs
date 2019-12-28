using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public interface IAcumulator
    {
        double TopTemperature { get; }
        double CenterTemperature { get; }
        double BottomTemperature { get; }
    }
}
