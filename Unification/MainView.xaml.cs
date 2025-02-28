using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using MahApps.Metro.Controls;
using MahApps.Metro;
using System.Diagnostics;

namespace Unification
{
    public partial class MainView : MetroWindow
    {

        //private ExternalCommandData _commandData;
        private UIApplication uiApp;
        private UIDocument uiDoc;
        private Document doc;

        // ExternalEvent и его обработчик
        private ExternalEventHandler _externalEventHandler;
        private ExternalEvent _externalEvent;
        public MainView(ExternalCommandData commandData)
        {
            InitializeComponent();
            //_commandData = commandData;
            TopmostCheckBox.IsChecked = this.Topmost;
            uiApp = commandData.Application;
            uiDoc = uiApp.ActiveUIDocument;
            doc = uiDoc.Document;

            // Инициализация ExternalEvent
            _externalEventHandler = new ExternalEventHandler();
            _externalEvent = ExternalEvent.Create(_externalEventHandler);

            
            // Центрируем окно
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }
        private void OpenWebsite_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://docs.google.com/document/d/1_64M_zX3mY2zJxrjzx5gwg7dxCaZqhUAaC6LFHMSOnI/",
                UseShellExecute = true
            });
        }
        private void TopmostCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void TopmostCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            _externalEventHandler.SetAction(app =>
            {
                try
                {

                    //  Предварительно надо выбрать элементы в Ревит
                    Selection selection = uiDoc.Selection;
                    ICollection<ElementId> selectedIds = uiDoc.Selection.GetElementIds();

                    if (0 == selectedIds.Count)
                    {
                        // Если нет выбранных.
                        TaskDialog.Show("Revit", "Не выбрано ни одного элемента.");
                    }
                    else
                    {
                        using (Transaction transaction = new Transaction(doc))
                        {
                            transaction.Start("Унификация");
                            List<Element> Instances = get_selected_elements();
                            List<Element> Elems = new List<Element>();
                            //String info = "Выбранные элементы: ";
                            foreach (FamilyInstance inst in Instances)
                            {
                                //double L = GetLength(inst);
                                //TaskDialog.Show("Приветсвие", GetLength(inst).ToString());
                                //FamilyInstance parentFam = inst.SuperComponent as FamilyInstance;
                                //ChangeLength(inst);
                                if (inst.SuperComponent != null && inst.GetSubComponentIds().Count == 0)
                                {
                                    FamilyInstance parentFam = inst.SuperComponent as FamilyInstance;
                                    if (FamilyName(parentFam).Contains("E-SUM"))
                                    {
                                        ChangeESUM(inst, parentFam);
                                    }
                                    else if (FamilyName(parentFam).Contains("E-SHP"))
                                    {
                                        ChangeESHP(inst, parentFam);
                                        //TaskDialog.Show("Приветсвие", FamilyName(parentFam));
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else if (inst.SuperComponent == null && inst.GetSubComponentIds().Count == 0)
                                {
                                    ChangeLength(inst);
                                    //TaskDialog.Show("Приветсвие", RebsLenghts()[1].ToString());
                                }
                                else if (inst.SuperComponent == null && inst.GetSubComponentIds().Count != 0)
                                {
                                    ChangeESHPParant(inst);
                                }

                                else
                                {
                                    continue;
                                }
                            }
                            transaction.Commit();
                        }

                    }

                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Ошибка", $"Произошла ошибка: {ex.Message}");

                }
            });
            // Запускаем ExternalEvent
            _externalEvent.Raise();

        }
        private List<int> RebsLenghts()
        {

            List<int> RebLengthsList = new List<int>
        {
          1300, 1650, 1950, 2300, 2900, 3300, 3900, 4600,
          5200, 5850, 6500, 7100, 7800, 8800, 9400, 9750,
          10400, 11700
        };
            //string list = "3000,5000,10000";
            List<int> RebLengths;

            if (User_lenghts_check.IsChecked == false)
            {
                RebLengths = RebLengthsList;
            }
            else
            {
                RebLengths = User_lenghts.Text.Split(',').Select(int.Parse).ToList();
            }

            return RebLengths;
        }
        private List<Element> get_selected_elements()
        {
            Selection selection = uiDoc.Selection;
            List<ElementId> selection_ids = selection.GetElementIds().ToList();
            List<Element> elements = new List<Element>();
            foreach (ElementId element_id in selection_ids)
            {
                elements.Add(doc.GetElement(element_id));
            }
            return elements;
        }
        private FamilyInstance ParantInstance(FamilyInstance nested) //добавить проверку на категорию
        {
            if (nested == null)
                return null;

            if (nested.Host != null)
            {
                return nested.Host as FamilyInstance;
            }
            else
            {
                return nested.SuperComponent as FamilyInstance;
            }

        }
        private string FamilyName(FamilyInstance inst)
        {
            Family elFam = inst.Symbol.Family;
            return elFam.Name;
            

        }
        private double GetLength(FamilyInstance inst)
        {
            Parameter ParamLength = inst.LookupParameter("• Длина");
            return Math.Round(ParamLength.AsDouble() / 0.003280839895);
        }

        private double GetHeight(FamilyInstance inst)
        {
            Parameter ParamLength = inst.LookupParameter("Высота");
            return Math.Round(ParamLength.AsDouble() / 0.003280839895);
        }
        private double GetLengthWork(Element inst)
        {
            Parameter paramLengthWork = inst.LookupParameter("ДлинаРабочая");
            return Math.Round(paramLengthWork.AsDouble() / 0.003280839895);
        }
        private double GetTop(Element inst)
        {
            Parameter paramTop = inst.LookupParameter("Длина верхнего отгиба");
            return Math.Round(paramTop.AsDouble() / 0.003280839895);
        }

        private double GetBot(Element inst)
        {
            Parameter paramBot = inst.LookupParameter("Длина нижнего отгиба");
            return Math.Round(paramBot.AsDouble() / 0.003280839895);
        }

        private double GetBend(Element inst)
        {
            Parameter paramBend = inst.LookupParameter("Длина отгиба");
            return Math.Round(paramBend.AsDouble() / 0.003280839895);
        }
        private int DeltaLength(FamilyInstance inst, List<int> rebLengths)
        {

            if (GetLength(inst) > rebLengths.Max())
            {
                TaskDialog taskDialog = new TaskDialog("Ошибка");
                taskDialog.MainInstruction = "Длина стержня = " + GetLength(inst) +
                    " больше максимального значения в списке = " + rebLengths.Max();
                taskDialog.Show();
            }

            int L = 0;
            double length = GetLength(inst);

            if (!rebLengths.Contains((int)length)) // Совпадает ли со списком длин
            {
                while (rebLengths[L] < length)
                {
                    L++;
                }

                int nearest = rebLengths[L];
                int delta = nearest - (int)length;
                return delta;
            }
            else
            {
                return 0;
            }
        }

        private int ChangeLength(FamilyInstance inst)
        {
            int delta = DeltaLength(inst, RebsLenghts());

            if (FamilyName(inst).Contains("E-SHP-21 - П-стержень"))
            {
                Parameter paramTop = inst.LookupParameter("Длина верхнего отгиба");
                Parameter paramBot = inst.LookupParameter("Длина нижнего отгиба");

                if (GetTop(inst) == GetBot(inst))
                {
                    paramTop.Set((GetTop(inst) + delta / 2) * 0.003280839895);
                    paramBot.Set((GetTop(inst) + delta / 2) * 0.003280839895);
                }
                else
                {
                    if (LongOrShort.IsChecked == false)
                    {
                        if (GetTop(inst) > GetBot(inst))
                        {
                            paramTop.Set((GetTop(inst) + delta) * 0.003280839895);
                        }
                        else
                        {
                            paramBot.Set((GetBot(inst) + delta) * 0.003280839895);
                        }
                    }
                    else if (LongOrShort.IsChecked == true)
                    {
                        if (GetTop(inst) < GetBot(inst))
                        {
                            paramTop.Set((GetTop(inst) + delta) * 0.003280839895);
                        }
                        else
                        {
                            paramBot.Set((GetBot(inst) + delta) * 0.003280839895);
                        }
                    }
                }
            }
            else if (FamilyName(inst).Contains("E-SHP-11 - Г-стержень"))
            {
                Parameter paramHeight = inst.LookupParameter("Высота");
                Parameter paramLengthWork = inst.LookupParameter("ДлинаРабочая");

                if (LongOrShort.IsChecked == false)
                {
                    if (GetHeight(inst) >= GetLengthWork(inst))
                    {
                        paramHeight.Set((GetHeight(inst) + delta) * 0.003280839895);
                    }
                    else
                    {
                        paramLengthWork.Set((GetLengthWork(inst) + delta) * 0.003280839895);
                    }
                }
                else
                {
                    if (GetHeight(inst) <= GetLengthWork(inst))
                    {
                        paramHeight.Set((GetHeight(inst) + delta) * 0.003280839895);
                    }
                    else
                    {
                        paramLengthWork.Set((GetLengthWork(inst) + delta) * 0.003280839895);
                    }
                }
            }
            else if (FamilyName(inst).Contains("E-SHP") && FamilyName(inst).Contains("Хомут"))
            {
                Parameter paramBend = inst.LookupParameter("Длина отгиба");
                double bend = GetBend(inst);
                paramBend.Set((bend + delta / 2) * 0.003280839895);
            }

            return delta;
        }
        private int ChangeESUM(FamilyInstance inst, FamilyInstance parent)
        {
            if (FamilyName(inst).Contains("E-SHP-01 - Точка"))
            {
                Parameter paramVypusk = parent.LookupParameter("ДлинаВыпускВертАрм(ЗаполниДлину)");
                double valueVypusk = paramVypusk.AsDouble() / 0.003280839895;

                Parameter paramVypuskTorec = parent.LookupParameter("ДлинаВыпускВертАрмТорца(ЗаполниДлину)");
                double valueVypuskTorec = paramVypuskTorec.AsDouble() / 0.003280839895;

                XYZ parentLocation = ((LocationCurve)parent.Location).Curve.GetEndPoint(0);
                double parentX = parentLocation.X;
                double parentY = parentLocation.Y;

                XYZ instLocation = ((LocationPoint)inst.Location).Point;
                double instX = instLocation.X;
                double instY = instLocation.Y;

                int dimens = (int)Math.Round(Math.Pow(Math.Pow(instX - parentX, 2) + Math.Pow(instY - parentY, 2), 0.5) * 1000000);
                int delta = DeltaLength(inst, RebsLenghts());

                if (FamilyName(parent).Contains("E-SUM-01") || FamilyName(parent).Contains("E-SUM-05") || FamilyName(parent).Contains("E-SUM-06"))
                {
                    if (dimens == 2041451 || dimens == 2042101 || dimens == 1866222)
                    {
                        paramVypusk.Set((valueVypusk + delta) * 0.003280839895);
                    }
                    else if (dimens == 282229 || dimens == 352213)
                    {
                        paramVypuskTorec.Set((valueVypuskTorec + delta) * 0.003280839895);
                    }
                }
                else if (FamilyName(parent).Contains("E-SUM-02"))
                {
                    if (dimens == 1840010)
                    {
                        paramVypusk.Set((valueVypusk + delta) * 0.003280839895);
                    }
                }
                else if (FamilyName(parent).Contains("E-SUM-08"))
                {
                    Parameter paramVypusk2ryad = parent.LookupParameter("ДлинаВыпускВертАрм2ряд(ЗаполниДлину)");
                    double valueVypusk2ryad = paramVypusk2ryad.AsDouble() / 0.003280839895;

                    Parameter paramVypuskTorec2ryad = parent.LookupParameter("ДлинаВыпускВертАрмТорца2ряд(ЗаполниДлину)");
                    double valueVypuskTorec2ryad = paramVypuskTorec2ryad.AsDouble() / 0.003280839895;

                    if (dimens == 2189686)
                    {
                        paramVypusk.Set((valueVypusk + delta) * 0.003280839895);
                    }
                    else if (dimens == 282229 || dimens == 352213)
                    {
                        paramVypuskTorec.Set((valueVypuskTorec + delta) * 0.003280839895);
                    }
                }

                return dimens;
            }
            if (FamilyName(inst).Contains("E-SHP-21 - П-стержень"))
            {
                Parameter paramDrugayaDlinaBool = parent.LookupParameter("ГорПкаДругаяДлинаВкл");
                int valueDrugayaDlinaBool = paramDrugayaDlinaBool.AsInteger();

                Parameter paramDrugayaDlina = parent.LookupParameter("ГорПкаДругаяДлина");
                double drugayaDlina = paramDrugayaDlina.AsDouble();

                int delta = DeltaLength(inst, RebsLenghts());

                if (GetTop(inst) == GetBot(inst))
                {
                    paramDrugayaDlina.Set((GetTop(inst) + delta / 2) * 0.003280839895);
                }
                else if (GetTop(inst) < GetBot(inst))
                {
                    paramDrugayaDlina.Set((GetTop(inst) + delta) * 0.003280839895);
                }
                else if (GetTop(inst) > GetBot(inst))
                {
                    paramDrugayaDlina.Set((GetBot(inst) + delta) * 0.003280839895);
                }

                paramDrugayaDlinaBool.Set(1);
                return delta;
            }
            else if (FamilyName(inst).Contains("E-SHP-11 - Г-стержень"))
            {
                if (FamilyName(parent).Contains("E-SUM-04-Вертикальное армирование (Г-ки)"))
                {
                    Parameter paramAnkerVplitu = parent.LookupParameter("АнкеровкаВплиту");
                    Parameter paramAnkerVplitu2Ryad = parent.LookupParameter("АнкеровкаВплиту2ряд");
                    Parameter paramAnkerVplituTorec = parent.LookupParameter("АнкеровкаВплитуТорца");
                    Parameter paramAnkerVplitu2Torec = parent.LookupParameter("АнкеровкаВплитуТорца2ряд");
                    Parameter paramVysota = inst.LookupParameter("Высота");

                    XYZ parentLocation = ((LocationCurve)parent.Location).Curve.GetEndPoint(0);
                    double parentX = parentLocation.X;
                    double parentY = parentLocation.Y;

                    XYZ instLocation = ((LocationCurve)inst.Location).Curve.GetEndPoint(1);
                    double instX = instLocation.X;
                    double instY = instLocation.Y;

                    int dimens = (int)Math.Round(Math.Sqrt(Math.Pow(instX - parentX, 2) + Math.Pow(instY - parentY, 2)) * 1000000);
                    int delta = DeltaLength(inst, RebsLenghts());

                    if (dimens == 2072993)
                    {
                        paramAnkerVplitu2Torec.Set((GetHeight(inst) + delta) * 0.003280839895);
                    }
                    else if (dimens == 2255120)
                    {
                        paramAnkerVplituTorec.Set((GetHeight(inst) + delta) * 0.003280839895);
                    }
                    else if (dimens == 3184850)
                    {
                        paramAnkerVplitu2Ryad.Set((GetHeight(inst) + delta) * 0.003280839895);
                    }
                    else if (dimens == 3380482)
                    {
                        paramAnkerVplitu.Set((GetHeight(inst) + delta) * 0.003280839895);
                    }

                    return dimens;
                }
            }
            return 0;


        }
        private int ChangeESHP(FamilyInstance inst, FamilyInstance parent)
        {
            int delta = DeltaLength(inst, RebsLenghts());
            string parentFamilyName = FamilyName(parent);

            if (parentFamilyName.Contains("E-SHP-01 - Дополнительная"))
            {
                Parameter paramA = parent.LookupParameter("a");
                Parameter paramB = parent.LookupParameter("b");
                Parameter paramC = parent.LookupParameter("c");
                double valueA = paramA.AsDouble() / 0.003280839895;
                double valueB = paramB.AsDouble() / 0.003280839895;
                double valueC = paramC.AsDouble() / 0.003280839895;

                if (parentFamilyName.Contains("анкер"))
                {
                    Parameter paramAnKer = parent.LookupParameter("Анкеровка");
                    double ankerovka = paramAnKer.AsDouble() / 0.003280839895;

                    if (valueB == 0 && valueC == 0 && ankerovka == 0)
                    {
                        paramA.Set((valueA + delta) * 0.003280839895);
                    }
                    else if (valueC == 0 && valueB > 0)
                    {
                        if (Math.Round(paramAnKer.AsDouble()) == 0)
                        {
                            if (valueB > valueA)
                            {
                                if (LongOrShort.IsChecked == false)
                                { paramB.Set((valueB + delta) * 0.003280839895); }
                                else
                                { paramA.Set((valueA + delta) * 0.003280839895); }
                            }
                            else
                            {
                                if (LongOrShort.IsChecked == true)
                                { paramA.Set((valueA + delta) * 0.003280839895); }
                                else
                                { paramB.Set((valueB + delta) * 0.003280839895); }
                            }
                        }
                        else
                        {
                            if (valueB > (valueA + ankerovka))
                            {
                                if (LongOrShort.IsChecked == false)
                                { paramB.Set((valueB + delta) * 0.003280839895); }
                                else
                                { paramA.Set((valueA + delta) * 0.003280839895); }
                            }
                            else
                            {
                                if (LongOrShort.IsChecked == true)
                                { paramA.Set((valueA + delta) * 0.003280839895); }
                                else
                                { paramB.Set((valueB + delta) * 0.003280839895); }
                            }
                        }
                    }
                    else if (valueB > 0 && valueC > 0)
                    {
                        if (Math.Round(paramAnKer.AsDouble()) == 0)
                        {
                            if (valueC > valueA)
                            {
                                if (LongOrShort.IsChecked == false)
                                { paramC.Set((valueC + delta) * 0.003280839895); }
                                else
                                { paramA.Set((valueA + delta) * 0.003280839895); }
                            }
                            else if (valueC < valueA)
                            {
                                if (LongOrShort.IsChecked == true)
                                { paramC.Set((valueC + delta) * 0.003280839895); }
                                else
                                { paramA.Set((valueA + delta) * 0.003280839895); }
                            }
                            else if (valueC == valueA)
                            {
                                paramC.Set((valueC + delta / 2) * 0.003280839895);
                                paramA.Set((valueA + delta / 2) * 0.003280839895);
                            }
                        }
                        else
                        {
                            paramA.Set((valueA + delta) * 0.003280839895);
                        }
                    }
                }
                else
                {
                    if (valueB == 0 && valueC == 0)
                    {
                        paramA.Set((valueA + delta) * 0.003280839895);
                    }
                    else if (valueC == 0 && valueB > 0)
                    {
                        if (valueB > valueA)
                        {
                            if (LongOrShort.IsChecked == false)
                            { paramB.Set((valueB + delta) * 0.003280839895); }
                            else
                            { paramA.Set((valueA + delta) * 0.003280839895); }
                        }
                        else
                        {
                            if (LongOrShort.IsChecked == true)
                            { paramA.Set((valueA + delta) * 0.003280839895); }
                            else
                            { paramB.Set((valueB + delta) * 0.003280839895); }
                        }
                    }
                    else if (valueC > 0 && valueB > 0)
                    {
                        if (valueC > valueA)
                        {
                            if (LongOrShort.IsChecked == false)
                            { paramC.Set((valueC + delta) * 0.003280839895); }
                            else
                            { paramA.Set((valueA + delta) * 0.003280839895); }
                        }
                        else
                        {
                            if (LongOrShort.IsChecked == true)
                            { paramC.Set((valueC + delta) * 0.003280839895); }
                            else
                            { paramA.Set((valueA + delta) * 0.003280839895); }
                        }
                    }
                }
            }
            else if (parentFamilyName.Contains("E-SHP-53-Распределение Хомут"))
            {
                Parameter paramOtgib = parent.LookupParameter("Длина отгиба");
                double valueOtgib = paramOtgib.AsDouble() / 0.003280839895;
                Parameter paramVysota = parent.LookupParameter("Высота хомута");
                double valueVysota = paramVysota.AsDouble() / 0.003280839895;

                if (valueOtgib + delta / 2 <= valueVysota)
                { paramOtgib.Set((valueOtgib + delta / 2) * 0.003280839895); }
            }

            return delta;
        }
        private int ChangeESHPParant(FamilyInstance inst)
        {
            int delta = DeltaLength(inst, RebsLenghts());

            if (FamilyName(inst).Contains("E-SHP-01 - Дополнительная"))
            {
                ICollection<ElementId> subElemsIds = inst.GetSubComponentIds();
                List<string> subElemsName = subElemsIds
                    .Select(id => inst.Symbol.Family.Name)
                    .ToList();
                if (subElemsName.Contains("E-SHP-01 - Точка"))
                {
                    Parameter paramLength = inst.LookupParameter("Длина стержней");
                    double valueLength = paramLength.AsDouble() / 0.003280839895;
                    paramLength.Set((valueLength + delta) * 0.003280839895);
                }
            }

            return delta;
        }

    }
}