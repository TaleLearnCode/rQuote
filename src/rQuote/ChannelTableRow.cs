using Azure;
using Azure.Data.Tables;
using System;

namespace TaleLearnCode.rQuote
{

	public class ChannelTableRow : ITableEntity
	{

		public string PartitionKey { get; set; }

		public string RowKey { get; set; }

		public DateTimeOffset? Timestamp { get; set; }

		public ETag ETag { get; set; }

		public int OnlineRandomInterval { get; set; }

		public int OfflineRandomInterval { get; set; }

		public DateTime LastRandomMessage { get; set; }

	}

}