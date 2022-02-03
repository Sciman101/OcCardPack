using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OcCardPack.Abilities
{
	public class PotionSeller : AbilityBehaviour
	{
		public static List<CardInfo> potionCardPool = new List<CardInfo>();

		public static Ability ability;
		public override Ability Ability => ability;

        public override bool RespondsToPlayFromHand()
        {
			return true;
        }

		// Give the player a potion
        public override IEnumerator OnPlayFromHand()
        {
			yield return base.PreSuccessfulTriggerSequence();

			List<CardInfo> shopContents = potionCardPool.OrderBy(x => UnityEngine.Random.value).Take(3).ToList();

			// Pick a card!
			Singleton<ViewManager>.Instance.SwitchToView(View.DeckSelection, false, true);
			SelectableCard selectedCard = null;
			yield return Singleton<BoardManager>.Instance.CardSelector.SelectCardFrom(shopContents, (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile, delegate (SelectableCard x)
			{
				selectedCard = x;
			}, null, true);
			Tween.Position(selectedCard.transform, selectedCard.transform.position + Vector3.back * 4f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
			Destroy(selectedCard.gameObject, 0.1f);
			// go back
			Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
			yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(selectedCard.Info, 0.25f);

			// learn ability
			yield return base.LearnAbility(0.5f);
			// make sure user input is unlocked
			Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
			yield break;
		}

        public static NewAbility Create()
		{
			return Utils.AddAbility<PotionSeller>("Potion Seller", "When this card is played, choose a random tonic to enter your hand.");
		}
	}
}
