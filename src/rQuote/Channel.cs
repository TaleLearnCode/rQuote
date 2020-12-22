using System;

namespace TaleLearnCode.rQuote
{

	public class Channel
	{

		public string ChannelName { get; set; }

		public int OnlineRandomInterval { get; set; }

		public int OfflineRandomInterval { get; set; }

		public DateTime LastRandomMessage { get; set; }

		public ChannelTableRow ToChannelTableRow()
		{
			return new ChannelTableRow()
			{
				PartitionKey = "rQuote",
				RowKey = ChannelName,
				OnlineRandomInterval = OnlineRandomInterval,
				OfflineRandomInterval = OfflineRandomInterval,
				LastRandomMessage = LastRandomMessage
			};
		}

	}

}