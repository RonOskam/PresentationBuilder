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
    private SlideControl _focusControl = null;
    private FontItem _songFont = null;
    private FontItem _indicatorFont = null;
    private FontItem _messageFont = null;
    private IContainer components = null;

    public bool IsDirty { get; set; }

    public FontItem SongFont
    {
      get
      {
        if (_songFont == null)
        {
          _songFont = new FontItem();
          _songFont.LoadFont("SongFont");
        }
        return _songFont;
      }
      set
      {
        _songFont = value;
      }
    }

    public FontItem IndicatorFont
    {
      get
      {
        if (_indicatorFont == null)
        {
          _indicatorFont = new FontItem();
          _indicatorFont.LoadFont("IndicatorFont");
        }
        return _indicatorFont;
      }
      set
      {
        _indicatorFont = value;
      }
    }

    public FontItem MessageFont
    {
      get
      {
        if (_messageFont == null)
        {
          _messageFont = new FontItem();
          _messageFont.LoadFont("MessageFont");
        }
        return _messageFont;
      }
      set
      {
        _messageFont = value;
      }
    }

    public bool HasSelectedSlide
    {
      get
      {
        return _focusControl != null;
      }
    }

    public bool CanSave
    {
      get
      {
        return (uint) Controls.Count > 0U;
      }
    }

    public SlideListBox()
    {
      InitializeComponent();
    }

    public void ReloadSongs()
    {
      foreach (object obj in (ArrangedElementCollection) Controls)
      {
        if (obj is SlideControl)
          ((SlideControl) obj).UpdateSongList();
      }
    }

    public void AddSlide()
    {
      SlideControl slide = CreateSlide();
      Controls.SetChildIndex((Control) slide, 0);
      SetFocusControl(slide);
    }

    public void Clear()
    {
      Controls.Clear();
      IsDirty = false;
    }

    private SlideControl CreateSlide()
    {
      IsDirty = true;
      SlideControl slideControl = new SlideControl();
      slideControl.ParentBox = this;
      slideControl.Dock = DockStyle.Top;
      Controls.Add((Control) slideControl);
      slideControl.typeLookup.Focus();
      slideControl.Changed += new SlideControl.ChangedEventHandler(slide_Changed);
      return slideControl;
    }

    private void slide_Changed(object sender, EventArgs e)
    {
      IsDirty = true;
    }

    public void GeneratePresentation()
    {
      if (!Validate())
        return;
      SlidePresentation slidePresentation = new SlidePresentation();
      slidePresentation.SongFont = SongFont;
      slidePresentation.IndicatorFont = IndicatorFont;
      slidePresentation.MessageFont = MessageFont;
      for (int index = Controls.Count - 1; index >= 0; --index)
        slidePresentation.AddSlide(((SlideControl) Controls[index]).CreateSlide());
      slidePresentation.Generate();
    }

    private bool Validate()
    {
      foreach (object obj in (ArrangedElementCollection) Controls)
      {
        string text = ((SlideControl) obj).CreateSlide().Validate();
        if (text != null)
        {
          SetFocusControl((SlideControl) obj);
          int num = (int) MessageBox.Show(text, Application.ProductName);
          return false;
        }
      }
      return true;
    }

    internal void SetFocusControl(SlideControl slide)
    {
      if (_focusControl != null)
        _focusControl.LostFocus();
      _focusControl = slide;
      slide.GotFocus();
      Point point = ScrollToControl((Control) slide);
      SetDisplayRectLocation(point.X, point.Y);
    }

    public void InsertSlide()
    {
      SlideControl slide = CreateSlide();
      int num = 0;
      if (_focusControl != null)
        num = Controls.IndexOf((Control) _focusControl);
      Controls.SetChildIndex((Control) slide, num + 1);
      SetFocusControl(slide);
    }

    public void DeleteSelected()
    {
      if (_focusControl == null)
        return;
      int index = Controls.IndexOf((Control) _focusControl);
      Controls.Remove((Control) _focusControl);
      if (index > Controls.Count - 1)
        index = Controls.Count - 1;
      if (index > 0)
        SetFocusControl(Controls[index] as SlideControl);
      IsDirty = true;
    }

    public void MoveSelectedDown()
    {
      int num = Controls.IndexOf((Control) _focusControl);
      if (num <= 0)
        return;
      IsDirty = true;
      Controls.SetChildIndex((Control) _focusControl, num - 1);
    }

    public void MoveSelectedUp()
    {
      int num = Controls.IndexOf((Control) _focusControl);
      if (num == Controls.Count - 1)
        return;
      Controls.SetChildIndex((Control) _focusControl, num + 1);
      IsDirty = true;
    }

    public void SaveToFile(string fileName)
    {
      XmlWriter writer = XmlWriter.Create(fileName);
      writer.WriteStartElement("Root");
      writer.WriteStartElement("Slides");
      for (int index = Controls.Count - 1; index >= 0; --index)
        ((SlideControl) Controls[index]).WriteToXML(writer);
      writer.WriteEndElement();
      writer.WriteStartElement("Fonts");
      SongFont.WriteToXML(writer, "SongFont");
      IndicatorFont.WriteToXML(writer, "IndicatorFont");
      MessageFont.WriteToXML(writer, "MessageFont");
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.Close();
      IsDirty = false;
    }

    public void OpenFromFile(string fileName)
    {
      Visible = false;
      try
      {
        Controls.Clear();
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
              SlideControl slide = CreateSlide();
              Controls.SetChildIndex((Control) slide, 0);
              slide.ReadFromXML(reader);
            }
            else if (reader.Name == "Font")
            {
              string attribute = reader.GetAttribute("Name");
              if (attribute == "SongFont")
                SongFont.ReadFromXML(reader);
              else if (attribute == "IndicatorFont")
                IndicatorFont.ReadFromXML(reader);
              else if (attribute == "MessageFont")
                MessageFont.ReadFromXML(reader);
            }
          }
        }
        reader.Close();
        IsDirty = false;
      }
      finally
      {
        Visible = true;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      components = (IContainer) new Container();
    }
  }
}
