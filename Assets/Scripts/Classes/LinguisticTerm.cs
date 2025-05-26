//This class will contain the enum of all the linguistic characters inside the game.
//We will use this class to help build the deck for the BELLEtro game.

using System.Collections;
using UnityEngine;

// Zacharia Alaoui: made enum

//This enum contains the list of linguistic characters
public enum LinguisticTerms
{
    //Consonants

    //Stop Consonants         
    Voiceless_Bilabial_Stop,                      //(p)
    Voiced_Bilabial_Stop,                         //(b)
    Voiced_Alveolar_Stop,                         //(d)
    Voiced_Velar_Stop,                            //(g)
    Voiceless_Alveolar_Stop,                      //(t)
    Voiceless_Velar_Stop,                         //(k)
    Voiceless_Glottal_Stop,                       //(ʔ)  
    
    //Fricative Consonants        
    Voiceless_Labiodental_Fricative,              //(f)
    Voiceless_Interdental_Fricative,              //(θ̼)
    Voiceless_Alveolar_Fricative,                 //(s)
    Voiceless_Palatal_Fricative,                  //(ʃ)
    Voiceless_Glottal_Fricative,                  //(h)
    Voiced_Labiodental_Fricative,                 //(v)
    Voiced_Interdental_Fricative,                 //(ð)
    Voiced_Alveolar_Fricative_z,                  //(z)  
    Voiced_Palatal_Fricative,                     //(ʒ)
    
    //Affricate Consonants        
    Voiceless_Palatal_Affricate,                  //(tʃ)
    Voiced_Palatal_Affricate,                     //(dʒ)
    
    //Nasal Consonants        
    Voiced_Alveolar_Nasal,                        //(n)
    Voiced_Velar_Nasal,                           //(ŋ)
    Voiced_Bilabial_Nasal,                        //(m)
    
    //Liquid Consonants       
    Voiced_Alveolar_Lateral,                      //(l)
    Voiced_Alveolar_Retroflex,                    //(ɹ)
    
    //Glide Consonants        
    Voiced_Palatal_Glide,                         //(j)
    Voiced_Velar_Glide,                           //(w)
    Voiceless_Velar_Glide,                        //((ʍ))


    //Following terms are vowels
    //Tense vowels requires more muscular effort from the vocal tract.
    //Lax vowels don't require much muscular effort from the vocal tract.

    //High Vowels
    High_Front_Tense,                             //(ij)
    High_Back_Tense,                              //(uw)
    High_Front_Lax,                               //(I)
    High_Back_Lax,                                //(ʊ)
    
    //Mid vowels          
    Mid_Front_Tense,                              //(ej)
    Mid_Back_Rounded_Diphthong,                   //(ow)
    Mid_Back_To_HighFront_Rounded_Diphthong,      //(oj)
    Mid_Front_Lax,                                //(ɛ)
    Mid_Central_Vowel,                            //(ə)
    Mid_Low_Back_Unrounded_Vowel,                 //(ʌ)
    Mid_Back_Rounded_Vowel,                       //(ɔ)
          
    //Low vowels          
    Low_Central_To_HighFront_Diphthong,           //(aj)
    Low_Central_To_HighBack_Diphthong,            //(aw)
    Low_Back_Tense_Vowel,                         //(a)
    Low_Front_Lax,                                //(æ)

}   