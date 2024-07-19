/*
 * @author: Cesar Lopez
 * @copyright 2024 - All rights reserved
 */
using Movies.App.Models;
using Movies.Contracts;

namespace Movies.Api;

public static class RatingMapping
{
    public static List<MovieRatingResponse> MapToMovieRatingResponse(this List<Rating> ratings) =>
        ratings.Select(r => new MovieRatingResponse(){
            Id = r.MovieId,
            Slug = r.Movie.Slug,
            Rating = r.UserRating
        })
        .ToList();


    public static float? GetAverageRating(this List<Rating>? ratings) =>
        ratings?.Count == 0
            ? null
            : (float?)ratings.Select(r => r.UserRating).Average();

    public static int? GetUserRating(this List<Rating>? ratings, Guid? userId) =>
        userId is null || ratings?.Count == 0
            ? null
            : ratings?.FirstOrDefault(r => r.UserId == userId)?.UserRating;
}
