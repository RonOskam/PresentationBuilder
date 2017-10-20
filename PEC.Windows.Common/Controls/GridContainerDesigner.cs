// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Controls.GridContainerDesigner
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PEC.Windows.Common.Controls
{
  internal class GridContainerDesigner : ParentControlDesigner
  {
    public override void Initialize(IComponent component)
    {
      base.Initialize(component);
      this.EnableDesignMode((Control) (component as GridContainer).ButtonPanel, "ButtonPanel");
    }
  }
}
