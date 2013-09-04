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

namespace ttmr
{
	public class reactor : PartModule
	{
		public bool Enabled;

		[KSPField(isPersistant = true)]		
		static public string material = "aluminum";														//What material is the part made of?

		[KSPField(isPersistant = true)]
		static public string fuel = "THF4";																//What sort of fuel are we planning to use? This will determine fissile power density in J/s											

		[KSPField(isPersistant = false, guiActive = true, guiName = "Temperature", guiFormat = "0.0")]	//Display the temperature of the reactor in K
		public float Temp = 290;																		//Roughly pad temperature in KSP
		public float Power = 5000; //Stand in for fuel
		[KSPField(isPersistant = false, guiActive = true, guiName = "J/k", guiFormat = "1.0")] 			//This doesn't display for some reason or another.
		public static float J = (float)Simulation.getSpecificHeat(material)*2000; 						//Determine the number of joules required to raise the temperature by 1K

		[KSPEvent(guiActive = true, guiName = "Enable Reactor", active = true)]							//Create a UI button for enabling the reactor
		public void Enable()
		{
			Enabled = true;
		}

		[KSPEvent(guiActive = true, guiName = "Disable Reactor", active = true)]						//Create a UI button for disabling the reactor
		public void Disable()
		{
			Enabled = false;
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
			Temp -= Simulation.getHeatLoss (0.5f, 4, Temp, part.temperature) / J;
			if(Enabled)
			{
				Temp += Power / J;
			}
		}
	}
}