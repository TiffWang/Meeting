using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace BusinessTier
{
    /// <summary>
    /// The class of binding tool
    /// </summary>
    public class Binding
    {
        /// <summary>
        /// Bind list control such as BulletedList, CheckBoxList, DropDownList, ListBox, RadioButtonList
        /// </summary>
        /// <param name="control">List control</param>
        /// <param name="dataSource">Data source</param>
        public static void Fill(ListControl control, Dictionary<string, string> dataSource)
        {
            Fill(control, null, dataSource);
        }

        /// <summary>
        /// Bind list control such as BulletedList, CheckBoxList, DropDownList, ListBox, RadioButtonList
        /// </summary>
        /// <param name="control">List control</param>
        /// <param name="leader">Selection leader</param>
        /// <param name="dataSource">Data source</param>
        public static void Fill(ListControl control, string leader, Dictionary<string, string> dataSource)
        {
            control.Items.Clear();
            if (leader != null)
                control.Items.Add(new ListItem(leader, String.Empty));
            foreach (KeyValuePair<string, string> item in dataSource)
                control.Items.Add(new ListItem(item.Value, item.Key));
        }

        /// <summary>
        /// 填充DropDownList
        /// </summary>
        /// <param name="drop">控件名称</param>
        /// <param name="list">数据</param>
        /// <param name="text">显示字段</param>
        /// <param name="value">值</param>
        public static void DropFill(DropDownList drop, object list, string text, string value)
        {
            drop.DataSource = list;
            drop.DataTextField = text;
            drop.DataValueField = value;
            drop.DataBind();
        }

        /// <summary>
        /// 绑定枚举
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="enumType"></param>
        public static void BindDropDownList(DropDownList dropDownList, Type enumType)
        {
            dropDownList.Items.Clear();
            foreach (int i in Enum.GetValues(enumType))
            {
                ListItem listitem = new ListItem(Enum.GetName(enumType, i), i.ToString());
                dropDownList.Items.Add(listitem);
            }
        }

        /// <summary>
        /// Make an Item in a  List Control(such as combox, DropDownListbox...) to selected
        /// </summary>
        /// <param name="control">The List Control</param>
        /// <param name="value">Value of the Item to be selected</param>
        public static void SelectItemByValue(ListControl control, object value)
        {
            if (control != null && value != null)
                control.SelectedIndex = control.Items.IndexOf(control.Items.FindByValue(value.ToString()));
        }
    }
}
