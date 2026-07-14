using HarmonyLib;

namespace HooksHangMore
{
    internal static class Extensions
    {
        public static object InvokePrivateMethod(this object obj, string method, params object[] parameters)
        {
            return AccessTools.Method(obj.GetType(), method).Invoke(obj, parameters);
        }

        public static T GetPrivateField<T>(this object obj, string field)
        {
            return (T)Traverse.Create(obj).Field(field).GetValue();
        }
    }
}
