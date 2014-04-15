using System.Configuration;

namespace ExtensionMethods.Examples
{
    public static class My
    {
        public static class Config
        {
            public static class ConnectionString
            {
                public static ConnectionStringSettings UnitTest = ConfigurationManager.ConnectionStrings["UnitTest"];
            }
        }
    }
}