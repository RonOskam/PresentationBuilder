// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Controls.LookupEdit
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PEC.Windows.Common.Controls
{
  public static class LookupEdit
  {
    public static void Initialize(this RepositoryItemLookUpEdit lookup, string descriptionField, string valueField, IEnumerable col)
    {
      lookup.ValueMember = valueField;
      lookup.DataSource = (object) col;
      LookupEdit.Initialize(lookup, descriptionField);
    }

    public static void Initialize(this RepositoryItemLookUpEdit lookup, string descriptionField)
    {
      lookup.DisplayMember = descriptionField;
      lookup.Columns.Add(new LookUpColumnInfo(descriptionField));
      LookupEdit.Initialize(lookup);
    }

    public static void Initialize(this RepositoryItemLookUpEdit lookup)
    {
      lookup.NullText = "";
      lookup.ShowFooter = false;
      lookup.ShowHeader = false;
      lookup.ShowLines = false;
    }

    public static void Initialize(this LookUpEdit lookup, string descriptionField, string valueField, IEnumerable col)
    {
      lookup.Properties.ValueMember = valueField;
      lookup.Properties.DataSource = (object) col;
      LookupEdit.Initialize(lookup, descriptionField);
    }

    public static void Initialize(this LookUpEdit lookup, string descriptionField)
    {
      lookup.Properties.DisplayMember = descriptionField;
      lookup.Properties.Columns.Add(new LookUpColumnInfo(descriptionField));
      LookupEdit.Initialize(lookup);
    }

    public static void Initialize(this LookUpEdit lookup, Type enumType)
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
      LookupEdit.Initialize(lookup, "Value", "Key", (IEnumerable) list);
    }

    public static void Initialize(this LookUpEdit lookup)
    {
      lookup.Properties.NullText = "";
      lookup.Properties.ShowFooter = false;
      lookup.Properties.ShowHeader = false;
      lookup.Properties.ShowLines = false;
      lookup.KeyDown += (KeyEventHandler) ((sender, e) =>
      {
        if (e.KeyCode != Keys.Delete)
          return;
        e.Handled = true;
        lookup.EditValue = (object) null;
      });
    }
  }
}
