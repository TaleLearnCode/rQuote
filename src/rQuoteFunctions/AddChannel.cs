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

	public static class AddChannel
	{
		[FunctionName("AddChannel")]
		public static async Task<IActionResult> Run(
				[HttpTrigger(AuthorizationLevel.Function, "post", Route = "AddChannel/{channelName}")] HttpRequest request,
				[Blob("quoteids/{channelName}", FileAccess.Write, Connection = "TableStorageKey")] Stream writeQuoteId,
				string channelName,
				ILogger log)
		{

			string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
			Channel channel = JsonConvert.DeserializeObject<Channel>(requestBody);
			if (channel == null)
				channel = new Channel()
				{
					ChannelName = channelName
				};
			channel.LastRandomMessage = DateTime.UtcNow;
			Common.GetTableClient(Environment.GetEnvironmentVariable("ChannelTableName")).UpsertEntity(channel.ToChannelTableRow());

			writeQuoteId.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new QuoteId(channelName))));

			return new OkObjectResult(channel);

		}

	}

}