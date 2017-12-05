using FluentValidation;
using Lykke.blue.Api.Models.TwitterModels;
using Lykke.blue.Api.Strings;

namespace Lykke.blue.Api.Models.ValidationModels.TwitterValidationModels
{
    public class TweetsRequestValidationModel : AbstractValidator<TweetsRequestModel>
    {
        public TweetsRequestValidationModel()
        {
            RuleFor(reg => reg.AccountEmail).NotNull().WithMessage(Phrases.FieldShouldNotBeEmpty);
            RuleFor(reg => reg.AccountEmail).EmailAddress().WithMessage(Phrases.InvalidEmailFormat);

            RuleFor(reg => reg.SearchQuery).NotNull().WithMessage(Phrases.FieldShouldNotBeEmpty);
            RuleFor(reg => reg.SearchQuery).NotEmpty().WithMessage(Phrases.FieldShouldNotBeEmpty);
        }
    }
}
