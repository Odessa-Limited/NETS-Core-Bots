using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OdessaEngine.NETS.Core.Bots {
	public abstract class NetsBotBehaviour : NetsBehavior {
        public Action<string> OnChangeModule;
		List<NetsBotModule> _modules = new List<NetsBotModule>();
		public abstract List<NetsBotModule> GetNetsBotModules();

        private NetsBotModule lastBotModule = null;
        private static int currGlobalBotTickOffset = 0;
        protected int localBotTickOffset = 0;
        public override void NetsInitialize() {
            localBotTickOffset = currGlobalBotTickOffset++;
        }

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
            if (Time.frameCount % 100 == localBotTickOffset) {
                _modules = GetNetsBotModules();
                _modules = _modules.OrderByDescending(o => o.CalculateUtility(transform)).ToList();
            }
			var currentBotModule = _modules.FirstOrDefault();
			if (currentBotModule != default) {
                if (currentBotModule != lastBotModule) {
                    if (NetsNetworking.instance.settings.DebugConnections) 
                        Debug.Log($"NETS Bot - Changing behavior to {currentBotModule.GetType().Name}. With Score {currentBotModule.CalculateUtility(transform)}");
                    OnChangeModule?.Invoke(currentBotModule.GetType().Name);
                    lastBotModule = currentBotModule;
                }
				currentBotModule.OnModuleTick(transform);
			}
		}
	}
}
