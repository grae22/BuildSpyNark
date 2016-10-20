using System;
using System.Collections.Generic;

namespace BuildSpyNark
{
  abstract class IBuildStatsProvider
  {
    //-------------------------------------------------------------------------

    // Returns collection of all BuildStats objects.
    public abstract IReadOnlyCollection<BuildStats> BuildStats { get; }

    // Returns the total number of builds.
    public abstract uint TotalBuildsCount { get; protected set; }

    // Returns the number of completed builds.
    public abstract uint CompletedBuildsCount { get; protected set; }

    // Returns the total build time for all builds.
    public abstract TimeSpan TotalBuildTime { get; protected set; }

    // Returns the average build time.
    public abstract TimeSpan AverageBuildTime { get; protected set; }

    // Returns the average build time.
    public abstract TimeSpan MaxBuildTime { get; protected set; }

    //-------------------------------------------------------------------------
  }
}
