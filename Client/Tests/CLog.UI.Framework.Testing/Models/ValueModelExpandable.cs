using System.ComponentModel;

namespace CLog.UI.Framework.Testing.Models
{
    public class ValueModelExpandable : ValueModel
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public object Value { get; set; }

        public override object GetValue()
        {
            return Value;
        }

        public override void SetValue(object value)
        {
            Value = value;
        }
    }
}
