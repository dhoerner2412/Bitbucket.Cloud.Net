﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bitbucket.Cloud.Net.Common.Converters;
using Bitbucket.Cloud.Net.Common.Models;
using Bitbucket.Cloud.Net.Common.MultiPart;
using Bitbucket.Cloud.Net.Models;
using Flurl.Http;

// ReSharper disable once CheckNamespace
namespace Bitbucket.Cloud.Net
{
	public partial class BitbucketCloudClient
	{
		private IFlurlRequest GetSnippetsUrl() => GetBaseUrl("2.0/snippets");

		private IFlurlRequest GetSnippetsUrl(string path) => GetSnippetsUrl()
			.AppendPathSegment(path);

		public async Task<IEnumerable<Snippet>> GetSnippetsAsync(Roles? role = null, int? maxPages = null)
		{
			var queryParamValues = new Dictionary<string, object>
			{
				[nameof(role)] = RolesConverter.ConvertToString(role)
			};

			return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
					await GetSnippetsUrl()
						.SetQueryParams(qpv)
						.GetJsonAsync<PagedResults<Snippet>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<IEnumerable<Snippet>> GetWorkspaceSnippetsAsync(string workspaceId, Roles? role = null, int? maxPages = null)
		{
			var queryParamValues = new Dictionary<string, object>
			{
				[nameof(role)] = RolesConverter.ConvertToString(role)
			};

			return await GetPagedResultsAsync(maxPages, queryParamValues, async qpv =>
					await GetSnippetsUrl(workspaceId)
						.SetQueryParams(qpv)
						.GetJsonAsync<PagedResults<Snippet>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<object> GetWorkspaceSnippetAsync(string workspaceId, string snippetId, SnippetsAccept accept = SnippetsAccept.ApplicationJson, Func<ContentPart, object> contentPartsHandler = null)
		{
			string acceptString = SnippetsAcceptConverter.ConvertToString(accept);
			var request = GetSnippetsUrl(workspaceId)
				.AppendPathSegment(snippetId)
				.WithHeader("Accept", acceptString);

			return accept switch
			{
				SnippetsAccept.MultiPartRelated => await request
					.GetMultiPartRelatedAsync()
					.WithContentPartsAsync(contentPartsHandler)
					.ConfigureAwait(false),

				SnippetsAccept.MultiPartFormData => await request
					.GetMultiPartFormDataAsync()
					.WithContentPartsAsync(contentPartsHandler)
					.ConfigureAwait(false),

				_ => await request
					.GetJsonAsync<Snippet>()
					.ConfigureAwait(false)
			};
		}
	}
}