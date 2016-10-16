using System;
using System.Collections.Generic;
using System.IO;

namespace BuildSpyNark
{
  class BuildLogFile
  {
    //-------------------------------------------------------------------------

    private const char c_tagSeparator = '#';

    private List<string> Tags { get; set; } = null;

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
        string[] fields = line.Split( '|' );

        DateTime timestamp =
          new DateTime(
            int.Parse( fields[ 0 ].Substring( 0, 4 ) ),    // yyyy
            int.Parse( fields[ 0 ].Substring( 4, 2 ) ),    // MM
            int.Parse( fields[ 0 ].Substring( 6, 2 ) ),    // dd
            int.Parse( fields[ 0 ].Substring( 8, 2 ) ),    // HH
            int.Parse( fields[ 0 ].Substring( 10, 2 ) ),   // MM
            int.Parse( fields[ 0 ].Substring( 12, 2 ) ) ); // SS
      }
    }

    //-------------------------------------------------------------------------
  }
}
