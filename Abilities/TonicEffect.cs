using APIPlugin;
using DiskCardGame;
using System.Collections;

namespace OcCardPack.Abilities
{
    class TonicEffect : AbilityBehaviour
    {
        public const string POT_HEALTH = "PotionHealth";
        public const string POT_POWER = "PotionPower";
        public const string POT_GROWTH = "PotionGrowth";
        public const string POT_FLIGHT = "PotionFlight";
        public const string POT_BLOOD = "PotionBlood";

        public static Ability ability;
		public override Ability Ability => ability;

        public override bool RespondsToSacrifice()
        {
            return true;
        }

        public override IEnumerator OnSacrifice()
        {
            CardModificationInfo cardMod;

            print(Card.Info.name);
            switch (Card.Info.name)
            {
                case POT_HEALTH:
                    cardMod = new CardModificationInfo(0, 2);
                    break;

               case POT_POWER:
                    cardMod = new CardModificationInfo(1, 0);
                    break;

                case POT_GROWTH:
                    cardMod = new CardModificationInfo(Ability.Evolve);
                    break;

                case POT_FLIGHT:
                    cardMod = new CardModificationInfo(Ability.Flying);
                    break;

                case POT_BLOOD:
                    cardMod = new CardModificationInfo(Ability.Sacrificial);
                    break;

                default:
                    cardMod = new CardModificationInfo(1, 1);
                    break;
            }

            // Give a bonus to the card we were sacrificed for
            BoardManager.Instance.currentSacrificeDemandingCard.AddTemporaryMod(cardMod);
            yield break;
        }

        public static NewAbility Create()
		{
			return Utils.AddAbility<TonicEffect>("Tonic", "When this card is sacrificed, the card it was sacrificed for gains a bonus depending on the type of potion sacrificed.");
		}
	}
}
