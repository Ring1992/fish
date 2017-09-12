using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public static class PlayerPrefsExtend
{
    public static void Save(string name, object o)
    {
        Type t = o.GetType();
        FieldInfo[] fiedls = t.GetFields();
        for (int i = 0; i < fiedls.Length; i++)
        {
            string saveName = name + "." + fiedls[i].Name;
            switch (fiedls[i].FieldType.Name)
            {
                case "String":
                    CryptoPrefs.SetString(saveName, fiedls[i].GetValue(o).ToString());
                    break;
                case "Int32":
                case "Int64":
                case "Int":
                case "uInt":
                    CryptoPrefs.SetInt(saveName, (int)fiedls[i].GetValue(o));
                    break;
                case "Float":
                    CryptoPrefs.SetFloat(saveName, (float)fiedls[i].GetValue(o));
                    break;
            }
        }
    }


    public static T GetValue<T>(string name) where T : new()
    {
        T newObj = new T();

        Type t = newObj.GetType();
        FieldInfo[] fiedls = t.GetFields();
        for (int i = 0; i < fiedls.Length; i++)
        {
            string saveName = name + "." + fiedls[i].Name;
            switch (fiedls[i].FieldType.Name)
            {
                case "String":
                    fiedls[i].SetValue(newObj, CryptoPrefs.GetString(saveName));
                    break;
                case "Int32":
                case "Int64":
                case "Int":
                case "uInt":
                    fiedls[i].SetValue(newObj, CryptoPrefs.GetInt(saveName));
                    break;
                case "Float":
                    fiedls[i].SetValue(newObj, CryptoPrefs.GetFloat(saveName));
                    break;
            }
        }

        return newObj;
    }
}