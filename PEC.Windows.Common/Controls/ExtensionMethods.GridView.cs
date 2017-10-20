// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Controls.GridViewMethods
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;

namespace PEC.Windows.Common.Controls
{
  public static class GridViewMethods
  {
    public static T CurrentObject<T>(this GridView view)
    {
      T obj = default (T);
      object focusedRow = view.GetFocusedRow();
      if (focusedRow != null && focusedRow is T)
        obj = (T) focusedRow;
      return obj;
    }

    public static void SetTypeProperties(this GridView view, GridType gridType)
    {
      if (gridType == GridType.SelectionGrid)
      {
        view.FocusRectStyle = DrawFocusRectStyle.RowFocus;
        view.OptionsBehavior.Editable = false;
        view.OptionsSelection.EnableAppearanceFocusedCell = false;
        view.OptionsSelection.EnableAppearanceFocusedRow = true;
        view.OptionsView.ShowHorizontalLines = view.OptionsView.RowAutoHeight ? DefaultBoolean.True : DefaultBoolean.False;
        view.OptionsView.ShowVerticalLines = view.OptionsView.ShowHorizontalLines;
        view.OptionsView.ShowIndicator = false;
      }
      else if (gridType == GridType.EditGrid)
      {
        view.FocusRectStyle = DrawFocusRectStyle.CellFocus;
        view.OptionsBehavior.Editable = true;
        view.OptionsSelection.EnableAppearanceFocusedCell = true;
        view.OptionsSelection.EnableAppearanceFocusedRow = false;
        view.OptionsView.ShowHorizontalLines = DefaultBoolean.True;
        view.OptionsView.ShowVerticalLines = DefaultBoolean.True;
        view.OptionsView.ShowIndicator = true;
      }
      if (gridType == GridType.Custom)
        return;
      view.OptionsView.ShowGroupPanel = false;
      view.OptionsDetail.EnableMasterViewMode = false;
      view.GridControl.ShowOnlyPredefinedDetails = true;
    }
  }
}
