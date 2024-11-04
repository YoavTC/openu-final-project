using System;
using External_Packages;
using UnityEngine;

public enum Rarity
{
    //Value is the colour codes in decimal (https://www.rapidtables.com/convert/number/hex-to-decimal.html)
    GENERIC = 12829635, 
    COMMON = 7377598, 
    UNCOMMON = 2273612,
    RARE = 16751925, 
    EPIC = 10701220, 
    LEGENDARY = 16763150, 
    MYTHIC = 15349376, 
    EXTRAORDINARY = 7601149
}

public static class RarityColours
{
    public static string GetRarityHexColour(Rarity rarity)
    {
        return rarity.ToString("x");
    }
    
    public static Color GetRarityColour(Rarity rarity)
    {
        string hex = rarity.ToString("x");
        return HelperFunctions.HexToColor(hex);
    }
}