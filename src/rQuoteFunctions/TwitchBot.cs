using System;
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
			while (!_twitchClient.IsConnected)
			{
				// wait until we get connected - probably should add something so we do not wait too long
			}
			_twitchClient.SendMessage(_destinationChannel, message);
		}

		private void TwitchClient_OnMessageSent(object sender, OnMessageSentArgs e)
		{
			if (_twitchClient.IsConnected) _twitchClient.Disconnect();
		}

	}

}