using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BlogAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using BlogAPI.Core.Application.BlogPosts.Queries.GetBlogPosts;
using System.Threading;
using BlogAPI.Core.Application.BlogPosts.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers.Tests
{
    [TestClass()]
    public class BlogPostControllerTests
    {
        public readonly Mock<IMediator> _mediatorMock = new();

        [TestMethod()]
        public async void GetPostsTest()
        {
            var posts = new List<BlogPostDTO> { new BlogPostDTO(), new BlogPostDTO() };

            var blogPostController = new BlogPostController();
            
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetBlogPostsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(posts)
                .Verifiable("Posts weren't send.");

            var actionResult = await blogPostController.GetPosts(null);

            var result = actionResult as OkObjectResult;
            Assert.IsNotNull(result.Value);
        }
    }
}