using System;
using System.Collections.Generic;

public static class RandomNumberGenerator
{
    private static Random random = new Random();

    public static int RandomInteger(int lowerBound, int upperBound)
    {
        return random.Next(lowerBound, upperBound);
    }

    public static int RandomIntegerProbability(List<KeyValuePair<int, double>> elements)
    {
        int selectedElement = 0;
        double cumulative = 0.0;

        double diceRoll = random.NextDouble();
        
        for (int i = 0; i < elements.Count; i++)
        {
            cumulative += elements[i].Value;

            if (diceRoll < cumulative)
            {
                selectedElement = elements[i].Key;
                break;
            }
        }

        return selectedElement;
    }
}
