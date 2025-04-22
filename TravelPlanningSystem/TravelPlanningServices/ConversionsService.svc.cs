using System;
using System.Collections.Generic;

namespace TravelPlanningServices
{
    public class ConversionService : IConversionsService
    {
        public int c2f(int c) // method to convert celsius to fahrenheit
        {
            return (c * 9 / 5) + 32; // we do the conversion based on the formula (c * 9/5) + 32
        }

        public int f2c(int f) // method to convert fahrenheit to celsius
        {
            return (f - 32) * 5 / 9; // we do the conversion based on the formula (f - 32) * 5/9
        }
    }
}