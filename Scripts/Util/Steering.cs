using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;

public static class Steering
{
    #region Do Nothing

    public static void DoNothing(Agent agent)
    {
        agent.goal.position = agent.transform.position;
    }

    #endregion
    
    #region Seek

    public static void Seek(Agent agent, Vector3 target)
    {
        agent.goal.position = target;
    }
    
    public static void SeekBall(Agent agent)
    {
        agent.goal.position = Ball.instance.transform.position;
    }

    #endregion

    #region Flee

    public static void Flee(Agent agent, float fleeDistance)
    {
        var linear = agent.transform.position - agent.goal.position;
        linear.Normalize();
        linear *= agent.aiPath.maxSpeed * fleeDistance;
        agent.goal.position = linear;
    }

    public static void FleeBall(Agent agent, float fleeDistance)
    {
        agent.goal.position = Ball.instance.transform.position;
        Flee(agent, fleeDistance);
    }

    #endregion

    #region Pursue

    public static void PursueBall(Agent agent, float maxPredict)
    {
        agent.goal.position = Ball.instance.transform.position;
        var direction = agent.goal.position - agent.transform.position;
        var distance = direction.magnitude;
        var speed = agent.rb.velocity.magnitude;
            
        float prediction;

        if (speed <= distance / maxPredict)
            prediction = maxPredict;
        else
            prediction = distance / speed;

        var velocity = Ball.instance.rb.velocity;
        velocity.Normalize();
        agent.goal.position += velocity * prediction;
    }
    
    public static void PursueAgent(Agent agent, Agent targetAgent, float maxPredict)
    {
        agent.goal.position = targetAgent.transform.position;
        var direction = agent.goal.position - agent.transform.position;
        var distance = direction.magnitude;
        var speed = agent.rb.velocity.magnitude;
            
        float prediction;

        if (speed <= distance / maxPredict)
            prediction = maxPredict;
        else
            prediction = distance / speed;

        var velocity = targetAgent.rb.velocity;
        velocity.Normalize();
        agent.goal.position += velocity * prediction;
        agent.destSetter.target = agent.goal;
    }

    #endregion

    #region Evade

    public static void EvadeBall(Agent agent, float lookAhead, float maxPrediction)
    {
        agent.goal.position = agent.transform.position + new Vector3(0,0, lookAhead);
        var direction = Ball.instance.transform.position - agent.goal.position;
        var distance = direction.magnitude;
        var speed = agent.rb.velocity.magnitude;

        var prediction = 0f;

        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else
            prediction = distance / speed;

        var targetVelocity = Ball.instance.rb.velocity;
        targetVelocity.Normalize();
        agent.goal.position += targetVelocity * prediction;
    }

    public static void EvadeAgent()
    {
        //
    }

    #endregion

    #region Wander

    public static void Wander(Agent agent, float wanderAmount, float wanderRadius, float wanderRate)
    {
        var wanderOffset = agent.transform.forward * wanderAmount;
        Vector3 targetPos;
        targetPos = wanderOffset +
                    new Vector3
                    (Random.Range(-1.0f, 1.0f) * wanderRate, 
                        0, 
                        Random.Range(-1.0f, 1.0f) * wanderRate);
        targetPos *= wanderRadius;
        var targetLocal = targetPos;
        var targetWorld = agent.gameObject.transform.InverseTransformVector(targetLocal);
        agent.goal.position = targetLocal;
    }

    #endregion
}
