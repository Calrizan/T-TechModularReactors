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
		[KSPField]		//What material is the part made of?
		static public string s_mat = "iron";
		[KSPField]		//This controls heat fine-tuning.
		public float heatProduction = 1;
		[KSPField]		//Power multiplier
		public float powerMultiplier = 5;

		double SHC = Simulation.getSpecificHeat (s_mat); //Get the SHC of the material
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
		}
	}
}