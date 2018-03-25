using DevExpress.LookAndFeel;
using PresentationBuilder.Win.Controls;
using PresentationBuilder.Win.Dialogs;
using PresentationBuilder.Win.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PresentationBuilder.BLL;
using System.Threading.Tasks;

namespace PresentationBuilder.Win
{
  public partial class MainForm : Form
  {
    private string _fileName = (string) null;
    private IContainer components = (IContainer) null;
    private SlideListBox slidePanel;
    private ToolStrip toolStrip1;
    private ToolStripButton deleteStripButton;
    private ToolStripButton openStripButton;
    private ToolStripButton saveStripButton;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton addStripButton;
    private ToolStripButton insertStripButton;
    private ToolStripButton powerpointStripButton;
    private ToolStripButton upStripButton;
    private ToolStripButton downStripButton;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripButton fontStripButton;
    private ToolStripButton songStripButton;
    private ToolStripButton messagesStripButton;
    private DefaultLookAndFeel defaultLookAndFeel1;
    private ToolStripButton newStripButton;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem newToolStripMenuItem;
    private ToolStripMenuItem openToolStripMenuItem;
    private ToolStripMenuItem saveToolStripMenuItem;
    private ToolStripMenuItem saveAsToolStripMenuItem;
    private ToolStripMenuItem slideToolStripMenuItem;
    private ToolStripMenuItem addToolStripMenuItem;
    private ToolStripMenuItem insertToolStripMenuItem;
    private ToolStripMenuItem deleteToolStripMenuItem;
    private ToolStripMenuItem moveUpToolStripMenuItem;
    private ToolStripMenuItem moveDownToolStripMenuItem;
    private ToolStripMenuItem toolsToolStripMenuItem;
    private ToolStripMenuItem songsToolStripMenuItem;
    private ToolStripMenuItem messagesToolStripMenuItem;
    private ToolStripMenuItem aboutToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem createInPowerpointToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem2;

    public MainForm()
    {
      InitializeComponent();
    }

    public MainForm(string startupFile)
    {
      InitializeComponent();
      OpenFile(startupFile);
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      slidePanel.AddSlide();
    }

    private void upButton_Click(object sender, EventArgs e)
    {
      slidePanel.MoveSelectedUp();
    }

    private void downButton_Click(object sender, EventArgs e)
    {
      slidePanel.MoveSelectedDown();
    }

    private void songStripButton_Click(object sender, EventArgs e)
    {
      int num = (int) new SongListDialog().ShowDialog();
      slidePanel.ReloadSongs();
    }

    private void powerpointStripButton_Click(object sender, EventArgs e)
    {
      slidePanel.GeneratePresentation();
    }

    private void fontStripButton_Click(object sender, EventArgs e)
    {
      FontListDialog fontListDialog = new FontListDialog();
      fontListDialog.SongFont = slidePanel.SongFont;
      fontListDialog.IndicatorFont = slidePanel.IndicatorFont;
      fontListDialog.MessageFont = slidePanel.MessageFont;
      if (fontListDialog.ShowDialog() != DialogResult.OK)
        return;
      slidePanel.SongFont = fontListDialog.SongFont;
      slidePanel.IndicatorFont = fontListDialog.IndicatorFont;
      slidePanel.MessageFont = fontListDialog.MessageFont;
      slidePanel.IsDirty = true;
    }

    private void deleteStripButton_Click(object sender, EventArgs e)
    {
      if (!slidePanel.HasSelectedSlide || MessageBox.Show("Are you sure you want to delete the selected slide?", Application.ProductName, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      slidePanel.DeleteSelected();
    }

    private void insertStripButton_Click(object sender, EventArgs e)
    {
      slidePanel.InsertSlide();
    }

    private void addStripButton_Click(object sender, EventArgs e)
    {
      slidePanel.AddSlide();
    }

    private void upStripButton_Click(object sender, EventArgs e)
    {
      slidePanel.MoveSelectedUp();
    }

    private void downStripButton_Click(object sender, EventArgs e)
    {
      slidePanel.MoveSelectedDown();
    }

    private void messagesStripButton_Click(object sender, EventArgs e)
    {
      int num = (int) new MessageEditDialog().ShowDialog();
    }

    private void saveStripButton_Click(object sender, EventArgs e)
    {
      if (!slidePanel.CanSave)
        return;
      SaveToFile(false);
    }

    private bool SaveToFile(bool savingAs)
    {
      bool flag;
      if (_fileName != null && !savingAs)
      {
        slidePanel.SaveToFile(_fileName);
        flag = true;
      }
      else
      {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Presentation Builder Files (*.pbl) | *.pbl";
        saveFileDialog.DefaultExt = "pbl";
        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
          slidePanel.SaveToFile(saveFileDialog.FileName);
          _fileName = saveFileDialog.FileName;
          flag = true;
        }
        else
          flag = false;
      }
      if (flag)
        Text = "Presentation Builder - " + Path.GetFileName(_fileName);
      return flag;
    }

    private void openStripButton_Click(object sender, EventArgs e)
    {
      if (slidePanel.IsDirty && MessageBox.Show("Are you sure you want to loose your changes?", Application.ProductName, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Presentation Builder Files (*.pbl) | *.pbl";
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      OpenFile(openFileDialog.FileName);
    }

    private void OpenFile(string file)
    {
      SuspendLayout();
      slidePanel.OpenFromFile(file);
      ResumeLayout();
      _fileName = file;
      Text = "Presentation Builder - " + Path.GetFileName(file);
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!slidePanel.IsDirty)
        return;
      switch (MessageBox.Show("Do you want to save your changes?", Application.ProductName, MessageBoxButtons.YesNoCancel))
      {
        case DialogResult.Yes:
          if (!SaveToFile(false))
          {
            e.Cancel = true;
            break;
          }
          break;
        case DialogResult.Cancel:
          e.Cancel = true;
          break;
      }
    }

    private void newStripButton_Click(object sender, EventArgs e)
    {
      bool flag = false;
      if (slidePanel.IsDirty)
      {
        switch (MessageBox.Show("Do you want to save your changes?", Application.ProductName, MessageBoxButtons.YesNoCancel))
        {
          case DialogResult.Yes:
            if (SaveToFile(false))
            {
              flag = true;
              break;
            }
            break;
          case DialogResult.No:
            flag = true;
            break;
        }
      }
      else
        flag = true;
      if (!flag)
        return;
      slidePanel.Clear();
      Text = "Presentation Builder";
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      //var reader = new OneDriveReader();
      //await reader.Connect(Application.ExecutablePath + "\\PresentationBuilder.Win_TemporaryKey.pfx");
     MessageBox.Show("Version " + Application.ProductVersion + "\r\n\r\nDeveloped by Ron Oskam\r\nronoskam@gmail.com", Application.ProductName);
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveToFile(true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      PresentationBuilder.BLL.PowerPoint.FontItem fontItem1 = new PresentationBuilder.BLL.PowerPoint.FontItem();
      PresentationBuilder.BLL.PowerPoint.FontItem fontItem2 = new PresentationBuilder.BLL.PowerPoint.FontItem();
      PresentationBuilder.BLL.PowerPoint.FontItem fontItem3 = new PresentationBuilder.BLL.PowerPoint.FontItem();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.newStripButton = new System.Windows.Forms.ToolStripButton();
      this.openStripButton = new System.Windows.Forms.ToolStripButton();
      this.saveStripButton = new System.Windows.Forms.ToolStripButton();
      this.powerpointStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.addStripButton = new System.Windows.Forms.ToolStripButton();
      this.insertStripButton = new System.Windows.Forms.ToolStripButton();
      this.deleteStripButton = new System.Windows.Forms.ToolStripButton();
      this.upStripButton = new System.Windows.Forms.ToolStripButton();
      this.downStripButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.fontStripButton = new System.Windows.Forms.ToolStripButton();
      this.songStripButton = new System.Windows.Forms.ToolStripButton();
      this.messagesStripButton = new System.Windows.Forms.ToolStripButton();
      this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.createInPowerpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.slideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.songsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.messagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.slidePanel = new PresentationBuilder.Win.Controls.SlideListBox();
      this.toolStrip1.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newStripButton,
            this.openStripButton,
            this.saveStripButton,
            this.powerpointStripButton,
            this.toolStripSeparator1,
            this.addStripButton,
            this.insertStripButton,
            this.deleteStripButton,
            this.upStripButton,
            this.downStripButton,
            this.toolStripSeparator2,
            this.fontStripButton,
            this.songStripButton,
            this.messagesStripButton});
      this.toolStrip1.Location = new System.Drawing.Point(0, 24);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(662, 25);
      this.toolStrip1.TabIndex = 5;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // newStripButton
      // 
      this.newStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.newStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.New;
      this.newStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.newStripButton.Name = "newStripButton";
      this.newStripButton.Size = new System.Drawing.Size(23, 22);
      this.newStripButton.Text = "toolStripButton2";
      this.newStripButton.ToolTipText = "Clear Slides";
      this.newStripButton.Click += new System.EventHandler(this.newStripButton_Click);
      // 
      // openStripButton
      // 
      this.openStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.openStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.openfolderHS;
      this.openStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.openStripButton.Name = "openStripButton";
      this.openStripButton.Size = new System.Drawing.Size(23, 22);
      this.openStripButton.Text = "toolStripButton2";
      this.openStripButton.ToolTipText = "Open File";
      this.openStripButton.Click += new System.EventHandler(this.openStripButton_Click);
      // 
      // saveStripButton
      // 
      this.saveStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.saveStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.saveHS;
      this.saveStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.saveStripButton.Name = "saveStripButton";
      this.saveStripButton.Size = new System.Drawing.Size(23, 22);
      this.saveStripButton.Text = "toolStripButton3";
      this.saveStripButton.ToolTipText = "Save File";
      this.saveStripButton.Click += new System.EventHandler(this.saveStripButton_Click);
      // 
      // powerpointStripButton
      // 
      this.powerpointStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.powerpointStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.Build;
      this.powerpointStripButton.ImageTransparentColor = System.Drawing.Color.White;
      this.powerpointStripButton.Name = "powerpointStripButton";
      this.powerpointStripButton.Size = new System.Drawing.Size(23, 22);
      this.powerpointStripButton.Text = "toolStripButton6";
      this.powerpointStripButton.ToolTipText = "Create in Powerpoint";
      this.powerpointStripButton.Click += new System.EventHandler(this.powerpointStripButton_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // addStripButton
      // 
      this.addStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.addStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.Add;
      this.addStripButton.ImageTransparentColor = System.Drawing.Color.White;
      this.addStripButton.Name = "addStripButton";
      this.addStripButton.Size = new System.Drawing.Size(23, 22);
      this.addStripButton.Text = "Add Slide";
      this.addStripButton.Click += new System.EventHandler(this.addStripButton_Click);
      // 
      // insertStripButton
      // 
      this.insertStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.insertStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.Insert;
      this.insertStripButton.ImageTransparentColor = System.Drawing.Color.White;
      this.insertStripButton.Name = "insertStripButton";
      this.insertStripButton.Size = new System.Drawing.Size(23, 22);
      this.insertStripButton.Text = "toolStripButton5";
      this.insertStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
      this.insertStripButton.ToolTipText = "Insert Slide";
      this.insertStripButton.Click += new System.EventHandler(this.insertStripButton_Click);
      // 
      // deleteStripButton
      // 
      this.deleteStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.deleteStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.DeleteHS;
      this.deleteStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.deleteStripButton.Name = "deleteStripButton";
      this.deleteStripButton.Size = new System.Drawing.Size(23, 22);
      this.deleteStripButton.Text = "toolStripButton1";
      this.deleteStripButton.ToolTipText = "Delete Slide";
      this.deleteStripButton.Click += new System.EventHandler(this.deleteStripButton_Click);
      // 
      // upStripButton
      // 
      this.upStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.upStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.MoveUp;
      this.upStripButton.ImageTransparentColor = System.Drawing.Color.White;
      this.upStripButton.Name = "upStripButton";
      this.upStripButton.Size = new System.Drawing.Size(23, 22);
      this.upStripButton.Text = "toolStripButton7";
      this.upStripButton.ToolTipText = "Move Slide Up";
      this.upStripButton.Click += new System.EventHandler(this.upStripButton_Click);
      // 
      // downStripButton
      // 
      this.downStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.downStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.MoveDown;
      this.downStripButton.ImageTransparentColor = System.Drawing.Color.White;
      this.downStripButton.Name = "downStripButton";
      this.downStripButton.Size = new System.Drawing.Size(23, 22);
      this.downStripButton.Text = "toolStripButton8";
      this.downStripButton.ToolTipText = "Move Slide Down";
      this.downStripButton.Click += new System.EventHandler(this.downStripButton_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // fontStripButton
      // 
      this.fontStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.fontStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.FontDialogHS;
      this.fontStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.fontStripButton.Name = "fontStripButton";
      this.fontStripButton.Size = new System.Drawing.Size(23, 22);
      this.fontStripButton.Text = "toolStripButton10";
      this.fontStripButton.ToolTipText = "Slide Font";
      this.fontStripButton.Click += new System.EventHandler(this.fontStripButton_Click);
      // 
      // songStripButton
      // 
      this.songStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.songStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.Note;
      this.songStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.songStripButton.Name = "songStripButton";
      this.songStripButton.Size = new System.Drawing.Size(23, 22);
      this.songStripButton.Text = "toolStripButton9";
      this.songStripButton.ToolTipText = "Edit Songs";
      this.songStripButton.Click += new System.EventHandler(this.songStripButton_Click);
      // 
      // messagesStripButton
      // 
      this.messagesStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.messagesStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.Book_angleHS;
      this.messagesStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.messagesStripButton.Name = "messagesStripButton";
      this.messagesStripButton.Size = new System.Drawing.Size(23, 22);
      this.messagesStripButton.Text = "toolStripButton11";
      this.messagesStripButton.ToolTipText = "Edit Messages";
      this.messagesStripButton.Click += new System.EventHandler(this.messagesStripButton_Click);
      // 
      // defaultLookAndFeel1
      // 
      this.defaultLookAndFeel1.LookAndFeel.UseWindowsXPTheme = true;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.slideToolStripMenuItem,
            this.toolsToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(662, 24);
      this.menuStrip1.TabIndex = 8;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.createInPowerpointToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // newToolStripMenuItem
      // 
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      this.newToolStripMenuItem.Text = "&New";
      this.newToolStripMenuItem.Click += new System.EventHandler(this.newStripButton_Click);
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      this.openToolStripMenuItem.Text = "&Open...";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openStripButton_Click);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      this.saveToolStripMenuItem.Text = "&Save";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveStripButton_Click);
      // 
      // saveAsToolStripMenuItem
      // 
      this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      this.saveAsToolStripMenuItem.Text = "&Save As...";
      this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(182, 6);
      // 
      // createInPowerpointToolStripMenuItem
      // 
      this.createInPowerpointToolStripMenuItem.Name = "createInPowerpointToolStripMenuItem";
      this.createInPowerpointToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      this.createInPowerpointToolStripMenuItem.Text = "&Create in Powerpoint";
      this.createInPowerpointToolStripMenuItem.Click += new System.EventHandler(this.powerpointStripButton_Click);
      // 
      // slideToolStripMenuItem
      // 
      this.slideToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.insertToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem});
      this.slideToolStripMenuItem.Name = "slideToolStripMenuItem";
      this.slideToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
      this.slideToolStripMenuItem.Text = "&Slide";
      // 
      // addToolStripMenuItem
      // 
      this.addToolStripMenuItem.Name = "addToolStripMenuItem";
      this.addToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
      this.addToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.addToolStripMenuItem.Text = "&Add";
      this.addToolStripMenuItem.Click += new System.EventHandler(this.addStripButton_Click);
      // 
      // insertToolStripMenuItem
      // 
      this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
      this.insertToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.insertToolStripMenuItem.Text = "&Insert";
      this.insertToolStripMenuItem.Click += new System.EventHandler(this.insertStripButton_Click);
      // 
      // deleteToolStripMenuItem
      // 
      this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
      this.deleteToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.deleteToolStripMenuItem.Text = "&Delete";
      this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteStripButton_Click);
      // 
      // moveUpToolStripMenuItem
      // 
      this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
      this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.moveUpToolStripMenuItem.Text = "Move &Up";
      this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.upStripButton_Click);
      // 
      // moveDownToolStripMenuItem
      // 
      this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
      this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.moveDownToolStripMenuItem.Text = "Move &Down";
      this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.downStripButton_Click);
      // 
      // toolsToolStripMenuItem
      // 
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.songsToolStripMenuItem,
            this.messagesToolStripMenuItem,
            this.toolStripMenuItem2,
            this.aboutToolStripMenuItem});
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
      this.toolsToolStripMenuItem.Text = "&Tools";
      // 
      // songsToolStripMenuItem
      // 
      this.songsToolStripMenuItem.Name = "songsToolStripMenuItem";
      this.songsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
      this.songsToolStripMenuItem.Text = "&Songs...";
      this.songsToolStripMenuItem.Click += new System.EventHandler(this.songStripButton_Click);
      // 
      // messagesToolStripMenuItem
      // 
      this.messagesToolStripMenuItem.Name = "messagesToolStripMenuItem";
      this.messagesToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
      this.messagesToolStripMenuItem.Text = "&Messages...";
      this.messagesToolStripMenuItem.Click += new System.EventHandler(this.messagesStripButton_Click);
      // 
      // toolStripMenuItem2
      // 
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new System.Drawing.Size(131, 6);
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
      this.aboutToolStripMenuItem.Text = "&About...";
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // slidePanel
      // 
      this.slidePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.slidePanel.AutoScroll = true;
      this.slidePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      fontItem1.Bold = true;
      fontItem1.FontColor = System.Drawing.Color.Black;
      fontItem1.FontName = "Arial";
      fontItem1.Italic = false;
      fontItem1.LineSpacing = 1F;
      fontItem1.Shadow = false;
      fontItem1.Size = 32F;
      fontItem1.Underline = false;
      this.slidePanel.IndicatorFont = fontItem1;
      this.slidePanel.IsDirty = false;
      this.slidePanel.Location = new System.Drawing.Point(7, 52);
      fontItem2.Bold = true;
      fontItem2.FontColor = System.Drawing.Color.Black;
      fontItem2.FontName = "Arial";
      fontItem2.Italic = false;
      fontItem2.LineSpacing = 1F;
      fontItem2.Shadow = false;
      fontItem2.Size = 32F;
      fontItem2.Underline = false;
      this.slidePanel.MessageFont = fontItem2;
      this.slidePanel.Name = "slidePanel";
      this.slidePanel.Size = new System.Drawing.Size(649, 415);
      fontItem3.Bold = true;
      fontItem3.FontColor = System.Drawing.Color.Black;
      fontItem3.FontName = "Arial";
      fontItem3.Italic = false;
      fontItem3.LineSpacing = 1F;
      fontItem3.Shadow = false;
      fontItem3.Size = 32F;
      fontItem3.Underline = false;
      this.slidePanel.SongFont = fontItem3;
      this.slidePanel.TabIndex = 3;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(662, 470);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.slidePanel);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MainForm";
      this.Text = "Presentation Builder";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
}
