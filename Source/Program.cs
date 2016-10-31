using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
using BuildSpyNark.Db;

namespace BuildSpyNark
{
  class Program
  {
    //-------------------------------------------------------------------------

    private static Dictionary<string, List<BuildLogFile.LogEntry>> LogEntriesByProject =
      new Dictionary<string, List<BuildLogFile.LogEntry>>();

    public List<IBuildStatsProvider> Stats { get; private set; } = new List<IBuildStatsProvider>();

    //-------------------------------------------------------------------------

    static void Main( string[] args )
    {
      try
      {
        new Program( args );
      }
      catch( Exception ex )
      {
        Console.WriteLine( "ERROR: " + ex.Message );
      }
    }

    //-------------------------------------------------------------------------

    public Program( string[] args )
    {
      // No args?
      if( args.Length == 0 )
      {
        return;
      }

      // Specfied folder doesn't exist?
      if( Directory.Exists( args[ 0 ] ) == false )
      {
        return;
      }

      // Significant builds only? Excludeds builds shorter than a certain time.
      bool significantBuildsOnly = false;

      if( args.Length > 1 )
      {
        significantBuildsOnly = ( args[ 1 ].ToLower() == "-s" );
      }

      // Command line args for sql connection?
      string sqlServerName = null;
      string sqlDbName = null;
      string sqlUsername = null;
      string sqlPassword = null;

      foreach( string a in args )
      {
        if( a.StartsWith( "-server=" ) )
        {
          sqlServerName = a.Remove( 0, "-server=".Length );
        }
        else if( a.StartsWith( "-dbname=" ) )
        {
          sqlDbName = a.Remove( 0, "-dbname=".Length );
        }
        else if( a.StartsWith( "-user=" ) )
        {
          sqlUsername = a.Remove( 0, "-user=".Length );
        }
        else if( a.StartsWith( "-password=" ) )
        {
          sqlPassword = a.Remove( 0, "-password=".Length );
        }
      }

      Db.DbConnection dbConnection = null;

      if( sqlServerName != null &&
          sqlDbName != null &&
          sqlUsername != null &&
          sqlPassword != null )
      {
        dbConnection =
          new Db.DbConnection(
            sqlServerName,
            sqlDbName,
            sqlUsername,
            sqlPassword );
      }

      // Get list of log files.
      string[] files =
        Directory.GetFiles(
          args[ 0 ],
          "*.buildspy-prj-log",
          SearchOption.TopDirectoryOnly );

      if( dbConnection == null )
      {
        ProcessFilesAndOutputSummary( files, significantBuildsOnly );
      }
      else
      {
        ProcessFilesAndImportIntoDb( files, dbConnection );
      }

      Console.WriteLine( "Done." );
      //Console.ReadKey();
    }

    //-------------------------------------------------------------------------

    private void ProcessFilesAndOutputSummary(
      string[] files,
      bool significantBuildsOnly )
    {
      foreach( string f in files )
      {
        try
        {
          // Parse the log file.
          BuildLogFile log = new BuildLogFile( f );

          string[] tags = log.GetTags();
          IReadOnlyCollection<BuildLogFile.LogEntry> entries = log.GetEntries();

          // Extract the project name.
          string projectName = tags[ 1 ];

          // Add log entries to collection.
          foreach( BuildLogFile.LogEntry entry in entries )
          {
            if( LogEntriesByProject.ContainsKey( projectName ) == false )
            {
              LogEntriesByProject.Add( projectName, new List<BuildLogFile.LogEntry>() );
            }

            LogEntriesByProject[ projectName ].Add( entry );
          }
        }
        catch( Exception )
        {
          // TODO
        }
      }

      // Sort the entries.
      foreach( List<BuildLogFile.LogEntry> entries in LogEntriesByProject.Values )
      {
        entries.Sort();
      }

      // Create stats.
      foreach( string projectName in LogEntriesByProject.Keys )
      {
        Project project = Project.GetProject( projectName );

        List< BuildLogFile.LogEntry > entries = LogEntriesByProject[ projectName ];

        for( int i = 0; i < entries.Count - 1; i++ )
        {
          if( entries[ i ].EntryType == BuildLogFile.LogEntry.LogEntryType.BUILD_STARTED &&
              entries[ i + 1].EntryType == BuildLogFile.LogEntry.LogEntryType.BUILD_ENDED )
          {
            if( significantBuildsOnly == false ||
                ( entries[ i + 1 ].Timestamp - entries[ i ].Timestamp ).TotalSeconds > 5 )
            {
              project.AddBuild(
                entries[ i ].Timestamp,
                entries[ i + 1 ].Timestamp,
                entries[ i ].Tags.ToArray() );
            }
          }
          else if( entries[ i ].EntryType == BuildLogFile.LogEntry.LogEntryType.BUILD_STARTED )
          {
            if( significantBuildsOnly == false )
            {
              project.AddBuild(
                entries[ i ].Timestamp,
                null,
                entries[ i ].Tags.ToArray() );
            }
          }
        }
      }

      foreach( Project prj in Project.GetProjects() )
      {
        IBuildStatsProvider stats = prj.GetStats( "All" );

        Stats.Add( stats );
      }

      // Summary.
      foreach( BuildTag tag in BuildTag.GetTags() )
      {
        OutputStats( tag );
      }
    }

    //-------------------------------------------------------------------------

    private static void OutputStats( BuildTag tag )
    {
      Console.WriteLine( "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++" );
      Console.WriteLine();

      if( tag != null )
      {
        Console.WriteLine(
          "Tag: {0}" + Environment.NewLine,
          tag.Text );
      }

      Console.WriteLine(
        "{0,-20} | {1,-9} | {2,-5} | {3,-5} | {4,-5} | {5,-5} | {6,-5} | {7,-5} |",
        "Project",
        "Completed",
        "Tot h",
        "Tot m",
        "Avg m",
        "Avg s",
        "Max m",
        "Max s" );

      Console.WriteLine( "----------------------------------------------------------------------------------" );

      double totTotalBuildTime = 0.0;
      double totAvgBuildTime = 0.0;
      double totMaxBuildTime = 0.0;

      foreach( Project prj in Project.GetProjects() )
      {
        IBuildStatsProvider stats = prj.GetStats( tag?.Text );

        if( stats == null )
        {
          continue;
        }

        Console.WriteLine(
          "{0,-20} | {1,4}/{2,-4} | {3,5} | {4,5} | {5,5} | {6,5} | {7,5} | {8,5} |",
          prj.Name,
          stats.CompletedBuildsCount,
          stats.TotalBuildsCount,
          (int)stats.TotalBuildTime.TotalHours,
          (int)stats.TotalBuildTime.TotalMinutes,
          (int)stats.AverageBuildTime.TotalMinutes,
          (int)stats.AverageBuildTime.TotalSeconds,
          (int)stats.MaxBuildTime.TotalMinutes,
          (int)stats.MaxBuildTime.TotalSeconds );

        totTotalBuildTime += stats.TotalBuildTime.TotalMinutes;
        totAvgBuildTime += stats.AverageBuildTime.TotalMinutes;
        totMaxBuildTime += stats.MaxBuildTime.TotalMinutes;
      }

      Console.WriteLine( "----------------------------------------------------------------------------------" );

      Console.WriteLine(
        "{0,-20} | {1,4} {2,-4} | {3,5} | {4,5} | {5,5} | {6,5} | {7,5} | {8,5} |",
        "",
        "",
        "",
        "",
        (int)totTotalBuildTime,
        (int)totAvgBuildTime,
        "",
        (int)totMaxBuildTime,
        "" );

      Console.WriteLine( Environment.NewLine );
    }

    //-------------------------------------------------------------------------

    void ProcessFilesAndImportIntoDb(
      string[] files,
      Db.DbConnection dbConnection )
    {
      SqlCommand sqlCmd = dbConnection.CreateCommand();
      sqlCmd.Parameters.Add( "@filename", System.Data.SqlDbType.NVarChar );
      sqlCmd.Parameters.Add( "@eventType", System.Data.SqlDbType.NVarChar );
      sqlCmd.Parameters.Add( "@timestamp", System.Data.SqlDbType.SmallDateTime );
      sqlCmd.CommandText = "INSERT INTO ImportData VALUES ( @filename, @eventType, @timestamp )";

      foreach( string f in files )
      {
        try
        {
          sqlCmd.Parameters[ "@filename" ].Value =
            Path.GetFileNameWithoutExtension( f );

          // Parse the log file.
          BuildLogFile log = new BuildLogFile( f );

          string[] tags = log.GetTags();
          IReadOnlyCollection<BuildLogFile.LogEntry> entries = log.GetEntries();

          // Extract the project name.
          string projectName = tags[ 1 ];

          // Add log entries to collection.
          foreach( BuildLogFile.LogEntry entry in entries )
          {
            sqlCmd.Parameters[ "@eventType" ].Value =
              ( entry.EntryType == BuildLogFile.LogEntry.LogEntryType.BUILD_STARTED ?
                "build.start" : "build.end" );

            sqlCmd.Parameters[ "@timestamp" ].Value = entry.Timestamp;

            sqlCmd.ExecuteScalar();
          }
        }
        catch( Exception ex )
        {
          // TODO
        }
      }
    }

    //-------------------------------------------------------------------------
  }
}
