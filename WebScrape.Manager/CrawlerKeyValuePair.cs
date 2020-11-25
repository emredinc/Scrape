using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebScrape.Manager
{
    public class CrawlerKeyValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class CrawlerKeyValuePairCollection
    {
        private List<CrawlerKeyValuePair> _list = new List<CrawlerKeyValuePair>();
        public void Add(string key, string value)
        {
            _list.Add(new CrawlerKeyValuePair() { Key = key, Value = value });
        }
        public void Clear()
        {
            _list.Clear();
        }
        public List<CrawlerKeyValuePair> List => _list;
    }
}
