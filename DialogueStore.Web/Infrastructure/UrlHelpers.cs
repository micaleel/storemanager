using System;
using System.Web.Mvc;
using DialogueStore.Web.Models;

namespace DialogueStore.Web.Infrastructure
{
    public static class UrlHelpers
    {
        public static string ObjectLinkFor(this UrlHelper url, TimelineActivity activity)
        {
            switch (activity.ObjectType)
            {
                case TimelineActivityObjectType.Item:
                    return url.Action("Details", "Items", new { id = activity.ObjectId });
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}