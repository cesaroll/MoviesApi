using Movies.App.Models;

namespace Movies.App.Services;

public record MovieContext(Movie Movie, Guid? UserId, CancellationToken Ct);
public record MovieIdContext(Guid Id, Guid? UserId, CancellationToken Ct);
public record SlugContext(string Slug, Guid? UserId, CancellationToken Ct);
public record RatingIdContext(Guid MovieId, Guid UserId, int Rating, CancellationToken Ct);