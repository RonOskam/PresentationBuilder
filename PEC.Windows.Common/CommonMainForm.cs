// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.CommonMainForm
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using DevExpress.XtraNavBar.ViewInfo;
using PEC.Configuration;
using PEC.Windows.Common.Controls;
using PEC.Windows.Common.Panes;
using PEC.Windows.Common.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace PEC.Windows.Common
{
  [ComVisible(true)]
  public class CommonMainForm : XtraForm
  {
    protected HomePane _homePane = (HomePane) null;
    private NavigationItemCollection _navigationItems = new NavigationItemCollection();
    protected NavBarItemLink _currentItem = (NavBarItemLink) null;
    private ArrayList _groupPositions = new ArrayList();
    private IContainer components = (IContainer) null;
    protected BasePane _currentPane;
    protected ISubject _subject;
    protected static CommonMainForm _instance;
    protected MenuStrip _mainMenu;
    private ToolStripMenuItem exitToolStripMenuItem;
    protected ToolStripMenuItem goToolStripMenuItem;
    private ToolStripMenuItem homeScreenToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem nextScreenToolStripMenuItem;
    private ToolStripMenuItem previousScreenToolStripMenuItem;
    private ToolStripMenuItem helpToolStripMenuItem1;
    private ToolStripMenuItem helpDocumentationToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem2;
    protected ToolStripMenuItem aboutToolStripMenuItem;
    protected ToolStrip shortcutToolBar;
    private ToolStrip actionToolbar;
    public Panel PanePanel;
    protected NavBarControl _mainExplorerBar;
    protected SplitContainer _mainSplitContainer;
    protected HeaderBar headerBar;
    protected ToolStripMenuItem fileToolStripMenuItem1;

    public static CommonMainForm Instance
    {
      get
      {
        return CommonMainForm._instance;
      }
    }

    public NavigationItemCollection NavigationItems
    {
      get
      {
        return this._navigationItems;
      }
    }

    public CommonMainForm()
    {
      this.InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
      CommonMainForm._instance = this;
      bool flag = false;
      if (ConfigurationManager.LocalSettingsProfile.GetValue("MainForm", "Height", 0) == 0)
      {
        int num1 = 950;
        int num2 = 750;
        if (Screen.PrimaryScreen.WorkingArea.Width < num1)
          num1 = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Width * 0.9);
        if (Screen.PrimaryScreen.WorkingArea.Height < num2)
          num2 = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Height * 0.9);
        ConfigurationManager.LocalSettingsProfile.SetValue("MainForm", "Height", (object) num2);
        ConfigurationManager.LocalSettingsProfile.SetValue("MainForm", "Width", (object) num1);
        flag = true;
      }
      ConfigurationManager.LoadFormBounds((Form) this);
      if (flag)
        this.CenterToScreen();
      string str1 = ConfigurationManager.LocalSettingsProfile.GetValue("MainForm", "Splitter", "");
      if (str1 != "")
        this._mainSplitContainer.SplitterDistance = Convert.ToInt32(str1);
      string str2 = ConfigurationManager.LocalSettingsProfile.GetValue("MainForm", "OutlookBarSplitter", "7");
      if (str2 != "")
        this._mainExplorerBar.NavigationPaneMaxVisibleGroups = Convert.ToInt32(str2);
      if (!this.ConfigureForm())
      {
        Application.Exit();
        this.Close();
      }
      this.LoadGoMenu();
    }

    private void LoadGoMenu()
    {
      int num = 1;
      foreach (NavigationItem navigationItem in Enumerable.Where<NavigationItem>((IEnumerable<NavigationItem>) this.NavigationItems, (Func<NavigationItem, bool>) (i => i.Name != "Home")))
      {
        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem.Text = navigationItem.Name;
        string name = navigationItem.Name;
        toolStripMenuItem.Click += (EventHandler) ((s, e) => this.RunNavigationItem(name));
        this.goToolStripMenuItem.DropDownItems.Insert(num++, (ToolStripItem) toolStripMenuItem);
      }
    }

    protected virtual HtmlContentSectionCollection GetHomePaneContent()
    {
      return new HtmlContentSectionCollection();
    }

    protected virtual bool ConfigureForm()
    {
      this._navigationItems.Add(new NavigationItem("Home", "Home")
      {
        EventHandler = (CancelEventHandler) ((s, e) => e.Cancel = !this.ShowPane((BasePane) this._homePane))
      });
      if (this._homePane != null)
      {
        this._homePane.KeepOpen = true;
        this._homePane.GetContent += new BasePane.GetContentHandler(this.GetHomePaneContent);
        this.ShowHomePane();
      }
      return true;
    }

    public ToolStripButton AddCommandItem(string commandName, string imageName, EventHandler eventHandler)
    {
      return this.AddCommandItem(commandName, Common.GetResourceImage(imageName, 24), eventHandler);
    }

    public ToolStripButton AddCommandItem(string commandName, Image commandImage, EventHandler eventHandler)
    {
      this.actionToolbar.Visible = true;
      if (this.actionToolbar.Items.Find(commandName, false).Length != 0)
        return (ToolStripButton) this.actionToolbar.Items.Find(commandName, false)[0];
      ToolStripButton toolStripButton = new ToolStripButton(commandName, commandImage);
      toolStripButton.TextImageRelation = TextImageRelation.ImageBeforeText;
      toolStripButton.ImageScaling = ToolStripItemImageScaling.None;
      toolStripButton.Font = new Font("Arial", Convert.ToSingle(8), FontStyle.Bold);
      toolStripButton.Padding = new Padding(3);
      toolStripButton.Click += eventHandler;
      toolStripButton.Name = commandName;
      this.actionToolbar.Items.Add((ToolStripItem) toolStripButton);
      return toolStripButton;
    }

    public ToolStripButton AddCommandItem(string commandName, string imageName, EventHandler eventHandler, string optionGroup)
    {
      if (this.actionToolbar.Items.Find(commandName, false).Length != 0)
        return (ToolStripButton) this.actionToolbar.Items.Find(commandName, false)[0];
      ToolStripButton button = new ToolStripButton(commandName, Common.GetResourceImage(imageName, 24));
      button.TextImageRelation = TextImageRelation.ImageBeforeText;
      button.ImageScaling = ToolStripItemImageScaling.None;
      button.Font = new Font("Arial", Convert.ToSingle(8), FontStyle.Bold);
      button.Padding = new Padding(3);
      button.CheckOnClick = true;
      button.Tag = (object) optionGroup;
      button.Click += eventHandler;
      button.Click += (EventHandler) ((sender, e) =>
      {
        foreach (ToolStripItem toolStripItem in (ArrangedElementCollection) this.actionToolbar.Items)
        {
          if (toolStripItem is ToolStripButton && toolStripItem.Tag != null && toolStripItem.Tag.ToString() == optionGroup && toolStripItem != button)
            ((ToolStripButton) toolStripItem).Checked = false;
        }
        button.Checked = true;
      });
      button.Name = commandName;
      this.actionToolbar.Items.Add((ToolStripItem) button);
      return button;
    }

    public void AddToolStripSeparator()
    {
      this.actionToolbar.Items.Add((ToolStripItem) new ToolStripSeparator());
    }

    public void ClearCommandItems()
    {
      this.actionToolbar.Items.Clear();
    }

    protected NavigationItem AddNavigationItem(string name, string imageName, Type paneType)
    {
      return this.AddNavigationItem(name, imageName, paneType, (string) null, (Image) null);
    }

    protected NavigationItem AddNavigationItem(string name, string imageName, Type paneType, string groupName, Image groupImage)
    {
      CancelEventHandler eventHandler = (CancelEventHandler) ((sender, e) => e.Cancel = !this.ShowPane(paneType));
      return this.AddNavigationItem(name, imageName, eventHandler, groupName, groupImage);
    }

    protected NavigationItem AddNavigationItem(string name, string imageName, CancelEventHandler eventHandler, string groupName, Image groupImage)
    {
      NavigationItem navigationItem = new NavigationItem(name, imageName);
      navigationItem.EventHandler = eventHandler;
      this.NavigationItems.Add(navigationItem);
      if (groupName != null)
        this.AddExplorerBarItem(navigationItem, groupName, groupImage);
      return navigationItem;
    }

    public void AddExplorerBarItem(NavigationItem navigationItem, string groupName, Image groupImage)
    {
      NavBarItem navBarItem = new NavBarItem();
      navBarItem.Caption = navigationItem.Name;
      navBarItem.Tag = (object) navigationItem;
      if (!string.IsNullOrEmpty(navigationItem.ImageName))
        navBarItem.SmallImage = Common.GetResourceImage(navigationItem.ImageName, 24);
      else
        navBarItem.SmallImage = navigationItem.Image;
      navBarItem.LinkClicked += (NavBarLinkEventHandler) ((sender, e) => this.RunNavigationItem(navigationItem));
      NavBarGroup newGroup = (NavBarGroup) null;
      foreach (NavBarGroup navBarGroup in (CollectionBase) this._mainExplorerBar.Groups)
      {
        if (navBarGroup.Caption == groupName)
          newGroup = navBarGroup;
      }
      if (newGroup == null)
      {
        newGroup = new NavBarGroup();
        newGroup.Name = groupName;
        newGroup.Caption = groupName;
        newGroup.Expanded = false;
        newGroup.SmallImage = groupImage;
        newGroup.LargeImage = groupImage;
        this.InsertGroup(newGroup, 0);
      }
      newGroup.ItemLinks.Add(navBarItem);
    }

    public void AddExplorerBarItem(NavigationItem navigationItem, Control control, string groupName, Image groupImage)
    {
      NavBarGroup newGroup = new NavBarGroup();
      newGroup.GroupStyle = NavBarGroupStyle.ControlContainer;
      this._mainExplorerBar.SuspendLayout();
      newGroup.ControlContainer = new NavBarGroupControlContainer();
      newGroup.Caption = groupName;
      newGroup.LargeImage = groupImage;
      newGroup.SmallImage = groupImage;
      newGroup.Tag = (object) navigationItem;
      newGroup.GroupStyle = NavBarGroupStyle.ControlContainer;
      this._mainExplorerBar.SuspendLayout();
      newGroup.ControlContainer = new NavBarGroupControlContainer();
      newGroup.ControlContainer.Controls.Add(control);
      this._mainExplorerBar.ResumeLayout(false);
      this.InsertGroup(newGroup, 0);
    }

    private void InsertGroup(NavBarGroup newGroup, int groupLocation)
    {
      bool flag = false;
      if (groupLocation > 0)
      {
        for (int index = 0; index < this._groupPositions.Count; ++index)
        {
          if ((int) this._groupPositions[index] > groupLocation)
          {
            this._mainExplorerBar.Groups.Insert(index, (ICollectionItem) newGroup);
            this._groupPositions.Insert(index, (object) groupLocation);
            flag = true;
            break;
          }
        }
      }
      if (groupLocation != -1 && flag)
        return;
      this._mainExplorerBar.Groups.Add(newGroup);
      this._groupPositions.Add((object) groupLocation);
    }

    public void AddMenuItem(string menuName, string menuItem, Image menuImage, EventHandler eventHandler, bool insertAtBeginning)
    {
      ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(menuItem, menuImage, eventHandler);
      this.AddMenuItem(menuName, (ToolStripItem) toolStripMenuItem, insertAtBeginning);
    }

    public void AddMenuItem(string menuName, ToolStripItem newToolStripItem, bool insertAtBeginning)
    {
      bool flag = false;
      if (menuName != null)
      {
        foreach (ToolStripItem toolStripItem in (ArrangedElementCollection) this._mainMenu.Items)
        {
          if (toolStripItem is ToolStripMenuItem)
          {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem) toolStripItem;
            if (toolStripItem.Text == menuName)
            {
              if (insertAtBeginning)
                toolStripMenuItem.DropDownItems.Insert(0, newToolStripItem);
              else
                toolStripMenuItem.DropDownItems.Add(newToolStripItem);
              flag = true;
              break;
            }
          }
        }
      }
      else
      {
        this._mainMenu.Items.Add(newToolStripItem);
        flag = true;
      }
      if (flag)
        return;
      ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(menuName);
      this._mainMenu.Items.Add((ToolStripItem) toolStripMenuItem1);
      toolStripMenuItem1.DropDownItems.Add(newToolStripItem);
    }

    public bool RunNavigationItem(string name)
    {
      NavigationItem byName = this._navigationItems.FindByName(name);
      if (byName == null)
        throw new Exception("Invalid navigation item: " + name);
      return this.RunNavigationItem(byName);
    }

    protected bool RunNavigationItem(NavigationItem navigationItem)
    {
      if (this._currentPane != null && !this._currentPane.CanChange())
        return false;
      if (navigationItem.EventHandler != null)
      {
        CancelEventArgs e = new CancelEventArgs();
        navigationItem.EventHandler((object) navigationItem, e);
        if (e.Cancel)
          return false;
      }
      if (navigationItem.AllowTabTo)
      {
        if (navigationItem.Image != null)
          this.SetHeader(navigationItem.Name, navigationItem.Image);
        else
          this.SetHeader(navigationItem.Name, navigationItem.ImageName);
      }
      foreach (NavBarGroup navBarGroup in (CollectionBase) this._mainExplorerBar.Groups)
      {
        if (navBarGroup.Tag != null && (NavigationItem) navBarGroup.Tag == navigationItem)
        {
          navBarGroup.Expanded = true;
          return true;
        }
        foreach (NavBarItemLink navBarItemLink in (CollectionBase) navBarGroup.ItemLinks)
        {
          if (navBarItemLink.Item.Tag != null && (NavigationItem) navBarItemLink.Item.Tag == navigationItem)
          {
            this._mainExplorerBar.SelectedLink = navBarItemLink;
            this._currentItem = navBarItemLink;
            return true;
          }
        }
        this._mainExplorerBar.SelectedLink = (NavBarItemLink) null;
      }
      return true;
    }

    public bool ShowPane(Type type)
    {
      if (this._currentPane != null && this._currentPane.GetType() == type)
        return true;
      return this.ShowPane((BasePane) Activator.CreateInstance(type));
    }

    public virtual bool ShowPane(BasePane pane)
    {
      if (pane is IObserver)
        ((IObserver) pane).Subject = this._subject;
      this.SuspendLayout();
      this.PanePanel.SuspendLayout();
      if (this._currentPane != null)
      {
        this._currentPane.Close();
        if (!this._currentPane.Equals((object) pane))
          this.RemoveCurrentPane();
      }
      this.actionToolbar.Items.Clear();
      this.actionToolbar.Visible = false;
      pane.Dock = DockStyle.Fill;
      if (pane.Parent != this.PanePanel)
        this.PanePanel.Controls.Add((Control) pane);
      this._currentPane = pane;
      this._currentPane.Open();
      this.FocusFirstControl();
      this.PanePanel.ResumeLayout();
      this.ResumeLayout();
      return true;
    }

    private void FocusFirstControl()
    {
      Control nextControl = this._currentPane.GetNextControl((Control) null, true);
      while (nextControl != null && (uint) nextControl.Controls.Count > 0U)
        nextControl = nextControl.Controls[0].GetNextControl((Control) null, true);
      if (nextControl is Label)
      {
        Control control = nextControl;
        do
        {
          nextControl = nextControl.Parent.GetNextControl(nextControl, true);
        }
        while (nextControl is Label && nextControl != control);
      }
      if (nextControl == null)
        return;
      nextControl.Focus();
    }

    private void RemoveCurrentPane()
    {
      if (this._currentPane == null)
        return;
      if (this._currentPane.ToolStrip != null)
      {
        this.PanePanel.Controls.Remove((Control) this._currentPane.ToolStrip);
        this._currentPane.ToolStrip.Parent = (Control) null;
      }
      this.PanePanel.Controls.Remove((Control) this._currentPane);
      this.actionToolbar.Items.Clear();
      if (!this._currentPane.KeepOpen)
        this._currentPane.Dispose();
      this._currentPane = (BasePane) null;
    }

    public void SetHeader(string headerText, Image headerImage)
    {
      this.headerBar.Text = headerText;
      this.headerBar.Image = headerImage;
    }

    public void SetHeader(string headerText, string headerImageName)
    {
      this.headerBar.Text = headerText;
      this.headerBar.Image = Common.GetResourceImage(headerImageName, 32);
    }

    public bool SaveCurrentPane()
    {
      if (this._currentPane == null)
        return true;
      return this._currentPane.CanChange();
    }

    protected virtual void LoadShortcutBar()
    {
      string str1 = ConfigurationManager.LocalSettingsProfile.GetValue("MainForm", "ShortCutBarList", "").ToString().Trim();
      this.shortcutToolBar.Items.Add((ToolStripItem) new ToolStripSeparator());
      string str2 = str1;
      char[] chArray = new char[1]
      {
        ','
      };
      foreach (string str3 in str2.Split(chArray))
        this.AddShortcutButton(str3);
    }

    protected ToolStripButton AddShortcutButton(string item)
    {
      NavigationItem byName = this._navigationItems.FindByName(item);
      if (byName != null)
        return this.AddShortcutButton(byName);
      return (ToolStripButton) null;
    }

    protected ToolStripButton AddShortcutButton(NavigationItem navigationItem)
    {
      return this.AddShortcutButton(navigationItem, TextImageRelation.ImageAboveText);
    }

    protected ToolStripButton AddShortcutButton(NavigationItem navigationItem, TextImageRelation imageRelation)
    {
      Image image = (navigationItem.Image ?? Common.GetResourceImage(navigationItem.ImageName, 32)) ?? Common.GetResourceImage("Resources.Window32");
      ToolStripButton toolStripButton = new ToolStripButton(navigationItem.Name, image);
      toolStripButton.TextImageRelation = imageRelation;
      toolStripButton.ImageScaling = ToolStripItemImageScaling.None;
      toolStripButton.Font = new Font("Arial", Convert.ToSingle(8), FontStyle.Bold);
      toolStripButton.Tag = (object) navigationItem;
      toolStripButton.Click += new EventHandler(this.ShortcutBarButton_Click);
      this.shortcutToolBar.Items.Add((ToolStripItem) toolStripButton);
      return toolStripButton;
    }

    private void ShortcutBarButton_Click(object sender, EventArgs e)
    {
      this.RunNavigationItem((NavigationItem) ((ToolStripItem) sender).Tag);
    }

    public void ShowHomePane()
    {
      this.ShowHomePane((object) null, (EventArgs) null);
    }

    private void ShowHomePane(object sender, EventArgs e)
    {
      this.RunNavigationItem("Home");
    }

    private void CommonMainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (this._currentPane != null)
        this._currentPane.Close();
      ConfigurationManager.LocalSettingsProfile.SetValue("MainForm", "Splitter", (object) this._mainSplitContainer.SplitterDistance);
      ConfigurationManager.LocalSettingsProfile.SetValue("MainForm", "OutlookBarSplitter", (object) this._mainExplorerBar.NavigationPaneMaxVisibleGroups.ToString());
      if (!this.DesignMode)
        ConfigurationManager.SaveFormBounds((Form) this);
      if (this._homePane == null)
        return;
      this._homePane.Close();
    }

    private void CommonMainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this._currentPane == null || this._currentPane.CanChange())
        return;
      e.Cancel = true;
    }

    private void nextScreenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this._currentItem == null)
        return;
      int index = this._currentItem.Group.ItemLinks.IndexOf((ICollectionItem) this._currentItem);
      do
      {
        ++index;
        if (index >= this._currentItem.Group.ItemLinks.Count)
          index = 0;
      }
      while (!((NavigationItem) this._currentItem.Group.ItemLinks[index].Item.Tag).AllowTabTo);
      this.RunNavigationItem((NavigationItem) this._currentItem.Group.ItemLinks[index].Item.Tag);
      this._currentItem.Group.Expanded = true;
    }

    private void previousScreenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this._currentItem == null)
        return;
      int index = this._currentItem.Group.ItemLinks.IndexOf((ICollectionItem) this._currentItem);
      do
      {
        --index;
        if (index < 0)
          index = this._currentItem.Group.ItemLinks.Count - 1;
      }
      while (!((NavigationItem) this._currentItem.Group.ItemLinks[index].Item.Tag).AllowTabTo);
      this.RunNavigationItem((NavigationItem) this._currentItem.Group.ItemLinks[index].Item.Tag);
      this._currentItem.Group.Expanded = true;
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
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
      this._mainMenu = new MenuStrip();
      this.fileToolStripMenuItem1 = new ToolStripMenuItem();
      this.exitToolStripMenuItem = new ToolStripMenuItem();
      this.goToolStripMenuItem = new ToolStripMenuItem();
      this.homeScreenToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.nextScreenToolStripMenuItem = new ToolStripMenuItem();
      this.previousScreenToolStripMenuItem = new ToolStripMenuItem();
      this.helpToolStripMenuItem1 = new ToolStripMenuItem();
      this.helpDocumentationToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripMenuItem2 = new ToolStripSeparator();
      this.aboutToolStripMenuItem = new ToolStripMenuItem();
      this.shortcutToolBar = new ToolStrip();
      this._mainSplitContainer = new SplitContainer();
      this._mainExplorerBar = new NavBarControl();
      this.PanePanel = new Panel();
      this.headerBar = new HeaderBar(this.components);
      this.actionToolbar = new ToolStrip();
      this._mainMenu.SuspendLayout();
      this._mainSplitContainer.Panel1.SuspendLayout();
      this._mainSplitContainer.Panel2.SuspendLayout();
      this._mainSplitContainer.SuspendLayout();
      this._mainExplorerBar.BeginInit();
      this.SuspendLayout();
      this._mainMenu.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.fileToolStripMenuItem1,
        (ToolStripItem) this.goToolStripMenuItem,
        (ToolStripItem) this.helpToolStripMenuItem1
      });
      this._mainMenu.Location = new Point(0, 0);
      this._mainMenu.Name = "_mainMenu";
      this._mainMenu.Size = new Size(731, 24);
      this._mainMenu.TabIndex = 10;
      this.fileToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.exitToolStripMenuItem
      });
      this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
      this.fileToolStripMenuItem1.Size = new Size(37, 20);
      this.fileToolStripMenuItem1.Text = "&File";
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new Size(152, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new EventHandler(this.exitToolStripMenuItem_Click);
      this.goToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.homeScreenToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.nextScreenToolStripMenuItem,
        (ToolStripItem) this.previousScreenToolStripMenuItem
      });
      this.goToolStripMenuItem.Name = "goToolStripMenuItem";
      this.goToolStripMenuItem.Size = new Size(34, 20);
      this.goToolStripMenuItem.Text = "&Go";
      this.homeScreenToolStripMenuItem.Name = "homeScreenToolStripMenuItem";
      this.homeScreenToolStripMenuItem.ShortcutKeys = Keys.H | Keys.Control;
      this.homeScreenToolStripMenuItem.Size = new Size(243, 22);
      this.homeScreenToolStripMenuItem.Text = "&Home Screen";
      this.homeScreenToolStripMenuItem.Click += new EventHandler(this.ShowHomePane);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(240, 6);
      this.nextScreenToolStripMenuItem.Name = "nextScreenToolStripMenuItem";
      this.nextScreenToolStripMenuItem.ShortcutKeys = Keys.Tab | Keys.Control;
      this.nextScreenToolStripMenuItem.Size = new Size(243, 22);
      this.nextScreenToolStripMenuItem.Text = "&Next Screen";
      this.nextScreenToolStripMenuItem.Click += new EventHandler(this.nextScreenToolStripMenuItem_Click);
      this.previousScreenToolStripMenuItem.Name = "previousScreenToolStripMenuItem";
      this.previousScreenToolStripMenuItem.ShortcutKeys = Keys.Tab | Keys.Shift | Keys.Control;
      this.previousScreenToolStripMenuItem.Size = new Size(243, 22);
      this.previousScreenToolStripMenuItem.Text = "&Previous Screen";
      this.previousScreenToolStripMenuItem.Click += new EventHandler(this.previousScreenToolStripMenuItem_Click);
      this.helpToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.helpDocumentationToolStripMenuItem,
        (ToolStripItem) this.toolStripMenuItem2,
        (ToolStripItem) this.aboutToolStripMenuItem
      });
      this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
      this.helpToolStripMenuItem1.Size = new Size(44, 20);
      this.helpToolStripMenuItem1.Text = "&Help";
      this.helpToolStripMenuItem1.Visible = false;
      this.helpDocumentationToolStripMenuItem.Name = "helpDocumentationToolStripMenuItem";
      this.helpDocumentationToolStripMenuItem.ShortcutKeys = Keys.F1;
      this.helpDocumentationToolStripMenuItem.Size = new Size(204, 22);
      this.helpDocumentationToolStripMenuItem.Text = "&Help Documentation";
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new Size(201, 6);
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new Size(204, 22);
      this.aboutToolStripMenuItem.Text = "&About";
      this.shortcutToolBar.GripStyle = ToolStripGripStyle.Hidden;
      this.shortcutToolBar.Location = new Point(0, 24);
      this.shortcutToolBar.Name = "shortcutToolBar";
      this.shortcutToolBar.Padding = new Padding(5, 0, 1, 0);
      this.shortcutToolBar.ShowItemToolTips = false;
      this.shortcutToolBar.Size = new Size(731, 25);
      this.shortcutToolBar.TabIndex = 11;
      this.shortcutToolBar.Text = "toolStrip1";
      this._mainSplitContainer.Dock = DockStyle.Fill;
      this._mainSplitContainer.Location = new Point(0, 49);
      this._mainSplitContainer.Name = "_mainSplitContainer";
      this._mainSplitContainer.Panel1.Controls.Add((Control) this._mainExplorerBar);
      this._mainSplitContainer.Panel2.Controls.Add((Control) this.PanePanel);
      this._mainSplitContainer.Panel2.Controls.Add((Control) this.headerBar);
      this._mainSplitContainer.Panel2.Controls.Add((Control) this.actionToolbar);
      this._mainSplitContainer.Size = new Size(731, 456);
      this._mainSplitContainer.SplitterDistance = 191;
      this._mainSplitContainer.TabIndex = 15;
      this._mainExplorerBar.ActiveGroup = (NavBarGroup) null;
      this._mainExplorerBar.AllowSelectedLink = true;
      this._mainExplorerBar.Appearance.NavigationPaneHeader.BackColor = SystemColors.ControlDark;
      this._mainExplorerBar.Appearance.NavigationPaneHeader.ForeColor = Color.White;
      this._mainExplorerBar.Appearance.NavigationPaneHeader.Options.UseBackColor = true;
      this._mainExplorerBar.Appearance.NavigationPaneHeader.Options.UseForeColor = true;
      this._mainExplorerBar.ContentButtonHint = (string) null;
      this._mainExplorerBar.Dock = DockStyle.Fill;
      this._mainExplorerBar.Location = new Point(0, 0);
      this._mainExplorerBar.Name = "_mainExplorerBar";
      this._mainExplorerBar.OptionsNavPane.ExpandedWidth = 191;
      this._mainExplorerBar.OptionsNavPane.ShowExpandButton = false;
      this._mainExplorerBar.Size = new Size(191, 456);
      this._mainExplorerBar.TabIndex = 1;
      this._mainExplorerBar.Text = "ExplorerBar";
      this._mainExplorerBar.View = (BaseViewInfoRegistrator) new NavigationPaneViewInfoRegistrator();
      this.PanePanel.Dock = DockStyle.Fill;
      this.PanePanel.Location = new Point(0, 40);
      this.PanePanel.Name = "PanePanel";
      this.PanePanel.Padding = new Padding(5);
      this.PanePanel.Size = new Size(536, 380);
      this.PanePanel.TabIndex = 14;
      this.headerBar.Dock = DockStyle.Top;
      this.headerBar.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.headerBar.ForeColor = SystemColors.Window;
      this.headerBar.Image = (Image) null;
      this.headerBar.Location = new Point(0, 0);
      this.headerBar.Name = "headerBar";
      this.headerBar.Padding = new Padding(2, 0, 0, 0);
      this.headerBar.Size = new Size(536, 40);
      this.headerBar.TabIndex = 15;
      this.actionToolbar.AutoSize = false;
      this.actionToolbar.Dock = DockStyle.Bottom;
      this.actionToolbar.GripStyle = ToolStripGripStyle.Hidden;
      this.actionToolbar.ImageScalingSize = new Size(24, 24);
      this.actionToolbar.Location = new Point(0, 420);
      this.actionToolbar.Name = "actionToolbar";
      this.actionToolbar.Padding = new Padding(10, 0, 1, 0);
      this.actionToolbar.Size = new Size(536, 36);
      this.actionToolbar.TabIndex = 13;
      this.actionToolbar.Text = "toolStrip1";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(731, 505);
      this.Controls.Add((Control) this._mainSplitContainer);
      this.Controls.Add((Control) this.shortcutToolBar);
      this.Controls.Add((Control) this._mainMenu);
      this.Name = "CommonMainForm";
      this.Text = "Form1";
      this.FormClosed += new FormClosedEventHandler(this.CommonMainForm_FormClosed);
      this.FormClosing += new FormClosingEventHandler(this.CommonMainForm_FormClosing);
      this._mainMenu.ResumeLayout(false);
      this._mainMenu.PerformLayout();
      this._mainSplitContainer.Panel1.ResumeLayout(false);
      this._mainSplitContainer.Panel2.ResumeLayout(false);
      this._mainSplitContainer.ResumeLayout(false);
      this._mainExplorerBar.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
