using System;
using System.Collections.Generic;

namespace EnumCacheSpace
{
    internal static class EnumCacheCore<TEnum> where TEnum : Enum
    {
        internal static readonly string[] Names;
        internal static readonly TEnum[] Values;
        internal static string TypeName;

        static EnumCacheCore()
        {
            Names = Enum.GetNames(typeof(TEnum));
            Values = Enum.GetValues(typeof(TEnum)) as TEnum[];
            TypeName = typeof(TEnum).Name;
        }
    }

    internal static class NumericalCache<TEnum, TValue>
        where TEnum : Enum
        where TValue : struct
    {
        internal static readonly TValue[] NumericalValues;
        static NumericalCache()
        {
            NumericalValues = EnumCacheCore<TEnum>.Values as TValue[];
        }
    }

    public static class EnumCache
    {
        public static IReadOnlyList<TEnum> GetValues<TEnum>() where TEnum : Enum => EnumCacheCore<TEnum>.Values;
        public static ReadOnlySpan<TEnum> GetValuesAsSpan<TEnum>() where TEnum : Enum => EnumCacheCore<TEnum>.Values;

        public static IReadOnlyList<TValue> GetNumericalValues<TEnum, TValue>()
            where TEnum : Enum
            where TValue : struct
            => NumericalCache<TEnum, TValue>.NumericalValues;

        public static ReadOnlySpan<TValue> GetNumericalValuesAsSpan<TEnum, TValue>()
            where TEnum : Enum
            where TValue : struct
            => NumericalCache<TEnum, TValue>.NumericalValues;

        public static IReadOnlyList<string> GetNames<TEnum>() where TEnum : Enum => EnumCacheCore<TEnum>.Names;
        public static ReadOnlySpan<string> GetNamesAsSpan<TEnum>() where TEnum : Enum => EnumCacheCore<TEnum>.Names;

        public static bool IsDefined<TEnum>(TEnum source) where TEnum : Enum
        {
            var values = EnumCacheCore<TEnum>.Values;
            foreach (var item in values)
            {
                if (EqualityComparer<TEnum>.Default.Equals(source, item))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsDefined<TEnum>(string name) where TEnum : Enum
        {
            var names = EnumCacheCore<TEnum>.Names;
            foreach (var item in names)
            {
                if (name == item)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsDefined<TEnum, TValue>(TValue value)
            where TEnum : Enum
            where TValue : struct
        {
            var values = NumericalCache<TEnum, TValue>.NumericalValues;
            foreach (var item in values)
            {
                if (EqualityComparer<TValue>.Default.Equals(value, item))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetName<TEnum>(TEnum source) where TEnum : Enum
        {
            var values = EnumCacheCore<TEnum>.Values;

            for (int i = 0; i < values.Length; i++)
            {
                if (EqualityComparer<TEnum>.Default.Equals(source, values[i]))
                {
                    return EnumCacheCore<TEnum>.Names[i];
                }
            }

            return string.Empty;
        }

        public static TEnum Parse<TEnum>(string value) where TEnum : Enum
        {
            var names = EnumCacheCore<TEnum>.Names;
            for (int i = 0; i < names.Length; i++)
            {
                if (value == names[i])
                {
                    return EnumCacheCore<TEnum>.Values[i];
                }
            }

            throw new ArgumentException($"{nameof(value)} is not defined in {EnumCacheCore<TEnum>.TypeName}");
        }

        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : Enum
        {
            var names = EnumCacheCore<TEnum>.Names;
            for (int i = 0; i < names.Length; i++)
            {
                if (value == names[i])
                {
                    result = EnumCacheCore<TEnum>.Values[i];
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static TEnum Parse<TEnum, TValue>(TValue value) 
            where TEnum : Enum 
            where TValue : struct
        {
            var values = NumericalCache<TEnum, TValue>.NumericalValues;

            for (int i = 0; i < values.Length; i++)
            {
                if (EqualityComparer<TValue>.Default.Equals(value, values[i]))
                {
                    return EnumCacheCore<TEnum>.Values[i];
                }
            }

            throw new ArgumentException($"{nameof(value)} is not defined in {EnumCacheCore<TEnum>.TypeName}");
        }


        public static bool TryParse<TEnum, TValue>(TValue value, out TEnum result)
            where TEnum : Enum
            where TValue : struct
        {
            var values = NumericalCache<TEnum, TValue>.NumericalValues;

            for (int i = 0; i < values.Length; i++)
            {
                if (EqualityComparer<TValue>.Default.Equals(value, values[i]))
                {
                    result = EnumCacheCore<TEnum>.Values[i];
                    return true;
                }
            }

            result = default;
            return false;
        }
    }

    public static class EnumCacheExtensions
    {
        public static string ToStringFromCache<TEnum>(this TEnum source) where TEnum : Enum
        {
            return EnumCache.GetName(source);
        }

        public static bool IsDefined<TEnum>(this TEnum source) where TEnum : Enum
        {
            return EnumCache.IsDefined(source);
        }
    }
}
