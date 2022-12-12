using BlogAPI.Core.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Core.Application.Tags.Queries.GetTags
{
    public class GetTagsQuery : IRequest<List<string>>
    {
        public GetTagsQuery()
        {

        }
    }
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, List<string>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GetTagsQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task<List<string>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = _applicationDbContext.Tags.Select(x => x.Name).ToListAsync();
            return tags;
        }
    }

}
