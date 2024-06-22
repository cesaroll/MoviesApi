using System.Runtime.CompilerServices;
using AutoFixture;
using FluentAssertions;
using Movies.Contracts.Requests;
using Movies.Api.Mapping;
using Movies.Api.Tests.Unit.DataFixtures;
using Movies.App.Models;

namespace Movies.Api.Tests.Unit.Mapping;

public class ContractMappingTests : IClassFixture<MoviesFixture>
{
    private readonly IFixture _fixture;
    private readonly MoviesFixture _moviesFixture;

    public ContractMappingTests(MoviesFixture moviesFixture)
    {
        _moviesFixture = moviesFixture;
        _fixture = _moviesFixture.Fixture;
    }
    
    [Fact]
    public void GenerateSlug_ShouldReturnSlug()
    {
        // Arrange
        var title = _fixture.Create<string>();
        var yearOfRelease = _fixture.Create<int>();
    
        // Act
        var slug = ContractMapping.GenerateSlug(title, yearOfRelease);
    
        // Assert
        slug.Should().NotBeNullOrEmpty();
        slug.Should().Be($"{title}-{yearOfRelease}");
    }
    
    [Fact]
    public void MapToMovie_CreateMovieRequest_ShouldReturnMovie()
    {
        // Arrange
        var createMovieRequest = _fixture.Create<CreateMovieRequest>();
    
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
        var id = _fixture.Create<Guid>();
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
        var movie = _moviesFixture.GetMovie();
        
        // Act
        var movieResponse = movie.MapToMovieResponse();

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
        var movies = _moviesFixture.GetMovies();
        
        // Act
        var moviesResponse = movies.MapToMoviesResponse();

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