using System.Xml.Schema;
using Entities.Models;

namespace Contracts;

public interface ICompanyRepository
{ 
    Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
    Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges);
    Company? GetCompany(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
    void DeleteCompany(Company company);
} 

