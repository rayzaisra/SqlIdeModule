using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlIdeModule.Core.Services
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString();
    }
}
