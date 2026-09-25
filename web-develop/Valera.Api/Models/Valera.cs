using System;

namespace Valera.Api.Models
{
    public class Valera
    {
        public int Health { get; private set; }        // Здоровье 0..100
        public int Mana { get; private set; }          // Мана / алкоголь в крови 0..100
        public int Cheerfulness { get; private set; }  // Жизнерадостность -10..10
        public int Fatigue { get; private set; }       // Усталость 0..100
        public int Money { get; private set; }         // Деньги

        public Valera(
            int health = 100,
            int mana = 0,
            int cheerfulness = 0,
            int fatigue = 0,
            int money = 0)
        {
            Health = Math.Clamp(health, 0, 100);
            Mana = Math.Clamp(mana, 0, 100);
            Cheerfulness = Math.Clamp(cheerfulness, -10, 10);
            Fatigue = Math.Clamp(fatigue, 0, 100);
            Money = money;
        }

        public bool CanGoToWork => Mana < 50 && Fatigue < 10;

        public void GoToWork()
        {
            if (!CanGoToWork)
                throw new InvalidOperationException(
                    "Валера не может пойти на работу: алкоголь >= 50 или усталость >= 10.");

            Cheerfulness -= 5;
            Mana -= 30;
            Money += 100;
            Fatigue += 70;

            Clamp();
        }

        public void ContemplateNature()
        {
            Cheerfulness += 1;
            Mana -= 10;
            Fatigue += 10;

            Clamp();
        }

        public void DrinkWineAndWatchSeries()
        {
            Cheerfulness -= 1;
            Mana += 30;
            Fatigue += 10;
            Health -= 5;
            Money -= 20;

            Clamp();
        }

        public void GoToBar()
        {
            Cheerfulness += 1;
            Mana += 60;
            Fatigue += 40;
            Health -= 10;
            Money -= 100;

            Clamp();
        }

        public void DrinkWithMarginals()
        {
            Cheerfulness += 5;
            Health -= 80;
            Mana += 90;
            Fatigue += 80;
            Money -= 150;

            Clamp();
        }

        public void SingInMetro()
        {
            bool bonus = Mana > 40 && Mana < 70;

            Cheerfulness += 1;
            Mana += 10;
            Money += 10 + (bonus ? 50 : 0);
            Fatigue += 20;

            Clamp();
        }

        public void Sleep()
        {
            if (Mana < 30)
                Health += 90;

            if (Mana > 70)
                Cheerfulness -= 3;

            Mana -= 50;
            Fatigue -= 70;

            Clamp();
        }

        private void Clamp()
        {
            Health = Math.Clamp(Health, 0, 100);
            Mana = Math.Clamp(Mana, 0, 100);
            Cheerfulness = Math.Clamp(Cheerfulness, -10, 10);
            Fatigue = Math.Clamp(Fatigue, 0, 100);
        }
    }
}