using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using WarSpot.MetroClient.ServiceClient;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using WarSpot.MetroClient.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WarSpot.MetroClient.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReplayPage : Page
    {
        StorageFolder _localFolder;
        public ReplayPage()
        {
            this.InitializeComponent();
            
        }

        List<TurnViewModel> _turns = new List<TurnViewModel>();
        Size _cellSize;
        List<Image> _team0 = new List<Image>();
        List<Image> _team1 = new List<Image>();


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                _localFolder = KnownFolders.DocumentsLibrary;

                MatchReplay replay;

                using (Stream s = await _localFolder.OpenStreamForReadAsync("replay.out"))
                {
                    var serializer = new DataContractSerializer(typeof(MatchReplay));
                    replay = serializer.ReadObject(s) as MatchReplay;
                }

                var worldParams = replay.Events.OfType<SystemEventWorldCreated>().First();

                _turns = MakeTurns(replay.Events);

                _cellSize = DetermineCellSize(SizeRect.ActualWidth, SizeRect.ActualHeight, worldParams.Width, worldParams.Height);
                MainGrid.ColumnDefinitions.Clear();
                for (int i = 0; i < worldParams.Width; i++)
                    MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(_cellSize.Width, GridUnitType.Pixel) });

                MainGrid.RowDefinitions.Clear();
                for (int i = 0; i < worldParams.Width; i++)
                    MainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(_cellSize.Height, GridUnitType.Pixel) });



                for (int x = 0; x < worldParams.Width; x++)
                {
                    for (int y = 0; y < worldParams.Height; y++)
                    {
                        var img = new Image()
                        {
                            Width = _cellSize.Width,
                            Height = _cellSize.Height,
                            Source = new BitmapImage(new Uri("ms-appx:///Assets/grass.png"))
                        };
                        img.SetValue(Grid.RowProperty, y);
                        img.SetValue(Grid.ColumnProperty, x);

                        MainGrid.Children.Add(img);
                    }
                }

                SetTurn(0);
            }
            catch (Exception ex)
            {
            }
        }

        private void SetTurn(int p)
        {
            var turn = _turns[p];

            //team0
            var team0Pos = turn.CreaturePositions[0];
            var replace0 = team0Pos.Take(_team0.Count).Zip(_team0,(x,y)=>new {x,y});
            foreach (var pair in replace0)
            {
                pair.y.SetValue(Grid.ColumnProperty, (int)pair.x.X);
                pair.y.SetValue(Grid.RowProperty, (int)pair.x.Y);
            }
            foreach (var newC in team0Pos.Skip(_team0.Count))
            {
                var img = new Image()
                {
                    Width = _cellSize.Width,
                    Height = _cellSize.Height,
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/creature_1.png"))
                };
                img.SetValue(Grid.ColumnProperty, (int)newC.X);
                img.SetValue(Grid.RowProperty, (int)newC.Y);
                MainGrid.Children.Add(img);
            }
            foreach (var img in _team0.Skip(team0Pos.Count))
            {
                MainGrid.Children.Remove(img);
            }
            _team0 = _team0.Take(team0Pos.Count).ToList();

            //team1
            var team1Pos = turn.CreaturePositions[1];
            var replace1 = team1Pos.Take(_team1.Count).Zip(_team1, (x, y) => new { x, y });
            foreach (var pair in replace1)
            {
                pair.y.SetValue(Grid.ColumnProperty, (int)pair.x.X);
                pair.y.SetValue(Grid.RowProperty, (int)pair.x.Y);
            }
            foreach (var newC in team1Pos.Skip(_team1.Count))
            {
                var img = new Image()
                {
                    Width = _cellSize.Width,
                    Height = _cellSize.Height,
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/creature_2.png"))
                };
                img.SetValue(Grid.ColumnProperty, (int)newC.X);
                img.SetValue(Grid.RowProperty, (int)newC.Y);
                MainGrid.Children.Add(img);
            }
            foreach (var img in _team1.Skip(team1Pos.Count))
            {
                MainGrid.Children.Remove(img);
            }
            _team0 = _team0.Take(team1Pos.Count).ToList();
        }

        private List<TurnViewModel> MakeTurns(System.Collections.ObjectModel.ObservableCollection<WarSpotEvent> observableCollection)
        {
            var result = new List<TurnViewModel>();

            var teams = new Dictionary<Guid, Dictionary<Guid, Point>>();

            foreach (var ev in observableCollection)
            {
                if (ev is GameEventBirth)
                {
                    var birth = (GameEventBirth)ev;
                    var team = birth.Newborn.Team;
                    if(!teams.ContainsKey(team))teams[team] = new Dictionary<Guid,Point>();
                    teams[team][birth.SubjectId] = new Point(birth.Newborn.X, birth.Newborn.Y);

                }
                else if(ev is GameEventMove)
                {
                    var gem = (GameEventMove)ev;
                    foreach (var pair in teams)
                    {
                        if (pair.Value.ContainsKey(gem.SubjectId))
                        {
                            var pt = pair.Value[gem.SubjectId];
                            pt.X += gem.ShiftX;
                            pt.Y += gem.ShiftY;
                            pair.Value[gem.SubjectId] = pt;
                            break;
                        }
                    }
                }
                else if (ev is GameEventDeath)
                {
                    var ged = (GameEventDeath)ev;
                    foreach (var pair in teams)
                    {
                        if (pair.Value.ContainsKey(ged.SubjectId))
                        {
                            pair.Value.Remove(ged.SubjectId);
                        }
                    }
                }
                else if (ev is SystemEventTurnStarted || ev is SystemEventMatchEnd)
                {
                    if (teams.Any())
                    {
                        var turn = new TurnViewModel();
                        turn.CreaturePositions = teams.ToDictionary(x => teams.Keys.ToList().IndexOf(x.Key), x => x.Value.Values.ToList());
                        result.Add(turn);
                    }
                }

            }

            return result;
        }

        private Size DetermineCellSize(double w, double h, int columns, int rows)
        {
            var dim = Math.Min((int)(w / columns), (int)(h / rows));
            return new Size(dim, dim);
        }
    }
}
