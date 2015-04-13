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
using System.Collections.ObjectModel;
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

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkID=390556 dokumentiert.

namespace VocabularyTrainer
{
    #region StyleSelector
    public class TestSelector : StyleSelector
    {
        private Style questionStyle;
        private Style resultStyle;
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            Style back = null;
            if (item is QuestionClass)
            {
                back = questionStyle;
            } else if (item is ResultClass)
            {
                back = ResultStyle;
            }
            return back;
        }
        public Style QuestionStyle
        {
            get
            {
                return this.questionStyle;
            }
            set
            {
                this.questionStyle = value;
            }
        }
        public Style ResultStyle
        {
            get
            {
                return this.resultStyle;
            }
            set
            {
                this.resultStyle = value;
            }
        }
    }
    #endregion
    #region Converters
    public class ButtonForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            QuestionClass Question = value as QuestionClass;
            string name = parameter as string;
            if (Question == null)
            {
                return null;
            }
            string back = "Black";
            if (Question.Answered)
            {
                if (name == Question.RightAnswer)
                {
                    back = "White";
                }
                if (name == Question.UserAnswer)
                {
                    back = "White";
                }
            }
            return back;
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ButtonBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            QuestionClass Question = value as QuestionClass;
            string name = parameter as string;
            if (Question == null)
            {
                return null;
            }

            string back = "White";
            if (Question.Answered)
            {
                if (name == Question.RightAnswer)
                {
                    back = "Green";
                }
                if (name == Question.UserAnswer)
                {
                    if (Question.AnsweredRight == "Wrong")
                    {
                        back = "Crimson";
                    }
                    else if (Question.AnsweredRight == "Unknown")
                    {
                        back = "Orange";
                    }
                }

            }
            return back;
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class FlipBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            QuestionClass Question = value as QuestionClass;
            string name = parameter as string;
            if (Question == null)
            {
                return null;
            }
            string back = "DarkGray";
            if (Question.Answered)
            {
                switch (Question.AnsweredRight)
                {
                    case "Right":
                        back = "Green";
                        break;
                    case "Wrong":
                        back = "Crimson";
                        break;
                    case "Unknown":
                        back = "Orange";
                        break;
                }
            }
            return back;
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class FlipBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            QuestionClass Question = value as QuestionClass;
            string name = parameter as string;
            if (Question == null)
            {
                return null;
            }
            string back = "Black";
            if (Question.Answered)
            {
                switch (Question.AnsweredRight)
                {
                    case "Right":
                        back = "Green";
                        break;
                    case "Wrong":
                        back = "Crimson";
                        break;
                    case "Unknown":
                        back = "Orange";
                        break;
                }
            }
            return back;
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class BorderBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            QuestionClass Question = value as QuestionClass;
            string name = parameter as string;
            if (Question==null)
            {
                return null;
            }

            string back = "Black";
            if (Question.Answered)
            {
                switch (Question.AnsweredRight)
                {
                    case "Right":
                        back = "Green";
                        break;
                    case "Wrong":
                        back = "Crimson";
                        break;
                    case "Unknown":
                        back = "Orange";
                        break;
                }
            }
            return back;
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class RightToImg : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            double rightanswerspercent = (double)value;
            string source = "/assets/1F62C.png";
            //NaN prevention?
            //Set Emoji
            if ((0.00 <= rightanswerspercent) && (rightanswerspercent <= 0.10))
            {
                source = "/assets/1F635.png";
            }
            else if ((0.10 < rightanswerspercent) && (rightanswerspercent <= 0.20))
            {
                source = "/assets/1F62D.png";
            }
            else if ((0.20 < rightanswerspercent) && (rightanswerspercent <= 0.30))
            {
                source = "/assets/1F614.png";
            }
            else if ((0.30 < rightanswerspercent) && (rightanswerspercent <= 0.40))
            {
                source = "/assets/1F628.png";
            }
            else if ((0.40 < rightanswerspercent) && (rightanswerspercent <= 0.50))
            {
                source = "/assets/1F615.png";
            }
            else if ((0.50 < rightanswerspercent) && (rightanswerspercent <= 0.60))
            {
                source = "/assets/1F62C.png";
            }
            else if ((0.60 < rightanswerspercent) && (rightanswerspercent <= 0.70))
            {
                source = "/assets/1F60F.png";
            }
            else if ((0.70 < rightanswerspercent) && (rightanswerspercent <= 0.80))
            {
                source = "/assets/1F604.png";
            }
            else if ((0.80 < rightanswerspercent) && (rightanswerspercent <= 0.90))
            {
                source = "/assets/1F60A.png";
            }
            else if ((0.90 < rightanswerspercent) && (rightanswerspercent <= 1.00))
            {
                source = "/assets/1F60D.png";
            }
            return source;
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            double percentage = (double)value;
            string format = parameter as string;

            return percentage.ToString(format);
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class MultiplyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, string language)
        {
            double a = (double)value;
            double b = System.Convert.ToDouble(parameter);

            return a * b;
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    #endregion


    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class QuizTest : Page
    {
        QuizClass quizzer;
        ResultClass results;
        private Settings.UserSettings UserSettings = new Settings.UserSettings();
        public QuizTest()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            UserSettings.SetDesign(this);
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
        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void Restart_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Frame angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            quizzer = e.Parameter as QuizClass;

            if (quizzer == null)
            {
                throw new NotImplementedException();
            }
            //Update Questions
            for (int i = 0; i < quizzer.questions.Count;i++ )
            {
                QuestionFlip.Items.Add(quizzer.questions[i]);
                QuestionMarker.Items.Add(quizzer.questions[i]);
                //QuestionFlip.Items.Add(quizzer.questions[i]);
                //QuestionMarker.Items.Add(quizzer.questions[i]);

            }
            //QuestionFlip.ItemsSource = Questions;
            //QuestionMarker.ItemsSource = Questions;
            //QuestionFlip.DataContext = Questions;
            //QuestionMarker.DataContext = Questions; 
            
            results = new ResultClass();
            QuestionFlip.Items.Add(results);
            QuestionMarker.Items.Add(results);

            
            QuestionMarker.SelectionChanged += ContextControl_SelectionChanged;

            UserSettings.SetDesign(this);
        }

        void ContextControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Make sure that the navigation buttons are updated by forcing focus to the FlipView
            QuestionFlip.Focus(Windows.UI.Xaml.FocusState.Pointer);
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            Button Answer = sender as Button;
            if (Answer != null)
            {
                //Saving Current Positions
                int index = QuestionFlip.SelectedIndex;
                int last = QuestionFlip.Items.Count-1;

                
                QuestionClass CurrentQuestion = QuestionFlip.Items[index] as QuestionClass;
                
                //Fuck you Microsoft! Why doesn't it update any other way?
                if (CurrentQuestion.Answered==false)
                {
                    CurrentQuestion.UserAnswer = Answer.Tag.ToString();

                    QuestionFlip.Items[index] = CurrentQuestion;
                    QuestionMarker.Items[index] = CurrentQuestion;
                    quizzer.questions[index] = CurrentQuestion;

                    //Update Results
                    results = QuestionFlip.Items[last] as ResultClass;
                    switch (CurrentQuestion.AnsweredRight)
                    {
                        case "Right":
                            results.Right++;
                            break;
                        case "Wrong":
                            results.Wrong++;
                            break;
                        case "Unknown":
                            results.Unknown++;
                            break;
                    }
                    QuestionFlip.Items[last] = results;
                    QuestionMarker.Items[last] = results;

                    //Readd Question?
                    if ((CurrentQuestion.AnsweredRight=="Unknown") && (quizzer.CanAdd))
                    {
                        QuestionClass NewQuestion = new QuestionClass(CurrentQuestion);
                        quizzer.questions.Add(NewQuestion);
                        QuestionFlip.Items.Insert(last, NewQuestion);
                        QuestionMarker.Items.Insert(last, NewQuestion);
                    }

                    //If we don't do this the selection jumps to the first item, because the items list changed.
                    QuestionFlip.SelectedIndex = index;
                    //QuestionMarker.SelectedIndex = index;
                }
            }
        }
    }
}
