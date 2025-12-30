using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlIdeModule.Core.Services
{
    public class CustomConnectionStringProvider : IConnectionStringProvider
    {
        private readonly Func<string> _connectionStringBuilder;

        public CustomConnectionStringProvider(Func<string> connectionStringBuilder)
        {
            _connectionStringBuilder = connectionStringBuilder ?? throw new ArgumentNullException(nameof(connectionStringBuilder));
        }

        public string GetConnectionString()
        {
            return _connectionStringBuilder();
        }
    }
}
