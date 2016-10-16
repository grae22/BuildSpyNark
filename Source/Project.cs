using System;
using System.Collections.Generic;

namespace BuildSpyNark
{
  internal class Project
  {
    //== Project factory ======================================================

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

    //== Project instance ======================================================

    // Project's name.
    public string Name { get; private set; } = "";

    // Project's build stats by tag.
    private Dictionary< BuildTag, BuildStatsCollection > Stats { get; set; } =
      new Dictionary< BuildTag, BuildStatsCollection >();

    // 'All' build tag.
    private BuildTag AllBuilds = BuildTag.GetTag( "All" );

    //-------------------------------------------------------------------------

    // Instantiate with Project.GetProject().

    private Project()
    {
    }

    //-------------------------------------------------------------------------

    // Instantiate with Project.GetProject().

    private Project( string name )
    {
      Name = name;
    }

    //-------------------------------------------------------------------------

    public void AddBuild(
      DateTime start,
      DateTime? end,
      string[] tags )
    {
      // Create new stats object.
      BuildStats stats = new BuildStats( start, end );

      // Include these stats under the 'all' tag.
      stats.AddTag( AllBuilds );

      // Add the tags to the stats object.
      foreach( string t in tags )
      {
        // 'All' is reserved.
        if( t.ToLower() == "all" )
        {
          throw new Exception( "The tag 'all' is reserved for internal use." );
        }

        // Add the tag to the stats.
        BuildTag tag = BuildTag.GetTag( t );
        stats.AddTag( tag );
      }

      // Add build to the relevant tags' build collection.
      foreach( BuildTag tag in stats.GetTags() )
      {
        if( Stats.ContainsKey( tag ) == false )
        {
          Stats.Add( tag, new BuildStatsCollection() );
        }

        Stats[ tag ].AddStats( stats );
      }
    }

    //-------------------------------------------------------------------------

    // Returns stats provider for the specified tag, use null for all stats.

    public IBuildStatsProvider GetStats( string tag = null )
    {
      if( tag == null )
      {
        tag = "All";
      }

      BuildTag tagOb = BuildTag.GetTag( tag, false );

      if( tagOb == null )
      {
        return null;
      }

      if( Stats.ContainsKey( tagOb ) )
      {
        return Stats[ tagOb ];
      }

      return null;
    }

    //-------------------------------------------------------------------------
  }
}
