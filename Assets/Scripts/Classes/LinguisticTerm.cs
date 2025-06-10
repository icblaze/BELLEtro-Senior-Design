//This class will contain the enum of all the linguistic characters inside the game.
//We will use this class to help build the deck for the BELLEtro game.

using System.Collections;
using UnityEngine;

// Zacharia Alaoui: made enum

//This enum contains the list of linguistic characters
public enum LinguisticTerms
{
    //Default Value
    None,

    //Consonants

    //Stop Consonants         
    Voiceless_Labial_Stop,                        //(p)
    Voiced_Labial_Stop,                           //(b)
    Voiced_Alveolar_Stop,                         //(d)
    Voiced_Velar_Stop,                            //(g)
    Voiceless_Alveolar_Stop,                      //(t)
    Voiceless_Velar_Stop,                         //(k)
    Voiceless_Glottal_Stop,                       //(ʔ)  
    
    //Fricative Consonants        
    Voiceless_Labiodental_Fricative,              //(f)
    Voiceless_Interdental_Fricative,              //(θ̼)
    Voiceless_Alveolar_Fricative,                 //(s)
    Voiceless_AlveoPalatal_Fricative,             //(ʃ)
    Voiceless_Glottal_Fricative,                  //(h)
    Voiced_Labiodental_Fricative,                 //(v)
    Voiced_Interdental_Fricative,                 //(ð)
    Voiced_Alveolar_Fricative,                    //(z)  
    Voiced_AlveoPalatal_Fricative,                //(ʒ)
    
    //Affricate Consonants        
    Voiceless_AlveoPalatal_Affricate,             //(tʃ)
    Voiced_AlveoPalatal_Affricate,                //(dʒ)
    
    //Nasal Consonants        
    Voiced_Alveolar_Nasal,                        //(n)
    Voiced_Velar_Nasal,                           //(ŋ)
    Voiced_Labial_Nasal,                          //(m)
    
    //Liquid Consonants       
    Voiced_Alveolar_Liquid_Lateral,               //(l)
    Voiced_Alveolar_Liquid_Retroflex,             //(ɹ)
    
    //Glide Consonants        
    Voiced_AlveoPalatal_Glide,                    //(j)
    Voiced_Velar_Glide,                           //(w)
    Voiceless_Velar_Glide,                        //((ʍ))


    //Following terms are vowels
    //Tense vowels requires more muscular effort from the vocal tract.
    //Lax vowels don't require much muscular effort from the vocal tract.

    //High Vowels
    Voiced_High_Front_Tense,                             //(ij)
    Voiced_High_Back_Tense,                              //(uw)
    Voiced_High_Front_Lax,                               //(I)
    Voiced_High_Back_Lax,                                //(ʊ)
    
    //Mid vowels          
    Voiced_Mid_Front_Tense_Diphthong,                    //(ej)
    Voiced_Mid_Central_Rounded_Diphthong,                //(ow)
    Voiced_Mid_Back_Rounded_Diphthong,                   //(oj)
    Voiced_Mid_Front_Lax,                                //(ɛ)
    Voiced_Mid_Central_Vowel_Schwa,                      //(ə)
    Voiced_Mid_Central_Vowel,                            //(ʌ)
    Voiced_Mid_Back_Rounded_Vowel,                       //(ɔ)
          
    //Low vowels          
    Voiced_Low_Central_Diphthong,                        //(aj)
    Voiced_Low_Back_Diphthong,                           //(aw)
    Voiced_Low_Back_Tense_Vowel,                         //(a)
    Voiced_Low_Front_Lax,                                //(æ)

}   