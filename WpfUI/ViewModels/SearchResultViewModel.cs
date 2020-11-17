using Caliburn.Micro;
using EmailMemoryClass.outlookSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUI.ViewModels
{
    public class SearchResultViewModel : Screen
    {
        private static SearchTracking _searchTracker;

        public SearchResultViewModel()
        {
            SearchItems = new BindableCollection<SearchResult>();
            _searchTracker = new SearchTracking();
            _searchTracker.OnIntervalSearchComplete += UpdateUI;
        }

        void UpdateUI(object sender, EventArgs e)
        {
            var results = sender as List<SearchResult>;

            foreach (var item in results)
            {
                SearchItems.Add(item);
            }

            SearchItems.OrderByDescending(x => x.Time);
        }

        BindableCollection<SearchResult> _searchItems;
        SearchResult _searchItem;

        public BindableCollection<SearchResult> SearchItems
        {
            get { return _searchItems; }
            set 
            { 
                _searchItems = value;
                NotifyOfPropertyChange();
            }
        }

        public SearchResult SelectedSearch
        {
            get { return _searchItem; }
            set
            {
                _searchItem = value;
                NotifyOfPropertyChange();
            }
        }

        
    }
}
