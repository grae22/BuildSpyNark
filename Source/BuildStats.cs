// A collection of stats relating to a single build.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BuildSpyNark
{
  class BuildStats
  {
    //-------------------------------------------------------------------------

    // Build's start time.
    public DateTime Start { get; private set; }

    // Build's end time, can be null if build didn't complete.
    public DateTime? End { get; private set; } = null;

    // Tags relating to this build.
    private List< BuildTag > Tags { get; set; } = new List< BuildTag >();

    //-------------------------------------------------------------------------

    public BuildStats(
      DateTime start,
      DateTime? end )
    {
      Start = start;
      End = end;
    }

    //-------------------------------------------------------------------------

    public void AddTag( BuildTag tag )
    {
      if( HasTag( tag ) == false )
      {
        Tags.Add( tag );
      }
    }

    //-------------------------------------------------------------------------

    public bool HasTag( BuildTag tag )
    {
      return Tags.Contains( tag );
    }

    //-------------------------------------------------------------------------

    public bool HasTag( string text )
    {
      foreach( BuildTag tag in Tags )
      {
        if( tag.Text.Equals( text, StringComparison.OrdinalIgnoreCase ) )
        {
          return true;
        }
      }

      return false;
    }

    //-------------------------------------------------------------------------

    public ReadOnlyCollection< BuildTag > GetTags()
    {
      return Tags.AsReadOnly();
    }

    //-------------------------------------------------------------------------

    // Returns null if build was not completed.

    public TimeSpan? GetBuildTime()
    {
      if( End == null )
      {
        return null;
      }

      return ( End - Start );
    }

    //-------------------------------------------------------------------------
  }
}
