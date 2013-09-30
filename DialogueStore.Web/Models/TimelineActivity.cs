using System;

namespace DialogueStore.Web.Models
{
    public class TimelineActivity
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectDescriptor { get; set; }
        public string Verb { get; set; }
        public int? ObjectId { get; set; }
        public string ObjectDescriptor { get; set; }
        public TimelineActivityObjectType ObjectType { get; set; }
        public DateTime Date { get; set; }
    }

    public enum TimelineActivityObjectType
    {
        Item
    }
}