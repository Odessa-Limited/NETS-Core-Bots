using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OdessaEngine.NETS.Core.Bots {
	public abstract class NetsBotBehaviour : NetsBehavior {
		List<NetsBotModule> _modules = new List<NetsBotModule>();
		public abstract List<NetsBotModule> GetNetsBotModules();
        /// <summary>
        /// Logic for each bot happens here. Need to override and have base called for NetsOwnedUpdate
        /// <code>
        ///     base.NetsOwnedUpdate();
        /// </code>
        /// </summary>
        /// <remarks>
        /// See documentation about how to use the system.
        /// </remarks>
        /// 
        /// <example>
        /// public override void NetsOwnedUpdate() {
        ///     base.NetsOwnedUpdate();
        ///     .... //your code here
        /// }
        /// </example>
        /// 
        /// <code>
        /// public override void NetsOwnedUpdate() {
        ///     base.NetsOwnedUpdate();
        ///     .... //your code here
        /// }
        /// </code>
        /// 
        public override void NetsOwnedUpdate() {
			base.NetsOwnedUpdate();
			_modules = GetNetsBotModules();
			_modules = _modules.OrderByDescending(o => o.CalculateUtility()).ToList();
			var firstModule = _modules.FirstOrDefault();
			if (firstModule != default) {
				firstModule.OnModuleTick();
			}
		}
	}
}
