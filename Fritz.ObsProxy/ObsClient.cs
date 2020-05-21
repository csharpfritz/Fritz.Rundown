﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;
using OBS.WebSocket.NET;
using OBS.WebSocket.NET.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fritz.ObsProxy
{

	public class ObsClient : IDisposable
	{
		private bool _DisposedValue;
		private ObsWebSocket _OBS;

		private readonly ILogger _Logger;
		private readonly IConfiguration _Configuration;
		private readonly string _IpAddress;

		public ObsClient(ILoggerFactory loggerFactory, IConfiguration configuration )
		{
			_Logger = loggerFactory.CreateLogger("ObsClient");
			_Configuration = configuration;
			_IpAddress = string.IsNullOrEmpty(configuration["ObsIpAddress"]) ? "127.0.0.1:4444" : configuration["ObsIpAddress"];
		}

		/// <summary>
		/// Establish a connection to OBS
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public Task Connect() {

			_OBS = new ObsWebSocket();
			_OBS.Connect($"ws://{_IpAddress}", "");

			return Task.CompletedTask;

		}

		public string ImageFolderLocation => _Configuration["ImageFolder"];

		public string CameraSource => _Configuration["CameraSource"];


		public string TakeScreenshot() {

			SourceScreenshotResponse response = null;
			if (string.IsNullOrEmpty(ImageFolderLocation)) {
				response = _OBS.Api.TakeSourceScreenshot(CameraSource,embedPictureFormat: "png");
			} else
			{

				try
				{
					response = _OBS.Api.TakeSourceScreenshot(CameraSource, saveToFilePath: ImageFolderLocation + "\\test.png");
				} catch (Exception e) {
					_Logger.LogError(e, "Error while taking screenshot");
					return null;
				}

			}

			return response.ImageData;

		}


		#region Dispose OBS Connection

		protected virtual void Dispose(bool disposing)
		{
			if (!_DisposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				_OBS.Disconnect();
				_OBS = null;
				_DisposedValue = true;
			}
		}

		~ObsClient()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		#endregion

	}

}
