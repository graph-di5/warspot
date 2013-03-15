using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

namespace WarSpot.MetroClient.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReplaysPage : Page
    {
        ObservableCollection<ReplayDescription> _replays;

        public ReplaysPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var loc = new ServiceLocator();
            Uname.Text+= loc.Username;

            var client = loc.ServiceClient;
            var replays = (await client.GetListOfReplaysAsync())??new ObservableCollection<ReplayDescription>();
            
            Progress.IsActive = false;
            
            foreach (var item in replays)
            {
                Reps.Items.Add(new ListBoxItem()
                {
                    Content = item.Name
                });
            }
            _replays = replays;
            Reps.IsEnabled = true;
        }

        private async void  Reps_DoubleTapped_1(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (Reps.SelectedItem != null)
            {
                var name = ((ListBoxItem)Reps.SelectedItem).Content as string;
                if (name != null)
                {
                    var rep = _replays.First(x => x.Name == name);
                    var loc = new ServiceLocator();
                    var client = loc.ServiceClient;
                    Progress.IsActive = true;
                    var result = await client.DownloadReplayAsync(rep.ID);
                    Progress.IsActive = false;
                    loc.Rep = result;
                    loc.RepDesc = rep;
                    var frame = Window.Current.Content as Frame;
                    
                    frame.Navigate(typeof(ReplayPage));
                }
            }
        }
    }
}
