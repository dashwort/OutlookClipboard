using EmailMemoryClass;
using EmailMemoryClass.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUI.ViewModels
{
    public class UserConfigViewModel
    {
        public UserConfigViewModel()
        {

        }

        private AccountConfig _config;

        public AccountConfig AccountConfiguration
        {
            get { return _config; }
            set { _config = value; }
        }



    }
}
