using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;

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
				[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request,
				ILogger log)
		{
			QuoteTableRow quoteTableRow = null;
			try
			{
				string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
				Quote input = JsonConvert.DeserializeObject<Quote>(requestBody);
				if (input == null) throw new ArgumentNullException();
				quoteTableRow = new QuoteTableRow() { PartitionKey = input.ChannelName, RowKey = DateTime.UtcNow.ToString("yyyyMMddHHmmssfffffff"), Text = input.Text };
			}
			catch (Exception ex)
			{
				log.LogError(ex.Message);
			}

			return quoteTableRow;
		}

	}

}