using Entities.Models;

namespace Contracts;

public interface IDataShaper<T> 
{ 
//     IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldsString); 
//     ExpandoObject ShapeData(T entity, string fieldsString); 

    IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString); 
    ShapedEntity ShapeData(T entity, string fieldsString); 
}