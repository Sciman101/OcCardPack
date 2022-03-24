using InscryptionAPI;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OcCardPack.Abilities
{
    class DarueAttackTriggerBehaviour : SpecialCardBehaviour
    {

        private bool hasBeenDamaged = false;

        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return !hasBeenDamaged;
        }

        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            hasBeenDamaged = true;
            if (PlayableCard.Health > 0)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.15f);
                PlayableCard.Anim.PlayTransformAnimation();
                // modify!
                yield return new WaitForSeconds(0.15f);
                PlayableCard.renderInfo.portraitOverride = Card.Info.alternatePortrait;
                PlayableCard.RenderCard();
            }

            yield break;
        }
    }
}
