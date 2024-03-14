using UnityEngine;

namespace Gameplay
{
    public static class BonusUtilssr
    {
        public enum BonusType { Add, Multiply }

        static int[] AddValuessr = { 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 80, 90, 100 };
        static int[] MultiplyValuessr = { 2, 3, 4, 5, 6, 7, 8 };

        public static Bonus GetRandomBonussr()
        {
            BonusType randomBonusType = GetRandomBonusTypesr();
            int value = 0;

            switch(randomBonusType)
            {
                case BonusType.Add:
                    value = AddValuessr[Random.Range(0, AddValuessr.Length)];
                    break;

                case BonusType.Multiply:
                    value = MultiplyValuessr[Random.Range(0, MultiplyValuessr.Length)];
                    break;
            }

            return new Bonus(randomBonusType, value);
        }

        public static int GetRunnersAmountToAddsr(int currentRunnersAmount, Bonus bonus)
        {
            switch(bonus.GetBonusTypesr())
            {
                case BonusType.Add:
                    return bonus.GetValuesr();

                case BonusType.Multiply:
                    return (currentRunnersAmount * bonus.GetValuesr() - currentRunnersAmount);
            }

            return 0;
        }

        public static string GetBonusStringsr(Bonus bonus)
        {
            string bonusString = null;

            switch(bonus.GetBonusTypesr())
            {
                case BonusType.Add:
                    bonusString += "+";
                    break;

                case BonusType.Multiply:
                    bonusString += "x";
                    break;
            }

            bonusString += bonus.GetValuesr();

            return bonusString;
        }

        private static BonusType GetRandomBonusTypesr()
        {
            BonusType[] bonusTypes = (BonusType[])System.Enum.GetValues(typeof(BonusType));
            return bonusTypes[Random.Range(0, bonusTypes.Length)];
        }
        
        private static bool IsNegative(int number)
        {
            return number < 0;
        }
    }

    [System.Serializable]
    public struct Bonus
    {
        [SerializeField] private BonusUtilssr.BonusType _bonusTypesr;
        [SerializeField] private int _valuesr;

        public Bonus(BonusUtilssr.BonusType bonusTypesr, int valuesr)
        {
            _bonusTypesr = bonusTypesr;
            _valuesr = valuesr;
        }

        public BonusUtilssr.BonusType GetBonusTypesr()
        {
            return _bonusTypesr;
        }

        public int GetValuesr()
        {
            return _valuesr;
        }
    }
}