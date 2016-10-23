using System;
using System.Data.SqlClient;

namespace BuildSpyNark.Db
{
  public class DbConnection
  {
    //-------------------------------------------------------------------------

    public SqlConnection Connection { get; private set; }

    //-------------------------------------------------------------------------

    public DbConnection( string serverName,
                         string dbName,
                         string username,
                         string password )
    {
      DbConnectionSettings settings = new DbConnectionSettings();
      settings.ServerName = serverName;
      settings.DbName = dbName;
      settings.SqlUserName = username;
      settings.SqlPassword = password;

      settings.UseSqlAuthentication = ( settings.SqlUserName.Length > 0 );

      Initialise( settings );
    }

    //-------------------------------------------------------------------------

    ~DbConnection()
    {
      try
      {
        Connection.Close();
      }
      catch {}
    }

    //-------------------------------------------------------------------------

    private bool Initialise( DbConnectionSettings settings )
    {
      try
      {
        Connection = new SqlConnection( settings.ConnectionString );
        Connection.Open();
      }
      catch( Exception ex )
      {
        // Something's gone wrong, close any connections that were opened.
        try
        {
          if( Connection != null )
          {
            Connection.Close();
          }
        }
        catch {}

        throw new Exception(
          "Critr database connection error." +
          Environment.NewLine +
          Environment.NewLine +
          ex.Message );
      }

      return true;
    }

    //-------------------------------------------------------------------------

    public SqlCommand CreateCommand()
    {
      return Connection.CreateCommand();
    }

    //-------------------------------------------------------------------------
  }
}
