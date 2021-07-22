using System;
using System.Reflection;

namespace InstantMultiplayer.Synchronization.Extensions
{
    public static class MemberInfoExtensions
    {
        public static object GetValueFromMemberInfo(this MemberInfo memberInfo, object memberHolder)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(memberHolder);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(memberHolder);
                default:
                    throw new ArgumentException();
            }
        }

        public static void SetValueFromMemberInfo(this MemberInfo memberInfo, object memberHolder, object value)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo)memberInfo).SetValue(memberHolder, value);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo)memberInfo).SetValue(memberHolder, value);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public static Func<object> GetValueFuncFromMemberInfo(this MemberInfo memberInfo, object memberHolder)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return () => ((FieldInfo)memberInfo).GetValue(memberHolder);
                case MemberTypes.Property:
                    return () => ((PropertyInfo)memberInfo).GetValue(memberHolder);
                default:
                    throw new ArgumentException();
            }
        }

        public static Action<object> SetValueFuncFromMemberInfo(this MemberInfo memberInfo, object memberHolder)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return (val) => ((FieldInfo)memberInfo).SetValue(memberHolder, val);
                case MemberTypes.Property:
                    return (val) => ((PropertyInfo)memberInfo).SetValue(memberHolder, val);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
