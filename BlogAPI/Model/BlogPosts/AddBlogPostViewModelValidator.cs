using BlogAPI.Core.Application.Common.Interfaces;
using FluentValidation;

namespace BlogAPI.Model.BlogPosts
{
    public class AddBlogPostViewModelValidator : AbstractValidator<AddBlogPostViewModel>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public AddBlogPostViewModelValidator(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required!");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required!");
            RuleFor(x => x.Body)
                .NotEmpty()
                .WithMessage("Body is required!");
            When(x => x.TagList != null, () =>
            {
                RuleFor(x => x.TagList)
                    .Must(x => AllTagsExist(x))
                    .WithMessage("Tag doesn't exist!");
            });

        }
        private bool AllTagsExist(List<string> tagsList)
        {
            var tags = _applicationDbContext.Tags.ToList();

            return  !tagsList.Any(x => !tags.Any(y => y.Name == x));
        }
    }
}
