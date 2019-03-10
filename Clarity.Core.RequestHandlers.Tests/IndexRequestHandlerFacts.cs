﻿namespace Clarity.Core.RequestHandlers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fakes;
    using Kendo.Mvc.UI;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class IndexRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(IndexRequestHandlerFacts).FullName;

        [Fact]
        public async Task Index()
        {
            // Arrange
            var entities = new []
            {
                new FakeEntity("Name 1"),
                new FakeEntity("Name 2"),
                new FakeEntity("Name 3")
            };
            var models = new []
            {
                Mock.Of<Model>(),
                Mock.Of<Model>(),
                Mock.Of<Model>()
            }.AsQueryable();
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Index)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                context.Set<FakeEntity>().AddRange(entities);
                await context.SaveChangesAsync();
            }

            var request = new Mock<IndexRequest<FakeEntity, Model>>(null, new DataSourceRequest());
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.ProjectTo(
                    It.IsAny<IQueryable>(),
                    It.IsAny<object>(),
                    It.IsAny<Expression<Func<Model, object>>[] > ()))
                .Returns(models);
            DataSourceResult index;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeIndexRequestHandler(context, mapper.Object);
                index = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            var result = Assert.IsType<DataSourceResult>(index);
            var data = Assert.IsAssignableFrom<IEnumerable<Model>>(result.Data);
            Assert.Equal(models.AsEnumerable().Count(), data.Count());
        }
    }
}
