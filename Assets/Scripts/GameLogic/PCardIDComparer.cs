using System;
using System.Collections.Generic;

public class PCardIDComparer : IEqualityComparer<PCard>
{
    public bool Equals(PCard x, PCard y)
    {
        if (x == null || y == null) return false;
        return x.cardID == y.cardID;
    }

    public int GetHashCode(PCard obj)
    {
        return obj.cardID.GetHashCode();
    }
}