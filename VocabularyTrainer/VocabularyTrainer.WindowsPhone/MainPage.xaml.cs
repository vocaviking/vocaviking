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
// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.



namespace VocabularyTrainer
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Frames navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Vocabulary.VocabList vlist;
        private Settings.UserSettings UserSettings = new Settings.UserSettings();
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Rahmen angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Seite vorbereiten, um sie hier anzuzeigen.

            // TODO: Wenn Ihre Anwendung mehrere Seiten enthält, stellen Sie sicher, dass
            // die Hardware-Zurück-Taste behandelt wird, indem Sie das
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed-Ereignis registrieren.
            // Wenn Sie den NavigationHelper verwenden, der bei einigen Vorlagen zur Verfügung steht,
            // wird dieses Ereignis für Sie behandelt.
            UserSettings.SetDesign(this);
        }
        private async void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            List<QuestionClass> questions = new List<QuestionClass>();
            //questions.Add(new QuestionClass("How are you?", "Well", "Sick", "Sad", "Beautiful", 4));
            await Loader();

            int qn = UserSettings.Number_of_Questions;
            QuizClass quizzer = new QuizClass();
            quizzer.SetQuestions(vlist.GenerateQuestions(qn,GetGeneratorMode()));
            quizzer.CanRestart = true;
            quizzer.MaxCount = (int)((double)qn*1.5);
            this.Frame.Navigate(typeof(QuizTest), quizzer);
        }
        private async Task Loader()
        {
            /*StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder subfolder = await folder.GetFolderAsync("Assets");
            //StorageFile file = await subfolder.GetFileAsync("questions.xml");
            //qlist = await XmlReadWriteUniversal.XmlIO.ReadObjectFromXmlPathAsync<QuestionList>(file);

            StorageFile vfile = await subfolder.GetFileAsync("vocab.xml");
            vlist = await XmlReadWriteUniversal.XmlIO.ReadObjectFromXmlPathAsync<Vocabulary.VocabList>(vfile);*/

            if (UserSettings.FirstRun)
            {
                await ResetStats();
                UserSettings.FirstRun = false;
            }
            vlist = await XmlReadWriteUniversal.XmlIO.ReadObjectFromXmlFileAsync<Vocabulary.VocabList>("vocab.xml");
            //No Errors! :(
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //UserSettings.SetDesign(this);
            SetGeneratorMode(UserSettings.GeneratorMode);
            QuestionNumber.Value = (double)UserSettings.Number_of_Questions;
            QuestionNumber.ValueChanged += QuestionNumber_ValueChanged;
        }

        private string GetGeneratorMode()
        {
            string GeneratorMode = "";
            if (CheckBoxKanaEnglish.IsChecked.Value)
            {
                GeneratorMode += "Ke";
            }
            if (CheckBoxEnglishKana.IsChecked.Value)
            {
                GeneratorMode += "Ek";
            }
            if (CheckBoxKanaHiragana.IsChecked.Value)
            {
                GeneratorMode += "Kh";
            }
            if (CheckBoxHiraganaKana.IsChecked.Value)
            {
                GeneratorMode += "Hk";
            }
            if (CheckBoxHiraganaEnglish.IsChecked.Value)
            {
                GeneratorMode += "He";
            }
            if (CheckBoxEnglishHiragana.IsChecked.Value)
            {
                GeneratorMode += "Eh";
            }
            return GeneratorMode;
        }
        private void SetGeneratorMode(string GeneratorMode)
        {
            if (GeneratorMode.Contains("Ke"))
            {
                CheckBoxKanaEnglish.IsChecked = true;
            }
            else
            {
                CheckBoxKanaEnglish.IsChecked = false;
            }
            if (GeneratorMode.Contains("Ek"))
            {
                CheckBoxEnglishKana.IsChecked = true;
            }
            else
            {
                CheckBoxEnglishKana.IsChecked = false;
            }
            if (GeneratorMode.Contains("Kh"))
            {
                CheckBoxKanaHiragana.IsChecked = true;
            }
            else
            {
                CheckBoxKanaHiragana.IsChecked = false;
            }
            if (GeneratorMode.Contains("Hk"))
            {
                CheckBoxHiraganaKana.IsChecked = true;
            }
            else
            {
                CheckBoxHiraganaKana.IsChecked = false;
            }
            if (GeneratorMode.Contains("He"))
            {
                CheckBoxHiraganaEnglish.IsChecked = true;
            }
            else
            {
                CheckBoxHiraganaEnglish.IsChecked = false;
            }
            if (GeneratorMode.Contains("Eh"))
            {
                CheckBoxEnglishHiragana.IsChecked = true;
            }
            else
            {
                CheckBoxEnglishHiragana.IsChecked = false;
            }
        }

        private void CheckBox_Change(object sender, RoutedEventArgs e)
        {
            UserSettings.GeneratorMode = GetGeneratorMode();
        }

        private void QuestionNumber_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            
            var slider = sender as Slider;
            if (slider != null)
            {
                UserSettings.Number_of_Questions = (int)slider.Value;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            List<QuestionClass> questions = new List<QuestionClass>();
            await Loader();

            int qn = UserSettings.Number_of_Questions;
            QuizClass quizzer = new QuizClass();
            quizzer.SetQuestions(vlist.GenerateQuestions(qn, GetGeneratorMode()));
            quizzer.CanRestart = true;
            quizzer.MaxCount = (int)((double)qn * 1.5);
            this.Frame.Navigate(typeof(QuizTest), quizzer);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton Button = sender as AppBarButton;
            if (Button!=null)
            {
                switch(Button.Tag.ToString())
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
