using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fritz.ObsProxy
{
	public class Worker : IHostedService
	{
		private readonly ILogger<Worker> _logger;
		private readonly ObsClient _ObsClient;
		private readonly BotClient _BotClient;
		private readonly IConfiguration _Configuration;

		public Worker(ILogger<Worker> logger, ObsClient obsClient, BotClient botClient, IConfiguration configuration)
		{
			_logger = logger;
			_ObsClient = obsClient;
			_BotClient = botClient;
			_Configuration = configuration;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{

			await _ObsClient.Connect();

			if (string.IsNullOrEmpty(_Configuration["ObsTest"]) || !bool.Parse(_Configuration["ObsTest"]))
			{
				await _BotClient.Connect();
			} else {
				_ObsClient.TakeScreenshot();
			}

		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{

			_ObsClient.Dispose();
			await _BotClient.DisposeAsync();

		}

	}
}
