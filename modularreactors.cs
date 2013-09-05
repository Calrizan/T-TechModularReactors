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
	public class reactor : PartModule
	{
		public bool Enabled;
        public float J;
        AtmosphereProbe probe = new AtmosphereProbe();

		[KSPField(isPersistant = false)]	
		public string material;														                        //What material is the part made of?

		[KSPField(isPersistant = false)] 
		public string fuel;																                    //What sort of fuel are we planning to use? This will determine fissile power density in J/s											

		[KSPField]
		public float emissivity;                                                                            //What is the emissivity of the object?

        [KSPField]
        public float surfaceArea;																			//What is the emissive surface area of the object?
            
		[KSPField(isPersistant = false, guiActive = true, guiName = "Temperature", guiFormat = "0.0")]	    //Display the temperature of the reactor in K
		public float Temp = 290;																		    //Roughly pad temperature in KSP

		public float Power = 5000; //Stand in for fuel
		
        [KSPEvent(guiActive = true, guiName = "Toggle Reactor", active = true)]							    //Create a UI button for enabling the reactor
		public void Toggle()
		{
			Enabled = !Enabled;
		}

        public override void OnLoad(ConfigNode node)
        {
            J = (float)Simulation.getSpecificHeat(material) * (part.mass*1000);                             //Determine the number of joules required to raise the temperature by 1K
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
			//End of logic
			Temp -= Simulation.getHeatLoss (emissivity, surfaceArea, Temp, probe.temperature) / J;
			if(Enabled)
			{
				Temp += Power / J;
                Debug.Log("E: "+emissivity+"\nsA:"+surfaceArea);
			}
		}
	}
}