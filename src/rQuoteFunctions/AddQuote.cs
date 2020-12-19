using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace TaleLearnCode.rQuote
{

	//public class Quote
	//{
	//	public string PartitionKey { get; set; }

	//	public string RowKey { get; set; }

	//	public string Text { get; set; }
	//}


	public static class AddQuote
	{
		[FunctionName("AddQuote")]
		[return: Table("QuoTable", Connection = "TableStorageKey")]
		public static async System.Threading.Tasks.Task<QuoteTableRow> RunAsync(
				[HttpTrigger(AuthorizationLevel.Function, "post", Route = "Add/{channelName}")] HttpRequest request,
				[Blob("quoteids/{channelName}", FileAccess.Read, Connection = "TableStorageKey")] Stream readQuoteId,
				[Blob("quoteids/{channelName}", FileAccess.Write, Connection = "TableStorageKey")] Stream writeQuoteId,
				string channelName,
				ILogger log)
		{
			QuoteTableRow quoteTableRow = null;
			try
			{
				string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
				QuoteInput input = JsonConvert.DeserializeObject<QuoteInput>(requestBody);
				if (input == null) throw new ArgumentNullException();


				QuoteId quoteId = JsonConvert.DeserializeObject<QuoteId>(await new StreamReader(readQuoteId).ReadToEndAsync());
				int newQuoteId = quoteId.MaxId += 1;
				//int newQuoteId2 = ++quoteId.MaxId;

				quoteTableRow = new QuoteTableRow()
				{
					PartitionKey = channelName,
					RowKey = newQuoteId.ToString(),
					Text = input.Text,
					Author = input.Author
				};

				quoteId.MaxId = newQuoteId;
				writeQuoteId.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(quoteId)));

			}
			catch (Exception ex)
			{
				log.LogError(ex.Message);
			}

			return quoteTableRow;

		}


	}

}