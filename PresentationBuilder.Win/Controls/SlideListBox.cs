// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.Win.Controls.SlideListBox
// Assembly: PresentationBuilder, Version=1.0.0.28120, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 295F5AD1-A97E-4830-A536-CA2F8525E5B1
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.exe

using PresentationBuilder.BLL.PowerPoint;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

namespace PresentationBuilder.Win.Controls
{
  public class SlideListBox : Panel
  {
    private SlideControl _focusControl = (SlideControl) null;
    private FontItem _songFont = (FontItem) null;
    private FontItem _indicatorFont = (FontItem) null;
    private FontItem _messageFont = (FontItem) null;
    private IContainer components = (IContainer) null;

    public bool IsDirty { get; set; }

    public FontItem SongFont
    {
      get
      {
        if (this._songFont == null)
        {
          this._songFont = new FontItem();
          this._songFont.LoadFont("SongFont");
        }
        return this._songFont;
      }
      set
      {
        this._songFont = value;
      }
    }

    public FontItem IndicatorFont
    {
      get
      {
        if (this._indicatorFont == null)
        {
          this._indicatorFont = new FontItem();
          this._indicatorFont.LoadFont("IndicatorFont");
        }
        return this._indicatorFont;
      }
      set
      {
        this._indicatorFont = value;
      }
    }

    public FontItem MessageFont
    {
      get
      {
        if (this._messageFont == null)
        {
          this._messageFont = new FontItem();
          this._messageFont.LoadFont("MessageFont");
        }
        return this._messageFont;
      }
      set
      {
        this._messageFont = value;
      }
    }

    public bool HasSelectedSlide
    {
      get
      {
        return this._focusControl != null;
      }
    }

    public bool CanSave
    {
      get
      {
        return (uint) this.Controls.Count > 0U;
      }
    }

    public SlideListBox()
    {
      this.InitializeComponent();
    }

    public void ReloadSongs()
    {
      foreach (object obj in (ArrangedElementCollection) this.Controls)
      {
        if (obj is SlideControl)
          ((SlideControl) obj).UpdateSongList();
      }
    }

    public void AddSlide()
    {
      SlideControl slide = this.CreateSlide();
      this.Controls.SetChildIndex((Control) slide, 0);
      this.SetFocusControl(slide);
    }

    public void Clear()
    {
      this.Controls.Clear();
      this.IsDirty = false;
    }

    private SlideControl CreateSlide()
    {
      this.IsDirty = true;
      SlideControl slideControl = new SlideControl();
      slideControl.ParentBox = this;
      slideControl.Dock = DockStyle.Top;
      this.Controls.Add((Control) slideControl);
      slideControl.typeLookup.Focus();
      slideControl.Changed += new SlideControl.ChangedEventHandler(this.slide_Changed);
      return slideControl;
    }

    private void slide_Changed(object sender, EventArgs e)
    {
      this.IsDirty = true;
    }

    public void GeneratePresentation()
    {
      if (!this.Validate())
        return;
      SlidePresentation slidePresentation = new SlidePresentation();
      slidePresentation.SongFont = this.SongFont;
      slidePresentation.IndicatorFont = this.IndicatorFont;
      slidePresentation.MessageFont = this.MessageFont;
      for (int index = this.Controls.Count - 1; index >= 0; --index)
        slidePresentation.AddSlide(((SlideControl) this.Controls[index]).CreateSlide());
      slidePresentation.Generate();
    }

    private bool Validate()
    {
      foreach (object obj in (ArrangedElementCollection) this.Controls)
      {
        string text = ((SlideControl) obj).CreateSlide().Validate();
        if (text != null)
        {
          this.SetFocusControl((SlideControl) obj);
          int num = (int) MessageBox.Show(text, Application.ProductName);
          return false;
        }
      }
      return true;
    }

    internal void SetFocusControl(SlideControl slide)
    {
      if (this._focusControl != null)
        this._focusControl.LostFocus();
      this._focusControl = slide;
      slide.GotFocus();
      Point point = this.ScrollToControl((Control) slide);
      this.SetDisplayRectLocation(point.X, point.Y);
    }

    public void InsertSlide()
    {
      SlideControl slide = this.CreateSlide();
      int num = 0;
      if (this._focusControl != null)
        num = this.Controls.IndexOf((Control) this._focusControl);
      this.Controls.SetChildIndex((Control) slide, num + 1);
      this.SetFocusControl(slide);
    }

    public void DeleteSelected()
    {
      if (this._focusControl == null)
        return;
      int index = this.Controls.IndexOf((Control) this._focusControl);
      this.Controls.Remove((Control) this._focusControl);
      if (index > this.Controls.Count - 1)
        index = this.Controls.Count - 1;
      if (index > 0)
        this.SetFocusControl(this.Controls[index] as SlideControl);
      this.IsDirty = true;
    }

    public void MoveSelectedDown()
    {
      int num = this.Controls.IndexOf((Control) this._focusControl);
      if (num <= 0)
        return;
      this.IsDirty = true;
      this.Controls.SetChildIndex((Control) this._focusControl, num - 1);
    }

    public void MoveSelectedUp()
    {
      int num = this.Controls.IndexOf((Control) this._focusControl);
      if (num == this.Controls.Count - 1)
        return;
      this.Controls.SetChildIndex((Control) this._focusControl, num + 1);
      this.IsDirty = true;
    }

    public void SaveToFile(string fileName)
    {
      XmlWriter writer = XmlWriter.Create(fileName);
      writer.WriteStartElement("Root");
      writer.WriteStartElement("Slides");
      for (int index = this.Controls.Count - 1; index >= 0; --index)
        ((SlideControl) this.Controls[index]).WriteToXML(writer);
      writer.WriteEndElement();
      writer.WriteStartElement("Fonts");
      this.SongFont.WriteToXML(writer, "SongFont");
      this.IndicatorFont.WriteToXML(writer, "IndicatorFont");
      this.MessageFont.WriteToXML(writer, "MessageFont");
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.Close();
      this.IsDirty = false;
    }

    public void OpenFromFile(string fileName)
    {
      this.Visible = false;
      try
      {
        this.Controls.Clear();
        XmlReader reader = XmlReader.Create((Stream) new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read));
        reader.Read();
        if (!reader.IsStartElement("Root"))
          throw new ApplicationException("An Invalid file format was selected.");
        while (reader.Read())
        {
          if (reader.IsStartElement() && reader.HasAttributes)
          {
            if (reader.Name == "Slide")
            {
              SlideControl slide = this.CreateSlide();
              this.Controls.SetChildIndex((Control) slide, 0);
              slide.ReadFromXML(reader);
            }
            else if (reader.Name == "Font")
            {
              string attribute = reader.GetAttribute("Name");
              if (attribute == "SongFont")
                this.SongFont.ReadFromXML(reader);
              else if (attribute == "IndicatorFont")
                this.IndicatorFont.ReadFromXML(reader);
              else if (attribute == "MessageFont")
                this.MessageFont.ReadFromXML(reader);
            }
          }
        }
        reader.Close();
        this.IsDirty = false;
      }
      finally
      {
        this.Visible = true;
      }
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
    }
  }
}
