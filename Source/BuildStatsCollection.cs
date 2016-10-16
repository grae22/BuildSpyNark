﻿// Collection of build stats with logic to compute further stats based
// on the contents of the collection.

using System;
using System.Collections.Generic;

namespace BuildSpyNark
{
  class BuildStatsCollection : IBuildStatsProvider
  {
    //-------------------------------------------------------------------------

    // Collection of build stats.
    private List<BuildStats> Stats { get; set; } = new List<BuildStats>();

    // Number of builds.
    private uint TotalBuildsCount { get; set; }

    // Number of completed builds.
    private uint CompletedBuildsCount { get; set; }

    // Total build time for all builds.
    private TimeSpan TotalBuildTime { get; set; }

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

    public override IReadOnlyCollection<BuildStats> GetBuildStats()
    {
      return Stats.AsReadOnly();
    }

    //-------------------------------------------------------------------------

    public override uint GetTotalBuildsCount()
    {
      return TotalBuildsCount;
    }

    //-------------------------------------------------------------------------

    public override uint GetCompletedBuildsCount()
    {
      return CompletedBuildsCount;
    }

    //-------------------------------------------------------------------------

    public override TimeSpan GetTotalBuildTime()
    {
      return TotalBuildTime;
    }

    //-------------------------------------------------------------------------
  }
}
