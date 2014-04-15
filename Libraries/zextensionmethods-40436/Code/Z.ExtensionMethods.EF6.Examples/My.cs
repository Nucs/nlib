using System.Configuration;

namespace Z.ExtensionMethods.EF6.Examples
{
    public static class My
    {
        public static class Config
        {
            public static class ConnectionString
            {
                public static ConnectionStringSettings UnitTest = ConfigurationManager.ConnectionStrings["UnitTest"];
                public static ConnectionStringSettings CodeFirstConnectionString = ConfigurationManager.ConnectionStrings["CodeFirstConnectionString"];
            }
        }
    }
}