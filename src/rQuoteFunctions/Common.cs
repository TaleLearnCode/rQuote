using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace TaleLearnCode.rQuote
{

	public static class Common
	{

		public static QuoteTableRow GetQuote(string channelName, int quoteId)
		{
			TableClient tableClient;
			tableClient = new TableClient(new Uri(Environment.GetEnvironmentVariable("TableStorageUrl")),
					Environment.GetEnvironmentVariable("QuoTableName"),
					new TableSharedKeyCredential(Environment.GetEnvironmentVariable("AccountName"), Environment.GetEnvironmentVariable("AccountKey")));
			return tableClient.Query<QuoteTableRow>(s => s.PartitionKey == channelName && s.RowKey == quoteId.ToString()).FirstOrDefault();
		}

		public static async System.Threading.Tasks.Task<QuoteTableRow> GetRandomQuoteAsync(string channelName)
		{

			BlobContainerClient container = new BlobContainerClient(Environment.GetEnvironmentVariable("BlobConnectionString"), "quoteids");
			BlobClient blob = container.GetBlobClient(channelName);
			BlobDownloadInfo download = blob.Download();
			Stream stream = new MemoryStream();
			QuoteId quoteId;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				download.Content.CopyTo(memoryStream);
				memoryStream.Position = 0;
				quoteId = JsonConvert.DeserializeObject<QuoteId>(await new StreamReader(memoryStream).ReadToEndAsync());
			}

			int randomId = new Random().Next(1, quoteId.MaxId);
			return GetQuote(channelName, randomId);
		}

	}

}