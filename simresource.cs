using System;
using System.Collections.Generic;
using UnityEngine;

namespace TTechModularReactors
{
    public class Simulation
    {
        //This function returns the maximum resource s1 of the current vessel. Refer to KSP's resource list to determine what strings are used.
        public static float getMaxResource(Vessel vessel, string s1)
        {
            float total = 0.0f;
            foreach (Part part in vessel.parts)
            {
                foreach (PartResource resource in part.Resources)
                {
                    if (resource.resourceName.Equals(s1))
                    {
                        total += (float)resource.maxAmount;
                    }
                }
            }
            return total;
        }
        //This function returns the current resource s1 of the current vessel. Refer to KSP's resource list to determine what strings are used.
        public static float getCurrentResource(Vessel vessel, string s1)
        {
            float total = 0.0f;
            foreach (Part part in vessel.parts)
            {
                foreach (PartResource resource in part.Resources)
                {
                    if (resource.resourceName.Equals(s1))
                    {
                        total += (float)resource.amount;
                    }
                }
            }
            return total;
        }
        //Calculate heat loss in joules. Emissivity, Area, Initial Temperature
        public static float getHeatLoss(float e, float area, float temp, float ambient)
        {
            return (e * 5.67e-8f * area * (float)(Math.Pow(temp, 4) - Math.Pow(ambient + 273.15, 4)));
        }
        //Dictionary of specific heat capacities for commonly-used structural materials. By no means complete.
        public static float getSpecificHeat(string S)
        {
            Dictionary<string, object> Mat = new Dictionary<string, object>();
            Mat.Add("aluminum", 0.91f);
            Mat.Add("iron", 0.444f);
            Mat.Add("magnesium", 1.05f);
            Mat.Add("nickel", 0.440f);
            Mat.Add("titanium", 0.54f);
            Mat.Add("glycol", 2.200f);
            Mat.Add("air", 1.020f);
            Mat.Add("water", 4.186f);
            Mat.Add("copper", 0.386f);
            return (float) Mat[S];
        }
        //Dictionary of fuel energy densities in J/kg
        public static float getEnergyDensity(string s)
        {
            Dictionary<string, float> Fuel = new Dictionary<string, float>();
            Fuel.Add("thorium", 82000000f);
            Fuel.Add("plutonium", 1e+14f);
            Fuel.Add("uranium", 83140000f);
            Fuel.Add("paper", 5f);
            return Fuel[s];
        }
    }
}

