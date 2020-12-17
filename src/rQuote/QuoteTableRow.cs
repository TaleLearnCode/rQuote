using System;

namespace TaleLearnCode.rQuote
{
	public class QuoteTableRow
	{

		public string PartitionKey { get; set; }

		public string RowKey { get; set; }

		public string Text { get; set; }

	}
}