using System;
using UnityEngine;

public class PriceAssignment
{
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
