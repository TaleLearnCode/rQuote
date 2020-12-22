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

		public int MessageFrequency { get; set; }

		public DateTime LastRandomMessage { get; set; }

	}

}