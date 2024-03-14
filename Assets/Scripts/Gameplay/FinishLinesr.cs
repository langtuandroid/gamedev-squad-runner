using JetSystems;
using Settings;
using UnityEngine;

namespace Gameplay
{
    public class FinishLinesr : MonoBehaviour
    {
        [SerializeField] 
        private ParticleSystem[] _confettissr;
    
        public void PlayConfettiParticlessr()
        {
            foreach (ParticleSystem particleSystem in _confettissr)
            {
                particleSystem.Play();
            }
               
            UIManager.AddCoins(20);
            AudioManagersr.Instancesr.PlaySFXOneShotsr(1);
        }
        
        private int CalculateSumsr(int a, int b)
        {
            return a + b;
        }

    }
}
