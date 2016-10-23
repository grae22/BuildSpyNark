namespace BuildSpyNark.Reports
{
  partial class ReportViewer
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
      this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
      this.BuildStatsCollectionBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.ProgramBindingSource = new System.Windows.Forms.BindingSource(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.BuildStatsCollectionBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ProgramBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // reportViewer1
      // 
      this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
      reportDataSource1.Name = "DataSet1";
      reportDataSource1.Value = this.ProgramBindingSource;
      this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
      this.reportViewer1.LocalReport.ReportEmbeddedResource = "BuildSpyNark.Reports.Report2.rdlc";
      this.reportViewer1.Location = new System.Drawing.Point(0, 0);
      this.reportViewer1.Name = "reportViewer1";
      this.reportViewer1.Size = new System.Drawing.Size(707, 492);
      this.reportViewer1.TabIndex = 0;
      // 
      // BuildStatsCollectionBindingSource
      // 
      this.BuildStatsCollectionBindingSource.DataSource = typeof(BuildSpyNark.BuildStatsCollection);
      // 
      // ProgramBindingSource
      // 
      this.ProgramBindingSource.DataMember = "Stats";
      this.ProgramBindingSource.DataSource = typeof(BuildSpyNark.Program);
      // 
      // ReportViewer
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(707, 492);
      this.Controls.Add(this.reportViewer1);
      this.Name = "ReportViewer";
      this.Text = "Report Viewer";
      this.Load += new System.EventHandler(this.ReportViewer_Load);
      ((System.ComponentModel.ISupportInitialize)(this.BuildStatsCollectionBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ProgramBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    private System.Windows.Forms.BindingSource BuildStatsCollectionBindingSource;
    private System.Windows.Forms.BindingSource ProgramBindingSource;
  }
}