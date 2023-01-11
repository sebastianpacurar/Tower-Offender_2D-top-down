using System;
using UnityEngine;

namespace ScriptableObjects {
    [CreateAssetMenu(fileName = "ParticleSystemDataSO", menuName = "SOs/ParticleSystemData")]
    public class ParticleSysSo : ScriptableObject {
        [Header("Main Module")]
        [SerializeField] private float duration;

        [SerializeField] private bool looping;
        [SerializeField] private float startLifetime;
        [SerializeField] private float startSpeed;
        [SerializeField] private float startSize;
        [SerializeField] private Color startColor;

        public float Duration => duration;
        public bool Looping => looping;
        public float StartLifetime => startLifetime;
        public float StartSpeed => startSpeed;
        public float StartSize => startSize;
        public Color StartColor => startColor;


        //TODO: Handle Bursts also
        [Space(20)]
        [Header("Emission Module")]
        [SerializeField] private bool isEmissionEnabled;

        [SerializeField] private float rateOverTime;

        public bool IsEmissionEnabled => isEmissionEnabled;
        public float RateOverTime => rateOverTime;

        [Space(20)]
        [Header("Shape Module")]
        [SerializeField] private bool isShapeEnabled;

        [SerializeField] private ParticleSystemShapeType shapeType;
        [SerializeField] private float angle;
        [SerializeField] private float radius;
        [SerializeField] private float radiusThickness;
        [SerializeField] private float arcVal;
        [SerializeField] private ParticleSystemShapeMultiModeValue arcMode;
        [SerializeField] private float randomizeDirection;

        public bool IsShapeEnabled => isShapeEnabled;
        public ParticleSystemShapeType ShapeType => shapeType;
        public float ShapeAngle => angle;
        public float ShapeRadius => radius;
        public float ShapeRadiusThickness => radiusThickness;
        public float ArcVal => arcVal;
        public ParticleSystemShapeMultiModeValue ArcMode => arcMode;
        public float RandomizeDirection => randomizeDirection;


        [Space(20)]
        [Header("Velocity over Lifetime Module")]
        [SerializeField] private bool isVelocityOverLifetimeEnabled;

        [SerializeField] private Vector3 velOverLifeLinear;

        public bool IsVelocityOverLifetimeEnabled => isVelocityOverLifetimeEnabled;
        public Vector3 VelOverLifeLinear => velOverLifeLinear;


        [Space(20)]
        [Header("Color over Lifetime Module")]
        [SerializeField] private bool isColorOverLifetimeEnabled;

        [SerializeField] private Gradient colOverLifetime = new() {
            alphaKeys = new[] {
                new GradientAlphaKey(0, 0f),
                new GradientAlphaKey(1, 1f)
            },
            colorKeys = new[] {
                new GradientColorKey(Color.red, 1f),
                new GradientColorKey(Color.green, 1f),
                new GradientColorKey(Color.blue, 1f)
            },
            mode = GradientMode.Blend
        };

        public bool IsColorOverLifetimeEnabled => isColorOverLifetimeEnabled;
        public Gradient ColOverLifetime => colOverLifetime;

        [Space(20)]
        [Header("Size over Lifetime Module")]
        [SerializeField] private bool isSizeOverLifetimeEnabled;

        public bool IsSizeOverLifetimeEnabled => isSizeOverLifetimeEnabled;


        [Space(20)] [Header("Renderer Component")] [SerializeField]
        private Material materialObj;

        public Material MaterialObj => materialObj; // this should be "Default-ParticleSystem"
    }
}