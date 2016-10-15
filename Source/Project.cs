using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BuildSpyNark
{
  class Project
  {
    //-------------------------------------------------------------------------

    // Static collection of all projects by name.
    private static Dictionary< string, Project > Projects { get; set; } =
      new Dictionary< string, Project >();

    //-------------------------------------------------------------------------

    // Returns specified project if it exists, otherwise creates it.

    public static Project GetProject( string name )
    {
      if( Projects.ContainsKey( name ) )
      {
        return Projects[ name ];
      }

      Project project = new Project( name );
      Projects.Add( name, project );

      return project;
    }

    //=========================================================================

    // Project's name.
    public string Name { get; private set; } = "";

    // Projects build stats.
    public BuildStatsCollection Stats { get; private set; } = new BuildStatsCollection();

    //-------------------------------------------------------------------------

    public Project( string name )
    {
      Name = name;
    }

    //-------------------------------------------------------------------------

    public void AddStats( BuildStats stats )
    {
      Stats.AddStats( stats );
    }

    //-------------------------------------------------------------------------

    public ReadOnlyCollection< BuildStats > GetStats()
    {
      return Stats.GetStats();
    }

    //-------------------------------------------------------------------------
  }
}
