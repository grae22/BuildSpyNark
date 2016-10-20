// Collection of build stats with logic to compute further stats based
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

    public override IReadOnlyCollection<BuildStats> BuildStats
    {
      get
      {
        return Stats.AsReadOnly();
      }
    }

    // Number of builds.
    override public uint TotalBuildsCount { get; protected set; }

    // Number of completed builds.
    override public uint CompletedBuildsCount { get; protected set; }

    // Total build time for all builds.
    override public TimeSpan TotalBuildTime { get; protected set; }

    // Avg build time.
    override public TimeSpan AverageBuildTime { get; protected set; }

    // Max build time.
    override public TimeSpan MaxBuildTime { get; protected set; }

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

        // Average build time.
        AverageBuildTime =
          new TimeSpan( 0, 0, (int)( TotalBuildTime.TotalSeconds / CompletedBuildsCount ) );

        // Max build time.
        if( buildTime != null &&
            ( MaxBuildTime == null ||
              buildTime > MaxBuildTime ) )
        {
          MaxBuildTime = (TimeSpan)buildTime;
        }
      }
    }

    //-------------------------------------------------------------------------
  }
}
