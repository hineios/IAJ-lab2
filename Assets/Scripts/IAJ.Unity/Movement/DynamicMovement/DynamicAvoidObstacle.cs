using System;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Util;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicAvoidObstacle : DynamicSeek
	{
		public DynamicAvoidObstacle ()
		{
		}

		public Collider collisionDetector { get; set; }

		public float avoidDistance { get; set; }

		public float lookAhead { get; set; }

		public override MovementOutput GetMovement ()
		{
			var rayVector = this.Character.velocity.normalized * lookAhead;
			RaycastHit hit;
			Ray ray = new Ray (this.Character.position, rayVector);
			Debug.DrawRay(this.Character.position, rayVector ,Color.yellow);
			var colision = collisionDetector.Raycast (ray, out hit, lookAhead);
			if (!colision)
				return new MovementOutput();
			this.Target = new KinematicData ();
			this.Target.position = hit.point + hit.normal * avoidDistance;
			return base.GetMovement ();
		}
	}
}

