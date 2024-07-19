/*
 * @author: Cesar Lopez
 * @copyright 2024 - All rights reserved
 */
namespace Movies.Contracts;

public class MovieRatingResponse
{
    public required Guid Id { get; init; }
    public required string Slug { get; init; }
    public required int Rating {get; init;}
}
