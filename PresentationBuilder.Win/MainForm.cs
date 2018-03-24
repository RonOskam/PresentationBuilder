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
      components = new System.ComponentModel.Container();
      PresentationBuilder.BLL.PowerPoint.FontItem fontItem1 = new PresentationBuilder.BLL.PowerPoint.FontItem();
      PresentationBuilder.BLL.PowerPoint.FontItem fontItem2 = new PresentationBuilder.BLL.PowerPoint.FontItem();
      PresentationBuilder.BLL.PowerPoint.FontItem fontItem3 = new PresentationBuilder.BLL.PowerPoint.FontItem();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      toolStrip1 = new System.Windows.Forms.ToolStrip();
      newStripButton = new System.Windows.Forms.ToolStripButton();
      openStripButton = new System.Windows.Forms.ToolStripButton();
      saveStripButton = new System.Windows.Forms.ToolStripButton();
      powerpointStripButton = new System.Windows.Forms.ToolStripButton();
      toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      addStripButton = new System.Windows.Forms.ToolStripButton();
      insertStripButton = new System.Windows.Forms.ToolStripButton();
      deleteStripButton = new System.Windows.Forms.ToolStripButton();
      upStripButton = new System.Windows.Forms.ToolStripButton();
      downStripButton = new System.Windows.Forms.ToolStripButton();
      toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      fontStripButton = new System.Windows.Forms.ToolStripButton();
      songStripButton = new System.Windows.Forms.ToolStripButton();
      messagesStripButton = new System.Windows.Forms.ToolStripButton();
      defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(components);
      menuStrip1 = new System.Windows.Forms.MenuStrip();
      fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      createInPowerpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      slideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      songsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      messagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
      aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      slidePanel = new PresentationBuilder.Win.Controls.SlideListBox();
      toolStrip1.SuspendLayout();
      menuStrip1.SuspendLayout();
      SuspendLayout();
      // 
      // toolStrip1
      // 
      toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            newStripButton,
            openStripButton,
            saveStripButton,
            powerpointStripButton,
            toolStripSeparator1,
            addStripButton,
            insertStripButton,
            deleteStripButton,
            upStripButton,
            downStripButton,
            toolStripSeparator2,
            fontStripButton,
            songStripButton,
            messagesStripButton});
      toolStrip1.Location = new System.Drawing.Point(0, 24);
      toolStrip1.Name = "toolStrip1";
      toolStrip1.Size = new System.Drawing.Size(662, 25);
      toolStrip1.TabIndex = 5;
      toolStrip1.Text = "toolStrip1";
      // 
      // newStripButton
      // 
      newStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      newStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.New;
      newStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      newStripButton.Name = "newStripButton";
      newStripButton.Size = new System.Drawing.Size(23, 22);
      newStripButton.Text = "toolStripButton2";
      newStripButton.ToolTipText = "Clear Slides";
      newStripButton.Click += new System.EventHandler(newStripButton_Click);
      // 
      // openStripButton
      // 
      openStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      openStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.openfolderHS;
      openStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      openStripButton.Name = "openStripButton";
      openStripButton.Size = new System.Drawing.Size(23, 22);
      openStripButton.Text = "toolStripButton2";
      openStripButton.ToolTipText = "Open File";
      openStripButton.Click += new System.EventHandler(openStripButton_Click);
      // 
      // saveStripButton
      // 
      saveStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      saveStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.saveHS;
      saveStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      saveStripButton.Name = "saveStripButton";
      saveStripButton.Size = new System.Drawing.Size(23, 22);
      saveStripButton.Text = "toolStripButton3";
      saveStripButton.ToolTipText = "Save File";
      saveStripButton.Click += new System.EventHandler(saveStripButton_Click);
      // 
      // powerpointStripButton
      // 
      powerpointStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      powerpointStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.Build;
      powerpointStripButton.ImageTransparentColor = System.Drawing.Color.White;
      powerpointStripButton.Name = "powerpointStripButton";
      powerpointStripButton.Size = new System.Drawing.Size(23, 22);
      powerpointStripButton.Text = "toolStripButton6";
      powerpointStripButton.ToolTipText = "Create in Powerpoint";
      powerpointStripButton.Click += new System.EventHandler(powerpointStripButton_Click);
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // addStripButton
      // 
      addStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      addStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.Add;
      addStripButton.ImageTransparentColor = System.Drawing.Color.White;
      addStripButton.Name = "addStripButton";
      addStripButton.Size = new System.Drawing.Size(23, 22);
      addStripButton.Text = "Add Slide";
      addStripButton.Click += new System.EventHandler(addStripButton_Click);
      // 
      // insertStripButton
      // 
      insertStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      insertStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.Insert;
      insertStripButton.ImageTransparentColor = System.Drawing.Color.White;
      insertStripButton.Name = "insertStripButton";
      insertStripButton.Size = new System.Drawing.Size(23, 22);
      insertStripButton.Text = "toolStripButton5";
      insertStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
      insertStripButton.ToolTipText = "Insert Slide";
      insertStripButton.Click += new System.EventHandler(insertStripButton_Click);
      // 
      // deleteStripButton
      // 
      deleteStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      deleteStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.DeleteHS;
      deleteStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      deleteStripButton.Name = "deleteStripButton";
      deleteStripButton.Size = new System.Drawing.Size(23, 22);
      deleteStripButton.Text = "toolStripButton1";
      deleteStripButton.ToolTipText = "Delete Slide";
      deleteStripButton.Click += new System.EventHandler(deleteStripButton_Click);
      // 
      // upStripButton
      // 
      upStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      upStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.MoveUp;
      upStripButton.ImageTransparentColor = System.Drawing.Color.White;
      upStripButton.Name = "upStripButton";
      upStripButton.Size = new System.Drawing.Size(23, 22);
      upStripButton.Text = "toolStripButton7";
      upStripButton.ToolTipText = "Move Slide Up";
      upStripButton.Click += new System.EventHandler(upStripButton_Click);
      // 
      // downStripButton
      // 
      downStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      downStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.MoveDown;
      downStripButton.ImageTransparentColor = System.Drawing.Color.White;
      downStripButton.Name = "downStripButton";
      downStripButton.Size = new System.Drawing.Size(23, 22);
      downStripButton.Text = "toolStripButton8";
      downStripButton.ToolTipText = "Move Slide Down";
      downStripButton.Click += new System.EventHandler(downStripButton_Click);
      // 
      // toolStripSeparator2
      // 
      toolStripSeparator2.Name = "toolStripSeparator2";
      toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // fontStripButton
      // 
      fontStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      fontStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.FontDialogHS;
      fontStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      fontStripButton.Name = "fontStripButton";
      fontStripButton.Size = new System.Drawing.Size(23, 22);
      fontStripButton.Text = "toolStripButton10";
      fontStripButton.ToolTipText = "Slide Font";
      fontStripButton.Click += new System.EventHandler(fontStripButton_Click);
      // 
      // songStripButton
      // 
      songStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      songStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.Note;
      songStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      songStripButton.Name = "songStripButton";
      songStripButton.Size = new System.Drawing.Size(23, 22);
      songStripButton.Text = "toolStripButton9";
      songStripButton.ToolTipText = "Edit Songs";
      songStripButton.Click += new System.EventHandler(songStripButton_Click);
      // 
      // messagesStripButton
      // 
      messagesStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      messagesStripButton.Image = global::PresentationBuilder.Win.Properties.Resources.Book_angleHS;
      messagesStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      messagesStripButton.Name = "messagesStripButton";
      messagesStripButton.Size = new System.Drawing.Size(23, 22);
      messagesStripButton.Text = "toolStripButton11";
      messagesStripButton.ToolTipText = "Edit Messages";
      messagesStripButton.Click += new System.EventHandler(messagesStripButton_Click);
      // 
      // defaultLookAndFeel1
      // 
      defaultLookAndFeel1.LookAndFeel.UseWindowsXPTheme = true;
      // 
      // menuStrip1
      // 
      menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            fileToolStripMenuItem,
            slideToolStripMenuItem,
            toolsToolStripMenuItem});
      menuStrip1.Location = new System.Drawing.Point(0, 0);
      menuStrip1.Name = "menuStrip1";
      menuStrip1.Size = new System.Drawing.Size(662, 24);
      menuStrip1.TabIndex = 8;
      menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            newToolStripMenuItem,
            openToolStripMenuItem,
            saveToolStripMenuItem,
            saveAsToolStripMenuItem,
            toolStripMenuItem1,
            createInPowerpointToolStripMenuItem});
      fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      fileToolStripMenuItem.Text = "&File";
      // 
      // newToolStripMenuItem
      // 
      newToolStripMenuItem.Name = "newToolStripMenuItem";
      newToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      newToolStripMenuItem.Text = "&New";
      newToolStripMenuItem.Click += new System.EventHandler(newStripButton_Click);
      // 
      // openToolStripMenuItem
      // 
      openToolStripMenuItem.Name = "openToolStripMenuItem";
      openToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      openToolStripMenuItem.Text = "&Open...";
      openToolStripMenuItem.Click += new System.EventHandler(openStripButton_Click);
      // 
      // saveToolStripMenuItem
      // 
      saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      saveToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      saveToolStripMenuItem.Text = "&Save";
      saveToolStripMenuItem.Click += new System.EventHandler(saveStripButton_Click);
      // 
      // saveAsToolStripMenuItem
      // 
      saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      saveAsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      saveAsToolStripMenuItem.Text = "&Save As...";
      saveAsToolStripMenuItem.Click += new System.EventHandler(saveAsToolStripMenuItem_Click);
      // 
      // toolStripMenuItem1
      // 
      toolStripMenuItem1.Name = "toolStripMenuItem1";
      toolStripMenuItem1.Size = new System.Drawing.Size(182, 6);
      // 
      // createInPowerpointToolStripMenuItem
      // 
      createInPowerpointToolStripMenuItem.Name = "createInPowerpointToolStripMenuItem";
      createInPowerpointToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      createInPowerpointToolStripMenuItem.Text = "&Create in Powerpoint";
      createInPowerpointToolStripMenuItem.Click += new System.EventHandler(powerpointStripButton_Click);
      // 
      // slideToolStripMenuItem
      // 
      slideToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            addToolStripMenuItem,
            insertToolStripMenuItem,
            deleteToolStripMenuItem,
            moveUpToolStripMenuItem,
            moveDownToolStripMenuItem});
      slideToolStripMenuItem.Name = "slideToolStripMenuItem";
      slideToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
      slideToolStripMenuItem.Text = "&Slide";
      // 
      // addToolStripMenuItem
      // 
      addToolStripMenuItem.Name = "addToolStripMenuItem";
      addToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
      addToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      addToolStripMenuItem.Text = "&Add";
      addToolStripMenuItem.Click += new System.EventHandler(addStripButton_Click);
      // 
      // insertToolStripMenuItem
      // 
      insertToolStripMenuItem.Name = "insertToolStripMenuItem";
      insertToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      insertToolStripMenuItem.Text = "&Insert";
      insertToolStripMenuItem.Click += new System.EventHandler(insertStripButton_Click);
      // 
      // deleteToolStripMenuItem
      // 
      deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
      deleteToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      deleteToolStripMenuItem.Text = "&Delete";
      deleteToolStripMenuItem.Click += new System.EventHandler(deleteStripButton_Click);
      // 
      // moveUpToolStripMenuItem
      // 
      moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
      moveUpToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      moveUpToolStripMenuItem.Text = "Move &Up";
      moveUpToolStripMenuItem.Click += new System.EventHandler(upStripButton_Click);
      // 
      // moveDownToolStripMenuItem
      // 
      moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
      moveDownToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      moveDownToolStripMenuItem.Text = "Move &Down";
      moveDownToolStripMenuItem.Click += new System.EventHandler(downStripButton_Click);
      // 
      // toolsToolStripMenuItem
      // 
      toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            songsToolStripMenuItem,
            messagesToolStripMenuItem,
            toolStripMenuItem2,
            aboutToolStripMenuItem});
      toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
      toolsToolStripMenuItem.Text = "&Tools";
      // 
      // songsToolStripMenuItem
      // 
      songsToolStripMenuItem.Name = "songsToolStripMenuItem";
      songsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
      songsToolStripMenuItem.Text = "&Songs...";
      songsToolStripMenuItem.Click += new System.EventHandler(songStripButton_Click);
      // 
      // messagesToolStripMenuItem
      // 
      messagesToolStripMenuItem.Name = "messagesToolStripMenuItem";
      messagesToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
      messagesToolStripMenuItem.Text = "&Messages...";
      messagesToolStripMenuItem.Click += new System.EventHandler(messagesStripButton_Click);
      // 
      // toolStripMenuItem2
      // 
      toolStripMenuItem2.Name = "toolStripMenuItem2";
      toolStripMenuItem2.Size = new System.Drawing.Size(131, 6);
      // 
      // aboutToolStripMenuItem
      // 
      aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      aboutToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
      aboutToolStripMenuItem.Text = "&About...";
      aboutToolStripMenuItem.Click += new System.EventHandler(aboutToolStripMenuItem_Click);
      // 
      // slidePanel
      // 
      slidePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      slidePanel.AutoScroll = true;
      slidePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      fontItem1.Bold = true;
      fontItem1.FontColor = System.Drawing.Color.Black;
      fontItem1.FontName = "Arial";
      fontItem1.Italic = false;
      fontItem1.LineSpacing = 1F;
      fontItem1.Shadow = false;
      fontItem1.Size = 32F;
      fontItem1.Underline = false;
      slidePanel.IndicatorFont = fontItem1;
      slidePanel.IsDirty = false;
      slidePanel.Location = new System.Drawing.Point(7, 52);
      fontItem2.Bold = true;
      fontItem2.FontColor = System.Drawing.Color.Black;
      fontItem2.FontName = "Arial";
      fontItem2.Italic = false;
      fontItem2.LineSpacing = 1F;
      fontItem2.Shadow = false;
      fontItem2.Size = 32F;
      fontItem2.Underline = false;
      slidePanel.MessageFont = fontItem2;
      slidePanel.Name = "slidePanel";
      slidePanel.Size = new System.Drawing.Size(649, 415);
      fontItem3.Bold = true;
      fontItem3.FontColor = System.Drawing.Color.Black;
      fontItem3.FontName = "Arial";
      fontItem3.Italic = false;
      fontItem3.LineSpacing = 1F;
      fontItem3.Shadow = false;
      fontItem3.Size = 32F;
      fontItem3.Underline = false;
      slidePanel.SongFont = fontItem3;
      slidePanel.TabIndex = 3;
      // 
      // MainForm
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(662, 470);
      Controls.Add(toolStrip1);
      Controls.Add(slidePanel);
      Controls.Add(menuStrip1);
      Icon = ((System.Drawing.Icon)(resources.GetObject("$Icon")));
      Name = "MainForm";
      Text = "Presentation Builder";
      FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_FormClosing);
      toolStrip1.ResumeLayout(false);
      toolStrip1.PerformLayout();
      menuStrip1.ResumeLayout(false);
      menuStrip1.PerformLayout();
      ResumeLayout(false);
      PerformLayout();

    }
  }
}
