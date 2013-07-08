using System;
namespace RavenUVE.Model
{
    public interface IConfiguration
    {
        void Save();
        System.Collections.Generic.ICollection<DatabaseConnection> Servers { get; }
    }
}
