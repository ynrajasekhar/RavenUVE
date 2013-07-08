using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Common.Logging;

namespace RavenUVE.Model
{
    class Configuration : RavenUVE.Model.IConfiguration
    {

        #region Fields

        private const string className = "Configuration";
        private readonly ILog logger;

        #endregion

        #region Constructor

        public Configuration(ILog logger)
        {
            Contract.Requires(null != logger);

            this.logger = logger;
            logger.Trace(m => m("{0}: Creating.", className));

            Servers = new List<DatabaseConnection>();
            Load();
        }

        #endregion

        #region Properties

        public ICollection<DatabaseConnection> Servers { get; private set; }

        #endregion

        #region Methods

        private void Load()
        {
            logger.Trace(m => m("{0}: Entering Load method.", className));

            if (null == Properties.Settings.Default.Databases)
            {
                Properties.Settings.Default.Databases = new StringCollection();
                return;
            }

            var serializer = new XmlSerializer(typeof(DatabaseConnection));

            foreach (var database in Properties.Settings.Default.Databases)
            {
                using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(database)))
                {
                    using (var reader = XmlReader.Create(ms))
                    {
                        var databaseObject = serializer.Deserialize(reader) as DatabaseConnection;
                        if (null != databaseObject)
                        {
                            Servers.Add(databaseObject);
                            logger.Debug(m => m("{0}: Read {1} from settings.", className, databaseObject.Name));
                        }
                        else
                        {
                            logger.Debug(m => m("{0}: Could not deserialize string to a server description \n\t{1}", className, database));
                        }
                    }
                }
            }

            logger.Trace(m => m("{0}: Exiting Load method.", className));
        }

        public void Save()
        {
            logger.Trace(m => m("{0}: Entering Save method.", className));

            var serializer = new XmlSerializer(typeof(DatabaseConnection));
            Properties.Settings.Default.Databases.Clear();

            foreach (var database in Servers)
            {
                using (var ms = new MemoryStream())
                {
                    using (var writer = XmlWriter.Create(ms, new XmlWriterSettings { Encoding = ASCIIEncoding.ASCII }))
                    {
                        serializer.Serialize(writer, database);
                        Properties.Settings.Default.Databases.Add(Encoding.ASCII.GetString(ms.GetBuffer()).TrimEnd('\0'));
                        logger.Debug(m => m("{0}: Wrote {1} to settings.", className, database.Name));
                    }
                }
            }

            Properties.Settings.Default.Save();

            logger.Trace(m => m("{0}: Exiting Save method.", className));
        }

        #endregion

    }
}
