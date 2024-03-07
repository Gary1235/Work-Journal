using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class AutoMappingExtense
    {
        public static M AutoMapping<T, M>(this T objt, M objm)
        {
            Type tType = typeof(T);
            Type mType = typeof(M);

            FieldInfo[] tFields = tType.GetFields();
            FieldInfo[] mFields = mType.GetFields();

            foreach (var field in tFields)
            {
                var mField = mFields.Where(x => x.Name == field.Name).FirstOrDefault();
                if (mField == null)
                {
                    continue;
                }

                if (field.FieldType == mField.FieldType)
                {
                    if (field.FieldType == typeof(string))
                    {
                        field.SetValue(objt, mField.GetValue(objm));
                    }
                    else if (field.FieldType == typeof(int))
                    {
                        field.SetValue(objt, mField.GetValue(objm));
                    }
                    else if (field.FieldType == typeof(bool))
                    {
                        field.SetValue(objt, mField.GetValue(objm));
                    }
                    else if (field.FieldType == typeof(Enum))
                    {
                        field.SetValue(objt, mField.GetValue(objm));
                    }
                }
            }

            return objm;
        }
    }
}
