using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ICompanyBll;

public interface IBaseBll<TEntity> where TEntity : class
{
    /// <summary>
    /// 查询全部
    /// </summary>
    /// <returns></returns>
    public Task<List<TEntity>> Query();

    /// <summary>
    /// 条件查询
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> func);



    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public Task<List<TEntity>> QueryPage(int page, int limit);

    /// <summary>
    /// 分页条件查询
    /// </summary>
    /// <param name="func"></param>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public Task<List<TEntity>> QueryPage(int page, int limit, Expression<Func<TEntity, bool>> Wherefunc);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Task<bool> Create(TEntity entity);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public Task<bool> Delete(string Id);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Task<bool> Update(TEntity entity);
    /// <summary>
    /// 根据主键查询实体
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public Task<TEntity> Find(string Id);

    /// <summary>
    /// 重复验证
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public bool CreateVerification(Expression<Func<TEntity, bool>> func);

    public DbSet<TEntity> QueryDb();

    public Task<int> FakeDelete(Expression<Func<TEntity, bool>> func, Expression<Func<TEntity, TEntity>> updateFunc);
    
    public Task BatchInsert(List<TEntity> entityList);
}