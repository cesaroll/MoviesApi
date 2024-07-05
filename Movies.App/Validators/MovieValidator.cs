using FluentValidation;
using Movies.App.Models;

namespace Movies.App.Validators;

public class MovieValidator : AbstractValidator<Movie>
{
    public MovieValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.YearOfRelease)
            .InclusiveBetween(1900, DateTime.UtcNow.Year);
        
        RuleFor(x => x.Genres)
            .NotEmpty();
    }
}