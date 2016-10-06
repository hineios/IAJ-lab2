using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{

    
    class CharacterAvoidance
    {
        public DynamicCharacter character { get; set; }
        public float  maxAcceleration { get; set; }
        public float collisionRadius { get; set; }
        public List<DynamicCharacter> targets { get; set; }
        public float maxTimeLookahead { get; set; }

        public MovementOutput getMovement() {
            //MovementOutput output = new MovementOutput();
            //KinematicData dataCharacter = character.KinematicData;
            //KinematicData dataTarget = target.KinematicData;
            //UnityEngine.Vector3 futureDeltaPos;
            //float futureDistance;
            //float timeToClosest;
            //UnityEngine.Vector3 deltaPos = dataTarget.position - dataCharacter.position;
            //UnityEngine.Vector3 deltaVel = dataTarget.velocity - dataCharacter.velocity;
            //float deltaSpeed = deltaVel.magnitude;

            //if (deltaSpeed == 0)
            //    return output;

            //timeToClosest = -(Vector3.Dot(deltaPos, deltaVel) / (deltaSpeed * deltaSpeed));

            //if (timeToClosest > maxTimeLookahead)
            //    return output;

            //futureDeltaPos = deltaPos + deltaVel * timeToClosest;
            //futureDistance = futureDeltaPos.magnitude;

            //if (futureDistance > 2 * collisionRadius)
            //    return new MovementOutput();

            //if (futureDistance <= 0 || deltaPos.magnitude < 2 * collisionRadius)
            //    output.linear = dataCharacter.position - dataTarget.position;
            //else
            //    output.linear = futureDeltaPos * -1;

            //output.linear = output.linear.normalized * maxAcceleration;
            //return output;

            float shortestTime = 100000000000;
            MovementOutput output = new MovementOutput();
            KinematicData dataCharacter = character.KinematicData;
            UnityEngine.Vector3 futureDeltaPos;
            UnityEngine.Vector3 closestFutureDeltaPos = new Vector3(0,0,0);
            UnityEngine.Vector3 closestDeltaPos = new Vector3(0, 0, 0);
            UnityEngine.Vector3 closestDeltaVel = new Vector3(0, 0, 0);
            UnityEngine.Vector3 avoidanceDirection;
            float futureDistance;
            float closestFutureDistance = 0;

            float timeToClosest;
            DynamicCharacter closestTarget = targets[0];


            foreach (DynamicCharacter t in targets) {
                KinematicData dataTarget = t.KinematicData;
                UnityEngine.Vector3 deltaPos = dataTarget.position - dataCharacter.position;
                UnityEngine.Vector3 deltaVel = dataTarget.velocity - dataCharacter.velocity;
                float deltaSpeed = deltaVel.magnitude;
                deltaPos = t.KinematicData.position - dataCharacter.position;
                deltaVel = t.KinematicData.velocity - dataCharacter.velocity;
                deltaSpeed = deltaVel.magnitude;
                if (deltaSpeed == 0)
                    break;
                    timeToClosest = -Vector3.Dot(deltaPos,deltaVel) / (deltaSpeed * deltaSpeed);
                if (timeToClosest > maxTimeLookahead)
                    break;
                futureDeltaPos = deltaPos + deltaVel * timeToClosest;
                futureDistance = futureDeltaPos.magnitude;
                if (futureDistance > 2 * collisionRadius)
                    break;
                if (timeToClosest > 0 && timeToClosest < shortestTime) {
                    shortestTime = timeToClosest;
                    closestTarget = t;
                    closestFutureDistance = futureDistance;
                    closestFutureDeltaPos = futureDeltaPos;
                    closestDeltaPos = deltaPos;
                    closestDeltaVel = deltaVel;
                        }
                    }
            if (shortestTime == 100000000000)
                return new MovementOutput();
            if (closestFutureDistance <= 0 || closestDeltaPos.magnitude < 2 * collisionRadius)
                avoidanceDirection = dataCharacter.position - closestTarget.KinematicData.position;
            else
                avoidanceDirection = closestFutureDeltaPos * -1;
            output = new MovementOutput();
            output.linear = avoidanceDirection.normalized * maxAcceleration;
            return output;
                }

    }
}
