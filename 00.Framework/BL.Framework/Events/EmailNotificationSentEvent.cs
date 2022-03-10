namespace BL.Framework.Events
{
    public class EmailNotificationSentEvent : DomainEvent
    {
        public string To { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
