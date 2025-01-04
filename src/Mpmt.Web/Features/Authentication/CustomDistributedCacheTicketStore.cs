using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Claims;

namespace Mpmt.Web.Features.Authentication
{
    public class CustomDistributedCacheTicketStore : ITicketStore
    {
        private const string KeyPrefix = "AuthSessionStore-";
        private readonly IDistributedCache _cache;

        public CustomDistributedCacheTicketStore(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<AuthenticationTicket> RetrieveAsync(string key, string scheme)
        {
            var encodedData = await _cache.GetAsync(key);
            if (encodedData == null)
            {
                return null;
            }

            var ticket = JsonSerializer.Deserialize<AuthenticationTicket>(Encoding.UTF8.GetString(encodedData));
            return ticket;
        }

        //public async Task StoreAsync(string key, string scheme, AuthenticationTicket ticket)
        //{
        //    var encodedData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(ticket));
        //    await _cache.SetAsync(key, encodedData, ticket.Properties.ExpiresUtc.HasValue ? ticket.Properties.ExpiresUtc.Value.Subtract(DateTime.UtcNow) : TimeSpan.FromHours(1)); // Adjust expiration as needed
        //}

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.FromResult(0);
        }

        public Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var guid = Guid.NewGuid();
            var key = KeyPrefix + guid.ToString();
            RenewAsync(key, ticket);

            return Task.FromResult(key);
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            //var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(15)};
            //var expiresUtc = ticket.Properties.ExpiresUtc;
            //if (expiresUtc.HasValue)
            //    options.SetAbsoluteExpiration(expiresUtc.Value);

            ////MakeClassSerializable(typeof(AuthenticationTicket));
            ////byte[] bytes = SerializeObjectToByteArray(ticket);
            ////_cache.Set(key, bytes, options);

            //return Task.FromResult(0);

            var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(15) };
            var expiresUtc = ticket.Properties.ExpiresUtc;
            if (expiresUtc.HasValue)
                options.SetAbsoluteExpiration(expiresUtc.Value);

            // Configure JsonSerializerOptions to preserve references
            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            string jsonString = JsonSerializer.Serialize(ticket, jsonOptions);
            _cache.SetString(key, jsonString, options);

            return Task.CompletedTask;
        }

        public async Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var encodedData = await _cache.GetAsync(key);
            if (encodedData == null)
            {
                return null;
            }

            //var ticket = JsonSerializer.Deserialize<AuthenticationTicket>(encodedData);

            // Deserialize surrogate object
            AuthenticationTicketSurrogate surrogate = JsonSerializer.Deserialize<AuthenticationTicketSurrogate>(Encoding.UTF8.GetString(encodedData));

            // Map surrogate properties to AuthenticationTicket
            AuthenticationTicket ticket = MapToAuthenticationTicket(surrogate);

            return ticket;
        }

        // Serialize an object to a byte array using JSON serialization
        public static byte[] SerializeObjectToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;

            // Configure JsonSerializerOptions to preserve references
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true // Optional: for pretty-printing the JSON
            };

            string jsonString = JsonSerializer.Serialize(obj, options);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        // Deserialize a byte array back to an object using JSON deserialization
        public static T DeserializeByteArrayToObject<T>(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return default;

            // Configure JsonSerializerOptions to preserve references
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            string jsonString = Encoding.UTF8.GetString(byteArray);
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }

        //public static byte[] ObjectToByteArray(Object obj)
        //{
        //    //BinaryFormatter bf = new BinaryFormatter();
        //    //using (var ms = new MemoryStream())
        //    //{
        //    //    bf.Serialize(ms, obj);
        //    //    return ms.ToArray();
        //    //}

        //    string jsonString = SerializeObject(obj);
        //    return Encoding.UTF8.GetBytes(jsonString);
        //}

        // Serialize an object to a JSON string
        public static string SerializeObject<T>(T obj)
        {
            if (obj == null)
                return null;

            return JsonSerializer.Serialize(obj);
        }

        // Deserialize a JSON string back to an object
        public static T DeserializeObject<T>(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                return default;

            return JsonSerializer.Deserialize<T>(jsonString);
        }


        // Make a class serializable by creating a new type dynamically
        public static Type MakeClassSerializable(Type type)
        {
            if (!type.IsSerializable)
            {
                // Create a new dynamic assembly
                AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
                AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

                // Create a new dynamic module within the assembly
                ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

                // Create a new type that inherits from the original type
                TypeBuilder typeBuilder = moduleBuilder.DefineType(type.FullName + "_Serializable", TypeAttributes.Public, type);

                // Add the SerializableAttribute to the new type
                ConstructorInfo serializableCtor = typeof(SerializableAttribute).GetConstructor(Type.EmptyTypes);
                CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(serializableCtor, new object[0]);
                typeBuilder.SetCustomAttribute(customAttributeBuilder);

                // Define a default constructor for the new type
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
                ILGenerator ilGenerator = constructorBuilder.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
                ilGenerator.Emit(OpCodes.Ret);

                // Create and return the new type
                return typeBuilder.CreateType();
            }

            // If the type is already serializable, return it
            return type;
        }

        private AuthenticationTicket MapToAuthenticationTicket(AuthenticationTicketSurrogate surrogate)
        {
           
            if (surrogate == null)
                return null;

            // Deserialize Principal
            ClaimsPrincipal principal = DeserializePrincipal(surrogate.Principal);

            // Deserialize Properties
            AuthenticationProperties properties = DeserializeProperties(surrogate.Properties);

            // Create AuthenticationTicket
            AuthenticationTicket ticket = new AuthenticationTicket(principal, properties, surrogate.AuthenticationScheme);


            // Perform mapping from surrogate to AuthenticationTicket
            // Example:
            // var principal = new ClaimsPrincipal(...);
            // var properties = new AuthenticationProperties(...);
            // return new AuthenticationTicket(principal, properties, surrogate.AuthenticationScheme);

            return null; // Placeholder, replace with actual mapping logic
        }

        // Deserialize ClaimsPrincipal from surrogate string
        private ClaimsPrincipal DeserializePrincipal(string principalString)
        {
            if (string.IsNullOrEmpty(principalString))
                return null;

            // Deserialize principalString into ClaimsPrincipal
            // Here, you need to implement logic to deserialize the principalString
            // and create a ClaimsPrincipal instance. 
            // This depends on how the ClaimsPrincipal was serialized in the surrogate.

            // Example (assuming principalString is JSON):
             ClaimsPrincipal principal = JsonSerializer.Deserialize<ClaimsPrincipal>(principalString);

            // Placeholder logic (replace with actual deserialization)
            //ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity()); // Placeholder, replace with actual deserialization logic

            return principal;
        }

        // Deserialize AuthenticationProperties from surrogate string
        private AuthenticationProperties DeserializeProperties(string propertiesString)
        {
            if (string.IsNullOrEmpty(propertiesString))
                return null;

            // Deserialize propertiesString into AuthenticationProperties
            // Here, you need to implement logic to deserialize the propertiesString
            // and create an AuthenticationProperties instance.
            // This depends on how the AuthenticationProperties was serialized in the surrogate.

            // Placeholder logic (replace with actual deserialization)
            //AuthenticationProperties properties = new AuthenticationProperties(new Dictionary<string, string>()); // Placeholder, replace with actual deserialization logic

            Dictionary<string, string> dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(propertiesString);
            AuthenticationProperties properties = new AuthenticationProperties(dictionary);


            return properties;
        }

    }
}
