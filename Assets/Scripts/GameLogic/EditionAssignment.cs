using System;
using System.Collections.Generic;
using UnityEngine;

public class EditionAssignment
{
    private static System.Random rand = new System.Random();
    //  Rates of editions out of 100 
    private static Dictionary<int, CardEdition> editionRates = new()
    {
        { 70, CardEdition.Base },
        { 16, CardEdition.Foil },
        { 8, CardEdition.Holographic },
        { 4, CardEdition.Polychrome },
        { 2, CardEdition.Negative }
    };

    //  Return additional price based on edition, 0 if base
    public static int EditionPrice(CardEdition edition)
    {
        switch (edition)
        {
            case (CardEdition.Foil):
                return 2;
            case (CardEdition.Holographic):
                return 3;
            case (CardEdition.Polychrome):
                return 5;
            case (CardEdition.Negative):
                return 5;
            default:
                return 0;
        }
    }

}
