using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Contracts;

public interface IRepositoryBase<T> 
{ 
    IQueryable<T> FindAll(bool trackChanges); 
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges); 
    void Create(T entity); 
    void Update(T entity); 
    void Delete(T entity);
}