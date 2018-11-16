namespace Cef.Core.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Moq;
    using Relationships;
    using Xunit;

    public class BaseRelationshipServiceFacts : BaseServiceFacts
    {
        [Fact]
        public void Index()
        {
            // Arrange
            Setup();
            var relationships = new List<Relationship> {new Relationship(), new Relationship(), new Relationship()};
            var mockSet = GetMockDbSet(relationships.AsQueryable());
            Context.Setup(x => x.Set<Relationship>()).Returns(mockSet.Object);
            var service = new RelationshipService(Context.Object);

            // Act
            var index = service.Index();

            // Assert
            var result = Assert.IsAssignableFrom<IEnumerable<Relationship>>(index);
            Assert.Equal(relationships.Count, result.Count());
        }

        [Fact]
        public async Task Details()
        {
            // Arrange
            Setup();
            var relationship = new Relationship {Model1Id = Guid.NewGuid(), Model2Id = Guid.NewGuid()};
            var relationships = new List<Relationship>(1) {relationship};
            var mockSet = GetMockDbSet(relationships.AsQueryable());
            Context.Setup(x => x.Set<Relationship>()).Returns(mockSet.Object);
            var service = new RelationshipService(Context.Object);

            // Act
            var details = await service.Details(relationship.Model1Id, relationship.Model2Id);

            // Assert
            var result = Assert.IsType<Relationship>(details);
            Assert.Equal(relationship.Model1Id, result.Model1Id);
            Assert.Equal(relationship.Model2Id, result.Model2Id);
        }

        [Fact]
        public async Task Create()
        {
            // Arrange
            Setup();
            var relationship = new Relationship { Model1Id = Guid.NewGuid(), Model2Id = Guid.NewGuid()};
            var service = new RelationshipService(Context.Object);

            // Act
            var create = await service.Create(relationship);

            // Assert
            Assert.IsType<Relationship>(create);
            Context.Verify(m => m.Add(It.Is<Relationship>(x =>
                x.Model1Id.Equals(relationship.Model1Id) &&
                x.Model2Id.Equals(relationship.Model2Id))));
            Context.Verify(m => m.SaveChangesAsync(default(CancellationToken)), Times.Once);
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            Setup();
            var relationship = new Relationship {Model1Id = Guid.NewGuid(), Model2Id = Guid.NewGuid()};
            var relationships = new List<Relationship>(1) { relationship };
            var mockSet = GetMockDbSet(relationships.AsQueryable());
            Context.Setup(x => x.Set<Relationship>()).Returns(mockSet.Object);
            var service = new RelationshipService(Context.Object);

            // Act
            await service.Delete(relationship.Model1Id, relationship.Model2Id);

            // Assert
            Context.Verify(m => m.Remove(It.Is<Relationship>(x =>
                x.Model1Id .Equals(relationship.Model1Id) &&
                x.Model2Id.Equals(relationship.Model2Id))), Times.Once());
            Context.Verify(m => m.SaveChangesAsync(default(CancellationToken)), Times.Once());
        }

        public class Relationship : BaseRelationship<BaseModel, BaseModel>
        {
        }

        private class RelationshipService : BaseRelationshipService<Relationship, BaseModel, BaseModel>
        {
            public RelationshipService(DbContext context) : base(context)
            {
            }

#pragma warning disable 1998
            // Added to satisfy abstract method, but not implemented/tested as base level
            public override async Task Edit(Relationship relationship)
#pragma warning restore 1998
            {
            }
        }
    }
}