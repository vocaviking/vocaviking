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
using Windows.UI.Xaml;
using Windows.Storage;

namespace Settings
{
    public class UserSettings
    {
        ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        public bool FirstRun
        {
            get
            {
                if (settings.Values.ContainsKey("FirstRun"))
                {
                    return (bool)settings.Values["FirstRun"];
                }
                else
                {
                    return true;
                }
            }
            set
            {
                settings.Values["FirstRun"] = value;
            }
        }
        public bool AutoDesign
        {
            get
            {
                if (settings.Values.ContainsKey("AutoDesign"))
                {
                    return (bool)settings.Values["AutoDesign"];
                } else{
                    return true;
                }
            }
            set{
                settings.Values["AutoDesign"] = value;
            }
        }
        public void SetDesign(FrameworkElement Element)
        {
            if (this.AutoDesign)
            {
                Element.RequestedTheme = ElementTheme.Light;
            } else
            {
                Element.RequestedTheme = ElementTheme.Default;
            }
        }
        public UserSettings()
        {
        }
        public string GeneratorMode
        {
            get
            {
                if (settings.Values.ContainsKey("GeneratorMode"))
                {
                    return (string)settings.Values["GeneratorMode"];
                }
                else
                {
                    return "KeEkKh";
                }
            }
            set
            {
                settings.Values["GeneratorMode"] = value;
            }
        }
        public int Number_of_Questions
        {
            get
            {
                if (settings.Values.ContainsKey("Questions"))
                {
                    return (int)settings.Values["Questions"];
                }
                else
                {
                    return 17;
                }
            }
            set
            {
                settings.Values["Questions"] = value;
            }
        }

    }
}