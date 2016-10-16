using System;
using System.Collections.Generic;

namespace BuildSpyNark
{
  abstract class IBuildStatsProvider
  {
    //-------------------------------------------------------------------------

    // Returns collection of all BuildStats objects.
    public abstract IReadOnlyCollection< BuildStats > GetBuildStats();

    // Returns the total number of builds.
    public abstract uint GetTotalBuildsCount();

    // Returns the number of completed builds.
    public abstract uint GetCompletedBuildsCount();

    // Returns the total build time for all builds.
    public abstract TimeSpan GetTotalBuildTime();

    // Returns the average build time.
    public abstract TimeSpan GetAverageBuildTime();

    //-------------------------------------------------------------------------
  }
}
