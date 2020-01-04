using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public interface IHome
    {
        double InsideTemperature { get; }
        double OutsideTemperature { get; }
    }
}
