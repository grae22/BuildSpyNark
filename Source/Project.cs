using System;
using System.Collections.Generic;

namespace BuildSpyNark
{
  internal class Project
  {
    //-------------------------------------------------------------------------

    // Static collection of all projects by name.
    private static Dictionary< string, Project > Projects { get; set; } =
      new Dictionary< string, Project >();

    //-------------------------------------------------------------------------

    // Returns specified project if it exists, otherwise creates it first.

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

    //-------------------------------------------------------------------------

    // Clears the static project collection.

    public static void Reset()
    {
      Projects.Clear();
    }

    //=========================================================================

    // Project's name.
    public string Name { get; private set; } = "";

    // Projects build stats.
    private  BuildStatsCollection Stats { get; set; } = new BuildStatsCollection();

    //-------------------------------------------------------------------------

    public Project( string name )
    {
      Name = name;
    }

    //-------------------------------------------------------------------------

    public void AddBuild(
      DateTime start,
      DateTime? end,
      string[] tags )
    {
      BuildStats stats = new BuildStats( start, end );

      foreach( string t in tags )
      {
        BuildTag tag = BuildTag.GetTag( t );

        stats.AddTag( tag );
      }

      Stats.AddStats( stats );
    }

    //-------------------------------------------------------------------------

    public IBuildStatsProvider GetStats()
    {
      return Stats;
    }

    //-------------------------------------------------------------------------
  }
}
