using System;
using System.IO;
using System.Collections.Generic;

namespace BuildSpyNark
{
  class Program
  {
    //-------------------------------------------------------------------------

    private static Dictionary<string, List<BuildLogFile.LogEntry>> LogEntriesByProject =
      new Dictionary<string, List<BuildLogFile.LogEntry>>();

    //-------------------------------------------------------------------------

    static void Main( string[] args )
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

      // Get list of log files.
      string[] files =
        Directory.GetFiles(
          args[ 0 ],
          "*.buildspy-prj-log",
          SearchOption.TopDirectoryOnly );

      foreach( string f in files )
      {
        try
        {
          // Parse the log file.
          BuildLogFile log = new BuildLogFile( f );

          string[] tags = log.GetTags();
          IReadOnlyCollection<BuildLogFile.LogEntry> entries = log.GetEntries();

          // Extract the project name.
          string projectName = tags[ 3 ];

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
    }

    //-------------------------------------------------------------------------
  }
}
