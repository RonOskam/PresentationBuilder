using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace PresentationBuilder.Win.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Resources.resourceMan == null)
          Resources.resourceMan = new ResourceManager("PresentationBuilder.Win.Properties.Resources", typeof (Resources).Assembly);
        return Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Resources.resourceCulture;
      }
      set
      {
        Resources.resourceCulture = value;
      }
    }

    internal static Bitmap Add
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("Add", Resources.resourceCulture);
      }
    }

    internal static Bitmap Book_angleHS
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("Book_angleHS", Resources.resourceCulture);
      }
    }

    internal static Bitmap Build
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("Build", Resources.resourceCulture);
      }
    }

    internal static Bitmap DeleteHS
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("DeleteHS", Resources.resourceCulture);
      }
    }

    internal static Bitmap FontDialogHS
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("FontDialogHS", Resources.resourceCulture);
      }
    }

    internal static Bitmap Insert
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("Insert", Resources.resourceCulture);
      }
    }

    internal static Bitmap MoveDown
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("MoveDown", Resources.resourceCulture);
      }
    }

    internal static Bitmap MoveUp
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("MoveUp", Resources.resourceCulture);
      }
    }

    internal static Bitmap New
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("New", Resources.resourceCulture);
      }
    }

    internal static Bitmap Note
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("Note", Resources.resourceCulture);
      }
    }

    internal static Bitmap openfolderHS
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("openfolderHS", Resources.resourceCulture);
      }
    }

    internal static Bitmap saveHS
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("saveHS", Resources.resourceCulture);
      }
    }

    internal Resources()
    {
    }
  }
}
