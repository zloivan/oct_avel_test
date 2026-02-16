using System;

namespace _Project.Octopus.Scripts.UI
{
    public interface IPopup
    {
        void Show(PopupConfig config);
        void Hide();
    }
    
    public struct PopupConfig
    {
        public string Title;
        public string Body;
        public PopupButton[] Buttons; // 1-5 buttons

        public PopupConfig(string title, string body, params PopupButton[] buttons)
        {
            Title = title;
            Body = body;
            Buttons = buttons;
        }
    }
    
    public struct PopupButton
    {
        public string Text;
        public Action Callback;

        public PopupButton(string text, Action callback)
        {
            Text = text;
            Callback = callback;
        }
    }
}