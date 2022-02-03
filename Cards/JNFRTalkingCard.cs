using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;

namespace OcCardPack.Cards
{
    class JNFRTalkingCard : PaperTalkingCard
    {
        public static DialogueEvent.Speaker Speaker => (DialogueEvent.Speaker)100;
        public override DialogueEvent.Speaker SpeakerType => Speaker;

        public override string OnDrawnDialogueId => "JnfrDrawn";

        public override string OnDrawnFallbackDialogueId => "JnfrDrawn2";

        public override string OnPlayFromHandDialogueId => "JnfrPlayed";

        public override string OnAttackedDialogueId => "JnfrAttacked";

        public override string OnBecomeSelectablePositiveDialogueId => "JnfrPositiveSelectable";

        public override string OnBecomeSelectableNegativeDialogueId => "JnfrNegativeSelectable";

        public override Dictionary<Opponent.Type, string> OnDrawnSpecialOpponentDialogueIds => new Dictionary<Opponent.Type, string>();

        // optionals
        public override string OnSacrificedDialogueId => "JnfrSacrificed";
        public override string OnSelectedForCardMergeDialogueId => "JnfrCardMerge";
        public override string OnSelectedForCardRemoveDialogueId => "JnfrCardRemove";
        public override string OnSelectedForDeckTrialDialogueId => "JnfrDeckTrial";
        public override string OnDiscoveredInExplorationDialogueId => "JnfrDiscovered";

        // actual strings
        public static Dictionary<string, DialogueEvent> GetDictionary()
        {
            Dictionary<string, DialogueEvent> events = new Dictionary<string, DialogueEvent>();

            AddSimpleDialougeEvent(events,"JnfrDrawn","It's me!");
            AddSimpleDialougeEvent(events,"JnfrDrawn2","Gamer time");
            AddSimpleDialougeEvent(events,"JnfrPlayed","This is the run");
            AddSimpleDialougeEvent(events,"JnfrAttacked","$!@*");
            AddSimpleDialougeEvent(events,"JnfrPositiveSelectable","JNFR time, baby");
            AddSimpleDialougeEvent(events,"JnfrNegativeSelectable","Don't like that");
            AddSimpleDialougeEvent(events,"JnfrSacrificed","Dies Anyways...");
            //AddSimpleDialougeEvent(events,"JnfrCardMerge","Pog");
            AddSimpleDialougeEvent(events,"JnfrDeckTrial","Easy win");
            //AddSimpleDialougeEvent(events,"JnfrDiscovered","Guess who ;)");

            events.Add("JnfrDiscovered", new DialogueEvent()
            {
                id = "JnfrDiscovered",
                speakers = new List<DialogueEvent.Speaker>() { Speaker, DialogueEvent.Speaker.Leshy },
                mainLines = new DialogueEvent.LineSet()
                {
                    lines = new List<DialogueEvent.Line>()
                    {
                        new DialogueEvent.Line { text = "Guess who!" },
                        new DialogueEvent.Line { text = "Oh no, not you...", speakerIndex = 1}
                    }
                }
            });

            events.Add("JnfrCardMerge", new DialogueEvent()
            {
                id = "JnfrCardMerge",
                speakers = new List<DialogueEvent.Speaker>() { Speaker, DialogueEvent.Speaker.Leshy },
                mainLines = new DialogueEvent.LineSet()
                {
                    lines = new List<DialogueEvent.Line>()
                    {
                        new DialogueEvent.Line { text = "Pogchamp" },
                        new DialogueEvent.Line { text = "Silence, you miserable thing", speakerIndex = 1}
                    }
                }
            });

            return events;

        }

        private static void AddSimpleDialougeEvent(Dictionary<string, DialogueEvent> events,string id, string dialouge)
        {
            events.Add(id, new DialogueEvent()
            {
                id = id,
                speakers = new List<DialogueEvent.Speaker>() { Speaker },
                mainLines = new DialogueEvent.LineSet()
                {
                    lines = new List<DialogueEvent.Line>()
                    {
                        new DialogueEvent.Line { text = dialouge }
                    }
                }
            });
        }
    }
}
