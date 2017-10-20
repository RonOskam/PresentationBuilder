// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Dialogs.ReportDesign
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace PEC.Windows.Common.Dialogs
{
  public class ReportDesign : XRDesignFormEx
  {
    private IContainer components = (IContainer) null;

    public ReportDesign()
    {
      this.InitializeComponent();
    }

    public void NewReport(object dataSource)
    {
      XtraReport report = new XtraReport();
      Font font = new Font("Arial", 10f);
      report.Font = font;
      report.Margins = new Margins(50, 50, 50, 50);
      ReportHeaderBand reportHeaderBand = new ReportHeaderBand();
      reportHeaderBand.Name = "ReportHeader";
      reportHeaderBand.Font = font;
      report.Bands.Add((Band) reportHeaderBand);
      DetailBand detailBand = new DetailBand();
      detailBand.Name = "Detail";
      detailBand.Font = font;
      report.Bands.Add((Band) detailBand);
      report.DataSource = dataSource;
      this.OpenReport(report);
    }

    public void LoadReport(MemoryStream reportStream, object dataSource)
    {
      XtraReport report = new XtraReport();
      report.LoadLayout((Stream) reportStream);
      report.DataSource = dataSource;
      this.OpenReport(report);
    }

    public MemoryStream GetReport()
    {
      MemoryStream memoryStream = new MemoryStream();
      this.xrDesignPanel.Report.SaveLayout((Stream) memoryStream);
      return memoryStream;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      if (this.xrDesignPanel.ReportState != ReportState.Changed)
        return;
      DialogResult dialogResult = MessageBox.Show("Do you want to save your changes to this report?", Application.ProductName, MessageBoxButtons.YesNoCancel);
      if (dialogResult == DialogResult.Cancel)
        e.Cancel = true;
      else
        this.DialogResult = dialogResult;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Text = "ReportDesignerEx";
    }
  }
}
