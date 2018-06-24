using System.Collections.Generic;
using Fritz.StreamTools.Hubs;
using Fritz.StreamTools.Models;
using Microsoft.AspNetCore.SignalR;

namespace Fritz.StreamTools.Services
{
	public class GithubyMcGithubFaceClient
	{

		public GithubyMcGithubFaceClient(IHubContext<GithubyMcGithubFace> mcGitHubContext)
		{

			this.McGitHubContext = mcGitHubContext;

		}

		private IHubContext<GithubyMcGithubFace> McGitHubContext { get; }

		internal void UpdateGitHub(IEnumerable<GitHubInformation> contributors)
		{
			McGitHubContext.Clients.Group("github").SendAsync("OnGitHubUpdated", contributors);
		}

	}

}
