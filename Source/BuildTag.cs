using System.Collections.Generic;

namespace BuildSpyNark
{
  class BuildTag
  {
    //-------------------------------------------------------------------------

    // Static collection of all tags by text.
    private static Dictionary< string, BuildTag > Tags { get; set; } =
      new Dictionary< string, BuildTag >();

    //-------------------------------------------------------------------------

    public static BuildTag GetTag( string text )
    {
      if( Tags.ContainsKey( text ) )
      {
        return Tags[ text ];
      }

      BuildTag tag = new BuildTag( text );
      Tags.Add( text, tag );

      return tag;
    }

    //=========================================================================

    public string Text { get; private set; }

    //-------------------------------------------------------------------------

    public BuildTag( string text )
    {
      Text = text;
    }

    //-------------------------------------------------------------------------
  }
}
