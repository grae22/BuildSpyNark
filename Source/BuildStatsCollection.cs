// Collection of build stats with logic to compute further stats based
// on the contents of the collection.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BuildSpyNark
{
  class BuildStatsCollection
  {
    //-------------------------------------------------------------------------

    // Collection of build stats.
    private List< BuildStats > Stats { get; set; } = new List< BuildStats >();

    // Number of builds.
    public uint TotalBuildsCount { get; private set; }

    // Number of completed builds.
    public uint CompletedBuildsCount { get; private set; }

    // Total build time for all builds.
    public TimeSpan TotalBuildTime { get; private set; }

    //-------------------------------------------------------------------------

    public void AddStats( BuildStats stats )
    {
      if( Stats.Contains( stats ) == false )
      {
        Stats.Add( stats );

        //-- Update stats.

        // Total build count.
        TotalBuildsCount++;

        // Completed builds count.
        if( stats.End != null )
        {
          CompletedBuildsCount++;
        }

        // Total build time for all builds.
        TimeSpan? buildTime = stats.GetBuildTime();

        if( buildTime != null )
        {
          TotalBuildTime += (TimeSpan)buildTime;
        }
      }
    }

    //-------------------------------------------------------------------------

    public ReadOnlyCollection< BuildStats > GetStats()
    {
      return Stats.AsReadOnly();
    }

    //-------------------------------------------------------------------------
  }
}
