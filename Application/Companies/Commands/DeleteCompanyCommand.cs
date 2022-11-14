using MediatR; 

namespace Application.Companies.Commands;


public record DeleteCompanyCommand(Guid Id, bool TrackChanges) : IRequest;