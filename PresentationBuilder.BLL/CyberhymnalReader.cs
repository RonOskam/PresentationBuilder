using System.Collections.Generic;
using System.Web;

namespace PresentationBuilder.BLL
{
  public class CyberhymnalReader : SiteReader
  {
    public Dictionary<string, string> SearchByName(string name)
    {
      string pageHtml = this.GetPageHTML("http://nethymnal.org/ttl/ttl-" + name.Substring(0, 1).ToLower() + ".htm");
      int startIndex = 0;
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      while (true)
      {
        int index;
        string tagContents = this.GetTagContents(pageHtml, "p", (string) null, startIndex, out index);
        if (tagContents != null)
        {
          foreach (KeyValuePair<string, string> keyValuePair in this.ParseAnchorTags(tagContents))
          {
            if (!dictionary1.ContainsKey(keyValuePair.Key))
              dictionary1.Add(keyValuePair.Key, keyValuePair.Value);
          }
          startIndex = index + 1;
        }
        else
          break;
      }
      Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
      foreach (KeyValuePair<string, string> keyValuePair in dictionary1)
      {
        if (keyValuePair.Key.ToLower().StartsWith(name.ToLower()))
          dictionary2.Add(keyValuePair.Key, keyValuePair.Value);
      }
      return dictionary2;
    }

    public Song GetSong(string url)
    {
      string pageHtml = this.GetPageHTML(url);
      string tagContents1 = this.GetTagContents(pageHtml, "title");
      Song song = new Song();
      song.Name = HttpUtility.HtmlDecode(tagContents1);
      string html = this.GetTagContents(pageHtml, "div", "class=\"lyrics\"") ?? this.GetTagContents(pageHtml, "td", "class=\"lyrics\"");
      int index1 = 0;
      byte num = (byte) 1;
      while (true)
      {
        int index2;
        string tagContents2 = this.GetTagContents(html, "p", index1, out index2);
        if (tagContents2 != null)
        {
          song.Verses.Add(new Verse()
          {
            Text = this.CleanUpVerse(tagContents2),
            VerseNumber = num
          });
          ++num;
          index1 = index2 + 1;
        }
        else
          break;
      }
      string tagContents3 = this.GetTagContents(pageHtml, "p", "class=\"chorus\"", 0, out index1);
      if (tagContents3 != null)
      {
        if (tagContents3 == "Refrain")
        {
          int index2;
          tagContents3 = this.GetTagContents(pageHtml, "p", "class=\"chorus\"", index1 + 1, out index2);
        }
        if (tagContents3 != null)
          song.Refrain = this.CleanUpVerse(tagContents3);
      }
      return song;
    }

    private string CleanUpVerse(string text)
    {
      text = text.Replace("<br />", "");
      text = HttpUtility.HtmlDecode(text);
      return text;
    }
  }
}
