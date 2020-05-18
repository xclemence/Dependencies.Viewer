using System;
using System.Collections.Generic;
using System.Linq;

namespace Dependencies.Viewer.Wpf.Controls.Base
{
    public class ObjectCacheTransformer
    {
        private readonly IDictionary<Type, IDictionary<dynamic, dynamic>> caches;

        public ObjectCacheTransformer() => caches = new Dictionary<Type, IDictionary<dynamic, dynamic>>();

        public TTo Transform<TFrom, TTo>(TFrom item, Func<TFrom, TTo> transform)
        {
            var type = typeof(TFrom);

            if (!caches.TryGetValue(type, out var typeCache))
            {
                typeCache = new Dictionary<dynamic, dynamic>();
                caches.Add(type, typeCache);
            }

            if (!typeCache.TryGetValue(item, out var value))
            {
                value = transform(item);
                typeCache.Add(item, value);
            }

            return value;
        }

        public IEnumerable<TTo> GetCacheItems<TFrom, TTo>()
        {
            var type = typeof(TFrom);

            if (!caches.TryGetValue(type, out var typeCache))
                return Enumerable.Empty<TTo>();

            return typeCache.Values.OfType<TTo>();
        }
    }
}

