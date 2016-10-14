using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{

    
    class DynamicAvoidMultipleCharacters : DynamicMovement
    {
        public override string Name
        {
            get { return "Avoid Character"; }
        }

        public override KinematicData Target { get; set; }

        public DynamicAvoidMultipleCharacters()
        {
        }

        public float CollisionRadius { get; set; }

        public List<KinematicData> Targets { get; set; }

        public float MaxTimeLookAhead { get; set; }

        public override MovementOutput GetMovement() {

            float shortestTime = float.MaxValue;
            KinematicData closestTarget = new KinematicData();
            float closestFutureDistance = float.MaxValue;
            Vector3 closestFutureDeltaPos = new Vector3();
            Vector3 closestDeltaPos = new Vector3();
            Vector3 closestDeltaVel = new Vector3();


            foreach (KinematicData t in Targets) {
                
                var deltaPos = Target.position - Character.position;
                var deltaVel = Target.velocity - Character.velocity;
                var deltaSpeed = deltaVel.magnitude;
                
                if (deltaSpeed == 0) continue;

                var timeToClosest = -(Vector3.Dot(deltaPos,deltaVel)) / (deltaSpeed * deltaSpeed);
                if (timeToClosest > MaxTimeLookAhead) continue;

                var futureDeltaPos = deltaPos + deltaVel * timeToClosest;
                var futureDistance = futureDeltaPos.magnitude;
                if (futureDistance > 2 * CollisionRadius) continue;

                if (timeToClosest > 0 && timeToClosest < shortestTime) {
                    shortestTime = timeToClosest;
                    closestTarget = t;
                    closestFutureDistance = futureDistance;
                    closestFutureDeltaPos = futureDeltaPos;
                    closestDeltaPos = deltaPos;
                    closestDeltaVel = deltaVel;
                }
            }

            if (shortestTime == float.MaxValue) return new MovementOutput();

            Vector3 avoidanceDirection;

            if (closestFutureDistance <= 0 || closestDeltaPos.magnitude < 2 * CollisionRadius)
                avoidanceDirection = Character.position - closestTarget.position;
            else
                avoidanceDirection = closestFutureDeltaPos * -1;

            var output = new MovementOutput();
            output.linear = avoidanceDirection.normalized * MaxAcceleration;
            return output;

        }
    }
}
