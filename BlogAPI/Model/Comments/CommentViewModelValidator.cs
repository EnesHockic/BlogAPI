using FluentValidation;

namespace BlogAPI.Model.Comments
{
    public class CommentViewModelValidator:AbstractValidator<CommentViewModel>
    {
        public CommentViewModelValidator()
        {
            RuleFor(x => x.Body)
                .NotEmpty()
                .WithMessage("Body is required!");
        }
    }
}
