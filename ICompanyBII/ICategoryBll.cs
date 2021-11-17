using Entity;

namespace ICompanyBll;

public interface ICategoryBll : IBaseBll<Category>
{
    public Task<(List<Category> list, int Count)> Query(string categoryName, int page, int limit);

    public Task<bool> Update(string Id, string categoryName, string description);
}