using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace PresentationBuilder.BLL
{
  public class PassageReader
  {
    public bool IsPassageValid(string verses)
    {
      var passage = GetBiblePassage(verses);
      return !string.IsNullOrWhiteSpace(passage);
    }

    public string GetBiblePassage(string verses)
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("https://api.esv.org/v3/passage/text/");
      //stringBuilder1.Append("?q=" + HttpUtility.UrlEncode(verses).ToString());
      stringBuilder1.Append("?q=" + verses);
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

      string url = stringBuilder1.ToString();

      WebRequest req = WebRequest.Create(url);
      req.Method = "GET";
      req.Headers.Add("Authorization", "Token 613b6c5e96253df981a14712b502f5e9a0141069");
      req.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;
      var response = req.GetResponse();

      StreamReader streamReader = new StreamReader(response.GetResponseStream());
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.Append(streamReader.ReadToEnd());
      streamReader.Close();
      string returnedJson = stringBuilder2.ToString();

      JObject details = JObject.Parse(returnedJson);
      var result = JsonConvert.DeserializeObject<List<string>>(details["passages"].ToString());
      
      return string.Join(" ", result).Trim();
    }

    public string GetBiblePassageOLD(string verses)
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
