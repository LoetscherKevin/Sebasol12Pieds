using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public class Option
    {
        public Option(string description, Action action)
        {
            Description = description;
            Action = action;
        }

        public string Description { get; }
        public Action Action { get; }
    }
}
