using System;
using System.Reflection;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BuildSpyNark;

namespace BuildSpyNark_Test
{
  [TestClass]
  public class BuildLogFile_Test
  {
    //-------------------------------------------------------------------------

    [TestMethod]
    public void ParseLogFile()
    {
      string absLogFilename =
        Path.GetDirectoryName(
          Assembly.GetExecutingAssembly().CodeBase ) +
        @"\..\..\Resources\" +
        "20161013221428#GraemePC#SomeSolution#SomeProject#SomeProfile.buildspy-prj-log";

      absLogFilename = absLogFilename.Replace( @"file:\", "" );

      BuildLogFile testOb = new BuildLogFile( absLogFilename );
    }

    //-------------------------------------------------------------------------
  }
}
