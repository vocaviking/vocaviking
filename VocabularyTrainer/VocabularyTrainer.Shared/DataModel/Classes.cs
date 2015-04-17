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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using Windows.Storage;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Vocabulary
{
    public class Vocab
    {
        public int ID { get; set; }
        public string Kana { get; set; }
        public string OnYomi { get; set; }

        public string KunYomi { get; set; }
        public string Wortart { get; set; }
        public string Stichworte { get; set; }
        public string Englisch { get; set; }
        public string Woerterbuch { get; set; }
        public int richtig { get; set; }
        public int falsch { get; set; }
        public int unbekannt { get; set; }
        public int Straehne { get; set; }
        public string Art { get; set; }

        public Vocab()
        {
            ID = 0;
            Kana = "";
            OnYomi = "";
            KunYomi = "";
            Wortart = "";
            Stichworte = "";
            Englisch = "";
            Woerterbuch = "";
        }
    }
    [XmlRoot("dataroot")]
    public class VocabList
    {
        [XmlIgnore]
        public Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        [XmlElement("Vokabel")]
        public List<Vocab> Items;
        public VocabList()
        {
            Items = new List<Vocab>();
        }
        public VocabList(List<Vocab> bla)
        {
            Items = new List<Vocab>(bla);
        }
        public List<VocabularyTrainer.QuestionClass> GenerateQuestions(int number, string GeneratorMode = "")
        {
            List<VocabularyTrainer.QuestionClass> questions = new List<VocabularyTrainer.QuestionClass>();

            for (int i = 0; i < number;i++)
            {
                List<int> rlist = new List<int>();
                //Set 1st Element + Mode
                string mode = "";
                while (rlist.Count<1)
                {
                    int j = rnd.Next(0, Items.Count);
                    bool usej = false;
                    if ((GeneratorMode.Contains("Ke") || GeneratorMode.Contains("Ek")) && (Items[j].Kana != "") && (Items[j].Englisch != ""))
                    {
                        usej = true;
                        mode += "KeEk";
                    }
                    //On-Yomi!!
                    if ((GeneratorMode.Contains("Kh") || GeneratorMode.Contains("Hk")) && (Items[j].Kana != "") && (Items[j].KunYomi != ""))
                    {
                        usej = true;
                        mode += "KhHk";
                    }
                    if ((GeneratorMode.Contains("He") || GeneratorMode.Contains("Eh")) && (Items[j].Englisch != "") && (Items[j].KunYomi != ""))
                    {
                        usej = true;
                        mode += "HeEh";
                    }
                    if ((rlist.Contains(j) == false) && usej)
                    {
                        rlist.Add(j);
                    }
                }
                //reduce mode
                mode = GetMode(GeneratorMode, mode);

                //Set other Elements
                while (rlist.Count<4)
                {
                    int j = rnd.Next(0, Items.Count);
                    bool usej = false;
                    if ((mode.Contains("Ke") || mode.Contains("Ek")) && (Items[j].Kana != "") && (Items[j].Englisch != ""))
                    {
                        usej = true;
                    }
                    //On-Yomi!!
                    if ((mode.Contains("Kh") || mode.Contains("Hk")) && (Items[j].Kana != "") && (Items[j].KunYomi != ""))
                    {
                        usej = true;
                    }
                    if ((mode.Contains("He") || mode.Contains("Eh")) && (Items[j].Englisch != "") && (Items[j].KunYomi != ""))
                    {
                        usej = true;
                    }
                    if ((rlist.Contains(j) == false) && usej)
                    {
                        rlist.Add(j);
                    }
                }

                VocabularyTrainer.QuestionClass Question = new VocabularyTrainer.QuestionClass();
                Question.rnd = rnd;
                if (mode.Contains("K"))
                {
                    Question.Question = Items[rlist[0]].Kana;
                } else if (mode.Contains("E"))
                {
                    Question.Question = Items[rlist[0]].Englisch;
                } else if (mode.Contains("H"))
                {
                    Question.Question = Items[rlist[0]].KunYomi;
                }
                //On-Yomi!!
                if (mode.Contains("k"))
                {
                    Question.AnswerA = Items[rlist[0]].Kana;
                    Question.AnswerB = Items[rlist[1]].Kana;
                    Question.AnswerC = Items[rlist[2]].Kana;
                    Question.AnswerD = Items[rlist[3]].Kana;
                }
                else if (mode.Contains("e"))
                {
                    Question.AnswerA = Items[rlist[0]].Englisch;
                    Question.AnswerB = Items[rlist[1]].Englisch;
                    Question.AnswerC = Items[rlist[2]].Englisch;
                    Question.AnswerD = Items[rlist[3]].Englisch;
                }
                else if (mode.Contains("h"))
                {
                    Question.AnswerA = Items[rlist[0]].KunYomi;
                    Question.AnswerB = Items[rlist[1]].KunYomi;
                    Question.AnswerC = Items[rlist[2]].KunYomi;
                    Question.AnswerD = Items[rlist[3]].KunYomi;
                }
                Question.RightAnswer = "A";
                Question.VocaID = Items[rlist[0]].ID;
                Question.Randomize();
                questions.Add(Question);
            }
            return questions;
        }
        private string GetMode(string GeneratorMode, string Category)
        {
            string mode = "";
            if (Category.Contains("Ek"))
            {
                if (GeneratorMode.Contains("Ek") && GeneratorMode.Contains("Ke"))
                {
                    if (rnd.Next(0,2)==0)
                    {
                        mode += "Ek";
                    } else
                    {
                        mode += "Ke";
                    }
                } else if (GeneratorMode.Contains("Ek"))
                {
                    mode += "Ek";
                } else if (GeneratorMode.Contains("Ke"))
                {
                    mode += "Ke";
                }
            } else if (Category.Contains("Eh"))
            {
                if (GeneratorMode.Contains("Eh") && GeneratorMode.Contains("He"))
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        mode += "Eh";
                    }
                    else
                    {
                        mode += "He";
                    }
                }
                else if (GeneratorMode.Contains("Eh"))
                {
                    mode += "Eh";
                }
                else if (GeneratorMode.Contains("He"))
                {
                    mode += "He";
                }
            } else if (Category.Contains("Kh"))
            {
                if (GeneratorMode.Contains("Kh") && GeneratorMode.Contains("Hk"))
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        mode += "Kh";
                    }
                    else
                    {
                        mode += "Hk";
                    }
                }
                else if (GeneratorMode.Contains("Kh"))
                {
                    mode += "Kh";
                }
                else if (GeneratorMode.Contains("Hk"))
                {
                    mode += "Hk";
                }
            }

            return mode;
        }
        public void UpdateQuestions(List<VocabularyTrainer.QuestionClass> questions)
        {
            for (int i=0;i<questions.Count;i++)
            {
                for (int j=0;j<this.Items.Count;j++)
                {
                    if (Items[j].ID==questions[i].VocaID)
                    {
                        switch(questions[i].AnsweredRight)
                        {
                            case "Wrong":
                                Items[j].falsch++;
                                break;
                            case "Right":
                                Items[j].richtig++;
                                break;
                            case "Unknown":
                                Items[j].unbekannt++;
                                break;
                            case "NotAnswered":
                                break;
                        }
                        if (questions[i].AnsweredRight==Items[j].Art)
                        {
                            Items[j].Straehne++;
                        }else
                        {
                            Items[j].Straehne = 0;
                            Items[j].Art = questions[i].AnsweredRight;
                        }
                        break;
                    }
                }
            }
        }
    }
}

namespace VocabularyTrainer
{
    public class ResultClass
    {
        private int right = 0;
        public int Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
            }
        }
        private int wrong = 0;
        public int Wrong
        {
            get
            {
                return wrong;
            }
            set
            {
                wrong = value;
            }
        }
        private int unknown = 0;
        public int Unknown
        {
            get
            {
                return unknown;
            }
            set
            {
                unknown = value;
            }
        }
        public int Total
        {
            get
            {
                return Right + Wrong + Unknown;
            }
        }
        public double ImgPercent
        {
            get
            {
                if ((Right + Wrong) != 0)
                {
                    return ((double)Right) / ((double)(Right + Wrong));
                }
                else if (Total == 0)
                {
                    return -1.0;
                }
                else
                {
                    return 0.0;
                }
            }
        }
        
        public double RightPercent
        {
            get
            {
                if ((Right + Wrong)!=0)
                {
                    return ((double)Right) / ((double)(Right + Wrong));
                }
                else
                {
                    return 0.0;
                }
            }
        }
        public double WrongPercent
        {
            get
            {
                if ((Right + Wrong) != 0)
                {
                    return ((double)Wrong) / ((double)(Right + Wrong));
                }
                else
                {
                    return 0.0;
                }
            }
        }
        public ResultClass()
        {

        }
    }
    class QuizClass
    {

        [XmlIgnore]
        public Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        public bool CanRestart { get; set;}
        public bool Finished
        {
            get
            {
                bool test = true;
                for (int i = 0; i<this.questions.Count;i++)
                {
                    if (questions[i].Answered == false)
                    {
                        test = false;
                        break;
                    }
                }
                return test;
            }
        }
        public List<QuestionClass> questions;
        public int MaxCount { get; set; }
        [XmlIgnore]
        public bool MaxCountReached
        {
            get
            {
                return questions.Count >= MaxCount;
            }
        }
        [XmlIgnore]
        public bool CanAdd
        {
            get
            {
                return !MaxCountReached;
            }
        }
        public QuizClass()
        {
            this.questions = null;
            this.MaxCount = 0;
            this.CanRestart = false;
        }
        public void SetQuestions(List<QuestionClass> Questions)
        {
            this.questions = Questions; //reset and randomize!!!
            this.Randomize();
        }
        public void RestartQuiz()
        {
            if (CanRestart == false)
            {
                throw new NotSupportedException();
            }
            //Get rid of unknowns and reset user answer
            List<QuestionClass> temp = new List<QuestionClass>();
            for (int i = 0; i < this.questions.Count; i++)
            {
                if (this.questions[i].AnsweredRight != "Unknown")
                {
                    temp.Add(new QuestionClass(this.questions[i]));
                }
            }
            this.questions = temp;
            this.Randomize();
        }
        private void Randomize()
        {
            List<QuestionClass> temp = new List<QuestionClass>();
            for (int i = 0; i < this.questions.Count; i++)
            {
                int j = rnd.Next(0, temp.Count);
                temp.Insert(j, this.questions[i]);
            }
            this.questions = temp;
            for (int i = 0; i < this.questions.Count; i++)
            {
                questions[i].rnd = rnd;
                questions[i].Randomize();
            }
        }
    }
    public class QuestionClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Variables
        private string question;
        public string Question
        {
            get
            {
                return question.Replace(";", "; ");
            }
            set
            {
                question = value;
            }
        }
        private List<string> AnswerList = new List<string> { "", "", "", "" };
        private List<int> RandomList = new List<int> { 0, 1, 2, 3 };
        public string AnswerA
        {
            get { return AnswerList[RandomList[0]].Replace(";","; "); }
            set { AnswerList[RandomList[0]] = value; }
        }
        public string AnswerB
        {
            get { return AnswerList[RandomList[1]].Replace(";", "; "); }
            set { AnswerList[RandomList[1]] = value; }
        }
        public string AnswerC
        {
            get { return AnswerList[RandomList[2]].Replace(";", "; "); }
            set { AnswerList[RandomList[2]] = value; }
        }
        public string AnswerD
        {
            get { return AnswerList[RandomList[3]].Replace(";", "; "); }
            set { AnswerList[RandomList[3]] = value; }
        }
        private int useranswer = -1;
        [XmlAttribute]
        public string UserAnswer {
            get
            {
                if ((useranswer!=-1)&&(useranswer!=4))
                {
                    return Index2Answer(RandomList.IndexOf(useranswer));
                } else
                {
                    return Index2Answer(useranswer);
                }
            }
            set
            {
                if (this.Answered == false)
                {
                    int index = Answer2Index(value);
                    if (index!=4)
                    {
                        useranswer = RandomList[index];
                    } else if (index == 4)
                    {
                        useranswer = 4;
                    }
                }
                NotifyPropertyChanged("Questions");
            }
        }//randomizing doesn't work here!!
        public bool Answered
        {
            get
            {
                return UserAnswer != "NotAnswered";
            }
        }
        [XmlIgnore]
        private int rightanswer = -1;
        [XmlAttribute]
        public string RightAnswer
        {
            get { return Index2Answer(RandomList.IndexOf(rightanswer)); }
            set { rightanswer = RandomList[Answer2Index(value)]; }
        }
        #endregion
        #region NumberIndex to String Converter
        private int Answer2Index(string Answer)
        {
            int Index = -1;
            switch (Answer)
            {
                case "A":
                    Index = 0;
                    break;
                case "B":
                    Index = 1;
                    break;
                case "C":
                    Index = 2;
                    break;
                case "D":
                    Index = 3;
                    break;
                case "Unknown":
                    Index = 4;
                    break;
                default:
                    throw new InvalidDataException();
                    break;
            }
            return Index;
        }
        private string Index2Answer(int Index)
        {
            string Answer = "NotAnswered";
            switch (Index)
            {
                case 0:
                    Answer = "A";
                    break;
                case 1:
                    Answer = "B";
                    break;
                case 2:
                    Answer = "C";
                    break;
                case 3:
                    Answer = "D";
                    break;
                case 4:
                    Answer = "Unknown";
                    break;
            }
            return Answer;
        }
        #endregion
        #region Constructors
        public QuestionClass()
        {
            this.Question = "Question";
            this.AnswerA  = "AnswerA";
            this.AnswerB  = "AnswerB";
            this.AnswerC  = "AnswerC";
            this.AnswerD  = "AnswerD";
            this.RightAnswer = "A";
            this.Reset();
        }
        public QuestionClass(string Question, string AnswerA, string AnswerB, string AnswerC, string AnswerD, string RightAnswer) : this (Question,AnswerA, AnswerB,  AnswerC,  AnswerD,  RightAnswer, -1)
        { }
        public QuestionClass(string Question, string AnswerA, string AnswerB, string AnswerC, string AnswerD, string RightAnswer, int VocaID)
        {
            this.Question = Question;
            this.AnswerA = AnswerA;
            this.AnswerB = AnswerB;
            this.AnswerC = AnswerC;
            this.AnswerD = AnswerD;
            this.RightAnswer = RightAnswer;
            
            this.VocaID = VocaID;
            this.Reset();
        }
        public QuestionClass(QuestionClass previousQuestion)
        {
            this.Question = previousQuestion.Question;
            this.AnswerList = previousQuestion.AnswerList;
            this.RandomList = previousQuestion.RandomList;
            this.rightanswer = previousQuestion.rightanswer;
            this.Reset();
        }
        #endregion
        #region Randomizing
        [XmlIgnore]
        private int seed = 0;
        public int Seed
        {
            get
            {
                return seed;
            }
            set
            {
                seed = value;
                rnd = new Random(seed);
            }
        }
        [XmlIgnore]
        public Random rnd = new Random(0);
        public void Randomize()
        {
            List<int> helper = new List<int> { 0, 1, 2, 3 };
            List<int> order = new List<int>();
            for (int i = 4; i > 0; i--)
            {
                int j = rnd.Next(0, i);
                order.Add(helper[j]);
                helper.RemoveAt(j);
            }
            RandomList = order;
        }
        #endregion
        #region Voca

        private int vocaid = -1;
        [XmlIgnore]
        public int VocaID {
            get { return vocaid; }
            set { vocaid = value; }
        }//DAMN!
        #endregion
        #region Functions
        public string AnsweredRight
        {
            get
            {
                if (this.Answered)
                {
                    if (this.RightAnswer == this.UserAnswer)
                    {
                        return "Right";
                    }
                    else if (this.UserAnswer == "Unknown")
                    {
                        return "Unknown";
                    }
                    else
                    {
                        return "Wrong";
                    }
                }
                else
                {
                    return "NotAnswered";
                }
            }
        }
        public void Reset()
        {
            this.useranswer = -1;
            this.Randomize();
        }
        #endregion
    }
    public class QuestionList
    {
        [XmlElement("QuestionItem")]
        public List<QuestionClass> Items;
        public QuestionList()
        {
            Items = new List<QuestionClass>();
        }
        public QuestionList(List<QuestionClass> bla)
        {
            Items = new List<QuestionClass>(bla);
        }
        public void Reset()
        {
            for (int i=0;i<Items.Count;i++)
            {
                Items[i].Reset();
            }
        }
    }
}

namespace XmlReadWriteUniversal
{

class XmlIO
{
    public static async Task<T> ReadObjectFromXmlFileAsync<T>(string filename)
    {
        // this reads XML content from a file ("filename") and returns an object  from the XML
        T objectFromXml = default(T);
        var serializer = new XmlSerializer(typeof(T));
        StorageFolder folder = ApplicationData.Current.LocalFolder;
        StorageFile file = await folder.GetFileAsync(filename);
        Stream stream = await file.OpenStreamForReadAsync();
        objectFromXml = (T)serializer.Deserialize(stream);
        stream.Dispose();
        return objectFromXml;
    }
    public static async Task<T> ReadObjectFromXmlPathAsync<T>(StorageFile file)
    {
        // this reads XML content from a file ("filename") and returns an object  from the XML
        T objectFromXml = default(T);
        var serializer = new XmlSerializer(typeof(T));
        //StorageFolder folder = ApplicationData.Current.LocalFolder;
        //StorageFile file = await folder.GetFileAsync(filename);
        Stream stream = await file.OpenStreamForReadAsync();
        objectFromXml = (T)serializer.Deserialize(stream);
        stream.Dispose();
        return objectFromXml;
    }
    public static async Task SaveObjectToXml<T>(T objectToSave, string filename)
    {
        // stores an object in XML format in file called 'filename'
        var serializer = new XmlSerializer(typeof(T));
        StorageFolder folder = ApplicationData.Current.LocalFolder;
        StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        Stream stream = await file.OpenStreamForWriteAsync();

        using (stream)
        {
            serializer.Serialize(stream, objectToSave);
        }
    }
    public static async Task SaveObjectToXmlPath<T>(T objectToSave, StorageFile file)
    {
        // stores an object in XML format in file called 'filename'
        var serializer = new XmlSerializer(typeof(T));
        //StorageFolder folder = ApplicationData.Current.LocalFolder;
        //StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        Stream stream = await file.OpenStreamForWriteAsync();

        using (stream)
        {
            serializer.Serialize(stream, objectToSave);
        }
    }
}

}