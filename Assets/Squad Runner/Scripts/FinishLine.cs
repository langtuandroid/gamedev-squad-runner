using System.Collections;
using System.Collections.Generic;
using JetSystems;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [Header(" Particles ")]
    [SerializeField] private ParticleSystem[] confettis;
    
    public void PlayConfettiParticles()
    {
        foreach (ParticleSystem ps in confettis)
            ps.Play();
        UIManager.AddCoins(20);
        Audio_Manager.instance.play("Level_Complete");
    }
}
