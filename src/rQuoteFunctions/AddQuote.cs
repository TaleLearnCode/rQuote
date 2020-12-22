using Azure.Data.Tables;
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

	public static class AddQuote
	{
		[FunctionName("AddQuote")]
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
				if (quoteId == null) quoteId = new QuoteId(channelName);
				quoteId.MaxId = ++quoteId.MaxId;

				quoteTableRow = new QuoteTableRow()
				{
					PartitionKey = channelName,
					RowKey = quoteId.MaxId.ToString(),
					Text = input.Text,
					Author = input.Author
				};

				TableClient tableClient;
				tableClient = new TableClient(new Uri(Environment.GetEnvironmentVariable("TableStorageUrl")),
						Environment.GetEnvironmentVariable("QuoTableName"),
						new TableSharedKeyCredential(Environment.GetEnvironmentVariable("AccountName"), Environment.GetEnvironmentVariable("AccountKey")));
				tableClient.UpsertEntity(quoteTableRow);

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