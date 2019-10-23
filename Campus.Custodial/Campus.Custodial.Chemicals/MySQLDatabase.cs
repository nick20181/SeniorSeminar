using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Campus.Custodial.Chemicals
{
    public class MySQLDatabase : IDatabase
    {
        private MySQLDatabase()
        {

        }

        public string databaseName = $"";
        public string pass = $"";
        public string userName = $"";
        public string serverLocation = $"";
        public MySqlConnection connection = null;

        private string makeConnectionString()
        {
            return $"server={serverLocation};userid={userName};password={pass};database={databaseName}";
        }

        public IChemical ReadChemical(string id)
        {
            throw new NotImplementedException();
        }

        public List<IChemical> ReadAllChemical()
        {
            throw new NotImplementedException();
        }

        public IChemical CreateChemical(IChemical newChemical)
        {
            try
            {
                if (connection == null)
                {
                    connection = new MySqlConnection(makeConnectionString());
                }
                connection.OpenAsync();
                //log
                //do actual stuff here

            }
            catch (MySqlException ex)
            {
                //errors
            }
            finally
            {
                if(connection != null)
                {
                    connection = null;
                }
            }
            throw new NotImplementedException();
        }

        public IChemical UpdateChemical(IChemical updatedChemical, string targetChemicalID)
        {
            throw new NotImplementedException();
        }
    }
}
