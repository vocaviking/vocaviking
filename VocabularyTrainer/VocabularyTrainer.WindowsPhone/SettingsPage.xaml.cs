//  Copyright 2015 Jan Sende
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Storage;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkID=390556 dokumentiert.

namespace VocabularyTrainer
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private Settings.UserSettings UserSettings = new Settings.UserSettings();
        public SettingsPage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            
        }
        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }
        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Frame angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ToggleDesign.IsOn = !UserSettings.AutoDesign;
            UserSettings.SetDesign(this);
        }
        private void ToggleDesign_Toggled(object sender, RoutedEventArgs e)
        {
            UserSettings.AutoDesign = !ToggleDesign.IsOn;
            UserSettings.SetDesign(this);
        }
        private async Task ResetStats()
        {
            StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder subfolder = await folder.GetFolderAsync("Assets");
            StorageFile vfile = await subfolder.GetFileAsync("vocab.xml");
            Vocabulary.VocabList temp = await XmlReadWriteUniversal.XmlIO.ReadObjectFromXmlPathAsync<Vocabulary.VocabList>(vfile);
            await XmlReadWriteUniversal.XmlIO.SaveObjectToXml<Vocabulary.VocabList>(temp, "vocab.xml");

            Windows.UI.Popups.MessageDialog msgbox = new Windows.UI.Popups.MessageDialog("Stats succesfully reset.");
            //Calling the Show method of MessageDialog class  
            //which will show the MessageBox  
            await msgbox.ShowAsync();
        }
        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            await ResetStats();
        }
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton Button = sender as AppBarButton;
            if (Button != null)
            {
                switch (Button.Tag.ToString())
                {
                    case "Start":
                        this.Frame.Navigate(typeof(MainPage));
                        break;
                    case "Select":
                        break;
                    case "Settings":
                        this.Frame.Navigate(typeof(SettingsPage));
                        break;
                    case "Bug":
                        break;
                }
            }
        }
    }
}
