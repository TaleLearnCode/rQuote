namespace TaleLearnCode.rQuote
{

	public class Quote
	{

		public string Id { get; set; }

		public string Text { get; set; }

		public string Author { get; set; }

		public Quote() { }

		public Quote(QuoteTableRow quoteTableRow)
		{
			Id = quoteTableRow.RowKey;
			Text = quoteTableRow.Text;
			Author = quoteTableRow.Author;
		}

	}

}