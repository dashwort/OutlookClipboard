using Caliburn.Micro;
using EmailMemoryClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUI.ViewModels;
using WpfUI.Views;

namespace WpfUI.Models
{
    public class DisplayContainer : Screen
    {
        private int _index;
        private EmailListViewModel _view;
        private bool _toLoad;
        private string _displayName;

        public int DisplayIndex
        {
            get { return _index; }
            set 
            { 
                _index = value;
                NotifyOfPropertyChange();
            }
        }

        public EmailListViewModel View
        {
            get { return _view; }
            set { _view = value; }
        }

        public bool ToLoad
        {
            get { return _toLoad; }
            set 
            {
                _toLoad = value;
                NotifyOfPropertyChange();
            }
        }

        public string DisplayNameView
        {
            get { return _displayName; }
            set 
            { 
                _displayName = value;
                NotifyOfPropertyChange();
            }
        }

        public DisplayContainer(AccountConfig account)
        {
                View = new EmailListViewModel(account);
                DisplayIndex = account.DisplayIndex;
                DisplayNameView = account.DisplayName;
                ToLoad = account.IsConfigured;
        }

        public DisplayContainer()
        {
            ToLoad = false;
        }
    }
}
