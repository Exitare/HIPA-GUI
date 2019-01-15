using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA
{
    class SelectedFiles
    {

        private int _ID;
        private string _Name;

        public SelectedFiles(int ID, string Name)
        {
            _ID = ID;
            _Name = Name;
        }

        public string Name { get => _Name; set => _Name = value; }
        public int ID { get => _ID; set => _ID = value; }
    }
}
