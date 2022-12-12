using BlogAPI.Core.Application.Tags.Queries.GetTags;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await Mediator.Send(new GetTagsQuery()).ConfigureAwait(false);
            return Ok(tags);
        }
    }
}
