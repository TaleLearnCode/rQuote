namespace TaleLearnCode.rQuote
{
	public class QuoteId
	{
		public string ChannelName { get; set; }
		public int MaxId { get; set; }
		public QuoteId() { }
		public QuoteId(string channelName)
		{
			ChannelName = channelName;
			MaxId = 0;
		}
	}
}