using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEngine.EventSystems
{
    public abstract class UIBehaviour : MonoBehaviour { protected UIBehaviour() => throw new NotSupportedException(); }
}
namespace UnityEngine.UI
{
    public abstract class Graphic : EventSystems.UIBehaviour
    {
        protected Graphic() => throw new NotSupportedException();
        public virtual Color color { set => throw new NotSupportedException(); }
        public virtual bool raycastTarget { set => throw new NotSupportedException(); }
    }
    public abstract class MaskableGraphic : Graphic { protected MaskableGraphic() => throw new NotSupportedException(); }
    public class Image : MaskableGraphic { private Image() { } }
    public class Text : MaskableGraphic
    {
        private Text() { }
        public Font font { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public int fontSize { set => throw new NotSupportedException(); }
        public virtual string text { set => throw new NotSupportedException(); }
        public TextAnchor alignment { set => throw new NotSupportedException(); }
    }
    public class Selectable : EventSystems.UIBehaviour
    {
        protected Selectable() => throw new NotSupportedException();
        public Graphic targetGraphic { set => throw new NotSupportedException(); }
        public bool interactable { set => throw new NotSupportedException(); }
    }
    public class Button : Selectable
    {
        private Button() { }
        public class ButtonClickedEvent : UnityEvent { private ButtonClickedEvent() { } }
        public ButtonClickedEvent onClick => throw new NotSupportedException();
    }
}
