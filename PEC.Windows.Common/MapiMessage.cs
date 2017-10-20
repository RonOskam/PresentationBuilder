// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.MapiMessage
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System;
using System.Runtime.InteropServices;

namespace PEC.Windows.Common
{
  [StructLayout(LayoutKind.Sequential)]
  public class MapiMessage
  {
    public int reserved;
    public string subject;
    public string noteText;
    public string messageType;
    public string dateReceived;
    public string conversationID;
    public int flags;
    public IntPtr originator;
    public int recipCount;
    public IntPtr recips;
    public int fileCount;
    public IntPtr files;
  }
}
