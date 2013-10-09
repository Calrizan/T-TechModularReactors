using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace TTechModularReactors
{
    public class Simulation
    {
        //This constant is critical to the heat simulation.
        public const float BoltzmannConstant = 5.67e-8f;

        /// <summary>
        /// Get the maximum total value of a resource on a vessel.
        /// </summary>
        /// <param name="vessel">The vessel you wish to query.</param>
        /// <param name="s1">The name of the resource you wish to query.</param>
        /// <returns>The max level of resource 's1'.</returns>
        public static double GetMaxResource(Vessel vessel, string s1)
        {
            double total = 0;
            foreach (Part part in vessel.parts)
            {
                foreach (PartResource resource in part.Resources)
                {
                    if (resource.resourceName.Equals(s1))
                    {
                        total += resource.maxAmount;
                    }
                }
            }
            return total;
        }
        
        /// <summary>
        /// Get the current total value of a resource on a vessel.
        /// </summary>
        /// <param name="vessel">The vessel you wish to query.</param>
        /// <param name="s1">The name of the resource you wish to query.</param>
        /// <returns>The current level of resource 's1'.</returns>
        public static double GetCurrentResource(Vessel vessel, string s1)
        {
            double total = 0;
            foreach (Part part in vessel.parts)
            {
                foreach (PartResource resource in part.Resources)
                {
                    if (resource.resourceName.Equals(s1))
                    {
                        total += resource.amount;
                    }
                }
            }
            return total;
        }

        /// <summary>
        /// Obtain and process adjacent heating for the vessel's reactors and sinks.</summary>
        /// <param name="vessel"> The ship to process.</param>
        public static void ProcessSink(Vessel vessel)
        {
            var reactors = vessel.parts.SelectMany(p => p.Modules.OfType<Reactor>());
            foreach (var reactor in reactors)
            {
                //var sinks = vessel.parts.SelectMany(p => p.Modules.OfType<Sink>());
                var sinks = reactor.part.AdjacentParts().SelectMany(p => p.Modules.OfType<Sink>());
                foreach (var sink in sinks)
                {
                    var q = 205 * reactor.SurfaceArea * ((reactor.Temp - sink.Temp) * 0.1);
                   // Debug.Log("Sink: " + sink.Temp + "\nReactor: " + reactor.Temp+"\nQ: "+q+"\nSurfaceArea: "+reactor.SurfaceArea);
                    reactor.Temp -= q/reactor.Joules;
                    sink.Temp += q/sink.Joules;
                }
                
            }   
        }

        /// <summary>
        /// Determine how much heat an object loses due to it's emissivity and surface area.</summary>
        /// <param name="e"> Emissivity of the object, between 0 and 1.</param>
        /// <param name="area"> The exposed surface area of the part.</param>
        /// <param name="temp"> The current temperature of the part.</param>
        /// <param name="ambient"> The current ambient temperature.</param>
        public static double GetHeatLoss(double e, double area, double temp, double ambient)
        {
            return (e * BoltzmannConstant * area * (Math.Pow(temp, 4) - Math.Pow(ambient + 273.15, 4)));
        }
        
        /// <summary>
        /// Dictionary of specific heat capacities for various materials. Common ones currently exist.
        /// </summary>
        /// <param name="S">Material string.</param>
        /// <returns>Specific heat capacity of material string S</returns>
        public static double GetSpecificHeat(string S)
        {
            Dictionary<string, object> Mat = new Dictionary<string, object>();
            Mat.Add("aluminum", 0.91);
            Mat.Add("iron", 0.444);
            Mat.Add("magnesium", 1.05);
            Mat.Add("nickel", 0.440);
            Mat.Add("titanium", 0.54);
            Mat.Add("glycol", 2.200);
            Mat.Add("air", 1.020);
            Mat.Add("water", 4.186);
            Mat.Add("copper", 0.386);
            return (double) Mat[S];
        }
        
        /// <summary>
        /// Dictionary of energy densities for various fules. Common ones currently exist.
        /// </summary>
        /// <param name="s">Fuel string.</param>
        /// <returns>Energy density of fuel in Joules/Kg.</returns>
        public static double GetEnergyDensity(string s)
        {
            Dictionary<string, float> Fuel = new Dictionary<string, float>();
            Fuel.Add("thorium", 82000000);
            Fuel.Add("plutonium", 1e+14f);
            Fuel.Add("uranium", 83140000);
            Fuel.Add("paper", 5);
            return Fuel[s];
        }
    }
    public static class PartExtensions
    {
        /// <summary>
        /// Find the adjacent parts of a specific part.
        /// </summary>
        /// <param name="part">The part to search from.</param>
        /// <returns>IEnumerable list of adjacent parts.</returns>
        public static IEnumerable<Part> AdjacentParts(this Part part)
        {
            yield return part.parent;
            foreach (var child in part.children)
            {
                yield return child;
            }
        }
    }
}

