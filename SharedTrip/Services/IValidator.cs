using SharedTrip.Models;
using System.Collections.Generic;

namespace SharedTrip.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserFormModel model);

       // ICollection<string> ValidateRepository(CreateRepositoryFormModel model);
    }
}