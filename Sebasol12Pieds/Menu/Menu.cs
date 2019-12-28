using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebasol12Pieds
{
    public class Menu
    {
        public Menu()
        {
            _options = new List<Option>();
        }
        public Menu(string title) : this()
        {
            _title = title;
        }

        public void Display()
        {
            Stop = false;
            do
            {
                Console.Clear();
                if (_title != null)
                    Console.WriteLine(_title);
                int index = 1;
                foreach (Option option in _options)
                {
                    Console.WriteLine(index + ". " + option.Description);
                    index++;
                }
                _options[ConsoleUtils.ReadInt(1, _options.Count) - 1].Action.Invoke();
            } while (!Stop);
        }

        public void AddOption(Option option)
        {
            _options.Add(option);
        }

        public bool Stop { get; set; }

        private List<Option> _options;

        private string _title;
    }
}
