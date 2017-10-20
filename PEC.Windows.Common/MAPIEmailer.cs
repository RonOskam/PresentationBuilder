// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.MAPIEmailer
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PEC.Windows.Common
{
  public class MAPIEmailer
  {
    private readonly string[] errors = new string[27]
    {
      "OK [0]",
      "User abort [1]",
      "General MAPI failure [2]",
      "MAPI login failure [3]",
      "Disk full [4]",
      "Insufficient memory [5]",
      "Access denied [6]",
      "-unknown- [7]",
      "Too many sessions [8]",
      "Too many files were specified [9]",
      "Too many recipients were specified [10]",
      "A specified attachment was not found [11]",
      "Attachment open failure [12]",
      "Attachment write failure [13]",
      "Unknown recipient [14]",
      "Bad recipient type [15]",
      "No messages [16]",
      "Invalid message [17]",
      "Text too large [18]",
      "Invalid session [19]",
      "Type not supported [20]",
      "A recipient was specified ambiguously [21]",
      "Message in use [22]",
      "Network failure [23]",
      "Invalid edit fields [24]",
      "Invalid recipients [25]",
      "Not supported [26]"
    };
    private List<MapiRecipDesc> m_recipients = new List<MapiRecipDesc>();
    private List<string> m_attachments = new List<string>();
    private int m_lastError = 0;
    private const int MAPI_LOGON_UI = 1;
    private const int MAPI_DIALOG = 8;
    private const int maxAttachments = 20;

    public bool AddRecipientTo(string email)
    {
      return this.AddRecipient(email, MAPIEmailer.HowTo.MAPI_TO);
    }

    public bool AddRecipientCC(string email)
    {
      return this.AddRecipient(email, MAPIEmailer.HowTo.MAPI_TO);
    }

    public bool AddRecipientBCC(string email)
    {
      return this.AddRecipient(email, MAPIEmailer.HowTo.MAPI_TO);
    }

    public void AddAttachment(string strAttachmentFileName)
    {
      this.m_attachments.Add(strAttachmentFileName);
    }

    public int SendMailPopup(string strSubject, string strBody)
    {
      return this.SendMail(strSubject, strBody, 9);
    }

    public int SendMailDirect(string strSubject, string strBody)
    {
      return this.SendMail(strSubject, strBody, 1);
    }

    [DllImport("MAPI32.DLL")]
    private static extern int MAPISendMail(IntPtr sess, IntPtr hwnd, MapiMessage message, int flg, int rsv);

    private int SendMail(string strSubject, string strBody, int how)
    {
      MapiMessage msg = new MapiMessage();
      msg.subject = strSubject;
      msg.noteText = strBody;
      msg.recips = this.GetRecipients(out msg.recipCount);
      msg.files = this.GetAttachments(out msg.fileCount);
      this.m_lastError = MAPIEmailer.MAPISendMail(new IntPtr(0), new IntPtr(0), msg, how, 0);
      if (this.m_lastError > 1)
      {
        int num = (int) MessageBox.Show("MAPISendMail failed! " + this.GetLastError(), "MAPISendMail");
      }
      this.Cleanup(ref msg);
      return this.m_lastError;
    }

    private bool AddRecipient(string email, MAPIEmailer.HowTo howTo)
    {
      this.m_recipients.Add(new MapiRecipDesc()
      {
        recipClass = (int) howTo,
        name = email
      });
      return true;
    }

    private IntPtr GetRecipients(out int recipCount)
    {
      recipCount = 0;
      if (this.m_recipients.Count == 0)
        return IntPtr.Zero;
      int num1 = Marshal.SizeOf(typeof (MapiRecipDesc));
      IntPtr num2 = Marshal.AllocHGlobal(this.m_recipients.Count * num1);
      int num3 = (int) num2;
      foreach (MapiRecipDesc structure in this.m_recipients)
      {
        Marshal.StructureToPtr<MapiRecipDesc>(structure, (IntPtr) num3, false);
        num3 += num1;
      }
      recipCount = this.m_recipients.Count;
      return num2;
    }

    private IntPtr GetAttachments(out int fileCount)
    {
      fileCount = 0;
      if (this.m_attachments == null || (this.m_attachments.Count <= 0 || this.m_attachments.Count > 20))
        return IntPtr.Zero;
      int num1 = Marshal.SizeOf(typeof (MapiFileDesc));
      IntPtr num2 = Marshal.AllocHGlobal(this.m_attachments.Count * num1);
      MapiFileDesc structure = new MapiFileDesc();
      structure.position = -1;
      int num3 = (int) num2;
      foreach (string path in this.m_attachments)
      {
        structure.name = Path.GetFileName(path);
        structure.path = path;
        Marshal.StructureToPtr<MapiFileDesc>(structure, (IntPtr) num3, false);
        num3 += num1;
      }
      fileCount = this.m_attachments.Count;
      return num2;
    }

    private void Cleanup(ref MapiMessage msg)
    {
      int num1 = Marshal.SizeOf(typeof (MapiRecipDesc));
      if (msg.recips != IntPtr.Zero)
      {
        int num2 = (int) msg.recips;
        for (int index = 0; index < msg.recipCount; ++index)
        {
          Marshal.DestroyStructure((IntPtr) num2, typeof (MapiRecipDesc));
          num2 += num1;
        }
        Marshal.FreeHGlobal(msg.recips);
      }
      if (msg.files != IntPtr.Zero)
      {
        int num2 = Marshal.SizeOf(typeof (MapiFileDesc));
        int num3 = (int) msg.files;
        for (int index = 0; index < msg.fileCount; ++index)
        {
          Marshal.DestroyStructure((IntPtr) num3, typeof (MapiFileDesc));
          num3 += num2;
        }
        Marshal.FreeHGlobal(msg.files);
      }
      this.m_recipients.Clear();
      this.m_attachments.Clear();
      this.m_lastError = 0;
    }

    public string GetLastError()
    {
      if (this.m_lastError <= 26)
        return this.errors[this.m_lastError];
      return "MAPI error [" + this.m_lastError.ToString() + "]";
    }

    private enum HowTo
    {
      MAPI_ORIG,
      MAPI_TO,
      MAPI_CC,
      MAPI_BCC,
    }
  }
}
