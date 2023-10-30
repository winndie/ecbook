using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace ECBook.App.Tests.Mocks
{
    public class MockRepo<T> : Mock<DbSet<T>>
        where T : class
    {
        public MockRepo() { }
        public MockRepo(List<T> data)
        {
            var sources = data.AsQueryable<T>();
            var primaryKey = typeof(T).GetProperty("Id");

            base.As<IQueryable<T>>().Setup(x => x.Provider).Returns(sources.Provider);
            base.As<IQueryable<T>>().Setup(x => x.Expression).Returns(sources.Expression);
            base.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(sources.ElementType);
            base.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(() => sources.GetEnumerator());

            base.Setup(x => x.Find(It.IsAny<object[]>())).Returns((object[] args) =>
            {
                var param = Expression.Parameter(typeof(T));
                var col = Expression.Property(param, primaryKey.Name);
                var body = Expression.Equal(col, Expression.Constant(args[0]));
                var predicate = Expression.Lambda<Func<T, bool>>(body, param);
                return data.AsQueryable<T>().FirstOrDefault(predicate);
            });

            base.Setup(x => x.Add(It.IsAny<T>())).Callback<T>(x => data.Add(x));
            base.Setup(x => x.Remove(It.IsAny<T>())).Callback<T>(x => data.Remove(x));
        }
    }
}
