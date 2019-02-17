﻿namespace Clarity.Core.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class EditRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(EditRequestHandlerFacts).FullName;

        [Fact]
        public async Task Edit()
        {
            // Arrange
            const string newDescription = "New Description";
            var entity = new FakeEntity("Name") { Description = "Description" };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Edit)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            using (var context = new FakeContext(options))
            {
                context.Add(entity);
                await context.SaveChangesAsync();
            }

            var keyValues = new object[] { entity.Id };
            entity.Description = newDescription;
            var request = new Mock<EditRequest<FakeEntity>>(entity);

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeEditRequestHandler(context);
                await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            using (var context = new FakeContext(options))
            {
                entity = await context.FindAsync<FakeEntity>(keyValues);
                Assert.Equal(newDescription, entity.Description);
            }
        }
    }
}