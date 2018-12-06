using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using System.Net.Http;
using Microsoft.Graph;
using System.IdentityModel.Tokens;

namespace PresentationBuilder.Win
{
  public class OneDriveReader
  {
    public void Initialize()
    {
      var authorization = new AuthenticationProvider();

      var oneDriveClient = new OneDriveClient(
           "https://api.onedrive.com/v1.0",
           authorization);
      //, this.httpProvider.Object);
    }
  }

  class AuthenticationProvider : IAuthenticationProvider
  {

    public async Task AuthenticateRequestAsync(HttpRequestMessage request)
    {
      var authContext = new AuthenticationContext(); //sAzureAuthUrl + _tenantname);
      var creds = new ClientCredential(_clientid, _clientsecret);

      AuthenticationResult authResult = await authContext.AcquireTokenAsync(sAzureGraphUrl, creds);
      request.Headers.Add("Authorization", "Bearer " + authResult.AccessToken);
      //TODO check token validity
    }
  }



}
