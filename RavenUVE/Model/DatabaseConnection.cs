using System;
using System.Diagnostics.Contracts;

namespace RavenUVE.Model
{

    [Serializable]
    public class DatabaseConnection
    {

        public DatabaseConnection()
        {
            Name = String.Empty;
            Url = String.Empty;
        }

        public DatabaseConnection(DatabaseConnection dbConnection)
        {
            Contract.Requires(null != dbConnection);

            Name = dbConnection.Name;
            Url = dbConnection.Url;
        }

        public String Name { get; set; }

        public String Url { get; set; }

    }
}
