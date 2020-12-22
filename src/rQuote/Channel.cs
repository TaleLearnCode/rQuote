using System;

namespace TaleLearnCode.rQuote
{

	public class Channel
	{

		public string ChannelName { get; set; }

		public int MessageFrequency { get; set; }

		public DateTime LastRandomMessage { get; set; }

		public ChannelTableRow ToChannelTableRow()
		{
			return new ChannelTableRow()
			{
				PartitionKey = "rQuote",
				RowKey = ChannelName,
				MessageFrequency = MessageFrequency,
				LastRandomMessage = LastRandomMessage
			};
		}

	}

}