// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Controls.GridColumnMethods
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PEC.Windows.Common.Controls
{
  public static class GridColumnMethods
  {
    public static void AssignLookUpEdit(this GridColumn column, string descriptionField, string valueField, object query)
    {
      RepositoryItemLookUpEdit repositoryItemLookUpEdit = new RepositoryItemLookUpEdit();
      repositoryItemLookUpEdit.DisplayMember = descriptionField;
      repositoryItemLookUpEdit.ValueMember = valueField;
      repositoryItemLookUpEdit.Columns.Add(new LookUpColumnInfo(descriptionField));
      repositoryItemLookUpEdit.NullText = "";
      repositoryItemLookUpEdit.ShowFooter = false;
      repositoryItemLookUpEdit.ShowHeader = false;
      repositoryItemLookUpEdit.ShowLines = false;
      repositoryItemLookUpEdit.DataSource = query;
      column.ColumnEdit = (RepositoryItem) repositoryItemLookUpEdit;
    }

    public static void AssignLookUpEdit(this GridColumn column, Type enumType)
    {
      List<DictionaryEntry> list = new List<DictionaryEntry>();
      Array values = Enum.GetValues(enumType);
      string[] names = Enum.GetNames(enumType);
      Type underlyingType = Enum.GetUnderlyingType(enumType);
      for (int index1 = 0; index1 < Enumerable.Count<string>((IEnumerable<string>) names); ++index1)
      {
        object key = Convert.ChangeType(values.GetValue(index1), underlyingType);
        string str1 = names[index1];
        char ch = str1[0];
        string str2 = ch.ToString();
        for (int index2 = 1; index2 < str1.Length; ++index2)
        {
          if (char.IsUpper(str1[index2]))
          {
            string str3 = str2;
            string str4 = " ";
            ch = str1[index2];
            string str5 = ch.ToString();
            str2 = str3 + str4 + str5;
          }
          else
          {
            string str3 = str2;
            ch = str1[index2];
            string str4 = ch.ToString();
            str2 = str3 + str4;
          }
        }
        list.Add(new DictionaryEntry(key, (object) str2));
      }
      GridColumnMethods.AssignLookUpEdit(column, "Value", "Key", (object) list);
    }

    public static void UpdateLookUpDataSource(this GridColumn column, IQueryable query)
    {
      if (column.ColumnEdit == null)
        throw new Exception("No editor is set for this column");
      if (!(column.ColumnEdit is RepositoryItemLookUpEdit))
        throw new Exception("The editor for this column is not a LookUp");
      (column.ColumnEdit as RepositoryItemLookUpEdit).DataSource = (object) query;
    }
  }
}
