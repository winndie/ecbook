using ECBook.App.Pages;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using ECBook.Data;
using ECBook.Data.Entities;
using ECBook.App.Tests.Mocks;

namespace ECBook.App.Tests
{
    public class PageTest
    {
        Mock<Context> context;
        MockRepo<Profile> profileRepo;
        MockRepo<Post> postRepo;
        public PageTest()
        {
            context = new Mock<Context>();
            profileRepo = new MockRepo<Profile>(new List<Profile> {
                new Profile { Id = 1, FirstName = "Jane", JobTitle = "Developer", LastName = "Doe" }
            });
            postRepo = new MockRepo<Post>(new List<Post> {
                new Post { Id = 1, AuthorId=1,
                    Content="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.", 
                    DateTimePosted=new DateTimeOffset(new DateTime(2020, 11, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
            });
            context.Setup(x => x.Profile).Returns(profileRepo.Object);
            context.Setup(x => x.Post).Returns(postRepo.Object);
        }

        [Fact]
        public void AddPost()
        {
            // Arrange
            var pageModel = new CreateModel(context.Object);

            // Act
            pageModel.OnPostAsync();

            // Assert
            postRepo.Verify(m => m.Add(It.IsAny<Post>()), Times.Once());
        }
    }
}
