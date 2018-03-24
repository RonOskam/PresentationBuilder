//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens;
//using System.Linq;
//using System.Net.Http;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Identity.Client;

//namespace PresentationBuilder.BLL
//{
//  public class OneDriveReader
//  {
//    private static string ClientId = "";

//    public static PublicClientApplication PublicClientApp = new PublicClientApplication(ClientId);
//    string _graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";

//    //Set the scope for API call to user.read
//    string[] _scopes = new string[] { "user.read" };


//    public async Task<string> Connect(string certFile)
//    {
//      string authority = appConfig.AuthorizationUri.Replace("common", tenantId);
//      var authenticationContext = new AuthenticationContext(authority, false);
      
  
//      var cert = new X509Certificate(certFile, "", X509KeyStorageFlags.MachineKeySet);
//      var cac = new ClientAssertionCertificate(appConfig.ClientId, cert);
//      var authenticationResult = await authenticationContext.AcquireTokenAsync(resource, cac);
//      return authenticationResult.AccessToken;
//    }

//    public async Task<string> GetHttpContentWithToken(string url, string token)
//    {
//      var httpClient = new HttpClient();
//      HttpResponseMessage response;
//      try
//      {
//        var request = new HttpRequestMessage(HttpMethod.Get, url);
//        //Add the token in Authorization header
//        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
//        response = await httpClient.SendAsync(request);
//        var content = await response.Content.ReadAsStringAsync();
//        return content;
//      }
//      catch (Exception ex)
//      {
//        return ex.ToString();
//      }
//    }

//  }
//}
