//This class will contain the enum of all the linguistic characters inside the game.
//We will use this class to help build the deck for the BELLEtro game.

using System.Collections;
using UnityEngine;

// Zacharia Alaoui: made enum

//This enum contains the list of linguistic characters
public enum LinguisticTerms
{
    //Default Value

    //Consonants

    //Stop Consonants         
    Voiceless_Stop_Labial,                        //(p)
    Voiced_Stop_Labial,                           //(b)
    Voiced_Stop_Alveolar,                         //(d)
    Voiced_Stop_Velar,                            //(g)
    Voiceless_Stop_Alveolar,                      //(t)
    Voiceless_Stop_Velar,                         //(k)
    Voiceless_Stop_Glottal,                       //(ʔ)  
    
    //Fricative Consonants        
    Voiceless_Fricative_Labiodental,              //(f)
    Voiceless_Fricative_Interdental,              //(θ̼)
    Voiceless_Fricative_Alveolar,                 //(s)
    Voiceless_Fricative_AlveoPalatal,             //(ʃ)
    Voiceless_Fricative_Glottal,                  //(h)
    Voiced_Fricative_Labiodental,                 //(v)
    Voiced_Fricative_Interdental,                 //(ð)
    Voiced_Fricative_Alveolar,                    //(z)  
    Voiced_Fricative_AlveoPalatal,                //(ʒ)
    
    //Affricate Consonants        
    Voiceless_Affricate_AlveoPalatal,             //(tʃ)
    Voiced_Affricate_AlveoPalatal,                //(dʒ)
    
    //Nasal Consonants        
    Voiced_Nasal_Alveolar,                        //(n)
    Voiced_Nasal_Velar,                           //(ŋ)
    Voiced_Nasal_Labial,                          //(m)
    
    //Liquid Consonants       
    Voiced_Liquid_Alveolar_Lateral,               //(l)
    Voiced_Liquid_Alveolar_Retroflex,             //(ɹ)
    
    //Glide Consonants        
    Voiced_Glide_AlveoPalatal,                    //(j)
    Voiced_Glide_Velar,                           //(w)
    Voiceless_Glide_Velar,                        //((ʍ))


    //Following terms are vowels
    //Tense vowels requires more muscular effort from the vocal tract.
    //Lax vowels don't require much muscular effort from the vocal tract.

    //High Vowels
    Tense_High_Front,                             //(i)
    Tense_High_Back,                              //(u)
    Lax_High_Front,                               //(I)
    Lax_High_Back,                                //(ʊ)
    
    //Mid vowels          
    Tense_Mid_Front_Diphthong,                          //(ej)
    Tense_Mid_Back_Rounded_Diphthong_ow,                //(ow)
    Tense_Mid_Back_Rounded_Diphthong_oj,                //(oj)
    Lax_Mid_Front,                                      //(ɛ)
    Tense_Mid_Central_Vowel_Schwa,                      //(ə)
    Lax_Mid_Central_Vowel,                              //(ʌ)
    Lax_Mid_Back_Rounded_Vowel,                         //(ɔ)
          
    //Low vowels          
    Tense_Low_Central_Diphthong_aj,                     //(aj)
    Tense_Low_Central_Diphthong_aw,                     //(aw)
    Tense_Low_Back_Vowel,                               //(a)
    Lax_Low_Front,                                      //(æ)

}   