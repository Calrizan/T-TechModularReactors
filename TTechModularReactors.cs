// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

//This code is freely allowed to be maintained if the developer should dissapear as long as a changelog is maintained with the code.
//This code is free to be used so long as the original author is given credit for the components used.

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TTechModularReactors;

namespace TTechModularReactors
{
	public class Reactor : PartModule
	{
		public bool Enabled;
        public double Joules;
        public double F1, F2;

        //Get the reactor material type. Material dictionary is available in simresource.cs under Simulation.
		[KSPField(isPersistant = false)]	
		public string Material;
        //Get the type of fuel the reactor is driven by.
        [KSPField(isPersistant = false)] 
		public string FuelType;																                  										
        //What is the blackbody emissivity of the part? This calculates heat-loss over time.
		[KSPField]
		public float Emissivity;
        //How many Kg of fuel are we carrying?
        [KSPField]
        public float FuelMass;                                            
        //Get the emissive surface area from the config.
        [KSPField]
        public float SurfaceArea;																	
        //Display Reactor Temperature
		[KSPField(isPersistant = false, guiActive = true, guiName = "Temperature", guiFormat = "0.0")]
		public double Temp = 290;
        //Display amount of fuel remaining in %.
        [KSPField(isPersistant = false, guiActive = true, guiName = "Fuel", guiFormat = "P1")]
        public double fuelLeft;
        //Stand in for the power slider, which will be introduced later.
		public double Power = 5000;
        //Create a UI button for enabling the reactor
        [KSPEvent(guiActive = true, guiName = "Toggle Reactor", active = true)]							    
		public void Toggle()
		{
			Enabled = !Enabled;
		}

        public override void OnLoad(ConfigNode node)
        {
            Debug.Log("Reactor Surface Area: " + SurfaceArea + "\nReactor Emissivity: " + Emissivity);
            //Determine how many J/s it takes to raise the core temp by 1 kelvin.
            Joules = Simulation.GetSpecificHeat(Material) * (part.mass*1000);
            F1 = Simulation.GetEnergyDensity(FuelType)*FuelMass;
            F2 = Simulation.GetEnergyDensity(FuelType)*FuelMass;
            fuelLeft = F2 / F1;
        }

		public void FixedUpdate()   
		{
			//Logic chunk for determining if we are in flight, paused, or destroyed.
			if (!HighLogic.LoadedSceneIsFlight)
				return;
			if (vessel == null)
				return;
			if (FlightDriver.Pause)
				return;
            if (TimeWarp.CurrentRate > 100)
                return;
			//End of logic
            //Get the part position
            Vector3 position = this.part.transform.position;
            //Calculate the reactor's heat with the values gathered from the config and world.
            Temp -= (Simulation.GetHeatLoss(Emissivity, SurfaceArea, Temp, FlightGlobals.getExternalTemperature(FlightGlobals.getAltitudeAtPos(position), FlightGlobals.getMainBody())) / Joules) * TimeWarp.fixedDeltaTime;
            Simulation.ProcessSink(vessel);
            //Clamp the temperature of the part. Absolute zero is unreachable, and 5000K is well beyond meltdown. This does not solve the wild oscillation problems we still have
            Temp = MathExtensions.Clamp(Temp, 1, 5000);
            //Reactor power loop.
			if(Enabled && F2 > 5000)
			{
				Temp += (Power / Joules) * TimeWarp.fixedDeltaTime;
                F2 -= Power * TimeWarp.fixedDeltaTime;
                fuelLeft = F2 / F1;
			}
		}
	}

    public class Sink : PartModule
    {
        public double Joules;
        //Set the sink material
        [KSPField]
        public string Material;
        //Set the sink emissivity
        [KSPField]
        public float Emissivity;
        //Set the emissive surface area
        [KSPField]
        public float SurfaceArea;
        //Display the temperature of the sink
        [KSPField(isPersistant = false, guiActive = true, guiName = "Temperature", guiFormat = "0.0")]
        public double Temp = 290;
        
        //Stuff to load when a partmodule is loaded.
        public override void OnLoad(ConfigNode node)
        {
            Debug.Log("Sink Surface Area: " + SurfaceArea + "\nSink Emissivity: " + Emissivity);
            Joules = Simulation.GetSpecificHeat(Material) * (part.mass * 1000);
        }

        public void FixedUpdate()
        {
            if (!HighLogic.LoadedSceneIsFlight) return;
            if (vessel == null) return;
            if (FlightDriver.Pause) return;

            Vector3 position = this.part.transform.position;
            Temp -= (Simulation.GetHeatLoss(Emissivity, SurfaceArea, Temp, FlightGlobals.getExternalTemperature(FlightGlobals.getAltitudeAtPos(position), FlightGlobals.getMainBody())) / Joules) * TimeWarp.fixedDeltaTime;
            Temp = MathExtensions.Clamp(Temp, 1, 5000);
        }
    }
}