// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Dialogs.ReportPreview
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Control;
using DevExpress.XtraPrinting.Preview;
using DevExpress.XtraReports.UI;
using PEC.Configuration;
using PEC.Windows.Common;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace PEC.Windows.Common.Dialogs
{
  public class ReportPreview : Form
  {
    private string _reportName = (string) null;
    private System.Collections.Specialized.StringCollection _emailTo = new System.Collections.Specialized.StringCollection();
    private IContainer components = (IContainer) null;
    private XtraReport _xtraReport;
    private ReportPreview.MyCommandHandler _commandHandler;
    private PrintControl printControl;
    private PrintingSystem printingSystem;
    private PrintBarManager printBarManager1;
    private PreviewBar previewBar1;
    private PrintPreviewBarItem printPreviewBarItem1;
    private PrintPreviewBarItem printPreviewBarItem2;
    private PrintPreviewBarItem printPreviewBarItem3;
    private PrintPreviewBarItem printPreviewBarItem4;
    private PrintPreviewBarItem printPreviewBarItem5;
    private PrintPreviewBarItem printPreviewBarItem6;
    private PrintPreviewBarItem printPreviewBarItem7;
    private PrintPreviewBarItem printPreviewBarItem8;
    private PrintPreviewBarItem printPreviewBarItem9;
    private PrintPreviewBarItem printPreviewBarItem10;
    private PrintPreviewBarItem printPreviewBarItem11;
    private ZoomBarEditItem zoomBarEditItem1;
    private PrintPreviewRepositoryItemComboBox printPreviewRepositoryItemComboBox1;
    private PrintPreviewBarItem printPreviewBarItem12;
    private PrintPreviewBarItem printPreviewBarItem13;
    private PrintPreviewBarItem printPreviewBarItem14;
    private PrintPreviewBarItem printPreviewBarItem15;
    private PrintPreviewBarItem printPreviewBarItem16;
    private PrintPreviewBarItem printPreviewBarItem17;
    private PrintPreviewBarItem printPreviewBarItem18;
    private PrintPreviewBarItem printPreviewBarItem19;
    private PrintPreviewBarItem printPreviewBarItem20;
    private PrintPreviewBarItem printPreviewBarItem21;
    private PrintPreviewBarItem printPreviewBarItem22;
    private PreviewBar previewBar2;
    private PrintPreviewStaticItem printPreviewStaticItem1;
    private PrintPreviewStaticItem printPreviewStaticItem2;
    private PrintPreviewStaticItem printPreviewStaticItem3;
    private PreviewBar previewBar3;
    private PrintPreviewSubItem printPreviewSubItem1;
    private PrintPreviewSubItem printPreviewSubItem2;
    private PrintPreviewSubItem printPreviewSubItem4;
    private PrintPreviewBarItem printPreviewBarItem23;
    private PrintPreviewBarItem printPreviewBarItem24;
    private BarToolbarsListItem barToolbarsListItem1;
    private PrintPreviewSubItem printPreviewSubItem3;
    private BarDockControl barDockControlTop;
    private BarDockControl barDockControlBottom;
    private BarDockControl barDockControlLeft;
    private BarDockControl barDockControlRight;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem1;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem2;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem3;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem4;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem5;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem6;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem7;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem8;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem9;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem10;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem11;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem12;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem13;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem14;
    private PrintPreviewBarCheckItem printPreviewBarCheckItem15;

    public System.Collections.Specialized.StringCollection EmailTo
    {
      get
      {
        return this._emailTo;
      }
      set
      {
        this._emailTo = value;
        if (this._commandHandler == null)
          return;
        this._commandHandler.EmailTo = value;
      }
    }

    public ReportPreview()
    {
      this.InitializeComponent();
      ConfigurationManager.LoadFormBounds((Form) this);
    }

    public bool LoadReport(MemoryStream reportStream, object dataSource, string reportName)
    {
      this._reportName = reportName;
      if ((ulong) reportStream.Length <= 0UL)
        return false;
      this._xtraReport = new XtraReport();
      this._xtraReport.LoadLayout((Stream) reportStream);
      this._xtraReport.Name = reportName.Replace(" ", "");
      reportStream.Close();
      this.SetupReport(dataSource, reportName);
      return true;
    }

    public bool LoadReport(XtraReport report, object dataSource, string reportName)
    {
      this._reportName = reportName;
      this._xtraReport = report;
      this.SetupReport(dataSource, reportName);
      return true;
    }

    private void SetupReport(object dataSource, string reportName)
    {
      this._xtraReport.DataSource = dataSource;
      this._xtraReport.Name = reportName.Replace(" ", "").Replace("-", "");
      this.printControl.PrintingSystem = this._xtraReport.PrintingSystem;
      this._xtraReport.CreateDocument();
      this._commandHandler = new ReportPreview.MyCommandHandler();
      this._commandHandler.EmailTo = this._emailTo;
      this._xtraReport.PrintingSystem.AddCommandHandler((ICommandHandler) this._commandHandler);
    }

    public void WireUpBeforePrint(string dataMemeber, PrintEventHandler beforePrint)
    {
      this.WireUpBeforePrint(this._xtraReport.Bands, dataMemeber, beforePrint);
    }

    private void WireUpBeforePrint(BandCollection bands, string dataMember, PrintEventHandler beforePrint)
    {
      foreach (Band band in (CollectionBase) bands)
      {
        if (band is DetailBand)
        {
          if (band.Report.DataMember == dataMember)
            band.BeforePrint += beforePrint;
        }
        else if (band is DetailReportBand)
          this.WireUpBeforePrint((band as DetailReportBand).Bands, dataMember, beforePrint);
      }
    }

    private void ReportPreview_FormClosed(object sender, FormClosedEventArgs e)
    {
      ConfigurationManager.SaveFormBounds((Form) this);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReportPreview));
      this.printControl = new PrintControl();
      this.printingSystem = new PrintingSystem(this.components);
      this.printBarManager1 = new PrintBarManager();
      this.previewBar1 = new PreviewBar();
      this.printPreviewBarItem1 = new PrintPreviewBarItem();
      this.printPreviewBarItem2 = new PrintPreviewBarItem();
      this.printPreviewBarItem3 = new PrintPreviewBarItem();
      this.printPreviewBarItem4 = new PrintPreviewBarItem();
      this.printPreviewBarItem5 = new PrintPreviewBarItem();
      this.printPreviewBarItem6 = new PrintPreviewBarItem();
      this.printPreviewBarItem7 = new PrintPreviewBarItem();
      this.printPreviewBarItem8 = new PrintPreviewBarItem();
      this.printPreviewBarItem9 = new PrintPreviewBarItem();
      this.printPreviewBarItem10 = new PrintPreviewBarItem();
      this.printPreviewBarItem11 = new PrintPreviewBarItem();
      this.zoomBarEditItem1 = new ZoomBarEditItem();
      this.printPreviewRepositoryItemComboBox1 = new PrintPreviewRepositoryItemComboBox();
      this.printPreviewBarItem12 = new PrintPreviewBarItem();
      this.printPreviewBarItem13 = new PrintPreviewBarItem();
      this.printPreviewBarItem14 = new PrintPreviewBarItem();
      this.printPreviewBarItem15 = new PrintPreviewBarItem();
      this.printPreviewBarItem16 = new PrintPreviewBarItem();
      this.printPreviewBarItem17 = new PrintPreviewBarItem();
      this.printPreviewBarItem18 = new PrintPreviewBarItem();
      this.printPreviewBarItem19 = new PrintPreviewBarItem();
      this.printPreviewBarItem20 = new PrintPreviewBarItem();
      this.printPreviewBarItem21 = new PrintPreviewBarItem();
      this.printPreviewBarItem22 = new PrintPreviewBarItem();
      this.previewBar2 = new PreviewBar();
      this.printPreviewStaticItem1 = new PrintPreviewStaticItem();
      this.printPreviewStaticItem2 = new PrintPreviewStaticItem();
      this.printPreviewStaticItem3 = new PrintPreviewStaticItem();
      this.previewBar3 = new PreviewBar();
      this.printPreviewSubItem1 = new PrintPreviewSubItem();
      this.printPreviewSubItem2 = new PrintPreviewSubItem();
      this.printPreviewSubItem4 = new PrintPreviewSubItem();
      this.printPreviewBarItem23 = new PrintPreviewBarItem();
      this.printPreviewBarItem24 = new PrintPreviewBarItem();
      this.barToolbarsListItem1 = new BarToolbarsListItem();
      this.printPreviewSubItem3 = new PrintPreviewSubItem();
      this.barDockControlTop = new BarDockControl();
      this.barDockControlBottom = new BarDockControl();
      this.barDockControlLeft = new BarDockControl();
      this.barDockControlRight = new BarDockControl();
      this.printPreviewBarCheckItem1 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem2 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem3 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem4 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem5 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem6 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem7 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem8 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem9 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem10 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem11 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem12 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem13 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem14 = new PrintPreviewBarCheckItem();
      this.printPreviewBarCheckItem15 = new PrintPreviewBarCheckItem();
      //this.printingSystem.BeginInit();
      this.printBarManager1.BeginInit();
      this.printPreviewRepositoryItemComboBox1.BeginInit();
      this.SuspendLayout();
      this.printControl.BackColor = Color.Empty;
      this.printControl.Dock = DockStyle.Fill;
      this.printControl.ForeColor = Color.Empty;
      this.printControl.IsMetric = false;
      this.printControl.Location = new Point(0, 51);
      this.printControl.Name = "printControl";
      this.printControl.PrintingSystem = (PrintingSystemBase) this.printingSystem;
      this.printControl.ShowPageMargins = false;
      this.printControl.Size = new Size(736, 482);
      this.printControl.TabIndex = 0;
      this.printControl.TabStop = false;
      this.printControl.TooltipFont = new Font("Tahoma", 8.25f);
      this.printBarManager1.Bars.AddRange(new Bar[3]
      {
        (Bar) this.previewBar1,
        (Bar) this.previewBar2,
        (Bar) this.previewBar3
      });
      this.printBarManager1.DockControls.Add(this.barDockControlTop);
      this.printBarManager1.DockControls.Add(this.barDockControlBottom);
      this.printBarManager1.DockControls.Add(this.barDockControlLeft);
      this.printBarManager1.DockControls.Add(this.barDockControlRight);
      this.printBarManager1.Form = (Control) this;
      this.printBarManager1.ImageStream = (ImageCollectionStreamer) componentResourceManager.GetObject("printBarManager1.ImageStream");
      this.printBarManager1.Items.AddRange(new BarItem[48]
      {
        (BarItem) this.printPreviewStaticItem1,
        (BarItem) this.printPreviewStaticItem2,
        (BarItem) this.printPreviewStaticItem3,
        (BarItem) this.printPreviewBarItem1,
        (BarItem) this.printPreviewBarItem2,
        (BarItem) this.printPreviewBarItem3,
        (BarItem) this.printPreviewBarItem4,
        (BarItem) this.printPreviewBarItem5,
        (BarItem) this.printPreviewBarItem6,
        (BarItem) this.printPreviewBarItem7,
        (BarItem) this.printPreviewBarItem8,
        (BarItem) this.printPreviewBarItem9,
        (BarItem) this.printPreviewBarItem10,
        (BarItem) this.printPreviewBarItem11,
        (BarItem) this.zoomBarEditItem1,
        (BarItem) this.printPreviewBarItem12,
        (BarItem) this.printPreviewBarItem13,
        (BarItem) this.printPreviewBarItem14,
        (BarItem) this.printPreviewBarItem15,
        (BarItem) this.printPreviewBarItem16,
        (BarItem) this.printPreviewBarItem17,
        (BarItem) this.printPreviewBarItem18,
        (BarItem) this.printPreviewBarItem19,
        (BarItem) this.printPreviewBarItem20,
        (BarItem) this.printPreviewBarItem21,
        (BarItem) this.printPreviewBarItem22,
        (BarItem) this.printPreviewSubItem1,
        (BarItem) this.printPreviewSubItem2,
        (BarItem) this.printPreviewSubItem3,
        (BarItem) this.printPreviewSubItem4,
        (BarItem) this.printPreviewBarItem23,
        (BarItem) this.printPreviewBarItem24,
        (BarItem) this.barToolbarsListItem1,
        (BarItem) this.printPreviewBarCheckItem1,
        (BarItem) this.printPreviewBarCheckItem2,
        (BarItem) this.printPreviewBarCheckItem3,
        (BarItem) this.printPreviewBarCheckItem4,
        (BarItem) this.printPreviewBarCheckItem5,
        (BarItem) this.printPreviewBarCheckItem6,
        (BarItem) this.printPreviewBarCheckItem7,
        (BarItem) this.printPreviewBarCheckItem8,
        (BarItem) this.printPreviewBarCheckItem9,
        (BarItem) this.printPreviewBarCheckItem10,
        (BarItem) this.printPreviewBarCheckItem11,
        (BarItem) this.printPreviewBarCheckItem12,
        (BarItem) this.printPreviewBarCheckItem13,
        (BarItem) this.printPreviewBarCheckItem14,
        (BarItem) this.printPreviewBarCheckItem15
      });
      this.printBarManager1.MainMenu = (Bar) this.previewBar3;
      this.printBarManager1.MaxItemId = 48;
      this.printBarManager1.PreviewBar = (Bar) this.previewBar1;
      this.printBarManager1.PrintControl = this.printControl;
      this.printBarManager1.RepositoryItems.AddRange(new RepositoryItem[1]
      {
        (RepositoryItem) this.printPreviewRepositoryItemComboBox1
      });
      this.printBarManager1.StatusBar = (Bar) this.previewBar2;
      this.previewBar1.BarName = "Toolbar";
      this.previewBar1.DockCol = 0;
      this.previewBar1.DockRow = 1;
      this.previewBar1.DockStyle = BarDockStyle.Top;
      this.previewBar1.LinksPersistInfo.AddRange(new LinkPersistInfo[23]
      {
        new LinkPersistInfo((BarItem) this.printPreviewBarItem1),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem2),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem3, true),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem4, true),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem5),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem6),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem7),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem8),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem9, true),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem10),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem11, true),
        new LinkPersistInfo((BarItem) this.zoomBarEditItem1),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem12),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem13, true),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem14),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem15),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem16),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem17, true),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem18),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem19),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem20, true),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem21),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem22, true)
      });
      this.previewBar1.Text = "Toolbar";
      this.printPreviewBarItem1.ButtonStyle = BarButtonStyle.Check;
      this.printPreviewBarItem1.Caption = "Document Map";
      this.printPreviewBarItem1.Command = PrintingSystemCommand.DocumentMap;
      this.printPreviewBarItem1.Enabled = false;
      this.printPreviewBarItem1.Hint = "Document Map";
      this.printPreviewBarItem1.Id = 3;
      this.printPreviewBarItem1.ImageIndex = 19;
      this.printPreviewBarItem1.Name = "printPreviewBarItem1";
      this.printPreviewBarItem1.Visibility = BarItemVisibility.Never;
      this.printPreviewBarItem2.Caption = "Search";
      this.printPreviewBarItem2.Command = PrintingSystemCommand.Find;
      this.printPreviewBarItem2.Enabled = false;
      this.printPreviewBarItem2.Hint = "Search";
      this.printPreviewBarItem2.Id = 4;
      this.printPreviewBarItem2.ImageIndex = 20;
      this.printPreviewBarItem2.Name = "printPreviewBarItem2";
      this.printPreviewBarItem3.ButtonStyle = BarButtonStyle.Check;
      this.printPreviewBarItem3.Caption = "Customize";
      this.printPreviewBarItem3.Command = PrintingSystemCommand.Customize;
      this.printPreviewBarItem3.Enabled = false;
      this.printPreviewBarItem3.Hint = "Customize";
      this.printPreviewBarItem3.Id = 5;
      this.printPreviewBarItem3.ImageIndex = 14;
      this.printPreviewBarItem3.Name = "printPreviewBarItem3";
      this.printPreviewBarItem4.ButtonStyle = BarButtonStyle.Check;
      this.printPreviewBarItem4.Caption = "&Print...";
      this.printPreviewBarItem4.Command = PrintingSystemCommand.Print;
      this.printPreviewBarItem4.Enabled = false;
      this.printPreviewBarItem4.Hint = "Print";
      this.printPreviewBarItem4.Id = 6;
      this.printPreviewBarItem4.ImageIndex = 0;
      this.printPreviewBarItem4.Name = "printPreviewBarItem4";
      this.printPreviewBarItem5.Caption = "P&rint";
      this.printPreviewBarItem5.Command = PrintingSystemCommand.PrintDirect;
      this.printPreviewBarItem5.Enabled = false;
      this.printPreviewBarItem5.Hint = "Print Direct";
      this.printPreviewBarItem5.Id = 7;
      this.printPreviewBarItem5.ImageIndex = 1;
      this.printPreviewBarItem5.Name = "printPreviewBarItem5";
      this.printPreviewBarItem6.ButtonStyle = BarButtonStyle.Check;
      this.printPreviewBarItem6.Caption = "Page Set&up...";
      this.printPreviewBarItem6.Command = PrintingSystemCommand.PageSetup;
      this.printPreviewBarItem6.Enabled = false;
      this.printPreviewBarItem6.Hint = "Page Setup";
      this.printPreviewBarItem6.Id = 8;
      this.printPreviewBarItem6.ImageIndex = 2;
      this.printPreviewBarItem6.Name = "printPreviewBarItem6";
      this.printPreviewBarItem7.Caption = "Header And Footer";
      this.printPreviewBarItem7.Command = PrintingSystemCommand.EditPageHF;
      this.printPreviewBarItem7.Enabled = false;
      this.printPreviewBarItem7.Hint = "Header And Footer";
      this.printPreviewBarItem7.Id = 9;
      this.printPreviewBarItem7.ImageIndex = 15;
      this.printPreviewBarItem7.Name = "printPreviewBarItem7";
      this.printPreviewBarItem8.ActAsDropDown = true;
      this.printPreviewBarItem8.ButtonStyle = BarButtonStyle.DropDown;
      this.printPreviewBarItem8.Caption = "Scale";
      this.printPreviewBarItem8.Command = PrintingSystemCommand.Scale;
      this.printPreviewBarItem8.Enabled = false;
      this.printPreviewBarItem8.Hint = "Scale";
      this.printPreviewBarItem8.Id = 10;
      this.printPreviewBarItem8.ImageIndex = 22;
      this.printPreviewBarItem8.Name = "printPreviewBarItem8";
      this.printPreviewBarItem9.ButtonStyle = BarButtonStyle.Check;
      this.printPreviewBarItem9.Caption = "Hand Tool";
      this.printPreviewBarItem9.Command = PrintingSystemCommand.HandTool;
      this.printPreviewBarItem9.Enabled = false;
      this.printPreviewBarItem9.Hint = "Hand Tool";
      this.printPreviewBarItem9.Id = 11;
      this.printPreviewBarItem9.ImageIndex = 16;
      this.printPreviewBarItem9.Name = "printPreviewBarItem9";
      this.printPreviewBarItem10.ButtonStyle = BarButtonStyle.Check;
      this.printPreviewBarItem10.Caption = "Magnifier";
      this.printPreviewBarItem10.Command = PrintingSystemCommand.Magnifier;
      this.printPreviewBarItem10.Enabled = false;
      this.printPreviewBarItem10.Hint = "Magnifier";
      this.printPreviewBarItem10.Id = 12;
      this.printPreviewBarItem10.ImageIndex = 3;
      this.printPreviewBarItem10.Name = "printPreviewBarItem10";
      this.printPreviewBarItem11.Caption = "Zoom Out";
      this.printPreviewBarItem11.Command = PrintingSystemCommand.ZoomOut;
      this.printPreviewBarItem11.Enabled = false;
      this.printPreviewBarItem11.Hint = "Zoom Out";
      this.printPreviewBarItem11.Id = 13;
      this.printPreviewBarItem11.ImageIndex = 5;
      this.printPreviewBarItem11.Name = "printPreviewBarItem11";
      this.zoomBarEditItem1.Caption = "Zoom";
      this.zoomBarEditItem1.Edit = (RepositoryItem) this.printPreviewRepositoryItemComboBox1;
      this.zoomBarEditItem1.EditValue = (object) "100%";
      this.zoomBarEditItem1.Enabled = false;
      this.zoomBarEditItem1.Hint = "Zoom";
      this.zoomBarEditItem1.Id = 14;
      this.zoomBarEditItem1.Name = "zoomBarEditItem1";
      this.zoomBarEditItem1.Width = 70;
      this.printPreviewRepositoryItemComboBox1.AutoComplete = false;
      this.printPreviewRepositoryItemComboBox1.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.printPreviewRepositoryItemComboBox1.DropDownRows = 11;
      this.printPreviewRepositoryItemComboBox1.Name = "printPreviewRepositoryItemComboBox1";
      this.printPreviewBarItem12.Caption = "Zoom In";
      this.printPreviewBarItem12.Command = PrintingSystemCommand.ZoomIn;
      this.printPreviewBarItem12.Enabled = false;
      this.printPreviewBarItem12.Hint = "Zoom In";
      this.printPreviewBarItem12.Id = 15;
      this.printPreviewBarItem12.ImageIndex = 4;
      this.printPreviewBarItem12.Name = "printPreviewBarItem12";
      this.printPreviewBarItem13.Caption = "First Page";
      this.printPreviewBarItem13.Command = PrintingSystemCommand.ShowFirstPage;
      this.printPreviewBarItem13.Enabled = false;
      this.printPreviewBarItem13.Hint = "First Page";
      this.printPreviewBarItem13.Id = 16;
      this.printPreviewBarItem13.ImageIndex = 7;
      this.printPreviewBarItem13.Name = "printPreviewBarItem13";
      this.printPreviewBarItem14.Caption = "Previous Page";
      this.printPreviewBarItem14.Command = PrintingSystemCommand.ShowPrevPage;
      this.printPreviewBarItem14.Enabled = false;
      this.printPreviewBarItem14.Hint = "Previous Page";
      this.printPreviewBarItem14.Id = 17;
      this.printPreviewBarItem14.ImageIndex = 8;
      this.printPreviewBarItem14.Name = "printPreviewBarItem14";
      this.printPreviewBarItem15.Caption = "Next Page";
      this.printPreviewBarItem15.Command = PrintingSystemCommand.ShowNextPage;
      this.printPreviewBarItem15.Enabled = false;
      this.printPreviewBarItem15.Hint = "Next Page";
      this.printPreviewBarItem15.Id = 18;
      this.printPreviewBarItem15.ImageIndex = 9;
      this.printPreviewBarItem15.Name = "printPreviewBarItem15";
      this.printPreviewBarItem16.Caption = "Last Page";
      this.printPreviewBarItem16.Command = PrintingSystemCommand.ShowLastPage;
      this.printPreviewBarItem16.Enabled = false;
      this.printPreviewBarItem16.Hint = "Last Page";
      this.printPreviewBarItem16.Id = 19;
      this.printPreviewBarItem16.ImageIndex = 10;
      this.printPreviewBarItem16.Name = "printPreviewBarItem16";
      this.printPreviewBarItem17.ButtonStyle = BarButtonStyle.DropDown;
      this.printPreviewBarItem17.Caption = "Multiple Pages";
      this.printPreviewBarItem17.Command = PrintingSystemCommand.MultiplePages;
      this.printPreviewBarItem17.Enabled = false;
      this.printPreviewBarItem17.Hint = "Multiple Pages";
      this.printPreviewBarItem17.Id = 20;
      this.printPreviewBarItem17.ImageIndex = 11;
      this.printPreviewBarItem17.Name = "printPreviewBarItem17";
      this.printPreviewBarItem18.ButtonStyle = BarButtonStyle.DropDown;
      this.printPreviewBarItem18.Caption = "&Color...";
      this.printPreviewBarItem18.Command = PrintingSystemCommand.FillBackground;
      this.printPreviewBarItem18.Enabled = false;
      this.printPreviewBarItem18.Hint = "Background";
      this.printPreviewBarItem18.Id = 21;
      this.printPreviewBarItem18.ImageIndex = 12;
      this.printPreviewBarItem18.Name = "printPreviewBarItem18";
      this.printPreviewBarItem19.Caption = "&Watermark...";
      this.printPreviewBarItem19.Command = PrintingSystemCommand.Watermark;
      this.printPreviewBarItem19.Enabled = false;
      this.printPreviewBarItem19.Hint = "&Watermark...";
      this.printPreviewBarItem19.Id = 22;
      this.printPreviewBarItem19.ImageIndex = 21;
      this.printPreviewBarItem19.Name = "printPreviewBarItem19";
      this.printPreviewBarItem20.ButtonStyle = BarButtonStyle.DropDown;
      this.printPreviewBarItem20.Caption = "Export Document...";
      this.printPreviewBarItem20.Command = PrintingSystemCommand.ExportFile;
      this.printPreviewBarItem20.Enabled = false;
      this.printPreviewBarItem20.Hint = "Export Document...";
      this.printPreviewBarItem20.Id = 23;
      this.printPreviewBarItem20.ImageIndex = 18;
      this.printPreviewBarItem20.Name = "printPreviewBarItem20";
      this.printPreviewBarItem21.ButtonStyle = BarButtonStyle.DropDown;
      this.printPreviewBarItem21.Caption = "Send via E-Mail...";
      this.printPreviewBarItem21.Command = PrintingSystemCommand.SendFile;
      this.printPreviewBarItem21.Enabled = false;
      this.printPreviewBarItem21.Hint = "Send via E-Mail...";
      this.printPreviewBarItem21.Id = 24;
      this.printPreviewBarItem21.ImageIndex = 17;
      this.printPreviewBarItem21.Name = "printPreviewBarItem21";
      this.printPreviewBarItem22.Caption = "E&xit";
      this.printPreviewBarItem22.Command = PrintingSystemCommand.ClosePreview;
      this.printPreviewBarItem22.Hint = "E&xit";
      this.printPreviewBarItem22.Id = 25;
      this.printPreviewBarItem22.ImageIndex = 13;
      this.printPreviewBarItem22.Name = "printPreviewBarItem22";
      this.previewBar2.BarName = "Status Bar";
      this.previewBar2.CanDockStyle = BarCanDockStyle.Bottom;
      this.previewBar2.DockCol = 0;
      this.previewBar2.DockRow = 0;
      this.previewBar2.DockStyle = BarDockStyle.Bottom;
      this.previewBar2.LinksPersistInfo.AddRange(new LinkPersistInfo[3]
      {
        new LinkPersistInfo((BarItem) this.printPreviewStaticItem1),
        new LinkPersistInfo((BarItem) this.printPreviewStaticItem2),
        new LinkPersistInfo((BarItem) this.printPreviewStaticItem3)
      });
      this.previewBar2.OptionsBar.AllowQuickCustomization = false;
      this.previewBar2.OptionsBar.DrawDragBorder = false;
      this.previewBar2.OptionsBar.UseWholeRow = true;
      this.previewBar2.Text = "Status Bar";
      this.printPreviewStaticItem1.AutoSize = BarStaticItemSize.Spring;
      this.printPreviewStaticItem1.Caption = "Current Page No: none";
      this.printPreviewStaticItem1.Id = 0;
      this.printPreviewStaticItem1.LeftIndent = 1;
      this.printPreviewStaticItem1.Name = "printPreviewStaticItem1";
      this.printPreviewStaticItem1.RightIndent = 1;
      this.printPreviewStaticItem1.TextAlignment = StringAlignment.Near;
      this.printPreviewStaticItem1.Type = "CurrentPageNo";
      this.printPreviewStaticItem1.Width = 200;
      this.printPreviewStaticItem2.AutoSize = BarStaticItemSize.Spring;
      this.printPreviewStaticItem2.Caption = "Total Page No: 0";
      this.printPreviewStaticItem2.Id = 1;
      this.printPreviewStaticItem2.LeftIndent = 1;
      this.printPreviewStaticItem2.Name = "printPreviewStaticItem2";
      this.printPreviewStaticItem2.RightIndent = 1;
      this.printPreviewStaticItem2.TextAlignment = StringAlignment.Near;
      this.printPreviewStaticItem2.Type = "TotalPageNo";
      this.printPreviewStaticItem2.Width = 200;
      this.printPreviewStaticItem3.AutoSize = BarStaticItemSize.Spring;
      this.printPreviewStaticItem3.Caption = "Zoom Factor: 100%";
      this.printPreviewStaticItem3.Id = 2;
      this.printPreviewStaticItem3.LeftIndent = 1;
      this.printPreviewStaticItem3.Name = "printPreviewStaticItem3";
      this.printPreviewStaticItem3.RightIndent = 1;
      this.printPreviewStaticItem3.TextAlignment = StringAlignment.Near;
      this.printPreviewStaticItem3.Type = "ZoomFactor";
      this.printPreviewStaticItem3.Width = 200;
      this.previewBar3.BarName = "Main Menu";
      this.previewBar3.DockCol = 0;
      this.previewBar3.DockRow = 0;
      this.previewBar3.DockStyle = BarDockStyle.Top;
      this.previewBar3.LinksPersistInfo.AddRange(new LinkPersistInfo[3]
      {
        new LinkPersistInfo((BarItem) this.printPreviewSubItem1),
        new LinkPersistInfo((BarItem) this.printPreviewSubItem2),
        new LinkPersistInfo((BarItem) this.printPreviewSubItem3)
      });
      this.previewBar3.OptionsBar.MultiLine = true;
      this.previewBar3.OptionsBar.UseWholeRow = true;
      this.previewBar3.Text = "Main Menu";
      this.previewBar3.Visible = false;
      this.printPreviewSubItem1.Caption = "&File";
      this.printPreviewSubItem1.Command = PrintingSystemCommand.File;
      this.printPreviewSubItem1.Id = 26;
      this.printPreviewSubItem1.LinksPersistInfo.AddRange(new LinkPersistInfo[6]
      {
        new LinkPersistInfo((BarItem) this.printPreviewBarItem6),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem4),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem5),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem20, true),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem21),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem22, true)
      });
      this.printPreviewSubItem1.Name = "printPreviewSubItem1";
      this.printPreviewSubItem2.Caption = "&View";
      this.printPreviewSubItem2.Command = PrintingSystemCommand.View;
      this.printPreviewSubItem2.Id = 27;
      this.printPreviewSubItem2.LinksPersistInfo.AddRange(new LinkPersistInfo[2]
      {
        new LinkPersistInfo((BarItem) this.printPreviewSubItem4, true),
        new LinkPersistInfo((BarItem) this.barToolbarsListItem1, true)
      });
      this.printPreviewSubItem2.Name = "printPreviewSubItem2";
      this.printPreviewSubItem4.Caption = "&Page Layout";
      this.printPreviewSubItem4.Command = PrintingSystemCommand.PageLayout;
      this.printPreviewSubItem4.Id = 29;
      this.printPreviewSubItem4.LinksPersistInfo.AddRange(new LinkPersistInfo[2]
      {
        new LinkPersistInfo((BarItem) this.printPreviewBarItem23),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem24)
      });
      this.printPreviewSubItem4.Name = "printPreviewSubItem4";
      this.printPreviewBarItem23.ButtonStyle = BarButtonStyle.Check;
      this.printPreviewBarItem23.Caption = "&Facing";
      this.printPreviewBarItem23.Command = PrintingSystemCommand.PageLayoutFacing;
      this.printPreviewBarItem23.Enabled = false;
      this.printPreviewBarItem23.GroupIndex = 100;
      this.printPreviewBarItem23.Id = 30;
      this.printPreviewBarItem23.Name = "printPreviewBarItem23";
      this.printPreviewBarItem24.ButtonStyle = BarButtonStyle.Check;
      this.printPreviewBarItem24.Caption = "&Continuous";
      this.printPreviewBarItem24.Command = PrintingSystemCommand.PageLayoutContinuous;
      this.printPreviewBarItem24.Enabled = false;
      this.printPreviewBarItem24.GroupIndex = 100;
      this.printPreviewBarItem24.Id = 31;
      this.printPreviewBarItem24.Name = "printPreviewBarItem24";
      this.barToolbarsListItem1.Caption = "Bars";
      this.barToolbarsListItem1.Id = 32;
      this.barToolbarsListItem1.Name = "barToolbarsListItem1";
      this.printPreviewSubItem3.Caption = "&Background";
      this.printPreviewSubItem3.Command = PrintingSystemCommand.Background;
      this.printPreviewSubItem3.Id = 28;
      this.printPreviewSubItem3.LinksPersistInfo.AddRange(new LinkPersistInfo[2]
      {
        new LinkPersistInfo((BarItem) this.printPreviewBarItem18),
        new LinkPersistInfo((BarItem) this.printPreviewBarItem19)
      });
      this.printPreviewSubItem3.Name = "printPreviewSubItem3";
      this.barDockControlTop.Dock = DockStyle.Top;
      this.barDockControlTop.Location = new Point(0, 0);
      this.barDockControlTop.Size = new Size(736, 51);
      this.barDockControlBottom.Dock = DockStyle.Bottom;
      this.barDockControlBottom.Location = new Point(0, 533);
      this.barDockControlBottom.Size = new Size(736, 28);
      this.barDockControlLeft.Dock = DockStyle.Left;
      this.barDockControlLeft.Location = new Point(0, 51);
      this.barDockControlLeft.Size = new Size(0, 482);
      this.barDockControlRight.Dock = DockStyle.Right;
      this.barDockControlRight.Location = new Point(736, 51);
      this.barDockControlRight.Size = new Size(0, 482);
      this.printPreviewBarCheckItem1.Caption = "PDF Document";
      this.printPreviewBarCheckItem1.Checked = true;
      this.printPreviewBarCheckItem1.Command = PrintingSystemCommand.ExportPdf;
      this.printPreviewBarCheckItem1.Enabled = false;
      this.printPreviewBarCheckItem1.GroupIndex = 2;
      this.printPreviewBarCheckItem1.Hint = "PDF Document";
      this.printPreviewBarCheckItem1.Id = 33;
      this.printPreviewBarCheckItem1.Name = "printPreviewBarCheckItem1";
      this.printPreviewBarCheckItem2.Caption = "HTML Document";
      this.printPreviewBarCheckItem2.Command = PrintingSystemCommand.ExportHtm;
      this.printPreviewBarCheckItem2.Enabled = false;
      this.printPreviewBarCheckItem2.GroupIndex = 2;
      this.printPreviewBarCheckItem2.Hint = "HTML Document";
      this.printPreviewBarCheckItem2.Id = 34;
      this.printPreviewBarCheckItem2.Name = "printPreviewBarCheckItem2";
      this.printPreviewBarCheckItem3.Caption = "Text Document";
      this.printPreviewBarCheckItem3.Command = PrintingSystemCommand.ExportTxt;
      this.printPreviewBarCheckItem3.Enabled = false;
      this.printPreviewBarCheckItem3.GroupIndex = 2;
      this.printPreviewBarCheckItem3.Hint = "Text Document";
      this.printPreviewBarCheckItem3.Id = 35;
      this.printPreviewBarCheckItem3.Name = "printPreviewBarCheckItem3";
      this.printPreviewBarCheckItem4.Caption = "CSV Document";
      this.printPreviewBarCheckItem4.Command = PrintingSystemCommand.ExportCsv;
      this.printPreviewBarCheckItem4.Enabled = false;
      this.printPreviewBarCheckItem4.GroupIndex = 2;
      this.printPreviewBarCheckItem4.Hint = "CSV Document";
      this.printPreviewBarCheckItem4.Id = 36;
      this.printPreviewBarCheckItem4.Name = "printPreviewBarCheckItem4";
      this.printPreviewBarCheckItem5.Caption = "MHT Document";
      this.printPreviewBarCheckItem5.Command = PrintingSystemCommand.ExportMht;
      this.printPreviewBarCheckItem5.Enabled = false;
      this.printPreviewBarCheckItem5.GroupIndex = 2;
      this.printPreviewBarCheckItem5.Hint = "MHT Document";
      this.printPreviewBarCheckItem5.Id = 37;
      this.printPreviewBarCheckItem5.Name = "printPreviewBarCheckItem5";
      this.printPreviewBarCheckItem6.Caption = "Excel Document";
      this.printPreviewBarCheckItem6.Command = PrintingSystemCommand.ExportXls;
      this.printPreviewBarCheckItem6.Enabled = false;
      this.printPreviewBarCheckItem6.GroupIndex = 2;
      this.printPreviewBarCheckItem6.Hint = "Excel Document";
      this.printPreviewBarCheckItem6.Id = 38;
      this.printPreviewBarCheckItem6.Name = "printPreviewBarCheckItem6";
      this.printPreviewBarCheckItem7.Caption = "Rich Text Document";
      this.printPreviewBarCheckItem7.Command = PrintingSystemCommand.ExportRtf;
      this.printPreviewBarCheckItem7.Enabled = false;
      this.printPreviewBarCheckItem7.GroupIndex = 2;
      this.printPreviewBarCheckItem7.Hint = "Rich Text Document";
      this.printPreviewBarCheckItem7.Id = 39;
      this.printPreviewBarCheckItem7.Name = "printPreviewBarCheckItem7";
      this.printPreviewBarCheckItem8.Caption = "Graphic Document";
      this.printPreviewBarCheckItem8.Command = PrintingSystemCommand.ExportGraphic;
      this.printPreviewBarCheckItem8.Enabled = false;
      this.printPreviewBarCheckItem8.GroupIndex = 2;
      this.printPreviewBarCheckItem8.Hint = "Graphic Document";
      this.printPreviewBarCheckItem8.Id = 40;
      this.printPreviewBarCheckItem8.Name = "printPreviewBarCheckItem8";
      this.printPreviewBarCheckItem9.Caption = "PDF Document";
      this.printPreviewBarCheckItem9.Checked = true;
      this.printPreviewBarCheckItem9.Command = PrintingSystemCommand.SendPdf;
      this.printPreviewBarCheckItem9.Enabled = false;
      this.printPreviewBarCheckItem9.GroupIndex = 1;
      this.printPreviewBarCheckItem9.Hint = "PDF Document";
      this.printPreviewBarCheckItem9.Id = 41;
      this.printPreviewBarCheckItem9.Name = "printPreviewBarCheckItem9";
      this.printPreviewBarCheckItem10.Caption = "Text Document";
      this.printPreviewBarCheckItem10.Command = PrintingSystemCommand.SendTxt;
      this.printPreviewBarCheckItem10.Enabled = false;
      this.printPreviewBarCheckItem10.GroupIndex = 1;
      this.printPreviewBarCheckItem10.Hint = "Text Document";
      this.printPreviewBarCheckItem10.Id = 42;
      this.printPreviewBarCheckItem10.Name = "printPreviewBarCheckItem10";
      this.printPreviewBarCheckItem11.Caption = "CSV Document";
      this.printPreviewBarCheckItem11.Command = PrintingSystemCommand.SendCsv;
      this.printPreviewBarCheckItem11.Enabled = false;
      this.printPreviewBarCheckItem11.GroupIndex = 1;
      this.printPreviewBarCheckItem11.Hint = "CSV Document";
      this.printPreviewBarCheckItem11.Id = 43;
      this.printPreviewBarCheckItem11.Name = "printPreviewBarCheckItem11";
      this.printPreviewBarCheckItem12.Caption = "MHT Document";
      this.printPreviewBarCheckItem12.Command = PrintingSystemCommand.SendMht;
      this.printPreviewBarCheckItem12.Enabled = false;
      this.printPreviewBarCheckItem12.GroupIndex = 1;
      this.printPreviewBarCheckItem12.Hint = "MHT Document";
      this.printPreviewBarCheckItem12.Id = 44;
      this.printPreviewBarCheckItem12.Name = "printPreviewBarCheckItem12";
      this.printPreviewBarCheckItem13.Caption = "Excel Document";
      this.printPreviewBarCheckItem13.Command = PrintingSystemCommand.SendXls;
      this.printPreviewBarCheckItem13.Enabled = false;
      this.printPreviewBarCheckItem13.GroupIndex = 1;
      this.printPreviewBarCheckItem13.Hint = "Excel Document";
      this.printPreviewBarCheckItem13.Id = 45;
      this.printPreviewBarCheckItem13.Name = "printPreviewBarCheckItem13";
      this.printPreviewBarCheckItem14.Caption = "Rich Text Document";
      this.printPreviewBarCheckItem14.Command = PrintingSystemCommand.SendRtf;
      this.printPreviewBarCheckItem14.Enabled = false;
      this.printPreviewBarCheckItem14.GroupIndex = 1;
      this.printPreviewBarCheckItem14.Hint = "Rich Text Document";
      this.printPreviewBarCheckItem14.Id = 46;
      this.printPreviewBarCheckItem14.Name = "printPreviewBarCheckItem14";
      this.printPreviewBarCheckItem15.Caption = "Graphic Document";
      this.printPreviewBarCheckItem15.Command = PrintingSystemCommand.SendGraphic;
      this.printPreviewBarCheckItem15.Enabled = false;
      this.printPreviewBarCheckItem15.GroupIndex = 1;
      this.printPreviewBarCheckItem15.Hint = "Graphic Document";
      this.printPreviewBarCheckItem15.Id = 47;
      this.printPreviewBarCheckItem15.Name = "printPreviewBarCheckItem15";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(736, 561);
      this.Controls.Add((Control) this.printControl);
      this.Controls.Add((Control) this.barDockControlLeft);
      this.Controls.Add((Control) this.barDockControlRight);
      this.Controls.Add((Control) this.barDockControlBottom);
      this.Controls.Add((Control) this.barDockControlTop);
      this.Name = "ReportPreview";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Report Preview";
      this.FormClosed += new FormClosedEventHandler(this.ReportPreview_FormClosed);
      //this.printingSystem.EndInit();
      this.printBarManager1.EndInit();
      this.printPreviewRepositoryItemComboBox1.EndInit();
      this.ResumeLayout(false);
    }

    private class MyCommandHandler : ICommandHandler
    {
      private System.Collections.Specialized.StringCollection _emailTo = new System.Collections.Specialized.StringCollection();

      public System.Collections.Specialized.StringCollection EmailTo
      {
        get
        {
          return this._emailTo;
        }
        set
        {
          this._emailTo = value;
        }
      }

      public bool CanHandleCommand(PrintingSystemCommand command, IPrintControl printControl)
      {
        return true;
      }

      public void HandleCommand(PrintingSystemCommand command, object[] args, IPrintControl printControl, ref bool handled)
      {
        string path = Path.GetTempPath() + Application.ProductName + ".tmp";
        Directory.CreateDirectory(path);
        string str1 = path + "\\ReportExport";
        string str2;
        if (command == PrintingSystemCommand.SendPdf)
        {
          str2 = str1 + ".pdf";
          printControl.PrintingSystem.ExportToPdf(str2);
        }
        else if (command == PrintingSystemCommand.SendTxt)
        {
          str2 = str1 + ".txt";
          printControl.PrintingSystem.ExportToText(str2);
        }
        else if (command == PrintingSystemCommand.SendCsv)
        {
          str2 = str1 + ".csv";
          printControl.PrintingSystem.ExportToCsv(str2);
        }
        else if (command == PrintingSystemCommand.SendMht)
        {
          str2 = str1 + ".mht";
          printControl.PrintingSystem.ExportToMht(str2);
        }
        else if (command == PrintingSystemCommand.SendXls)
        {
          str2 = str1 + ".xls";
          printControl.PrintingSystem.ExportToXls(str2);
        }
        else if (command == PrintingSystemCommand.SendRtf)
        {
          str2 = str1 + ".rtf";
          printControl.PrintingSystem.ExportToRtf(str2);
        }
        else
        {
          if (command != PrintingSystemCommand.SendGraphic)
            return;
          str2 = str1 + ".jpg";
          printControl.PrintingSystem.ExportToImage(str2, ImageFormat.Jpeg);
        }
        handled = true;
        MAPIEmailer mapiEmailer = new MAPIEmailer();
        mapiEmailer.AddAttachment(str2);
        foreach (string email in this._emailTo)
          mapiEmailer.AddRecipientTo(email);
        mapiEmailer.SendMailPopup("", "");
        Application.DoEvents();
        File.Delete(str2);
      }
    }
  }
}
