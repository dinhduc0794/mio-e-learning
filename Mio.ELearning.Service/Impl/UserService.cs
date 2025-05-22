using Mio.ELearning.Core.UnitOfWorks;

namespace Mio.ELearning.Service.Services.Impl;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}