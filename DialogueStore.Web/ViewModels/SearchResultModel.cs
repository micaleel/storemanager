using DialogueStore.Web.Models;
using System.Collections.Generic;

namespace DialogueStore.Web.ViewModels
{
    public class SearchResultModel
    {
        public string Query { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}