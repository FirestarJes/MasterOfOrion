using System;
using DigitalRune.Game.Input;
using DigitalRune.Game.UI.Controls;

namespace MasterOfCentauri.UIControls.Base
{
    public class ClickableContentControl : ContentControl
    {
        public EventHandler Clicked;

        protected override void OnHandleInput(InputContext context)
        {
            base.OnHandleInput(context);

            IsClicked = false;

            if (InputService.IsDown(MouseButtons.Left) && IsMouseOver && IsDown == false)
            {
                InputService.IsMouseOrTouchHandled = true;
                IsClicked = true;
            }

            IsDown = InputService.IsDown(MouseButtons.Left);

            if (IsClicked && Clicked != null)
                Clicked(this, EventArgs.Empty);
        }

        protected bool IsClicked { get; set; }
        protected bool IsDown { get; set; }
    }
}