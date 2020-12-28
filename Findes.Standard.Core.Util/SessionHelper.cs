using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Findes.Standard.Core.Util
{
    public interface ISessionHelper
    {
        void SetSession(string key, object value);
        T GetSession<T>(string key);
        void ClearSession();
        void ClearSessionByKey(string key, bool partialKey = false);
        Dictionary<string, T> GetPartialSession<T>(string partialKey);
        int HasKey(string key, bool partialKey = false);
    }

    public class SessionHelper : ISessionHelper
    {
        private readonly IHttpContextAccessor HttpContextAccessor;

        public SessionHelper(IHttpContextAccessor httpContextAccessor) {
            HttpContextAccessor = httpContextAccessor;
        }

        public void SetSession(string key, object value) {
            HttpContextAccessor.HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public T GetSession<T>(string key) {
            if (key == null) return default(T);
            var value = HttpContextAccessor.HttpContext.Session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public Dictionary<string, T> GetPartialSession<T>(string partialKey) {
            var keyList = new Dictionary<string, T>();
            foreach (var keyItem in HttpContextAccessor.HttpContext.Session.Keys) {
                if (keyItem.Contains(partialKey)) {
                    var _value = HttpContextAccessor.HttpContext.Session.GetString(keyItem);
                    var partialValue = _value == null ? default(T) : JsonConvert.DeserializeObject<T>(_value);
                    keyList.Add(keyItem, partialValue);
                }
            }

            return keyList;
        }

        public void ClearSession() {
            HttpContextAccessor.HttpContext.Session.Clear();
        }

        public void ClearSessionByKey(string key, bool partialKey = false) {
            if (partialKey) {
                var keyToRemove = new List<string>();
                foreach (var keyItem in HttpContextAccessor.HttpContext.Session.Keys) {
                    if (keyItem.Contains(key)) keyToRemove.Add(keyItem);
                }
                foreach (var item in keyToRemove) {
                    HttpContextAccessor.HttpContext.Session.Remove(item);
                }
            } else {
                HttpContextAccessor.HttpContext.Session.Remove(key);
            }
        }

        public int HasKey(string key, bool partialKey = false) {
            var keys = 0;
            IEnumerable<string> result = null;

            if (partialKey) {
                result = HttpContextAccessor.HttpContext.Session.Keys.Where(keyItem => keyItem.Contains(key));
            } else {
                result = HttpContextAccessor.HttpContext.Session.Keys.Where(keyItem => keyItem.Equals(key));
            }

            if (result != null && result.Count() > 0) {
                keys = result.Count();
            }

            return keys;
        }

    }
}
