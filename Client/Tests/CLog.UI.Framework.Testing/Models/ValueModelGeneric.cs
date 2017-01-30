using System;

namespace CLog.UI.Framework.Testing.Models
{
    public class ValueModelGeneric<T> : ValueModel
    {
        public T Value { get; set; }

        public override object GetValue()
        {
            return Value;
        }

        public override void SetValue(object value)
        {
            Value = (T)value;
        }
    }
}
