using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;

namespace PresentationBuilder.BLL
{
  public class SiteReader
  {
    private string _pageUrl;

    protected string GetPageHTML(string url)
    {
      string str = "";
      this._pageUrl = url;
      using (StreamReader streamReader = new StreamReader(WebRequest.Create(url).GetResponse().GetResponseStream()))
      {
        str = streamReader.ReadToEnd();
        streamReader.Close();
      }
      return str;
    }

    protected Dictionary<string, string> ParseAnchorTags(string html)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      int startIndex1 = 0;
      while (true)
      {
        int startIndex2 = html.IndexOf("<a ", startIndex1);
        if (startIndex2 != -1)
        {
          int num1 = html.IndexOf("</a>", startIndex2);
          string str1 = html.Substring(startIndex2, num1 - startIndex2 + 4);
          int num2 = str1.IndexOf("href=\"");
          int startIndex3 = str1.IndexOf('"', num2 + 6);
          string str2 = str1.Substring(num2 + 6, startIndex3 - num2 - 6);
          if (str2.StartsWith(".."))
            str2 = this._pageUrl.Substring(0, this._pageUrl.Substring(0, this._pageUrl.LastIndexOf('/') - 1).LastIndexOf('/')) + str2.Remove(0, 2);
          int startIndex4 = str1.IndexOf('>', startIndex3);
          int num3 = str1.IndexOf("</", startIndex4);
          string str3 = HttpUtility.HtmlDecode(str1.Substring(startIndex4 + 1, num3 - startIndex4 - 1));
          string key = str3;
          int num4 = 1;
          while (dictionary.ContainsKey(key))
          {
            key = str3 + " " + num4.ToString();
            ++num4;
          }
          dictionary.Add(key, str2);
          startIndex1 = num1;
        }
        else
          break;
      }
      return dictionary;
    }

    protected string GetTagContents(string html, string tag)
    {
      int index;
      return this.GetTagContents(html, tag, (string) null, 0, out index);
    }

    protected string GetTagContents(string html, string tag, int startIndex, out int index)
    {
      return this.GetTagContents(html, tag, (string) null, startIndex, out index);
    }

    protected string GetTagContents(string html, string tag, string attributes)
    {
      int index;
      return this.GetTagContents(html, tag, attributes, 0, out index);
    }

    protected string GetTagContents(string html, string tag, string attributes, int startIndex, out int index)
    {
      string str;
      if (!string.IsNullOrEmpty(attributes))
        str = "<" + tag + " " + attributes + ">";
      else
        str = "<" + tag + ">";
      int startIndex1 = html.IndexOf(str, startIndex);
      index = startIndex1;
      if (startIndex1 == -1)
        return (string) null;
      int num = html.IndexOf("</" + tag + ">", startIndex1);
      if (num == -1)
        num = html.IndexOf("/>", startIndex1);
      if (num == -1)
        return (string) null;
      int startIndex2 = startIndex1 + str.Length;
      return html.Substring(startIndex2, num - startIndex2);
    }
  }
}
