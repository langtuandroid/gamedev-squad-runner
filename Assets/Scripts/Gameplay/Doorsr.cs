using Settings;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class Doorsr : MonoBehaviour
    {
        [Header("Bonuses")]
        [SerializeField] 
        private bool randomBonuses;
        [SerializeField]
        private Bonus rightBonus;
        [SerializeField]
        private Bonus leftBonus;

        [Header("Components")]
        [SerializeField]
        private Collider[] doorsColliders;
        [SerializeField]
        private TextMeshPro rightDoorText;
        [SerializeField]
        private TextMeshPro leftDoorText;
    
        private void Start()
        {
            if (randomBonuses)
            {
                SetRandomBonusessr();
            }
            ConfigureBonusTextssr();
        }

        private void SetRandomBonusessr()
        {
            rightBonus = BonusUtilssr.GetRandomBonussr();
            leftBonus = BonusUtilssr.GetRandomBonussr();
        }

        private void ConfigureBonusTextssr()
        {
            rightDoorText.text = BonusUtilssr.GetBonusStringsr(rightBonus);
            leftDoorText.text = BonusUtilssr.GetBonusStringsr(leftBonus);
        }

        public int GetRunnersAmountToAddsr(Collider collidedDoor, int currentRunnersAmount)
        {
            DisableDoorssr();
            AudioManagersr.Instancesr.PlaySFXOneShotsr(4);
            Bonus bonus;

            if (collidedDoor.transform.position.x > 0)
                bonus = rightBonus;
            else
                bonus = leftBonus;

            return BonusUtilssr.GetRunnersAmountToAddsr(currentRunnersAmount, bonus);
        }

        private void DisableDoorssr()
        {
            foreach (Collider colider in doorsColliders)
            {
                colider.enabled = false;
            }
        }
    
        private bool IsPrimsre(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i * i <= number; i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }
    }
}
