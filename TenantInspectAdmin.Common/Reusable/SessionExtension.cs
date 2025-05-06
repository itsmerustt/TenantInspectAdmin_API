using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantInspectAdmin.Common.Reusable
{
    public static class SessionExtension
    {
        //On How to Consume and Set the Session:  HttpContext.Session.SetObjectAsJson("UserSession", isValidated.FirstOrDefault());
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {

            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
