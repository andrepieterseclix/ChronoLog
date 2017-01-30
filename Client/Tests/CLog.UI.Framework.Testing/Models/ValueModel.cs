using System;

namespace CLog.UI.Framework.Testing.Models
{
    public abstract class ValueModel
    {
        public abstract object GetValue();

        public abstract void SetValue(object value);
    }
}
