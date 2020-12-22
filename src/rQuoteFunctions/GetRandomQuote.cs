using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TaleLearnCode.rQuote
{

	public static class GetRandomQuote
	{
		[FunctionName("GetRandomQuote")]
		public static async Task<IActionResult> Run(
				[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "GetRandom/{channelName}")] HttpRequest request,
				[Blob("quoteids/{channelName}", FileAccess.Read, Connection = "TableStorageKey")] Stream readQuoteId,
				string channelName,
				ILogger log)
		{

			QuoteId quoteId = JsonConvert.DeserializeObject<QuoteId>(await new StreamReader(readQuoteId).ReadToEndAsync());
			int randomId = new Random().Next(1, quoteId.MaxId);

			return new OkObjectResult(Common.GetQuote(channelName, randomId));

		}

	}

}