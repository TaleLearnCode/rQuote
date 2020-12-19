using Azure;
using Azure.Data.Tables;
using System;

namespace TaleLearnCode.rQuote
{
	public class QuoteTableRow : ITableEntity
	{

		public string PartitionKey { get; set; }

		public string RowKey { get; set; }

		public string Text { get; set; }

		public string Author { get; set; }
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }
	}

}