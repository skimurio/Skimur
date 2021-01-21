using System;
using System.Collections.Generic;
using Skimur.Data.Models;

namespace Skimur.Data.ReadModel
{
    public interface IMembershipDao
    {
        User GetUserById(Guid userId);

        User GetUserByUserName(string username);

        User GetUserByEmail(string email);

        List<User> GetUsersByIds(List<Guid> ids);
    }
}
