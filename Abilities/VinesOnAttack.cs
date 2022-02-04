using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace OcCardPack.Abilities
{
	public class VinesOnAttack : CreateCardsAdjacent
	{
		public static Ability ability;
		public override Ability Ability => ability;

        public override string SpawnedCardId => "Thorn_Vines";

        public override string CannotSpawnDialogue => "No room for your plant to grow. Shame.";

        private bool hasBeenAttacked = false;

        public override bool RespondsToResolveOnBoard()
        {
            return false;
        }

        public override bool RespondsToTakeDamage(PlayableCard source)
        {
			return !hasBeenAttacked;
        }

        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            hasBeenAttacked = true;
            if (Card.Health > 0)
            {
                yield return OnResolveOnBoard();
            }
        }

        public override IEnumerator OnResolveOnBoard()
        {
			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
			CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
			CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
			bool toLeftValid = toLeft != null && toLeft.Card == null;
			bool toRightValid = toRight != null && toRight.Card == null;
			CardInfo cardToCreate = CardLoader.GetCardByName(this.SpawnedCardId);
			yield return base.PreSuccessfulTriggerSequence();
			if (toLeftValid)
			{
				yield return new WaitForSeconds(0.1f);
				yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardToCreate, toLeft, 0.15f, true);
				// flip card art on left side
				toLeft.Card.renderInfo.flippedPortrait = true;
				toLeft.Card.RenderCard();
			}
			if (toRightValid)
			{
				yield return new WaitForSeconds(0.1f);
				yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardToCreate, toRight, 0.15f, true);
			}
			if (toLeftValid || toRightValid)
			{
				yield return base.LearnAbility(0f);
			}
			else if (!base.HasLearned && (Localization.CurrentLanguage == Language.English || Localization.Translate(this.CannotSpawnDialogue) != this.CannotSpawnDialogue))
			{
				yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(this.CannotSpawnDialogue, -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);
			}
			yield break;
		}

        public static NewAbility Create()
		{
			return Utils.AddAbility<VinesOnAttack>("Vine Grower", "When a card bearing this sigil is played, Thorny Vines are created on each empty adjacent space. Thorny Vines are defined as: 1 Power, 1 Health, Thorns");
		}
	}
}
