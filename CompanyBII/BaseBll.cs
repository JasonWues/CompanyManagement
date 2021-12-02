using ICompanyBll;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CompanyBll;

public class BaseBll<TEntity> : IBaseBll<TEntity> where TEntity : class
{
    protected IBaseDal<TEntity> _iBaseDal;
    public async Task<bool> Create(TEntity entity)
    {
        return await _iBaseDal.Create(entity);
    }

    public bool CreateVerification(Expression<Func<TEntity, bool>> func)
    {
        return _iBaseDal.CreateVerification(func);
    }

    public async Task<bool> Delete(string Id)
    {
        return await _iBaseDal.Delete(Id);
    }


    public async Task<TEntity> Find(string Id)
    {
        return await _iBaseDal.Find(Id);
    }

    public async Task<List<TEntity>> Query()
    {
        return await _iBaseDal.Query();
    }

    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> func)
    {
        return await _iBaseDal.Query(func);
    }

    public async Task<List<TEntity>> QueryPage(int page, int limit)
    {
        return await _iBaseDal.QueryPage(page, limit);
    }

    public async Task<List<TEntity>> QueryPage(int page, int limit, Expression<Func<TEntity, bool>> func)
    {
        return await _iBaseDal.QueryPage(page, limit, func);
    }

    public async Task<bool> Update(TEntity entity)
    {
        return await _iBaseDal.Update(entity);
    }

    public DbSet<TEntity> QueryDb()
    {
        return _iBaseDal.QueryDb();
    }

    public async Task<int> FakeDelete(Expression<Func<TEntity, bool>> func, Expression<Func<TEntity, TEntity>> updateFunc)
    {
        return await _iBaseDal.FakeDelete(func, updateFunc);
    }

    public async Task BatchInsert(List<TEntity> entityList)
    {
        await _iBaseDal.BatchInsert(entityList);
    }


}