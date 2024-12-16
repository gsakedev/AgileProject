using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Interfaces
{
    public interface IUser
    {
        string Id { get; }
        string UserName { get; }
        string Email { get; }
    }
}
