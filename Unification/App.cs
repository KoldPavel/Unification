
using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using Autodesk.Revit.DB.Structure;

namespace Unification
{
    internal class App: IExternalApplication
    {
        private const string TabName = "OSK.Olimproekt";
        private const string PanelName = "OSK";
        private static readonly string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
        private static RibbonPanel ribbonPanel;
        //private static List<PushButtonData> pushButtonsData;


        public Result OnStartup(UIControlledApplication application)
        {
            string location = Assembly.GetExecutingAssembly().Location;
            try
            { 
            ribbonPanel = application.GetRibbonPanels(TabName).FirstOrDefault(p => p.Name == PanelName);
                try
                {
                    application.CreateRibbonTab(TabName);
                }
                catch { }

                if (ribbonPanel == null)
            {
                ribbonPanel = application.CreateRibbonPanel(TabName, PanelName);
            }

            PushButtonData buttonData = new PushButtonData(nameof(UnificCommand), "Унификация", location, typeof(UnificCommand).FullName)
            {
                ToolTip = "Унификация длин стержней с кратностью Олимпроекта или пользовательской",
                Image = new BitmapImage(new Uri(@"pack://application:,,,/Unification;component/Resources/Images/Rebar16.png")),
                LargeImage = new BitmapImage(new Uri(@"pack://application:,,,/Unification;component/Resources/Images/Rebar32.png"))
            };
            
                ribbonPanel.AddItem(buttonData);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error: Unification", ex.StackTrace);
                return Result.Failed;
            }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            try
            {
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error: Unification", ex.StackTrace);
                return Result.Failed;
            }
        }
    }
}