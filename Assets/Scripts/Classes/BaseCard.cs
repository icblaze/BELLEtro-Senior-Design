// This Document contains the code for the BaseCard class.
// This class is used as an interface for all of the cards in BELLEtro
// Current Devs:
// Robert (momomonkeyman): made base class with the variables
// Zacharia Alaoui(ZachariaAlaoui): I added a abstract property that provides derived classes to provide their own implementation

using System.Collections;
using UnityEngine;

// This abstract class is the base for all of the other cards in BELLEtro
public abstract class BaseCard
{
    public abstract CardType kindOfCard { get; set; } //Getter and setter for typeOfCard
}
