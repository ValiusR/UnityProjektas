using System;
using System.Reflection;

public static class TestHelpers
{
    public static T GetPrivateField<T>(this object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance);
        return (T)field?.GetValue(obj);
    }

    public static void SetPrivateField(this object obj, string fieldName, object value)
    {
        var field = obj.GetType().GetField(fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(obj, value);
    }

    public static void InvokePrivateMethod(this object obj, string methodName, params object[] parameters)
    {
        var method = obj.GetType().GetMethod(methodName,
            BindingFlags.NonPublic | BindingFlags.Instance);
        method?.Invoke(obj, parameters);
    }
}
