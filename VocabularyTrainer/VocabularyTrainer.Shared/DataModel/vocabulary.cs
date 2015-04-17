using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internationalization
{
    class DictionaryClass
    {
        string Name;
        List<LanguageClass> Languages;
        List<WordClass> Translations;
        //Language
        //from
        //to
    }
    class LanguageClass
    {
        string Name; //Internationalize?
        AlphabetClass Alphabet;
        GrammaticsClass Grammatics;
    }
    class AlphabetClass
    {
        string Name;
        List<char> Characters;
    }
    class GrammaticsClass
    {

    }
    class WordClass
    {
        int ID;
        string Description;
        string Etymology;
        List<string> Groups;
        //Wortart
        //Wortstamm
        //Sprache, von nach
    }

}
