using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMemoryClass
{
    public class AccountConfig
    {
        private string _emailAddress;
        private int _timerInterval;
        private int _searchSize;
        private int _searchTime;
        private string _searchTag;
        private int _displayItems;
        private bool _isConfigured;
        private string _displayName;
        private int _displayIndex;

        public AccountConfig()
        {
            if(!IsConfigured)
            {
                SearchSize = 1000;
                DisplayItems = 6;
                TimerInterval = 30;
                DisplayIndex = 1;
                SearchTag = "Enter a search tag";
                EmailAddress = "example@exampledomain.com";
            }
        }

        public int DisplayIndex
        {
            get { return _displayIndex; }
            set 
            { 
                if (value >= 1 && value <=3)
                {
                    _displayIndex = value;
                } else
                {
                    if(value > 3)
                        _displayIndex = 3;

                    if(value < 1)
                        _displayIndex = 1;
                }
            }

        }

        public bool IsConfigured
        {
            get { return _isConfigured; }
            set { _isConfigured = value; }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        public string SearchTag
        {
            get { return _searchTag; }
            set { _searchTag = value; }
        }

        public int SearchSize
        {
            get { return _searchSize; }
            set { _searchSize = value; }
        }

        public int SearchTime
        {
            get { return _searchTime; }
            set { _searchTime = value; }
        }

        public int TimerInterval
        {
            get { return _timerInterval; }
            set { _timerInterval = value; }
        }

        public int DisplayItems
        {
            get { return _displayItems; }
            set { _displayItems = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }


    }
}
