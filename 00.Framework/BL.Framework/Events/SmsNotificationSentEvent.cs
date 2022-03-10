namespace BL.Framework.Events
{
	public class SmsNotificationSentEvent : DomainEvent
	{
		public string To { get; set; }
		public string Message { get; set; }
	}
}
