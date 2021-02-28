using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OdessaEngine.NETS.Core.Bots {
	public abstract class NetsBotModule {
		public abstract int CalculateUtility(Transform owner);
		public abstract void OnModuleTick(Transform owner);
	}
}
