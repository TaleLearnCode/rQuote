using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaleLearnCode.rQuote
{
	public static class RandomQuoteTimer
	{

		[FunctionName("RandomQuoteTimer")]
		public static async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
		{

			List<ChannelTableRow> channels = Common.GetTableClient(Environment.GetEnvironmentVariable("ChannelTableName")).Query<ChannelTableRow>(s => s.PartitionKey == "rQuote").ToList();
			foreach (ChannelTableRow channel in channels)
			{
				if (DateTime.UtcNow >= channel.LastRandomMessage.AddMinutes(channel.MessageFrequency))
				{
					await SendRandomQuoteAsync(channel.RowKey, log);
					channel.LastRandomMessage = DateTime.UtcNow;
					Common.GetTableClient(Environment.GetEnvironmentVariable("ChannelTableName")).UpsertEntity(channel);
				}
			}

		}

		private static async Task SendRandomQuoteAsync(string channelName, ILogger log)
		{
			TwitchBot twitchBot = new TwitchBot(channelName);
			QuoteTableRow quoteTableRow = await Common.GetRandomQuoteAsync(channelName);
			string message = $"\"{quoteTableRow.Text}\" — {quoteTableRow.Author}";
			twitchBot.SendMessage(message);
			log.LogInformation($"Quote sent to {channelName}: {message}");

		}

	}
}