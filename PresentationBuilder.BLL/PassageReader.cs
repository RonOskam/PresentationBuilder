﻿using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace PresentationBuilder.BLL
{
  public class PassageReader
  {
    public bool IsPassageValid(string verses)
    {
      return !this.GetBiblePassage(verses).Contains("No passage found for your query");
    }

    public string GetBiblePassage(string verses)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("http://www.esvapi.org/v2/rest/passageQuery");
      stringBuilder1.Append("?key=236c882c31dd72f9");
      stringBuilder1.Append("&passage=" + HttpUtility.UrlEncode(verses).ToString());
      stringBuilder1.Append("&include-passage-references=false");
      stringBuilder1.Append("&include -first-verse-numbers=false");
      stringBuilder1.Append("&include-verse-numbers=false");
      stringBuilder1.Append("&include-footnotes=false");
      stringBuilder1.Append("&include-short-copyright=false");
      stringBuilder1.Append("&include-passage-horizontal-lines=false");
      stringBuilder1.Append("&include-heading-horizontal-lines=false");
      stringBuilder1.Append("&include-headings=false");
      stringBuilder1.Append("&include-subheadings=false");
      stringBuilder1.Append("&include-selahs=false");
      stringBuilder1.Append("&include-headings=false");
      stringBuilder1.Append("&output-format=plain-text");
      StreamReader streamReader = new StreamReader(WebRequest.Create(stringBuilder1.ToString()).GetResponse().GetResponseStream());
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.Append(streamReader.ReadToEnd());
      streamReader.Close();
      string str = stringBuilder2.ToString();
      if (str.IndexOf(']') > 0)
        str = str.Substring(str.IndexOf(']') + 1);
      return str + "\r\n\r\n" + verses;
    }
  }
}
