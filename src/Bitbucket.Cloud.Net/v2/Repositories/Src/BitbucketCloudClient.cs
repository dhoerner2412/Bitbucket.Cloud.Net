using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bitbucket.Cloud.Net.Common.Models;
using Bitbucket.Cloud.Net.Models.v2;
using Flurl.Http;

// ReSharper disable once CheckNamespace
namespace Bitbucket.Cloud.Net
{
	public partial class BitbucketCloudClient
	{
		private IFlurlRequest GetSourceUrl(string workspaceId, string repositorySlug, SourceFormats? format = null) => GetBaseUrl($"2.0/repositories/{workspaceId}/{repositorySlug}/src")
			.SetQueryParam("format", format);

        private IFlurlRequest GetSourceUrl(string workspaceId, string repositorySlug, string node, string path, SourceFormats? format = null) => GetBaseUrl($"2.0/repositories/{workspaceId}/{repositorySlug}/src/{node}/{path}")
            .SetQueryParam("format", format);

    public async Task<IEnumerable<Source>> GetRepositorySourceAsync(string workspaceId, string repositorySlug, SourceFormats? format = null, int? maxPages = null)
	{
			return await GetPagedResultsAsync(maxPages, GetSourceUrl(workspaceId, repositorySlug, format), async req =>
					await req
						.GetJsonAsync<PagedResults<Source>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
	}

    public async Task<IEnumerable<Source>> GetRepositorySourceAsync(string workspaceId, string repositorySlug, string node, string path, SourceFormats? format = null, int? maxPages = null)
    {
      return await GetPagedResultsAsync(maxPages, GetSourceUrl(workspaceId, repositorySlug, node, path, format), async req =>
              await req
                  .GetJsonAsync<PagedResults<Source>>()
                  .ConfigureAwait(false))
          .ConfigureAwait(false);
    }

    public async Task<Stream> ReadRepositorySourceAsync(string workspaceId, string repositorySlug, string node, string path)
      {
        return await GetSourceUrl(workspaceId, repositorySlug, node, path).
            GetStreamAsync()
            .ConfigureAwait(false);
      }
  }
}
