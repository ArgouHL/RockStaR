using UnityEngine;
using System.Collections;

namespace EpicToonFX
{
    public class ETFXLightFade : MonoBehaviour
    {
        [Header("Seconds to dim the light")]
        public float life = 0.2f;
        public bool killAfterLife = true;

        private Light li;
        private float initIntensity;
        private ParticleSystem ps;
       

        // Use this for initialization
        void Start()
        {
            ps = GetComponent<ParticleSystem>();
            if (gameObject.TryGetComponent<Light>(out li))
            {
               
                initIntensity = li.intensity;
                li.intensity = 0;
            }
            else
                print("No light object found on " + gameObject.name);
        }

        // Update is called once per frame
        void Update()
        {
            if (gameObject.GetComponent<Light>())
            {
                li.intensity =initIntensity * ((life-ps.time )/ life);

            }
        }
    }
}