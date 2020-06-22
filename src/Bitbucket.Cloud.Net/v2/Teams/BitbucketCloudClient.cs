﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Bitbucket.Cloud.Net.Common.Converters;
using Bitbucket.Cloud.Net.Common.Models;
using Bitbucket.Cloud.Net.Models.v2;
using Flurl.Http;

// ReSharper disable once CheckNamespace
namespace Bitbucket.Cloud.Net
{
	public partial class BitbucketCloudClient
	{
		private IFlurlRequest GetTeamsUrl() => GetBaseUrl("2.0/teams");

		private IFlurlRequest GetTeamsUrl(string userName) => GetTeamsUrl().AppendPathSegment(userName);

		public async Task<IEnumerable<Team>> GetTeamsAsync(Permissions role, int? maxPages = null)
		{
			var queryParamValues = new Dictionary<string, object>
			{
				[nameof(role)] = PermissionsConverter.ToString(role)
			};

			return await GetPagedResultsAsync(maxPages, GetTeamsUrl(), async req =>
					await req
						.SetQueryParams(queryParamValues)
						.GetJsonAsync<PagedResults<Team>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<Team> GetTeamPublicInformationAsync(string teamName)
		{
			return await GetTeamsUrl(teamName)
				.GetJsonAsync<Team>()
				.ConfigureAwait(false);
		}

		public async Task<IEnumerable<User>> GetTeamFollowersAsync(string teamName, int? maxPages = null)
		{
			return await GetPagedResultsAsync(maxPages, 
                        GetTeamsUrl(teamName)
                        .AppendPathSegment("/followers"), async req =>
					await req
						.GetJsonAsync<PagedResults<User>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<IEnumerable<User>> GetTeamFollowingAsync(string teamName, int? maxPages = null)
		{
			return await GetPagedResultsAsync(maxPages, 
                        GetTeamsUrl(teamName)
                        .AppendPathSegment("/following"), async req =>
					await req
						.GetJsonAsync<PagedResults<User>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<Webhook> CreateTeamWebhookAsync(string userName, Webhook webhook)
		{
			var response = await GetTeamsUrl()
				.AppendPathSegment($"/{userName}/hooks")
				.PostJsonAsync(webhook)
				.ConfigureAwait(false);

			return await HandleResponseAsync<Webhook>(response).ConfigureAwait(false);
		}

		public async Task<IEnumerable<Webhook>> GetTeamWebhooksAsync(string userName, int? maxPages = null)
		{
			return await GetPagedResultsAsync(maxPages, 
                        GetTeamsUrl()
                        .AppendPathSegment($"/{userName}/hooks"), async req =>
					await req
						.GetJsonAsync<PagedResults<Webhook>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<Webhook> UpdateTeamWebhookAsync(string userName, string webhookId, Webhook webhook)
		{
			var response = await GetTeamsUrl()
				.AppendPathSegment($"/{userName}/hooks/{webhookId}")
				.PutJsonAsync(webhook)
				.ConfigureAwait(false);

			return await HandleResponseAsync<Webhook>(response).ConfigureAwait(false);
		}

		public async Task<Webhook> GetTeamWebhookAsync(string userName, string webhookId)
		{
			return await GetTeamsUrl()
				.AppendPathSegment($"/{userName}/hooks/{webhookId}")
				.GetJsonAsync<Webhook>()
				.ConfigureAwait(false);
		}

		public async Task<bool> DeleteTeamWebhookAsync(string userName, string webhookId)
		{
			var response = await GetTeamsUrl()
				.AppendPathSegment($"/{userName}/hooks/{webhookId}")
				.DeleteAsync()
				.ConfigureAwait(false);

			return await HandleResponseAsync<bool>(response).ConfigureAwait(false);
		}

		public async Task<IEnumerable<TeamPermission>> GetTeamPermissionsAsync(string userName, int? maxPages = null, string q = null, string sort = null)
		{
			var queryParamValues = new Dictionary<string, object>
			{
				[nameof(q)] = q,
				[nameof(sort)] = sort
			};

			return await GetPagedResultsAsync(maxPages, 
                         GetTeamsUrl(userName)
                        .AppendPathSegment("/permissions"), async req =>
					await req
						.SetQueryParams(queryParamValues)
						.GetJsonAsync<PagedResults<TeamPermission>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<IEnumerable<TeamRepositoryPermission>> GetTeamRepositoryPermissionsAsync(string userName, int? maxPages = null, string q = null, string sort = null)
		{
			var queryParamValues = new Dictionary<string, object>
			{
				[nameof(q)] = q,
				[nameof(sort)] = sort
			};

			return await GetPagedResultsAsync(maxPages, 
                        GetTeamsUrl(userName)
                        .AppendPathSegment("/permissions")
                        .AppendPathSegment("/repositories"), async req =>
					await req
						.SetQueryParams(queryParamValues)
						.GetJsonAsync<PagedResults<TeamRepositoryPermission>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<IEnumerable<TeamRepositoryPermission>> GetTeamRepositoryPermissionsAsync(string userName, string repositorySlug, int? maxPages = null, string q = null, string sort = null)
		{
			var queryParamValues = new Dictionary<string, object>
			{
				[nameof(q)] = q,
				[nameof(sort)] = sort
			};

			return await GetPagedResultsAsync(maxPages,
                         GetTeamsUrl(userName)
                        .AppendPathSegment("/permissions")
                        .AppendPathSegment("/repositories")
                        .AppendPathSegment(repositorySlug), async req =>
					await req
						.SetQueryParams(queryParamValues)
						.GetJsonAsync<PagedResults<TeamRepositoryPermission>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<Variable> CreateTeamVariableAsync(string userName, Variable variable)
		{
			var response = await GetTeamsUrl()
				.AppendPathSegment($"/{userName}/pipelines_config/variables/")
				.PostJsonAsync(variable)
				.ConfigureAwait(false);

			return await HandleResponseAsync<Variable>(response).ConfigureAwait(false);
		}

		public async Task<IEnumerable<Variable>> GetTeamVariablesAsync(string userName, int? maxPages = null)
		{
			return await GetPagedResultsAsync(maxPages,
                         GetTeamsUrl()
                        .AppendPathSegment($"/{userName}/pipelines_config/variables/"), async req =>
					await req
						.GetJsonAsync<PagedResults<Variable>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<Variable> UpdateTeamVariableAsync(string userName, string variableId, Variable variable)
		{
			var response = await GetTeamsUrl()
				.AppendPathSegment($"/{userName}/pipelines_config/variables/{variableId}")
				.PutJsonAsync(variable)
				.ConfigureAwait(false);

			return await HandleResponseAsync<Variable>(response).ConfigureAwait(false);
		}

		public async Task<Variable> GetTeamVariableAsync(string userName, string variableId)
		{
			return await GetTeamsUrl()
				.AppendPathSegment($"/{userName}/pipelines_config/variables/{variableId}")
				.GetJsonAsync<Variable>()
				.ConfigureAwait(false);
		}

		public async Task<bool> DeleteTeamVariableAsync(string userName, string variableId)
		{
			var response = await GetTeamsUrl()
				.AppendPathSegment($"/{userName}/pipelines_config/variables/{variableId}")
				.DeleteAsync()
				.ConfigureAwait(false);

			return await HandleResponseAsync<bool>(response).ConfigureAwait(false);
		}

		public async Task<Project> CreateTeamProjectAsync(string teamName, Project project)
		{
			var response = await GetTeamsUrl(teamName)
				.AppendPathSegment("/projects")
				.PostJsonAsync(project)
				.ConfigureAwait(false);

			return await HandleResponseAsync<Project>(response).ConfigureAwait(false);
		}

		public async Task<IEnumerable<Project>> GetTeamProjectsAsync(string teamName, int? maxPages = null)
		{
			return await GetPagedResultsAsync(maxPages, 
                         GetTeamsUrl(teamName)
                        .AppendPathSegment("/projects"), async req =>
					await req
						.GetJsonAsync<PagedResults<Project>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<Project> UpdateTeamProjectAsync(string teamName, string projectKey, Project project)
		{
			var response = await GetTeamsUrl(teamName)
				.AppendPathSegment($"/projects/{projectKey}")
				.PutJsonAsync(project)
				.ConfigureAwait(false);

			return await HandleResponseAsync<Project>(response).ConfigureAwait(false);
		}

		public async Task<Project> GetTeamProjectAsync(string teamName, string projectKey)
		{
			return await GetTeamsUrl(teamName)
				.AppendPathSegment($"/projects/{projectKey}")
				.GetJsonAsync<Project>()
				.ConfigureAwait(false);
		}

		public async Task<bool> DeleteTeamProjectAsync(string teamName, string projectKey)
		{
			var response = await GetTeamsUrl(teamName)
				.AppendPathSegment($"/projects/{projectKey}")
				.DeleteAsync()
				.ConfigureAwait(false);

			return await HandleResponseAsync<bool>(response).ConfigureAwait(false);
		}

		public async Task<IEnumerable<Repository>> GetTeamRepositoriesAsync(string userName, int? maxPages = null)
		{
			return await GetPagedResultsAsync(maxPages,
                         GetTeamsUrl()
                        .AppendPathSegment($"/{userName}/repositories"), async req =>
					await req
						.GetJsonAsync<PagedResults<Repository>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}

		public async Task<IEnumerable<SearchResult>> SearchTeamCodeAsync(string teamName, string searchQuery, int? maxPages = null)
		{
			var queryParamValues = new Dictionary<string, object>
			{
				["search_query"] = searchQuery
			};

			return await GetPagedResultsAsync(maxPages,
                         GetTeamsUrl()
                        .AppendPathSegment($"/{teamName}/search/code"), async req =>
					await req
						.SetQueryParams(queryParamValues)
						.GetJsonAsync<PagedResults<SearchResult>>()
						.ConfigureAwait(false))
				.ConfigureAwait(false);
		}
	}
}
