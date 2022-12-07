using FirstGearGames.Utilities.Editors;
using FirstGearGames.Utilities.Maths;
using UnityEngine;

namespace FirstGearGames.SmoothCameraShaker
{

    [CreateAssetMenu(fileName = "CamShake", menuName = "Data Object/Cam Shake", order = 1)]
    public class ShakeData : ScriptableObject
    {
        public bool Instanced { get; private set; }

        [SerializeField] bool _scaledTime = true;
        [SerializeField] bool _shakeCameras = true;
        [SerializeField] bool _shakeCanvases = true;
        [SerializeField] bool _shakeObjects = true;
        [SerializeField] float _iterationPercent = 1f;
        [SerializeField] float _fadeOutDuration = 0.5f;
        [SerializeField] bool _unlimitedDuration = false;
        [SerializeField] float _totalDuration = 4f;
        [SerializeField] float _fadeInDuration = 0.5f;
        [Range(0f, 1f)] [SerializeField] float _magnitudeNoise = 0.1f;
        [Range(0f, 1f)] [SerializeField] float _roughnessNoise = 0.3f;
        [SerializeField] float _roughness = 7.5f;
        [SerializeField] AnimationCurve _magnitudeCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(1f, 1f) });
        [SerializeField] AnimationCurve _roughnessCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(1f, 1f) });

        public bool ScaledTime
        {
            get { return _scaledTime; }
            private set { _scaledTime = value; }
        }

        public bool ShakeCameras
        {
            get { return _shakeCameras; }
            private set { _shakeCameras = value; }
        }

        public bool ShakeCanvases
        {
            get { return _shakeCanvases; }
            private set { _shakeCanvases = value; }
        }

        public bool ShakeObjects
        {
            get { return _shakeObjects; }
            private set { _shakeObjects = value; }
        }

        public float IterationPercent
        {
            get { return _iterationPercent; }
            private set { _iterationPercent = value; }
        }


        public bool UnlimitedDuration
        {
            get { return _unlimitedDuration; }
            private set { _unlimitedDuration = value; }
        }

        public float TotalDuration
        {
            get { return _totalDuration; }
            private set { _totalDuration = value; }
        }

        private void ValidateTotalDuration(float value, bool alterUnlimitedDuration)
        {
            if (alterUnlimitedDuration)
                UnlimitedDuration = (value < 0f);

            if (!UnlimitedDuration)
                TotalDuration = Mathf.Max(value, FadeInDuration + FadeOutDuration);
            else
                TotalDuration = -1f;
        }

        public float FadeInDuration
        {
            get { return _fadeInDuration; }
            private set { _fadeInDuration = value; }
        }


        public float FadeOutDuration
        {
            get { return _fadeOutDuration; }
            private set { _fadeOutDuration = value; }
        }

        [SerializeField] float _magnitude = 1f;

        public float Magnitude
        {
            get { return _magnitude; }
            set { _magnitude = value; }
        }


        public float MagnitudeNoise
        {
            get { return _magnitudeNoise; }
            private set { _magnitudeNoise = value; }
        }

        public AnimationCurve MagnitudeCurve
        {
            get { return _magnitudeCurve; }
            private set { _magnitudeCurve = value; }
        }

        public float Roughness
        {
            get { return _roughness; }
            private set { _roughness = value; }
        }
        
        public float RoughnessNoise
        {
            get { return _roughnessNoise; }
            private set { _roughnessNoise = value; }
        }
        /// <summary>
        /// Percentage curve applied to roughness over the total duration.
        /// </summary>
        public AnimationCurve RoughnessCurve
        {
            get { return _roughnessCurve; }
            private set { _roughnessCurve = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("Values in either sign which the shake positioning will occur.")]
        [SerializeField]
        private Vector3 _positionalInfluence = new Vector3(1f, 1f, 0f);
        /// <summary>
        /// Values in either sign which the shake positioning will occur.
        /// </summary>
        public Vector3 PositionalInfluence
        {
            get { return _positionalInfluence; }
            private set { _positionalInfluence = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("Positional axes which may be randomly inverted when this ShakeData is instanced.")]
        [SerializeField]
        [BitMask(typeof(InvertibleAxes))]
        private InvertibleAxes _positionalInverts;
        /// <summary>
        /// Positional axes which may be randomly inverted when this ShakeData is instanced.
        /// </summary>
        public InvertibleAxes PositionalInverts
        {
            get { return _positionalInverts; }
            set { _positionalInverts = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("Values in either sign which the shake rotation will occur.")]
        [SerializeField]
        private Vector3 _rotationalInfluence = new Vector3(0f, 0f, 1f);
        /// <summary>
        /// Values in either sign which the shake rotation will occur.
        /// </summary>
        public Vector3 RotationalInfluence
        {
            get { return _rotationalInfluence; }
            private set { _rotationalInfluence = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("Rotational axes which may be randomly inverted when this ShakeData is instanced.")]
        [SerializeField]
        [BitMask(typeof(InvertibleAxes))]
        private InvertibleAxes _rotationalInverts = 0;
        /// <summary>
        /// Rotational axes which may be randomly inverted when this ShakeData is instanced.
        /// </summary>
        public InvertibleAxes RotationalInverts
        {
            get { return _rotationalInverts; }
            set { _rotationalInverts = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("While checked a new starting position and direction is used with every shake; shakes are more randomized. If unchecked shakes are guaranteed to start at the same position, and move the same direction with every shake; configured curves and noise are still applied.")]
        [SerializeField]
        private bool _randomSeed = true;
        /// <summary>
        /// While checked a new starting position and direction is used with every shake; shakes are more randomized. If unchecked shakes are guaranteed to start at the same position, and move the same direction with every shake; configured curves and noise are still applied.
        /// </summary>
        public bool RandomSeed
        {
            get { return _randomSeed; }
            private set { _randomSeed = value; }
        }

        #region Private.
        /* Be sure to add [System.NonSerialized] to private fields
         * which should not be serialized. ScriptableObjects have a tendacy
         * of serializing fields that aren't explicitly marked as non serialized. */
        #endregion

        #region Const.
        /// <summary>
        /// Text to display when an action cannot be performed due to the ShakeData not being instanced.
        /// </summary>
        private const string ACTION_INSTANCE_REQUIRED = "ShakeData is not instanced. You must use ShakeData.CreateInstance to create an instance before using this action.";
        /// <summary>
        /// Minimum time to fade a shake in to force smoothing.
        /// </summary>
        private const float SMOOTH_IN_DURATION = 0.05f;
        /// <summary>
        /// Minimum time to fade a shake out to force smoothing.
        /// </summary>
        private const float SMOOTH_OUT_DURATION = 0.125f;
        #endregion

        /// <summary>
        /// Initializes this data to ensure values make sense. This method is for internal use only.
        /// </summary>
        internal void Initialize()
        {
            Magnitude = Mathf.Max(0.01f, Magnitude);
            Roughness = Mathf.Max(0.01f, Roughness);
            SetFadeInDuration(FadeInDuration);
            SetFadeOutDuration(FadeOutDuration);
            ValidateTotalDuration(TotalDuration, false);
        }

        #region API.
        /// <summary>
        /// Sets a new fade out duration.
        /// </summary>
        /// <param name="value">New fade out duration.</param>
        public void SetFadeOutDuration(float value)
        {
            if (!InstancedWithDebug())
                return;

            /* Code not used as it needs further testing. Additionally,
             * results acheived via this modification likely won't prove noticeable. */
            //bool curveFadesOut;
            ////Only compare against curve if has multiple keys.
            //if (MagnitudeCurve.length > 1)
            //{
            //    float curveDuration  = MagnitudeCurve.keys[MagnitudeCurve.length - 1].time;
            //    float endPercent = MagnitudeCurve.Evaluate(curveDuration);
            //    //Consider 5% or less at end fading out.
            //    curveFadesOut = (endPercent <= 0.05f);
            //}
            ////Not enough curve data.
            //else
            //{
            //    curveFadesOut = false;
            //}

            ///* If curve already fades out then use whatever value
            // * is set. */
            //if (curveFadesOut)
            //    FadeOutDuration = value;
            ///* If curve does not fade out then determine if
            // * fading in needs to be applied. */
            //else
            //    FadeOutDuration = Mathf.Max(SMOOTH_OUT_DURATION, value);

            FadeOutDuration = Mathf.Max(SMOOTH_OUT_DURATION, value);
        }


        /// <summary>
        /// Sets a new fade in duration.
        /// </summary>
        /// <param name="value">New fade in duration.</param>
        public void SetFadeInDuration(float value)
        {
            if (!InstancedWithDebug())
                return;

            /* Code not used as it needs further testing. Additionally,
            * results acheived via this modification likely won't prove noticeable. */
            //bool curveFadesIn;
            ////Only compare against curve if has multiple keys.
            //if (MagnitudeCurve.length > 1)
            //{
            //    float startPercent = MagnitudeCurve.Evaluate(0f);
            //    //Consider 10% or less at start fading in.
            //    curveFadesIn = (startPercent <= 0.1f);
            //}
            ////Not enough curve data.
            //else
            //{
            //    curveFadesIn = false;
            //}

            ///* If curve already fades in then use whatever value
            // * is set. */
            //if (curveFadesIn)
            //    FadeInDuration = value;
            ///* If curve does not fade in then determine if
            // * fading in needs to be applied. */
            //else
            //    FadeInDuration = Mathf.Max(SMOOTH_IN_DURATION, value);

            FadeInDuration = Mathf.Max(SMOOTH_IN_DURATION, value);
        }


        /// <summary>
        /// Sets a new TotalDuration value. Setting this value to 0 or greater removes unlimited duration; just as setting it to less than 0 sets unlimited duration.
        /// </summary>
        /// <param name="value">New total duration. Using values 0f and lower will make duration unlimited.</param>
        public void SetTotalDuration(float value)
        {
            if (!InstancedWithDebug())
                return;
            ValidateTotalDuration(value, true);
        }

        /// <summary>
        /// Sets a new ShakeCameras value.
        /// </summary>
        /// <param name="value"></param>
        public void SetShakeCameras(bool value)
        {
            if (!InstancedWithDebug())
                return;
            ShakeCameras = value;
        }

        /// <summary>
        /// Sets a new ShakeCanvases value.
        /// </summary>
        /// <param name="value"></param>
        public void SetShakeCanvases(bool value)
        {
            if (!InstancedWithDebug())
                return;
            ShakeCanvases = value;
        }

        /// <summary>
        /// Sets a new ShakeRigidbodies value.
        /// </summary>
        /// <param name="value"></param>
        public void SetShakeRigidbodies(bool value)
        {
            if (!InstancedWithDebug())
                return;
            ShakeObjects = value;
        }

        /// <summary>
        /// Creates and returns an instance of this data.
        /// </summary>
        /// <returns></returns>
        public ShakeData CreateInstance()
        {
            ShakeData data = ScriptableObject.CreateInstance<ShakeData>();
            data.SetInstancedWithProperties(ScaledTime, ShakeCameras, ShakeCanvases, ShakeObjects, UnlimitedDuration, Magnitude, MagnitudeNoise, MagnitudeCurve,
                Roughness, RoughnessNoise, RoughnessCurve, TotalDuration, FadeInDuration, FadeOutDuration,
                PositionalInfluence, PositionalInverts, RotationalInfluence, RotationalInverts, RandomSeed);

            return data;
        }

        /// <summary>
        /// Inversts specified positional axes. Using this in the middle of a shake may create jarring the next frame.
        /// </summary>
        /// <param name="axes">Axes to invert.</param>
        public void InvertPositionalAxes(InvertibleAxes axes)
        {
            if (axes.Contains(InvertibleAxes.X))
                _positionalInfluence.x *= -1f;
            if (axes.Contains(InvertibleAxes.Y))
                _positionalInfluence.y *= -1f;
            if (axes.Contains(InvertibleAxes.Z))
                _positionalInfluence.z *= -1f;
        }
        /// <summary>
        /// Inverts specified rotational axes. Using this in the middle of a shake may create jarring the next frame.
        /// </summary>
        /// <param name="axes">Axes to invert.</param>
        public void InvertRotationalAxes(InvertibleAxes axes)
        {
            if (axes.Contains(InvertibleAxes.X))
                _rotationalInfluence.x *= -1f;
            if (axes.Contains(InvertibleAxes.Y))
                _rotationalInfluence.y *= -1f;
            if (axes.Contains(InvertibleAxes.Z))
                _rotationalInfluence.z *= -1f;
        }
        /// <summary>
        /// Randomizes inversion for specified positional axes. Using this in the middle of a shake may create jarring the next frame.
        /// </summary>
        /// <param name="axes">Axes to randomly invert.</param>
        public void RandomlyInvertPositionalAxes(InvertibleAxes axes)
        {
            //If there is anything to invert.
            if ((int)axes != 0)
            {
                //X
                if (axes.Contains(InvertibleAxes.X))
                {
                    float multiplier = Floats.RandomlyFlip(1f);
                    _positionalInfluence.x *= multiplier;
                }
                //Y
                if (axes.Contains(InvertibleAxes.Y))
                {
                    float multiplier = Floats.RandomlyFlip(1f);
                    _positionalInfluence.y *= multiplier;
                }
                //Z
                if (axes.Contains(InvertibleAxes.Z))
                {
                    float multiplier = Floats.RandomlyFlip(1f);
                    _positionalInfluence.z *= multiplier;
                }
            }
        }
        /// <summary>
        /// Randomizes inversion for specified rotational axes. Using this in the middle of a shake may create jarring the next frame.
        /// </summary>
        /// <param name="axes">Axes to randomly invert.</param>
        public void RandomlyInvertRotationalAxes(InvertibleAxes axes)
        {
            //If there is anything to invert.
            if ((int)axes != 0)
            {
                //X
                if (axes.Contains(InvertibleAxes.X))
                {
                    float multiplier = Floats.RandomlyFlip(1f);
                    _rotationalInfluence.x *= multiplier;
                }
                //Y
                if (axes.Contains(InvertibleAxes.Y))
                {
                    float multiplier = Floats.RandomlyFlip(1f);
                    _rotationalInfluence.y *= multiplier;
                }
                //Z
                if (axes.Contains(InvertibleAxes.Z))
                {
                    float multiplier = Floats.RandomlyFlip(1f);
                    _rotationalInfluence.z *= multiplier;
                }
            }
        }
        #endregion

        /// <summary>
        /// Sets instanced to true, and sets serialized properties. This method is for internal use only.
        /// </summary>
        /// <param name="scaledTime"></param>
        /// <param name="magnitude"></param>
        /// <param name="roughness"></param>
        /// <param name="totalDuration"></param>
        /// <param name="fadeInDuration"></param>
        /// <param name="fadeOutDuration"></param>
        /// <param name="positionalInfluence"></param>
        /// <param name="rotationalInfluence"></param>
        internal void SetInstancedWithProperties(bool scaledTime, bool shakeCameras, bool shakeCanvases, bool shakeRigidbodies, bool unlimitedDuration, float magnitude, float magnitudeNoise, AnimationCurve magnitudeCurve, float roughness, float roughnessNoise, AnimationCurve roughnessCurve, float totalDuration, float fadeInDuration, float fadeOutDuration, Vector3 positionalInfluence, InvertibleAxes positionalInverts, Vector3 rotationalInfluence, InvertibleAxes rotationalInverts, bool randomSeed)
        {
            ScaledTime = scaledTime;
            ShakeCameras = shakeCameras;
            ShakeCanvases = shakeCanvases;
            ShakeObjects = shakeRigidbodies;
            UnlimitedDuration = unlimitedDuration;
            Magnitude = magnitude;
            MagnitudeNoise = magnitudeNoise;
            MagnitudeCurve = magnitudeCurve;
            Roughness = roughness;
            RoughnessNoise = roughnessNoise;
            RoughnessCurve = roughnessCurve;
            FadeInDuration = fadeInDuration;
            FadeOutDuration = fadeOutDuration;
            TotalDuration = totalDuration;
            PositionalInfluence = positionalInfluence;
            PositionalInverts = positionalInverts;
            RotationalInfluence = rotationalInfluence;
            RotationalInverts = rotationalInverts;
            RandomSeed = randomSeed;

            RandomlyInvertPositionalAxes(PositionalInverts);
            RandomlyInvertRotationalAxes(RotationalInverts);

            Instanced = true;
        }
        /// <summary>
        /// Returns if instanced and outputs debug when not.
        /// </summary>
        /// <returns></returns>
        private bool InstancedWithDebug()
        {
            if (!Instanced && Debug.isDebugBuild)
                Debug.LogError(ACTION_INSTANCE_REQUIRED);

            return Instanced;
        }

        #region Editor checks.
        private void OnValidate()
        {
            if (!UnlimitedDuration && _totalDuration <= 0f)
                _totalDuration = 1f;
        }
        #endregion


    }

}