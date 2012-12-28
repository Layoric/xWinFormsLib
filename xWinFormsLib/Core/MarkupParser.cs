using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace xWinFormsLib
{
    public static class MarkupParser
    {
        public static void GenerateFromMarkup(this FormCollection formCollection, XDocument document)
        {
            //Form collection object
            XElement rootNode = document.Root;

            if (rootNode != null)
            {
                var allMarkedupForms = 
                    new List<XElement>(rootNode.Descendants()
                        .Where(x => x.Name.LocalName == "Form"));

                foreach (XElement formElement in allMarkedupForms)
                {
                    var id = GetId(formElement);
                    var title = GetTitle(formElement);
                    var posVector = GetPositionVector2(formElement);
                    var sizeVector = GetSizeVector2(formElement);
                    var borderStyle = GetBorderStyle(formElement);
                    var form = new Form(id, title,sizeVector,posVector,borderStyle);
                    ProcessChildNodes(formElement.Descendants(),ref form);
                    formCollection.Add(form);
                }
            }
        }

        private static void ProcessChildNodes(IEnumerable<XElement> elements, ref Form form)
        {
            foreach (XElement element in elements)
            {
                form.Controls.Add(GetControlFromElement(element));
                if (element.HasElements)
                {
                    ProcessChildNodes(element.Descendants(), ref form);
                }
            }
        }

        private static Control GetControlFromElement(XElement element)
        {
            ControlType controlType;
            Enum.TryParse(element.Name.LocalName, true, out controlType);
            if (controlType != ControlType.None)
            {
                switch (controlType)
                {
                    case ControlType.None:
                        break;
                    case ControlType.Label:
                        string lblId = GetId(element);
                        Vector2 lblposVector = GetPositionVector2(element);
                        var lblwidth = GetWidth(element);
                        var lblforeColor = GetForeColor(element);
                        var lblbackColor = GetBackColor(element);
                        string lblbodyVal = element.Value;
                        var lblalign = GetAlignment(element);
                        var label = new Label(lblId, lblposVector,
                                              lblbodyVal, lblbackColor, lblforeColor,
                                              lblwidth, lblalign);
                        return label;
                    case ControlType.Textbox:
                        string txtId = GetId(element);
                        Vector2 txtposVector = GetPositionVector2(element);
                        int txtwidth = GetWidth(element);
                        int txtheight = GetHeight(element);
                        string txtbodyVal = element.Value;
                        var txtBox = new Textbox(txtId,txtposVector, txtwidth,txtheight, txtbodyVal);
                        return txtBox;
                    case ControlType.Button:
                        string btnId = GetId(element);
                        Vector2 btnposVector = GetPositionVector2(element);
                        int btnwidth = GetWidth(element);
                        Color btnforecolor = GetForeColor(element);
                        Color btnbackColor = GetBackColor(element);
                        string btnbodyVal = element.Value;
                        var button = new Button(btnId, btnposVector, btnwidth, btnbodyVal, btnbackColor, btnforecolor);
                        return button;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            throw new IndexOutOfRangeException("No type matching name '" + controlType + "' found");
        }

        private static int GetHeight(XElement element)
        {
            string heightVal = element.Attributes("height").First().Value;
            int width = int.Parse(heightVal);
            return width;
        }

        private static Label.Align GetAlignment(XElement element)
        {
            string alignVal = element.Attributes("alignment").First().Value;
            var align = (Label.Align) Enum.Parse(typeof (Label.Align), alignVal);
            return align;
        }

        private static Color GetBackColor(XElement element)
        {
            string backColorVal = element.Attributes("backcolor").First().Value;
            Color backColor = GetColorFromAttributeValue(backColorVal);
            return backColor;
        }

        private static Color GetForeColor(XElement element)
        {
            string foreColorVal = element.Attributes("forecolor").First().Value;
            Color foreColor = GetColorFromAttributeValue(foreColorVal);
            return foreColor;
        }

        private static int GetWidth(XElement element)
        {
            string widthVal = element.Attributes("width").First().Value;
            int width = int.Parse(widthVal);
            return width;
        }

        private static Form.BorderStyle GetBorderStyle(XElement formElement)
        {
            var borderVal = formElement.Attributes("style").First().Value;
            var borderStyle = (Form.BorderStyle)Enum.Parse(typeof(Form.BorderStyle), borderVal);
            return borderStyle;
        }

        private static string GetId(XElement formElement)
        {
            var id = formElement.Attributes("id").First().Value;
            return id;
        }

        private static string GetTitle(XElement formElement)
        {
            var title = formElement.Attributes("title").First().Value;
            return title;
        }

        private static Vector2 GetPositionVector2(XElement formElement)
        {
            var posVal = formElement.Attributes("position").First().Value;
            var posVector = GetVectorFromPositionAttributeValue(posVal);
            return posVector;
        }

        private static Vector2 GetSizeVector2(XElement formElement)
        {
            var sizeVal = formElement.Attributes("size").First().Value;
            var sizeVector = GetVectorFromPositionAttributeValue(sizeVal);
            return sizeVector;
        }

        private static Vector2 GetVectorFromPositionAttributeValue(string value)
        {
            string[] val = value.Split(',');
            string posX = val[0];
            string posY = val[1];
            return new Vector2(float.Parse(posX), float.Parse(posY));
        }

        private static Color GetColorFromAttributeValue(string value)
        {
            Type myType = typeof(Color);
            PropertyInfo[] properties = myType.GetProperties(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            IEnumerable<PropertyInfo> colorProps =
                properties.Where(x => x.PropertyType == typeof(Color) && x.Name == value);

            return (Color)colorProps.First().GetValue(myType, null);
        }
    }

    public enum ControlType
    {
        None,
        Label,
        Textbox,
        Button
    }
}