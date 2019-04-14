using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ShowsService.Tools.Serialization;

namespace ShowsService.Ingester.TvMaze
{
    public interface ITvMazeClient
    {
        Task<IEnumerable<TvMazeShow>> GetShows(long pageNumber, CancellationToken cancellationToken);

        Task<IEnumerable<TvMazePerson>> GetCast(long showId, CancellationToken cancellationToken);
    }

    public class TvMazeClient : ITvMazeClient
    {
        private readonly HttpClient httpClient;
        private readonly IJsonSerializer jsonSerializer;

        public TvMazeClient(
            HttpClient httpClient, 
            IJsonSerializer jsonSerializer)
        {
            this.httpClient = httpClient;
            this.jsonSerializer = jsonSerializer;
        }

        public async Task<IEnumerable<TvMazeShow>> GetShows(long pageNumber, CancellationToken cancellationToken)
        {
            var requestUri = new Uri($"/shows?page={pageNumber}", UriKind.Relative);
            var response = await this.httpClient.GetAsync(requestUri, cancellationToken);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }

            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            return this.jsonSerializer.Deserialize<IEnumerable<TvMazeShow>>(responseJson);
        }

        public async Task<IEnumerable<TvMazePerson>> GetCast(long showId, CancellationToken cancellationToken)
        {
            var requestUri = new Uri($"/shows/{showId}/cast", UriKind.Relative);
            var response = await this.httpClient.GetAsync(requestUri, cancellationToken);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }

            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            return this.jsonSerializer
                       .Deserialize<IEnumerable<TvMazeCastMember>>(responseJson)
                       .Select(x => x.Person)
                       .ToList();
        }
    }
}
