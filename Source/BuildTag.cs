using System.Collections.Generic;

namespace BuildSpyNark
{
  class BuildTag
  {
    //== BuildTag factory =====================================================

    // Static collection of all tags by text.
    private static Dictionary<string, BuildTag> Tags { get; set; } =
      new Dictionary<string, BuildTag>();

    //-------------------------------------------------------------------------

    // Returns a tag if it already exists, otherwise creates it first.

    public static BuildTag GetTag(
      string text,
      bool createTagIfNotFound = true )
    {
      foreach( string tagText in Tags.Keys )
      {
        if( text.ToLower() == tagText.ToLower() )
        {
          return Tags[ tagText ];
        }
      }

      if( createTagIfNotFound == false )
      {
        return null;
      }

      BuildTag tag = new BuildTag( text );
      Tags.Add( text, tag );

      return tag;
    }

    //-------------------------------------------------------------------------

    public static IReadOnlyCollection<BuildTag> GetTags()
    {
      return Tags.Values;
    }

    //-------------------------------------------------------------------------

    // Clears the static tag collection.

    public static void Reset()
    {
      Tags.Clear();
    }

    //== BuildTag intance =====================================================

    public string Text { get; private set; }

    //-------------------------------------------------------------------------

    // Instantiate with BuildTag.GetTag().

    private BuildTag()
    {
    }

    //-------------------------------------------------------------------------

    // Instantiate with BuildTag.GetTag().

    private BuildTag( string text )
    {
      Text = text;
    }

    //-------------------------------------------------------------------------
  }
}
