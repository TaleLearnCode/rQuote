using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TaleLearnCode.rQuote
{
	public static class RandomQuoteTimer
	{
		
		[FunctionName("RandomQuoteTimer")]
		public static async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
		{
			QuoteTableRow quoteTableRow = await Common.GetRandomQuoteAsync("TaleLearnCode");
			SendRandomQuote("TaleLearnCode", quoteTableRow);
			log.LogInformation($"Quote sent to {"TaleLearnCode"}: {quoteTableRow.Text} — {quoteTableRow.Author}");
		}

		private static void SendRandomQuote(string channelName, QuoteTableRow quoteTableRow)
		{
			SendMessage(channelName, $"{quoteTableRow.Text} — {quoteTableRow.Author}");
		}

		private static void SendMessage(string channelName, string message)
		{
			// TODO: Look DI for Azure Functions
			// TODO: Create a static twitch connection
			ConnectionCredentials credentials = new ConnectionCredentials(Environment.GetEnvironmentVariable("TwitchChannelName"), Environment.GetEnvironmentVariable("TwitchAccessToken"));
			TwitchClient twitchClient = new TwitchClient();
			twitchClient.Initialize(credentials, channelName);
			twitchClient.Connect();
			Thread.Sleep(1000);
			twitchClient.SendMessage(channelName, message);
			Thread.Sleep(5000);
			twitchClient.Disconnect();
		}

	}
}