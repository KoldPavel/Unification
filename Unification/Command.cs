using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Unification
{
    [Autodesk.Revit.Attributes.TransactionAttribute(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class UnificCommand : IExternalCommand
    {
        static AddInId addinId = new AddInId(new Guid("88DE31C1-FD7C-4C99-BF2A-6A175EDB2791"));

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
           
            MainView mainView = new MainView(commandData);
            mainView.Show();

            return Result.Succeeded;
        }
    }
}