using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public interface IHeatingSystem
    {
        IAcumulator IAccumulator { get; }
        IWaterHeater ISolarPanel { get; }
        IWaterHeater IWaterStove { get; }
        IWaterHeater IGazBoiler { get; }
        IHome IHome { get; }
    }
}
