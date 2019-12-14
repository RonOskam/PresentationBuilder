using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PresentationBuilder.BLL.PowerPoint
{

  public class FileSlide : SlideItem
  {
    private string _fileName;

    public FileSlide(string fileName)
    {
      _fileName = fileName;
    }

    internal override void Generate(Presentation presentation)
    {
      if (_fileName.StartsWith(@"\\"))
      {
        var newFile = $"{ Path.GetTempPath() }TempPowerPoint.pptx";
        if (File.Exists(newFile))
          File.Delete(newFile);

        File.Copy(_fileName, newFile);

        _fileName = newFile;
      }

      try
      {
        presentation.Slides.InsertFromFile(_fileName, presentation.Slides.Count);
      }
      catch (Exception e)
      {
        throw new Exception($"Error inserting file { _fileName }: { e.Message }");
      }
    }

    public override string Validate()
    {
      if (string.IsNullOrWhiteSpace(_fileName))
        return "A file must be selected first.";
      else
        return base.Validate();
    }
  }
}
