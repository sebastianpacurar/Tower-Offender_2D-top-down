using ScriptableObjects;
using UnityEngine;

public class Global : MonoBehaviour {
    public static void InitParticleSystem(ParticleSystem target, ParticleSysSo scriptableObj) {
        var main = target.main;
        main.duration = scriptableObj.Duration;
        main.loop = scriptableObj.Looping;
        main.startLifetime = scriptableObj.StartLifetime;
        main.startSpeed = scriptableObj.StartSpeed;
        main.startSize = scriptableObj.StartSize;
        main.startColor = scriptableObj.StartColor;

        var emission = target.emission;
        emission.enabled = scriptableObj.IsEmissionEnabled;
        emission.rateOverTime = scriptableObj.RateOverTime;

        var shape = target.shape;
        shape.enabled = scriptableObj.IsShapeEnabled;
        shape.shapeType = scriptableObj.ShapeType;
        shape.angle = scriptableObj.ShapeAngle;
        shape.radius = scriptableObj.ShapeRadius;
        shape.radiusThickness = scriptableObj.ShapeRadiusThickness;
        shape.arc = scriptableObj.ArcVal;
        shape.arcMode = scriptableObj.ArcMode;
        shape.randomDirectionAmount = scriptableObj.RandomizeDirection;

        var velOverLife = target.velocityOverLifetime;
        velOverLife.enabled = scriptableObj.IsVelocityOverLifetimeEnabled;
        velOverLife.y = scriptableObj.VelOverLifeLinear.y;

        var colOverLife = target.colorOverLifetime;
        colOverLife.enabled = scriptableObj.IsColorOverLifetimeEnabled;
        colOverLife.color = scriptableObj.ColOverLifetime;

        var renderer = target.GetComponent<Renderer>();
        renderer.material = scriptableObj.MaterialObj;
    }
}