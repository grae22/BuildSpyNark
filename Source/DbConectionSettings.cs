using System;

namespace Critr.Db
{

public class DbConnectionSettings
{
  //---------------------------------------------------------------------------

  private string serverName = "";
  private string dbName = "";
  private bool useSqlAuthentication = false;
  private string sqlUserName = "";
  private string sqlPassword = "";

  //---------------------------------------------------------------------------

  public string ServerName
  {
    get
    {
      return serverName;
    }

    set
    {
      if( value == null )
      {
        serverName = "";
      }
      else
      {
        serverName = value;
      }
    }
  }

  //---------------------------------------------------------------------------

  public string DbName
  {
    get
    {
      return dbName;
    }

    set
    {
      if( value == null )
      {
        dbName = "";
      }
      else
      {
        dbName = value;
      }
    }
  }

  //---------------------------------------------------------------------------

  public bool UseSqlAuthentication
  {
    get
    {
      return useSqlAuthentication;
    }

    set
    {
      useSqlAuthentication = value;
    }
  }

  //---------------------------------------------------------------------------

  public string SqlUserName
  {
    get
    {
      return sqlUserName;
    }

    set
    {
      if( value == null )
      {
        sqlUserName = "";
      }
      else
      {
        sqlUserName = value;
      }
    }
  }

  //---------------------------------------------------------------------------

  public string SqlPassword
  {
    get
    {
      return sqlPassword;
    }

    set
    {
      if( value == null )
      {
        sqlPassword = "";
      }
      else
      {
        sqlPassword = value;
      }
    }
  }

  //---------------------------------------------------------------------------

  public string ConnectionString
  {
    get
    {
      string connectionString =
        "Server=" + serverName + ';' +
        "Database=" + dbName + ';';

      if( useSqlAuthentication )
      {
        connectionString +=
          "User Id=" + sqlUserName + ';' +
          "Password=" + sqlPassword + ';';
      }
      else
      {
        connectionString += "Trusted_Connection=True;";
      }

      connectionString += "Timeout=5;";

      return connectionString;
    }
  }

  //---------------------------------------------------------------------------
}

}
