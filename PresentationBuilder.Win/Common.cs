using PresentationBuilder.BLL;

namespace PowerPointBuilder
{
  public class Common
  {
    private static PresentationBuilderEntities _dataSource = (PresentationBuilderEntities) null;

    public static PresentationBuilderEntities DataSource
    {
      get
      {
        if (Common._dataSource == null)
          Common._dataSource = new PresentationBuilderEntities();
        return Common._dataSource;
      }
    }
  }
}
