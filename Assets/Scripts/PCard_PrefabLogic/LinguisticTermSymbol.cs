//This class will contain the enum of all the linguistic characters inside the game.
//We will use this class to help build the deck for the BELLEtro game.

using System.Collections.Generic;
using UnityEngine;

// Andy Van: made unicode mapping from LinguisticTerm Enum

//This enum contains the list of linguistic character symbols
public class LinguisticTermSymbol
{
    public static Dictionary<LinguisticTerms, string> unicodeMap = new()
    {
        //Default Value
        { LinguisticTerms.None, ""},

        //Stop Consonants         
        { LinguisticTerms.Voiceless_Labial_Stop, "p" },
        { LinguisticTerms.Voiced_Labial_Stop, "b" },
        { LinguisticTerms.Voiced_Alveolar_Stop, "d" },
        { LinguisticTerms.Voiced_Velar_Stop, "g" },
        { LinguisticTerms.Voiceless_Alveolar_Stop, "t" },
        { LinguisticTerms.Voiceless_Velar_Stop, "k" },
        { LinguisticTerms.Voiceless_Glottal_Stop, "ʔ" },

        //Fricative Consonants        
        { LinguisticTerms.Voiceless_Labiodental_Fricative, "f" },
        { LinguisticTerms.Voiceless_Interdental_Fricative, "θ" },
        { LinguisticTerms.Voiceless_Alveolar_Fricative, "s" },
        { LinguisticTerms.Voiceless_AlveoPalatal_Fricative, "ʃ" },
        { LinguisticTerms.Voiceless_Glottal_Fricative, "h" },
        { LinguisticTerms.Voiced_Labiodental_Fricative, "v" },
        { LinguisticTerms.Voiced_Interdental_Fricative, "ð" },
        { LinguisticTerms.Voiced_Alveolar_Fricative, "z" },
        { LinguisticTerms.Voiced_AlveoPalatal_Fricative, "ʒ" },

        //Affricate Consonants        
        { LinguisticTerms.Voiceless_AlveoPalatal_Affricate, "tʃ" },
        { LinguisticTerms.Voiced_AlveoPalatal_Affricate, "dʒ" },

        //Nasal Consonants        
        { LinguisticTerms.Voiced_Alveolar_Nasal, "n" },
        { LinguisticTerms.Voiced_Velar_Nasal, "ŋ" },
        { LinguisticTerms.Voiced_Labial_Nasal, "m" },

        //Liquid Consonants       
        { LinguisticTerms.Voiced_Alveolar_Liquid_Lateral, "l" },
        { LinguisticTerms.Voiced_Alveolar_Liquid_Retroflex, "ɹ" },

        //Glide Consonants        
        { LinguisticTerms.Voiced_AlveoPalatal_Glide, "j" },
        { LinguisticTerms.Voiced_Velar_Glide, "w" },
        { LinguisticTerms.Voiceless_Velar_Glide, "ʍ" },

        //High Vowels
        { LinguisticTerms.Voiced_High_Front_Tense, "ij" },
        { LinguisticTerms.Voiced_High_Back_Tense, "uw" },
        { LinguisticTerms.Voiced_High_Front_Lax, "ɪ" },
        { LinguisticTerms.Voiced_High_Back_Lax, "ʊ" },

        //Mid vowels          
        { LinguisticTerms.Voiced_Mid_Front_Tense_Diphthong, "ej" },
        { LinguisticTerms.Voiced_Mid_Central_Rounded_Diphthong, "ow" },
        { LinguisticTerms.Voiced_Mid_Back_Rounded_Diphthong, "oj" },
        { LinguisticTerms.Voiced_Mid_Front_Lax, "ɛ" },
        { LinguisticTerms.Voiced_Mid_Central_Vowel_Schwa, "ə" },
        { LinguisticTerms.Voiced_Mid_Central_Vowel, "ʌ" },
        { LinguisticTerms.Voiced_Mid_Back_Rounded_Vowel, "ɔ" },

        //Low vowels          
        { LinguisticTerms.Voiced_Low_Central_Diphthong, "aj" },
        { LinguisticTerms.Voiced_Low_Back_Diphthong, "aw" },
        { LinguisticTerms.Voiced_Low_Back_Tense_Vowel, "a" },
        { LinguisticTerms.Voiced_Low_Front_Lax, "æ" },
    };

}   