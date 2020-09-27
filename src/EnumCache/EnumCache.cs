using System;
using System.Collections.Generic;

namespace EnumCacheSpace
{
    public static class EnumCache
    {
        public static string TypeName<TEnum>() where TEnum : struct, Enum => CacheCore<TEnum>.TypeName;

        public static IReadOnlyList<TEnum> GetValues<TEnum>() where TEnum : struct, Enum => CacheCore<TEnum>.Values;
        public static ReadOnlySpan<TEnum> GetValuesAsSpan<TEnum>() where TEnum : struct, Enum => CacheCore<TEnum>.Values;

        public static IReadOnlyList<TUnderlying> GetUnderlyingValues<TEnum, TUnderlying>()
            where TEnum : struct, Enum
            where TUnderlying : struct
            => UnderlyingCache<TEnum, TUnderlying>.UnderlyingValues;

        public static ReadOnlySpan<TUnderlying> GetUnderlyingValuesAsSpan<TEnum, TUnderlying>()
            where TEnum : struct, Enum
            where TUnderlying : struct
            => UnderlyingCache<TEnum, TUnderlying>.UnderlyingValues;

        public static IReadOnlyList<string> GetNames<TEnum>() where TEnum : struct, Enum => CacheCore<TEnum>.Names;
        public static ReadOnlySpan<string> GetNamesAsSpan<TEnum>() where TEnum : struct, Enum => CacheCore<TEnum>.Names;

        public static bool IsDefined<TEnum>(TEnum source) where TEnum : struct, Enum
        {
            var values = CacheCore<TEnum>.Values;
            foreach (var item in values)
            {
                if (EqualityComparer<TEnum>.Default.Equals(source, item))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsDefined<TEnum>(string value) where TEnum : struct, Enum
        {
            var names = CacheCore<TEnum>.Names;
            foreach (var item in names)
            {
                if (value == item)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsDefined<TEnum, TUnderlying>(TUnderlying value)
            where TEnum : struct, Enum
            where TUnderlying : struct
        {
            var values = UnderlyingCache<TEnum, TUnderlying>.UnderlyingValues;
            foreach (var item in values)
            {
                if (EqualityComparer<TUnderlying>.Default.Equals(value, item))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetName<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            var values = CacheCore<TEnum>.Values;

            for (int i = 0; i < values.Length; i++)
            {
                if (EqualityComparer<TEnum>.Default.Equals(value, values[i]))
                {
                    return CacheCore<TEnum>.Names[i];
                }
            }

            return string.Empty;
        }

        public static TEnum Parse<TEnum>(string value) where TEnum : struct, Enum
        {
            var names = CacheCore<TEnum>.Names;
            for (int i = 0; i < names.Length; i++)
            {
                if (value == names[i])
                {
                    return CacheCore<TEnum>.Values[i];
                }
            }

            throw new ArgumentException($"{nameof(value)} is not defined in {CacheCore<TEnum>.TypeName}");
        }

        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct, Enum
        {
            var names = CacheCore<TEnum>.Names;
            for (int i = 0; i < names.Length; i++)
            {
                if (value == names[i])
                {
                    result = CacheCore<TEnum>.Values[i];
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static TEnum Parse<TEnum, TUnderlying>(TUnderlying value)
            where TEnum : struct, Enum
            where TUnderlying : struct
        {
            var values = UnderlyingCache<TEnum, TUnderlying>.UnderlyingValues;

            for (int i = 0; i < values.Length; i++)
            {
                if (EqualityComparer<TUnderlying>.Default.Equals(value, values[i]))
                {
                    return CacheCore<TEnum>.Values[i];
                }
            }

            throw new ArgumentException($"{nameof(value)} is not defined in {CacheCore<TEnum>.TypeName}");
        }


        public static bool TryParse<TEnum, TUnderlying>(TUnderlying value, out TEnum result)
            where TEnum : struct, Enum
            where TUnderlying : struct
        {
            var values = UnderlyingCache<TEnum, TUnderlying>.UnderlyingValues;

            for (int i = 0; i < values.Length; i++)
            {
                if (EqualityComparer<TUnderlying>.Default.Equals(value, values[i]))
                {
                    result = CacheCore<TEnum>.Values[i];
                    return true;
                }
            }

            result = default;
            return false;
        }

        private static class CacheCore<TEnum> where TEnum : struct, Enum
        {
            internal static readonly string[] Names;
            internal static readonly TEnum[] Values;
            internal static string TypeName;

            static CacheCore()
            {
                Names = Enum.GetNames(typeof(TEnum));
                Values = Enum.GetValues(typeof(TEnum)) as TEnum[];
                TypeName = typeof(TEnum).Name;
            }
        }

        private static class UnderlyingCache<TEnum, TUnderlying>
            where TEnum : struct, Enum
            where TUnderlying : struct
        {
            internal static readonly TUnderlying[] UnderlyingValues;
            static UnderlyingCache()
            {
                UnderlyingValues = CacheCore<TEnum>.Values as TUnderlying[];
            }
        }
    }

    public static class EnumCacheExtensions
    {
        public static string ToStringFromCache<TEnum>(this TEnum source) where TEnum : struct, Enum
            => EnumCache.GetName(source);

        public static bool IsDefined<TEnum>(this TEnum source) where TEnum : struct, Enum
            => EnumCache.IsDefined(source);
    }
}
