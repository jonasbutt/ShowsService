using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShowsService.Api.Model;
using ShowsService.Data;

namespace ShowsService.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class ShowsController
    {
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 100;
        private const int MaximumPageSize = 500;

        private readonly IShowsRepository showsRepository;

        public ShowsController(IShowsRepository showsRepository)
        {
            this.showsRepository = showsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Show>>> GetShows(
            [Range(1, int.MaxValue)] int pageNumber = DefaultPageNumber,
            [Range(1, MaximumPageSize)] short pageSize = DefaultPageSize,
            CancellationToken cancellationToken = default)
        {
            var shows = await this.showsRepository.GetShows(pageNumber, pageSize, cancellationToken);
            return new ActionResult<IEnumerable<Show>>(shows.Select(MapShow));
        }

        private static Show MapShow(Data.Model.Show show)
        {
            return new Show
            {
                Id = show.Id,
                Name = show.Name,
                Cast = show.Cast?
                           .Select(x => new CastMember
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Birthday = x.Birthday
                            })
                           .OrderByDescending(x => x.Birthday)
            };
        }
    }
}
