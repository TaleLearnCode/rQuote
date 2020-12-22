using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TaleLearnCode.rQuote
{
	public static class RandomQuoteTimer
	{

		[FunctionName("RandomQuoteTimer")]
		public static async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
		{
			TwitchBot twitchBot = new TwitchBot("TaleLearnCode");
			QuoteTableRow quoteTableRow = await Common.GetRandomQuoteAsync("TaleLearnCode");
			string message = $"{quoteTableRow.Text} — {quoteTableRow.Author}";
			twitchBot.SendMessage(message);
			log.LogInformation($"Quote sent to {"TaleLearnCode"}: {message}");
		}

	}
}