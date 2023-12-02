using CustomerBackend.Database;
using System.Diagnostics;

namespace CustomerBackend.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CustomerBackendContext context)
        {
            context.Database.EnsureCreated();
            return;
        }
    }
}
