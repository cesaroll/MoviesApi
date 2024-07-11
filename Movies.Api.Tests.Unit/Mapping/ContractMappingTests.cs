using AutoFixture;
using Bogus;
using FluentAssertions;
using Movies.Api.Fakers.Contracts.Requests;
using Movies.Api.Fakers.Models;
using Movies.Contracts.Requests;
using Movies.Api.Mapping;

namespace Movies.Api.Tests.Unit.Mapping;

public class ContractMappingTests //: IClassFixture<MoviesFixture>
{
    private readonly IFixture _fixture;
    private readonly Faker _faker;

    public ContractMappingTests()//MoviesFixture moviesFixture)
    {
        _fixture = new Fixture();
        _faker = new Faker();
    }
    
    [Theory]
    [InlineData("Titanic", 1995, "titanic-1995")]
    [InlineData("The Matrix", 1999, "the-matrix-1999")]
    [InlineData("@something", 2005, "something-2005")]
    public void GenerateSlug_ShouldReturnSlug(string title, int year, string expectedSlug)
    {
        // Act
        var slug = ContractMapping.GenerateSlug(title, year);
    
        // Assert
        slug.Should().NotBeNullOrEmpty();
        slug.Should().Be(expectedSlug);
    }
    
    [Fact]
    public void MapToMovie_CreateMovieRequest_ShouldReturnMovie()
    {
        // Arrange
        var createMovieRequest = CreateMovieRequestBuilder.CreateOne();
    
        // Act
        var movie = createMovieRequest.MapToMovie();
    
        // Assert
        movie.Should().NotBeNull();
        movie.Title.Should().Be(createMovieRequest.Title);
        movie.Slug.Should().Be(ContractMapping.GenerateSlug(movie.Title, movie.YearOfRelease));
        movie.YearOfRelease.Should().Be(createMovieRequest.YearOfRelease);
        movie.Genres.Count.Should().Be(createMovieRequest.Genres.Count());
        movie.Genres.Select(g => g.Name).Should().BeEquivalentTo(createMovieRequest.Genres);
    }
    
    [Fact]
    public void MapToMovie_UpdateMovieRequest_ShouldReturnMovie()
    {
        // Arrange
        var id = _faker.Random.Guid();
        var updateMovieRequest = _fixture.Create<UpdateMovieRequest>();
    
        // Act
        var movie = updateMovieRequest.MapToMovie(id);
    
        // Assert
        movie.Should().NotBeNull();
        movie.Title.Should().Be(updateMovieRequest.Title);
        movie.Slug.Should().Be(ContractMapping.GenerateSlug(movie.Title, movie.YearOfRelease));
        movie.YearOfRelease.Should().Be(updateMovieRequest.YearOfRelease);
        movie.Genres.Count.Should().Be(updateMovieRequest.Genres.Count());
        movie.Genres.Select(g => g.Name).Should().BeEquivalentTo(updateMovieRequest.Genres);
    }
    
    [Fact]
    public void MapToMovieResponse_ShouldReturnMovieResponse()
    {
        // Arrange
        var movie = MovieBuilder.CreateOne();
        var userId = _faker.Random.Guid();
        
        // Act
        var movieResponse = movie.MapToMovieResponse(userId);

        // Assert
        movieResponse.Should().NotBeNull();
        movieResponse.Id.Should().Be(movie.Id);
        movieResponse.Title.Should().Be(movie.Title);
        movieResponse.Slug.Should().Be(movie.Slug);
        movieResponse.YearOfRelease.Should().Be(movie.YearOfRelease);
        movieResponse.Genres.Should().BeEquivalentTo(movie.Genres.Select(g => g.Name));
    }
    
    [Fact]
    public void MapToMoviesResponse_ShouldReturnMoviesResponse()
    {
        // Arrange
        var movies = MovieBuilder.CreateMany();
        var userId = _faker.Random.Guid();
        
        // Act
        var moviesResponse = movies.MapToMoviesResponse(userId);

        // Assert
        moviesResponse.Should().NotBeNull();
        moviesResponse.Items.Should().NotBeNullOrEmpty();
        moviesResponse.Items.Should().HaveCount(movies.Count);
        moviesResponse.Items.Select(m => m.Id).Should().BeEquivalentTo(movies.Select(m => m.Id));
        moviesResponse.Items.Select(m => m.Title).Should().BeEquivalentTo(movies.Select(m => m.Title));
        moviesResponse.Items.Select(m => m.Slug).Should().BeEquivalentTo(movies.Select(m => m.Slug));
        moviesResponse.Items.Select(m => m.YearOfRelease).Should().BeEquivalentTo(movies.Select(m => m.YearOfRelease));
        moviesResponse.Items.Select(m => m.Genres).Should().BeEquivalentTo(movies.Select(m => m.Genres.Select(g => g.Name)));
    }
}