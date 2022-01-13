using System;
using System.Configuration;

namespace EcoplastERP.WebMobile.model
{
    public static class SqlProvider
    {
        private static String _connectionString;
        public static String ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                }
                return _connectionString;
            }
        }
    }
}