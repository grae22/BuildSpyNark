using System;
using System.Data.SqlClient;

namespace Critr.Db
{

public class DbConnection
{
  //---------------------------------------------------------------------------

  private static DbConnection instance;
  private SqlConnection connectionCritr;

  //---------------------------------------------------------------------------

  public static DbConnection Instance
  {
    get
    {
      if( instance == null )
      {
        instance = new DbConnection();
      }

      return instance;
    }
  }

  //---------------------------------------------------------------------------

  private DbConnection()
  {
    DbConnectionSettings settings = new DbConnectionSettings();
    settings.ServerName = Program.Settings.GetSetting< string >( "Db.Critr.ServerName", @"odin\thoroughtec", true );
    settings.DbName = Program.Settings.GetSetting< string >( "Db.Critr.DbName", "Critr", true );
    settings.SqlUserName = Program.Settings.GetSetting< string >( "Db.Critr.UserName", "", true );
    settings.SqlPassword = Program.Settings.GetSetting< string >( "Db.Critr.Password", "", true );

    settings.UseSqlAuthentication = ( settings.SqlUserName.Length > 0 );

    Initialise( settings );
  }

  //---------------------------------------------------------------------------

  ~DbConnection()
  {
    try
    {
      connectionCritr.Close();
    }
    catch {}
  }

  //---------------------------------------------------------------------------

  public bool Initialise( DbConnectionSettings settings )
  {
    // Critr.
    try
    {
      connectionCritr = new SqlConnection( settings.ConnectionString );
      connectionCritr.Open();
    }
    catch( Exception ex )
    {
      // Something's gone wrong, close any connections that were opened.
      try
      {
        if( connectionCritr != null )
        {
          connectionCritr.Close();
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

  //---------------------------------------------------------------------------

  public SqlConnection Critr
  {
    get
    {
      return connectionCritr;
    }
  }

  //---------------------------------------------------------------------------

  public SqlCommand CreateCommand()
  {
    return connectionCritr.CreateCommand();
  }

  //---------------------------------------------------------------------------
}

}
