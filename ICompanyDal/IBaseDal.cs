using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ICompanyDal;

public interface IBaseDal<TEntity> where TEntity : class
{

    public Task<List<TEntity>> Query();

    public Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> func);

    public Task<List<TEntity>> QueryPage(int page, int limit);

    public Task<List<TEntity>> QueryPage(int page, int limit, Expression<Func<TEntity, bool>> func);

    public Task<bool> Create(TEntity entity);

    public Task<bool> Delete(string Id);

    public Task<bool> Update(TEntity entity);

    public Task<bool> Update(Expression<Func<TEntity, bool>> func, Expression<Func<TEntity, TEntity>> updateFunc);

    public Task<TEntity> Find(string Id);

    public bool CreateVerification(Expression<Func<TEntity, bool>> func);

    public DbSet<TEntity> QueryDb();

    public Task<int> FakeDelete(Expression<Func<TEntity, bool>> func, Expression<Func<TEntity, TEntity>> updateFunc);

    public Task BatchInsert(List<TEntity> entityList);

}