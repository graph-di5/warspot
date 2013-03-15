using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WarSpot.MetroClient.Common;
using WarSpot.MetroClient.Pages;
using WarSpot.MetroClient.ServiceClient;
using WarSpot.MetroClient.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Warspot.MetroClient.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        ServiceLocator _locator = new ServiceLocator();
        public LoginPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void Login_Click_1(object sender, RoutedEventArgs e)
        {
            Error.Text = "";
            Waiter.IsActive = true;
            var client = _locator.ServiceClient;
            try
            {
                var loginTask = client.LoginAsync(Login.Text, HashHelper.GetMd5Hash(Password.Password));
                var loginResult = await loginTask;
                if (loginResult.Type != ErrorType.Ok)
                {
                    Error.Text = loginResult.Message;
                }
                else
                {
                    var frame = Window.Current.Content as Frame;
                    _locator.Username = Login.Text;
                    frame.Navigate(typeof(ReplaysPage));
                }
            }
            catch (Exception ex)
            {
                Error.Text = "Login failed";
            }
            finally
            {
                Waiter.IsActive = false;
            }
        }

        private async void RegisterButton_Click_1(object sender, RoutedEventArgs e)
        {
            Error.Text = "";
            Waiter.IsActive = true;
            var client = _locator.ServiceClient;
            try
            {
                var loginTask = client.RegisterAsync(Login.Text, HashHelper.GetMd5Hash(Password.Password));
                var loginResult = await loginTask;
                if (loginResult.Type != ErrorType.Ok)
                {
                    Error.Text = loginResult.Message;
                }
                else
                {
                    var frame = Window.Current.Content as Frame;
                    _locator.Username = Login.Text;
                    frame.Navigate(typeof(ReplaysPage));
                }
            }
            catch (Exception ex)
            {
                Error.Text = "Register failed";
            }
            finally
            {
                Waiter.IsActive = false;
            }
        }
    }
}
