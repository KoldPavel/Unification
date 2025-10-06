using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RpsRuntime;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Unification
{
    [Transaction(TransactionMode.Manual)]
    public class UnificCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                string scriptPath = @"E:\Python\PythonProjects\ForRevit\MyPythonScript.py";

                if (!File.Exists(scriptPath))
                {
                    TaskDialog.Show("Ошибка", $"Не найден скрипт: {scriptPath}");
                    return Result.Failed;
                }

                // создаём Python runtime
                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();

                // можно пробросить данные Revit внутрь Python
                scope.SetVariable("commandData", commandData);
                scope.SetVariable("uiapp", commandData.Application);
                scope.SetVariable("app", commandData.Application.Application);
                scope.SetVariable("doc", commandData.Application.ActiveUIDocument.Document);

                engine.ExecuteFile(scriptPath, scope);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка при запуске Python", $"{ex.Message}\n{ex.StackTrace}");
                return Result.Failed;
            }
        }
    }
}