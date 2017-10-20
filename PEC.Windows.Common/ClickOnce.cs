// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.ClickOnce
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Deployment.Application;
using System.Web;
using System.Windows.Forms;

namespace PEC.Windows.Common
{
  public class ClickOnce
  {
    public static string GetQueryString()
    {
      string str = "";
      if (ApplicationDeployment.IsNetworkDeployed)
      {
        try
        {
          if (ApplicationDeployment.CurrentDeployment.ActivationUri != (Uri) null)
            str = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("GetQueryString exception: " + ex.Message);
        }
      }
      return str;
    }

    public static NameValueCollection GetQueryStringParameters()
    {
      return ClickOnce.GetQueryStringParameters(ClickOnce.GetQueryString());
    }

    public static NameValueCollection GetQueryStringParameters(string queryString)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      if (queryString != null)
        nameValueCollection = HttpUtility.ParseQueryString(queryString);
      return nameValueCollection;
    }

    public static void InstallUpdateSyncWithInfo()
    {
      if (!ApplicationDeployment.IsNetworkDeployed)
        return;
      ApplicationDeployment currentDeployment = ApplicationDeployment.CurrentDeployment;
      UpdateCheckInfo updateCheckInfo;
      try
      {
        updateCheckInfo = currentDeployment.CheckForDetailedUpdate();
      }
      catch (DeploymentDownloadException ex)
      {
        int num = (int) MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + ex.Message);
        return;
      }
      catch (InvalidDeploymentException ex)
      {
        int num = (int) MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ex.Message);
        return;
      }
      catch (InvalidOperationException ex)
      {
        int num = (int) MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ex.Message);
        return;
      }
      if (updateCheckInfo.UpdateAvailable)
      {
        bool flag = true;
        if (!updateCheckInfo.IsUpdateRequired)
        {
          if (DialogResult.OK != MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel))
            flag = false;
        }
        else
        {
          int num1 = (int) MessageBox.Show("This application has detected a mandatory update from your current version to version " + updateCheckInfo.MinimumRequiredVersion.ToString() + ". The application will now install the update and restart.", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        if (flag)
        {
          try
          {
            currentDeployment.Update();
            int num2 = (int) MessageBox.Show("The application has been upgraded, and will now restart.");
            Application.Restart();
          }
          catch (DeploymentDownloadException ex)
          {
            int num2 = (int) MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + (object) ex);
          }
        }
      }
    }

    public static bool IsUpdateAvailable()
    {
      return ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.CheckForUpdate();
    }

    public static void CheckForUpdate()
    {
      if (ApplicationDeployment.IsNetworkDeployed)
      {
        ApplicationDeployment deployment = ApplicationDeployment.CurrentDeployment;
        if (deployment.CheckForUpdate())
        {
          if (MessageBox.Show("A new version of the application is available, do you want to update? You can continue to work while the update is installed.", "Application Updater", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
          BackgroundWorker backgroundWorker = new BackgroundWorker();
          backgroundWorker.DoWork += (DoWorkEventHandler) ((s, e) => e.Result = deployment.Update());
          backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((s, e) =>
          {
            if ((bool) e.Result)
            {
              if (MessageBox.Show("Update complete, do you want to restart the application to apply the update?", "Application Updater", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
              Application.Restart();
            }
            else
            {
              int num1 = (int) MessageBox.Show("Update failed. Please retry at another time.");
            }
          });
          backgroundWorker.RunWorkerAsync();
        }
        else
        {
          int num1 = (int) MessageBox.Show("No updates available.", "Application Updater");
        }
      }
      else
      {
        int num3 = (int) MessageBox.Show("Updates not allowed unless you are launched through ClickOnce.");
      }
    }
  }
}
