﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Bitbucket.Cloud.Net.Common.Models;
using Bitbucket.Cloud.Net.Models.v2;
using Flurl.Http;

// ReSharper disable once CheckNamespace
namespace Bitbucket.Cloud.Net
{
	public partial class BitbucketCloudClient
	{
		private IFlurlRequest GetVersionsUrl(string workspaceId, string repositorySlug) => GetBaseUrl($"2.0/repositories/{workspaceId}/{repositorySlug}/versions");

		public async Task<IEnumerable<Version>> GetRepositoryVersionsAsync(string workspaceId, string repositorySlug, int? maxPages = null)
		{
			return await GetPagedResultsAsync(maxPages, GetVersionsUrl(workspaceId, repositorySlug), async req =>
					await req
						.GetJsonAsync<PagedResults<Version>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<Version> GetRepositoryVersionAsync(string workspaceId, string repositorySlug, string versionId)
		{
			return await GetVersionsUrl(workspaceId, repositorySlug)
				.AppendPathSegment($"{versionId}")
				.GetJsonAsync<Version>()
				.ConfigureAwait(false);
		}
	}
}
