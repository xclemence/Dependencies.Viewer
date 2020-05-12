using System;
using System.Collections.Generic;
using System.Linq;

namespace Dependencies.Viewer.Wpf.Controls
{
    public class ObjectCacheTransformer
    {
        private readonly IDictionary<Type, IDictionary<dynamic, dynamic>> caches;

        public ObjectCacheTransformer() => caches = new Dictionary<Type, IDictionary<dynamic, dynamic>>();

        public TTo Transform<TFrom, TTo>(TFrom item, Func<TFrom, TTo> transform)
        {
            var type = typeof(TFrom);

            if (!caches.TryGetValue(type, out var typeCahe))
            {
                typeCahe = new Dictionary<dynamic, dynamic>();
                caches.Add(type, typeCahe);
            }

            if (!typeCahe.TryGetValue(item, out var value))
            {
                value = transform(item);
                typeCahe.Add(item, value);
            }

            return value;
        }

        public IEnumerable<TTo> GetCacheItems<TFrom, TTo>()
        {
            var type = typeof(TFrom);

            if (!caches.TryGetValue(type, out var typeCahe))
            {
                return Enumerable.Empty<TTo>();
            }

            return typeCahe.Values.OfType<TTo>();
        }
    }
}

