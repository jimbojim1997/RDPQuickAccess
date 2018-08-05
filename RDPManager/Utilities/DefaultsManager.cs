using System.ComponentModel;
using System.Reflection;

namespace RDPManager.Utilities
{
    public abstract class DefaultsManager
    {
        public DefaultsManager()
        {
            foreach(PropertyInfo property in GetType().GetRuntimeProperties())
            {
                DefaultValueAttribute attr = property.GetCustomAttribute<DefaultValueAttribute>(true);

                if (property.PropertyType == attr.Value.GetType()) property.SetValue(this, attr.Value);
                else throw new InvalidTypeException($"The property '{property.Name}' is being assigned a value of type '{attr.Value.GetType()}' when it's a '{property.PropertyType}'.");
            }
        }
    }
}
