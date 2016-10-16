using System;
using System.Collections.Generic;
using System.IO;

namespace BuildSpyNark
{
  internal class BuildLogFile
  {
    //-------------------------------------------------------------------------

    public struct LogEntry : IComparable
    {
      public enum LogEntryType
      {
        BUILD_STARTED,
        BUILD_ENDED
      }

      public DateTime Timestamp { get; set; }
      public LogEntryType EntryType { get; set; }

      public int CompareTo( object obj )
      {
        if( obj is LogEntry == false )
        {
          throw new Exception( "Comparison cannot be applied to type." );
        }

        LogEntry compareToOb = (LogEntry)obj;

        if( compareToOb.Timestamp < Timestamp )
        {
          return 1;
        }
        else if( compareToOb.Timestamp > Timestamp )
        {
          return -1;
        }
        else
        {
          return 0;
        }
      }
    }

    private const char c_tagSeparator = '#';

    private List<string> Tags { get; set; } = null;
    private List<LogEntry> Entries { get; set; } = new List<LogEntry>();

    //-------------------------------------------------------------------------

    public BuildLogFile( string absFilename )
    {
      if( File.Exists( absFilename ) == false )
      {
        throw new FileNotFoundException( absFilename );
      }

      // Extract tags from the filename.
      string filenameOnly = Path.GetFileNameWithoutExtension( absFilename );
      Tags = new List<string>( filenameOnly.Split( c_tagSeparator ) );

      // Parse content.
      string[] lines = File.ReadAllLines( absFilename );

      foreach( string line in lines )
      {
        LogEntry entry = new LogEntry();

        string[] fields = line.Split( '|' );

        try
        {
          entry.Timestamp =
            new DateTime(
              int.Parse( fields[ 0 ].Substring( 0, 4 ) ),    // yyyy
              int.Parse( fields[ 0 ].Substring( 5, 2 ) ),    // MM
              int.Parse( fields[ 0 ].Substring( 8, 2 ) ),    // dd
              int.Parse( fields[ 0 ].Substring( 11, 2 ) ),   // HH
              int.Parse( fields[ 0 ].Substring( 14, 2 ) ),   // MM
              int.Parse( fields[ 0 ].Substring( 17, 2 ) ) ); // SS

          if( fields[ 1 ] == "build_start" )
          {
            entry.EntryType = LogEntry.LogEntryType.BUILD_STARTED;
          }
          else if( fields[ 1 ] == "build_end" )
          {
            entry.EntryType = LogEntry.LogEntryType.BUILD_ENDED;
          }
        }
        catch( Exception )
        {
          // TODO
        }

        Entries.Add( entry );
      }
    }

    //-------------------------------------------------------------------------

    public string[] GetTags()
    {
      return Tags.ToArray();
    }

    //-------------------------------------------------------------------------

    public IReadOnlyCollection<LogEntry> GetEntries()
    {
      return Entries;
    }

    //-------------------------------------------------------------------------
  }
}
