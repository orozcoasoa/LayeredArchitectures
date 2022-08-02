using CatalogService.BLL.Entities;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace CatalogClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string AadInstance = App.Config["ida:AADInstance"];
        private static readonly string Tenant = App.Config["ida:Tenant"];
        private static readonly string ClientId = App.Config["ida:ClientId"];

        private static readonly string Authority = string.Format(CultureInfo.InvariantCulture, AadInstance, Tenant);

        private static readonly string CatalogScope = App.Config["cat:CatalogScope"];
        private static readonly string CatalogBaseAddress = App.Config["cat:CatalogBaseAddress"];

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly IPublicClientApplication _app;

        private static readonly string[] Scopes = { CatalogScope };
        private static string CatalogApiAddress
        {
            get
            {
                string baseAddress = CatalogBaseAddress;
                return baseAddress.EndsWith("/") ? CatalogBaseAddress + "api/items"
                                                 : CatalogBaseAddress + "/api/items";
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            UserName.Content = "User not idendified.";
            _app = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority(Authority)
                .WithDefaultRedirectUri()
                .Build();
            LoadItems();
        }

        private void LoadItems()
        {
            LoadItemsAsync().ConfigureAwait(false);
        }

        private async Task LoadItemsAsync(int categoryId = 4)
        {
            var baseUri = new UriBuilder(CatalogApiAddress);
            baseUri.Query = "page=1&limit=20&categoryid=" + categoryId;
            var response = await _httpClient.GetAsync(baseUri.Uri);
            if (response.IsSuccessStatusCode)
            {
                var itemTxt = await response.Content.ReadAsStringAsync();
                var items = JsonSerializer.Deserialize<List<Item>>(itemTxt, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Dispatcher.Invoke(() =>
                {
                    ItemsList.ItemsSource = items.ToList();
                });
            }
            else
            {
                MessageBox.Show(response.ReasonPhrase);
            }
        }

        private async void CreateItemButton_Click(object sender, RoutedEventArgs e)
        {
            var accounts = (await _app.GetAccountsAsync()).ToList();

            if (!accounts.Any())
            {
                MessageBox.Show("Please sign in first");
                return;
            }
            if (string.IsNullOrEmpty(ItemName.Text) || string.IsNullOrEmpty(ItemDescription.Text) ||
                string.IsNullOrEmpty(ItemAmount.Text) || string.IsNullOrEmpty(ItemPrice.Text) ||
                string.IsNullOrEmpty(ItemCategoryId.Text))
            {
                MessageBox.Show("Invalid item.");
                return;
            }

            // Get an access token to call the Catalog service.
            AuthenticationResult result = null;
            try
            {
                result = await _app.AcquireTokenSilent(Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();
                Dispatcher.Invoke(() =>
                {
                    UserName.Content = result.Account.Username;
                });
            }
            // There is no access token in the cache, so prompt the user to sign-in.
            catch (MsalUiRequiredException)
            {
                MessageBox.Show("Please re-sign");
                SignInButton.IsEnabled = true;
            }
            catch (MsalException ex)
            {
                // An unexpected error occurred.
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                }

                Dispatcher.Invoke(() =>
                {
                    UserName.Content = "";
                    SignInButton.IsEnabled = true;
                    MessageBox.Show("Unexpected error: " + message);
                });


                return;
            }

            // Call the Catalog service.
            // Once the token has been returned by MSAL, add it to the http authorization header, before making the call to access the Catalog service.
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            // Forms encode Catalog item, to POST to the catalog item list web api.
            var itemToAdd = new ItemDTO()
            {
                Name = ItemName.Text,
                Description = ItemDescription.Text,
                Image = "",
                CategoryId = int.Parse(ItemCategoryId.Text),
                Price = decimal.Parse(ItemPrice.Text),
                Amount = int.Parse(ItemAmount.Text)
            };
            string json = JsonSerializer.Serialize(itemToAdd);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // Call the catalog item list service.

            HttpResponseMessage response = await _httpClient.PostAsync(CatalogApiAddress, content);

            if (response.IsSuccessStatusCode)
            {
                ItemName.Text = "";
                ItemDescription.Text = "";
                ItemCategoryId.Text = "";
                ItemPrice.Text = "";
                ItemAmount.Text = "";
                await LoadItemsAsync(itemToAdd.CategoryId);
            }
            else
            {

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden && response.Headers.WwwAuthenticate.Any())
                {
                    //Requires to refresh token
                    MessageBox.Show(response.ReasonPhrase, "Refresh");
                }
                else
                {
                    MessageBox.Show(response.ReasonPhrase);
                }
            }

        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            var accounts = (await _app.GetAccountsAsync()).ToList();
            // If there is already a token in the cache, clear the cache 
            while (accounts.Any())
            {
                await _app.RemoveAsync(accounts.First());
                accounts = (await _app.GetAccountsAsync()).ToList();
            }
            UserName.Content = "";

            // Get an access token to call the Catalog service.
            try
            {
                var result = await _app.AcquireTokenSilent(Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();

                Dispatcher.Invoke(() =>
                {
                    UserName.Content = result.Account.Username;
                    SignInButton.IsEnabled = false;
                });
            }
            catch (MsalUiRequiredException)
            {
                try
                {
                    // Force a sign-in (Prompt.SelectAccount), as the MSAL web browser might contain cookies for the current user
                    // and we don't necessarily want to re-sign-in the same user
                    var result = await _app.AcquireTokenInteractive(Scopes)
                        .WithAccount(accounts.FirstOrDefault())
                        .WithPrompt(Prompt.SelectAccount)
                        .ExecuteAsync()
                        .ConfigureAwait(false);

                    Dispatcher.Invoke(() =>
                    {
                        UserName.Content = result.Account.Username;
                        SignInButton.IsEnabled = false;
                    });
                }
                catch (MsalException ex)
                {
                    if (ex.ErrorCode == "access_denied")
                    {
                        // The user canceled sign in, take no action.
                    }
                    else
                    {
                        // An unexpected error occurred.
                        string message = ex.Message;
                        if (ex.InnerException != null)
                        {
                            message += "Error Code: " + ex.ErrorCode + "Inner Exception : " + ex.InnerException.Message;
                        }

                        MessageBox.Show(message);
                    }

                    Dispatcher.Invoke(() =>
                    {
                        UserName.Content = "";
                        SignInButton.IsEnabled = true;
                    });
                }
            }
        }
    }
}
