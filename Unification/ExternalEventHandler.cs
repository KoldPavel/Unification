using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Unification
{
    public class ExternalEventHandler : IExternalEventHandler
    {
        private Action<UIApplication> _action;

        public void SetAction(Action<UIApplication> action)
        {
            _action = action;
        }

        public void Execute(UIApplication app)
        {
            // Выполняем действие в контексте Revit API
            _action?.Invoke(app);
        }

        public string GetName()
        {
            return "ExternalEventHandler";
        }
    }
}