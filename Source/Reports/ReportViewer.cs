using System;
using System.Windows.Forms;

namespace BuildSpyNark.Reports
{
  partial class ReportViewer : Form
  {
    //-------------------------------------------------------------------------

    public ReportViewer( Program stats )
    {
      InitializeComponent();

      //ProgramBindingSource.DataSource = stats;
    }

    //-------------------------------------------------------------------------

    private void ReportViewer_Load( object sender, EventArgs e )
    {
      this.reportViewer1.RefreshReport();
    }

    //-------------------------------------------------------------------------
  }
}
