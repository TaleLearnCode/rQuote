using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TaleLearnCode.rQuote
{

	public static class AddQuote
	{
		[FunctionName("AddQuote")]
		public static async Task<IActionResult> RunAsync(
				[HttpTrigger(AuthorizationLevel.Function, "post", Route = "AddQuote/{channelName}")] HttpRequest request,
				[Blob("quoteids/{channelName}", FileAccess.Read, Connection = "TableStorageKey")] Stream readQuoteId,
				[Blob("quoteids/{channelName}", FileAccess.Write, Connection = "TableStorageKey")] Stream writeQuoteId,
				string channelName,
				ILogger log)
		{
			QuoteTableRow quoteTableRow = null;
			string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
			Quote input = JsonConvert.DeserializeObject<Quote>(requestBody);
			if (input == null) throw new ArgumentNullException();

			QuoteId quoteId = JsonConvert.DeserializeObject<QuoteId>(await new StreamReader(readQuoteId).ReadToEndAsync());
			if (quoteId == null) quoteId = new QuoteId(channelName);
			quoteId.MaxId = ++quoteId.MaxId;

			quoteTableRow = new QuoteTableRow()
			{
				PartitionKey = channelName,
				RowKey = quoteId.MaxId.ToString(),
				Text = input.Text,
				Author = input.Author
			};

			Common.GetTableClient(Environment.GetEnvironmentVariable("QuoteTableName")).UpsertEntity(quoteTableRow);

			writeQuoteId.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(quoteId)));

			return new OkObjectResult(new Quote(quoteTableRow));

		}

	}

}