using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Entity;
using ICompanyDal;
using Microsoft.EntityFrameworkCore;

namespace CompanyDal;

public class BaseDal<TEntity> :IBaseDal<TEntity> where TEntity : BaseId
{
    readonly CompanyContext _companyContext;
    public BaseDal(CompanyContext companyContext)
    {
        _companyContext = companyContext;
    }
    
    ///
    public async Task<bool> Create(TEntity entity)
    {
        await _companyContext.Set<TEntity>().AddAsync(entity);
        var b = await _companyContext.SaveChangesAsync();
        if (b > 0)return true;
        return false;
    }
    public async Task<bool> Delete(string Id)
    {
        var enity =  await _companyContext.Set<TEntity>().FindAsync(Id);
        _companyContext.Set<TEntity>().Remove(enity ?? throw new InvalidOperationException());
        var b = await _companyContext.SaveChangesAsync();
        if(b > 0)return true;
        return false;
    }
    public async Task<List<TEntity>> Query()
    {
        return await _companyContext.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<List<TEntity>> QueryPage(int page, int limit)
    {
        return await _companyContext.Set<TEntity>().Skip((page-1)*5).Take(limit).ToListAsync();
    }

    public async Task<List<TEntity>> QueryPage(int page, int limit, Expression<Func<TEntity, bool>> func)
    {
        return await _companyContext.Set<TEntity>().Where(func).Skip((page - 1)*limit).Take(limit).ToListAsync();
    }

    public async Task<bool> Update(TEntity entity)
    {
        TEntity oldEntity = await _companyContext.Set<TEntity>().FirstAsync(x=>x.Id == entity.Id);
        var newEntityparams = entity.GetType().GetProperties();
        var oldEntityparams = oldEntity.GetType().GetProperties();

        foreach (var newparams in newEntityparams)
        {
            foreach(var sourceparams in oldEntityparams)
            {
                if(newparams.Name == sourceparams.Name && newparams.GetType() == oldEntityparams.GetType())
                {
                    sourceparams.SetValue(oldEntity, newparams.GetValue(entity));
                    break;
                }
            }
        }

        var b = await _companyContext.SaveChangesAsync();
        if (b > 0) return true;
        return false;
    }

    public async Task<bool> Update(Expression<Func<TEntity,bool>> func,Expression<Func<TEntity,TEntity>> updateFunc)
    {
        return await _companyContext.Set<TEntity>().Where(func).BatchUpdateAsync(updateFunc) > 1 ? true: false;
    }

    public async Task<TEntity> Find(string Id)
    {
        return await _companyContext.Set<TEntity>().FindAsync(Id) ?? throw new InvalidOperationException();
    }

    public bool CreateVerification(Expression<Func<TEntity, bool>> func)
    {
        return _companyContext.Set<TEntity>().Any(func);
    }

    public DbSet<TEntity> QueryDb()
    {
        return _companyContext.Set<TEntity>();
    }

    public async Task<int> FakeDelete(Expression<Func<TEntity, bool>> func, Expression<Func<TEntity, TEntity>> updateFunc)
    {
        return await _companyContext.Set<TEntity>().Where(func).BatchUpdateAsync(updateFunc);
    }

    public async Task BatchInsert(List<TEntity> entityList)
    {
        await _companyContext.BulkInsertAsync(entityList);
    }

    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> func)
    {
        return await _companyContext.Set<TEntity>().AsNoTracking().Where(func).ToListAsync();
    }

}