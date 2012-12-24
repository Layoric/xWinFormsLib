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
                    var id = formElement.Attributes("id").First().Value;
                    var title = formElement.Attributes("title").First().Value;
                    var posVal = formElement.Attributes("position").First().Value;
                    var posVector = GetVectorFromPositionAttributeValue(posVal);
                    var sizeVal = formElement.Attributes("size").First().Value;
                    var sizeVector = GetVectorFromPositionAttributeValue(sizeVal);
                    var borderVal = formElement.Attributes("style").First().Value;
                    var borderStyle = (Form.BorderStyle) Enum.Parse(typeof (Form.BorderStyle), borderVal);
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
                        string lblId = element.Attributes("id").First().Value;
                        string lblPosition = element.Attributes("position").First().Value;
                        Vector2 lblposVector = GetVectorFromPositionAttributeValue(lblPosition);
                        string lblwidthVal = element.Attributes("width").First().Value;
                        int lblwidth = int.Parse(lblwidthVal);
                        string lblforeColorVal = element.Attributes("forecolor").First().Value;
                        Color lblforeColor = GetColorFromAttributeValue(lblforeColorVal);
                        string lblbackColorVal = element.Attributes("backcolor").First().Value;
                        Color lblbackColor = GetColorFromAttributeValue(lblbackColorVal);
                        string lblbodyVal = element.Value;
                        string lblalignVal = element.Attributes("alignment").First().Value;
                        var lblalign = (Label.Align) Enum.Parse(typeof (Label.Align), lblalignVal);
                        var label = new Label(lblId, lblposVector,
                                              lblbodyVal, lblbackColor, lblforeColor,
                                              lblwidth, lblalign);
                        return label;
                    case ControlType.Textbox:
                        string txtId = element.Attributes("id").First().Value;
                        string txtPosition = element.Attributes("position").First().Value;
                        Vector2 txtposVector = GetVectorFromPositionAttributeValue(txtPosition);
                        string txtwidthVal = element.Attributes("width").First().Value;
                        int txtwidth = int.Parse(txtwidthVal);
                        string txtbodyVal = element.Value;
                        var txtBox = new Textbox(txtId,txtposVector, txtwidth, txtbodyVal);
                        return txtBox;
                    case ControlType.Button:
                        string btnId = element.Attributes("id").First().Value;
                        string btnPosition = element.Attributes("position").First().Value;
                        Vector2 btnposVector = GetVectorFromPositionAttributeValue(btnPosition);
                        string btnwidthVal = element.Attributes("width").First().Value;
                        int btnwidth = int.Parse(btnwidthVal);
                        string btnforeColorVal = element.Attributes("forecolor").First().Value;
                        Color btnforecolor = GetColorFromAttributeValue(btnforeColorVal);
                        string btnbackColorVal = element.Attributes("backcolor").First().Value;
                        Color btnbackColor = GetColorFromAttributeValue(btnbackColorVal);
                        string btnbodyVal = element.Value;
                        var button = new Button(btnId, btnposVector, btnwidth, btnbodyVal, btnbackColor, btnforecolor);
                        return button;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            throw new IndexOutOfRangeException("No type matching name '" + controlType + "' found");
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
            Type myType = typeof (Color);
            PropertyInfo[] properties = myType.GetProperties(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            IEnumerable<PropertyInfo> colorProps =
                properties.Where(x => x.PropertyType == typeof (Color) && x.Name == value);

            return (Color) colorProps.First().GetValue(myType, null);
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