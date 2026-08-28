using System;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>Petite boite de saisie pour nommer ou renommer un groupe.</summary>
    public class Dialog_GroupName : Window
    {
        private readonly Action<string> onAccept;
        private string current;
        private bool focused;

        public override Vector2 InitialSize => new Vector2(420f, 160f);

        public Dialog_GroupName(string initial, Action<string> onAccept)
        {
            this.onAccept = onAccept;
            current = initial ?? "";
            doCloseX = true;
            forcePause = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            Widgets.Label(new Rect(0f, 0f, inRect.width, 26f), "ArchitectStudio.Dropdowns.GroupNameTitle".Translate());

            GUI.SetNextControlName("ArchitectStudioGroupName");
            current = Widgets.TextField(new Rect(0f, 30f, inRect.width, 30f), current);
            if (!focused)
            {
                UI.FocusControl("ArchitectStudioGroupName", this);
                focused = true;
            }

            var accept = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return;
            if (Widgets.ButtonText(new Rect(inRect.width - 120f, inRect.height - 34f, 120f, 32f), "OK".Translate()) || accept)
            {
                if (accept)
                {
                    Event.current.Use();
                }

                var name = current.Trim();
                if (!name.NullOrEmpty())
                {
                    onAccept(name);
                    Close();
                }
            }
        }
    }
}
