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

        //Stop Consonants         
        { LinguisticTerms.Voiceless_Stop_Labial, "p" },
        { LinguisticTerms.Voiced_Stop_Labial, "b" },
        { LinguisticTerms.Voiced_Stop_Alveolar, "d" },
        { LinguisticTerms.Voiced_Stop_Velar, "g" },
        { LinguisticTerms.Voiceless_Stop_Alveolar, "t" },
        { LinguisticTerms.Voiceless_Stop_Velar, "k" },
        { LinguisticTerms.Voiceless_Stop_Glottal, "ʔ" },

        //Fricative Consonants        
        { LinguisticTerms.Voiceless_Fricative_Labiodental, "f" },
        { LinguisticTerms.Voiceless_Fricative_Interdental, "θ" },
        { LinguisticTerms.Voiceless_Fricative_Alveolar, "s" },
        { LinguisticTerms.Voiceless_Fricative_AlveoPalatal, "ʃ" },
        { LinguisticTerms.Voiceless_Fricative_Glottal, "h" },
        { LinguisticTerms.Voiced_Fricative_Labiodental, "v" },
        { LinguisticTerms.Voiced_Fricative_Interdental, "ð" },
        { LinguisticTerms.Voiced_Fricative_Alveolar, "z" },
        { LinguisticTerms.Voiced_Fricative_AlveoPalatal, "ʒ" },

        //Affricate Consonants        
        { LinguisticTerms.Voiceless_Affricate_AlveoPalatal, "tʃ" },
        { LinguisticTerms.Voiced_Affricate_AlveoPalatal, "dʒ" },

        //Nasal Consonants        
        { LinguisticTerms.Voiced_Nasal_Alveolar, "n" },
        { LinguisticTerms.Voiced_Nasal_Velar, "ŋ" },
        { LinguisticTerms.Voiced_Nasal_Labial, "m" },

        //Liquid Consonants       
        { LinguisticTerms.Voiced_Liquid_Alveolar_Lateral, "l" },
        { LinguisticTerms.Voiced_Liquid_Alveolar_Retroflex, "ɹ" },

        //Glide Consonants        
        { LinguisticTerms.Voiced_Glide_AlveoPalatal, "j" },
        { LinguisticTerms.Voiced_Glide_Velar, "w" },
        { LinguisticTerms.Voiceless_Glide_Velar, "ʍ" },

        //High Vowels
        { LinguisticTerms.Tense_High_Front, "ij" },
        { LinguisticTerms.Tense_High_Back, "uw" },
        { LinguisticTerms.Lax_High_Front, "ɪ" },
        { LinguisticTerms.Lax_High_Back, "ʊ" },

        //Mid vowels          
        { LinguisticTerms.Tense_Mid_Front_Diphthong, "ej" },
        { LinguisticTerms.Tense_Mid_Back_Rounded_Diphthong_ow, "ow" },
        { LinguisticTerms.Tense_Mid_Back_Rounded_Diphthong_oj, "oj" },
        { LinguisticTerms.Lax_Mid_Front, "ɛ" },
        { LinguisticTerms.Tense_Mid_Central_Vowel_Schwa, "ə" },
        { LinguisticTerms.Lax_Mid_Central_Vowel, "ʌ" },
        { LinguisticTerms.Lax_Mid_Back_Rounded_Vowel, "ɔ" },

        //Low vowels          
        { LinguisticTerms.Tense_Low_Central_Diphthong_aj, "aj" },
        { LinguisticTerms.Tense_Low_Central_Diphthong_aw, "aw" },
        { LinguisticTerms.Tense_Low_Back_Vowel, "a" },
        { LinguisticTerms.Lax_Low_Front, "æ" },
    };

}   