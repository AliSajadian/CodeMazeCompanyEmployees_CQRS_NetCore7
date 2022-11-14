using Contracts;

namespace Repository;

public sealed class RepositoryManager : IRepositoryManager 
{ 
    private readonly RepositoryContext _repositoryContext; 
    private readonly Lazy<ICompanyRepository> _companyRepository; 
    private readonly Lazy<IEmployeeRepository> _employeeRepository; 

   //  private readonly List<ICompanyRepository> _companyRepository; 
   //  private readonly List<IEmployeeRepository> _employeeRepository; 

    public RepositoryManager(RepositoryContext repositoryContext)
    {
         _repositoryContext = repositoryContext;
         _companyRepository = new Lazy<ICompanyRepository>(() => 
            new CompanyRepository(repositoryContext));
         _employeeRepository = new Lazy<IEmployeeRepository>(() => 
            new EmployeeRepository(repositoryContext));
         // _companyRepository = new List<ICompanyRepository>(() => 
         //    new CompanyRepository(repositoryContext));
         // _employeeRepository = new List<IEmployeeRepository>(() => 
         //    new EmployeeRepository(repositoryContext)); 
    } 

    public ICompanyRepository Company => _companyRepository.Value; 
    public IEmployeeRepository Employee => _employeeRepository.Value; 
    public void Save() => _repositoryContext.SaveChanges(); 
    public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
}