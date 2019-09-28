using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Nexus.Data.Helpers
{
    public static class TestHelpers
    {
        public static DbSet<T> MockDbSet<T>() where T : class
        {
            return MockDbSet<T>(null);
        }

        public static DbSet<T> MockDbSet<T>(List<T> inMemoryData) where T : class
        {
            if (inMemoryData == null)
                inMemoryData = new List<T>(); ;

            Mock<DbSet<T>> mockDbSet = new Mock<DbSet<T>>();
            IQueryable<T> queryableData = inMemoryData.AsQueryable();

            // fake dbsete sorgu atabilmemiz için gerekli setup:
            //mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider); // async desteği için eklenen alttaki kod bu kod parçasının işlevini kapsıyor
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator);

            //mockDbSet.Setup(x => x.AsNoTracking()).Returns(mockDbSet.Object);
            //mockDbSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDbSet.Object);

            // TODO: Needs testing
            mockDbSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(CancellationToken.None))
                .Returns(new TestAsyncEnumerator<T>(queryableData.GetEnumerator()));

            mockDbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(queryableData.Provider));


            //mockDbSet.Setup(x => x.AsNoTracking()).Returns(mockDbSet.Object); // not working
            //mockDbSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDbSet.Object); // include metodu için
            //mockDbSet.SetupGet(t => t).Returns(mockDbSet.Object);
            
            return mockDbSet.Object;
        }
    }
}
