using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TaleLearnCode.rQuote
{
	public static class GetRandomQuote
	{
		[FunctionName("GetRandomQuote")]
		public static async Task<IActionResult> Run(
				[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Random/{channelName}")] HttpRequest request,
				[Blob("quoteids/{channelName}", FileAccess.Read, Connection = "TableStorageKey")] Stream readQuoteId,
				string channelName,
				ILogger log)
		{
			//log.LogInformation("C# HTTP trigger function processed a request.");

			//string name = request.Query["name"];

			//string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
			//dynamic data = JsonConvert.DeserializeObject(requestBody);
			//name = name ?? data?.name;

			//string responseMessage = string.IsNullOrEmpty(name)
			//		? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
			//		: $"Hello, {name}. This HTTP triggered function executed successfully.";

			//return new OkObjectResult(responseMessage);

			QuoteId quoteId = JsonConvert.DeserializeObject<QuoteId>(await new StreamReader(readQuoteId).ReadToEndAsync());
			int randomId = new Random().Next(1, quoteId.MaxId);


			TableClient tableClient;
			tableClient = new TableClient(new Uri(Environment.GetEnvironmentVariable("TableStorageUrl")),
					Environment.GetEnvironmentVariable("QuoTableName"),
					new TableSharedKeyCredential(Environment.GetEnvironmentVariable("AccountName"), Environment.GetEnvironmentVariable("AccountKey")));
			QuoteTableRow quoteTableRow = tableClient.Query<QuoteTableRow>(s => s.PartitionKey == channelName && s.RowKey == randomId.ToString()).FirstOrDefault();

			return new OkObjectResult(quoteTableRow);

		}
	}
}
