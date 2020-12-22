using System;
using System.Diagnostics;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TaleLearnCode.rQuote
{

	public class TwitchBot
	{

		private TwitchClient _twitchClient = new TwitchClient();
		private string _destinationChannel;

		public TwitchBot(string destinationChannel)
		{
			_destinationChannel = destinationChannel;
			ConnectionCredentials credentials = new ConnectionCredentials(Environment.GetEnvironmentVariable("TwitchChannelName"), Environment.GetEnvironmentVariable("TwitchAccessToken"));
			_twitchClient.OnMessageSent += TwitchClient_OnMessageSent;
			_twitchClient.Initialize(credentials, _destinationChannel);
			_twitchClient.Connect();
		}

		public void SendMessage(string message)
		{
			bool sendMessage = true;
			Stopwatch stopwatch = Stopwatch.StartNew();
			// Need to wait for the connection to be made; waiting for up to 5 seconds for that to happen
			while (!_twitchClient.IsConnected && sendMessage)
				if (stopwatch.ElapsedMilliseconds >= 5000) sendMessage = false;
			if (sendMessage) _twitchClient.SendMessage(_destinationChannel, message);
		}

		private void TwitchClient_OnMessageSent(object sender, OnMessageSentArgs e)
		{
			if (_twitchClient.IsConnected) _twitchClient.Disconnect();
		}

	}

}