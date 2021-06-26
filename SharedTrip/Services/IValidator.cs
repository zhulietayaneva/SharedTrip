using SharedTrip.Models.Trip;
using SharedTrip.Models.User;
using System.Collections.Generic;

namespace SharedTrip.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserFormModel model);

        ICollection<string> ValidateTrip(AddTripFormModel model);
    }
}