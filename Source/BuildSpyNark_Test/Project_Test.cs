using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BuildSpyNark;

namespace BuildSpyNark_Test
{
  [TestClass]
  public class Project_Test
  {
    //-------------------------------------------------------------------------

    private Project TestObject { get; set; }

    //-------------------------------------------------------------------------

    [TestInitialize]
    public void Initialise()
    {
      Project.Reset();

      TestObject = Project.GetProject( "Test" );
    }

    //=========================================================================

    [TestMethod]
    public void CreateProject()
    {
      Assert.IsNotNull( TestObject );
      Assert.AreEqual( "Test", TestObject.Name );
    }

    //-------------------------------------------------------------------------

    [TestMethod]
    public void AddBuild()
    {
      DateTime start = new DateTime( 0 );
      DateTime end = new DateTime( 2016, 1, 1, 0, 0, 10 );
      string[] tags = { "TestTag" };

      TestObject.AddBuild( start, end, tags );

      Assert.AreEqual< uint >( 1, TestObject.GetStats().GetTotalBuildsCount() );
      Assert.AreEqual< uint >( 1, TestObject.GetStats().GetCompletedBuildsCount() );
      Assert.AreEqual( end - start, TestObject.GetStats().GetTotalBuildTime() );
      Assert.AreEqual( 1, TestObject.GetStats().GetBuildStats().Count );
    }

    //-------------------------------------------------------------------------

    [TestMethod]
    public void AddIncompleteBuild()
    {
      DateTime start = new DateTime( 0 );
      DateTime? end = null;
      string[] tags = { "TestTag" };

      TestObject.AddBuild( start, end, tags );

      Assert.AreEqual< uint >( 1, TestObject.GetStats().GetTotalBuildsCount() );
      Assert.AreEqual< uint >( 0, TestObject.GetStats().GetCompletedBuildsCount() );
      Assert.AreEqual( new TimeSpan(), TestObject.GetStats().GetTotalBuildTime() );
      Assert.AreEqual( 1, TestObject.GetStats().GetBuildStats().Count );
    }

    //-------------------------------------------------------------------------

    [TestMethod]
    [ExpectedException( typeof( Exception ) )]
    public void AllTagIsReserved()
    {
      DateTime start = new DateTime( 0 );
      DateTime? end = null;
      string[] tags = { "All" };

      TestObject.AddBuild( start, end, tags );
    }

    //-------------------------------------------------------------------------
  }
}
