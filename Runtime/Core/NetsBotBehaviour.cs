using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OdessaEngine.NETS.Core.Bots {
	public abstract class NetsBotBehaviour : NetsBehavior {

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

            foreach (var module in _modules) 
                if (module.OnModuleTick(transform))
                    break;
		}
    }
    public class ClosestObject<T> where T : MonoBehaviour {
        public T controller;
        public float distance;
        public ClosestObject(T _controller, float dist) {
            controller = _controller;
            distance = dist;
        }
        /// <summary>
        /// Distance useful look up and comparison extension of UnityEngine.Object.FindObjectsOfType<typeparamref name="U"/>()
        /// The below example will give you the closest object to the "Owner" (owner being the bot in question) with CharacterController script attached
        /// <code>
        ///     ClosestObject<CharacterController>.GetClosestPlayer(owner.transform)
        /// </code>
        /// </summary>
        /// <remarks>
        /// See documentation about how to use the system.
        /// </remarks>
        /// 
        /// <example>
        /// public class ChacterBotDefendModule : NetsBotModule {
        ///    public override int CalculateUtility(Transform owner) {
        ///    var ownerCharacterController = owner.GetComponent<CharacterController>();
        ///    var ownerGoal = UnityEngine.Object.FindObjectsOfType<Goal>().Where(o => o.GetTeam() == ownerCharacterController.teamId).FirstOrDefault();
        ///    if (ownerGoal) {
        ///    var closestEnemyToGoalDistance = ClosestObject<CharacterController>.GetClosestObject(ownerGoal.transform, UnityEngine.Object.FindObjectsOfType<CharacterController>().Where(o => o.transform != owner && o.teamId != ownerCharacterController.teamId).ToList()).distance;
        ///     if (closestEnemyToGoalDistance > 10) return 0;
        ///         var value = (int)(Mathf.Pow(closestEnemyToGoalDistance, -1f) * 100);
        ///         return value;
        ///        }
        ///        return 0;
        ///    }
        ///
        ///    public override void OnModuleTick(Transform owner) {
        ///        var ownerCharacterController = owner.GetComponent<CharacterController>();
        ///        var ownerGoal = UnityEngine.Object.FindObjectsOfType<Goal>().Where(o => o.GetTeam() == ownerCharacterController.teamId).FirstOrDefault();
        ///        ownerCharacterController.DoLook(ClosestObject<CharacterController>.GetClosestObject<CharacterController>(owner).controller.transform.position);
        ///        ownerCharacterController.MoveTowards(Vector3.MoveTowards(ownerGoal.transform.position, ownerCharacterController.transform.position, 1));
        ///        ownerCharacterController.BotFire();
        ///    }
        ///}
        /// </example>
        /// 
        /// <code>
        ///     ClosestObject<CharacterController>.GetClosestPlayer(owner.transform)
        /// </code>
        /// 
        public static ClosestObject<T> GetClosestObject(Transform tocheck, List<T> ListOf = default)  {
            var objs = ListOf != default && ListOf.Count() > 0 ? ListOf : UnityEngine.Object.FindObjectsOfType<T>().Where(o => o.GetInstanceID() != tocheck.GetInstanceID()).ToList();
            var closest = objs.FirstOrDefault();
            if (closest != default) {
                var closestDist = Vector3.Distance(tocheck.position, closest.transform.position);
                foreach (var obj in objs) {
                    var dist = Vector3.Distance(tocheck.position, obj.transform.position);
                    if (dist < closestDist) {
                        closestDist = dist;
                        closest = obj;
                    }
                }
                return new ClosestObject<T>(closest, closestDist);
            }
            return null;
        }
    }
}
