using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Xml.Linq;
using ReactiveUI;

namespace FlickrBrowser
{
    public class FlickrPhoto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }

    public class AppViewModel : ReactiveObject
    {
        private string _searchTerm;
        private ReactiveList<string> _collection;

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { this.RaiseAndSetIfChanged(ref _searchTerm, value); }
        }

        public ReactiveCommand<string, List<FlickrPhoto>> ExecuteSearch { get; protected set; }

        private ObservableAsPropertyHelper<List<FlickrPhoto>> _searchResult;
        public List<FlickrPhoto> SearchResults => _searchResult.Value;

        private readonly ObservableAsPropertyHelper<Visibility> _spinnerVisibility;
        public Visibility SpinnerVisibility => _spinnerVisibility.Value;

        public AppViewModel()
        {
            ExecuteSearch = ReactiveCommand.CreateFromTask<string, List<FlickrPhoto>>(GetSearchResultsFromFlickr);


            this.WhenAnyValue(x => x.SearchTerm)
                .Throttle(TimeSpan.FromMilliseconds(800), RxApp.MainThreadScheduler)
                .Select(x => x?.Trim())
                .DistinctUntilChanged()
                .Where(x => !string.IsNullOrEmpty(x))
                .InvokeCommand(ExecuteSearch);

       
            _spinnerVisibility = ExecuteSearch.IsExecuting
                .Select(x => x ? Visibility.Visible : Visibility.Collapsed)
                .ToProperty(this, x => x.SpinnerVisibility, Visibility.Hidden);

            ExecuteSearch.ThrownExceptions.Subscribe(ex => { });

            _searchResult = ExecuteSearch.ToProperty(this, x => x.SearchResults, new List<FlickrPhoto>());
        }

        public static async Task<List<FlickrPhoto>> GetSearchResultsFromFlickr(string searchTerm)
        {
            var doc = await Task.Run(() => XDocument.Load(String.Format(CultureInfo.InvariantCulture,
                "http://api.flickr.com/services/feeds/photos_public.gne?tags={0}&format=rss_200",
                HttpUtility.UrlEncode(searchTerm))));

            if (doc.Root == null)
                return null;

            var titles = doc.Root.Descendants("{http://search.yahoo.com/mrss/}title")
                .Select(x => x.Value);

            var tagRegex = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            var descriptions = doc.Root.Descendants("{http://search.yahoo.com/mrss/}description")
                .Select(x => tagRegex.Replace(HttpUtility.HtmlDecode(x.Value), ""));

            var items = titles.Zip(descriptions,
                (t, d) => new FlickrPhoto { Title = t, Description = d }).ToArray();

            var urls = doc.Root.Descendants("{http://search.yahoo.com/mrss/}thumbnail")
                .Select(x => x.Attributes("url").First().Value);

            var ret = items.Zip(urls, (item, url) => { item.Url = url; return item; }).ToList();
            return ret;
        }

    }

   
}
