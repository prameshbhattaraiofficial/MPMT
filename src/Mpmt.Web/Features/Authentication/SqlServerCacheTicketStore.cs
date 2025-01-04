using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;


public class SqlServerCacheTicketStore : ITicketStore
{
    private readonly IDistributedCache _cache;

    public SqlServerCacheTicketStore(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var key = Guid.NewGuid().ToString();
        var options = new DistributedCacheEntryOptions();
        //options.SetAbsoluteExpiration(ticket.Properties.ExpiresUtc);
        var expiresUtc = ticket.Properties.ExpiresUtc;
        if (expiresUtc.HasValue)
            options.SetAbsoluteExpiration(expiresUtc.Value);

        var serializedTicket = SerializeTicket(ticket);
        await _cache.SetStringAsync(key, serializedTicket, options);

        return key;
    }

    public async Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        var serializedTicket = SerializeTicket(ticket);
        await _cache.SetStringAsync(key, serializedTicket);
    }

    public async Task<AuthenticationTicket> RetrieveAsync(string key)
    {
        var serializedTicket = await _cache.GetStringAsync(key);
        return serializedTicket == null ? null : DeserializeTicket(serializedTicket);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    private string SerializeTicket(AuthenticationTicket ticket)
    {
        return JsonConvert.SerializeObject(ticket);
    }

    private AuthenticationTicket DeserializeTicket(string serializedTicket)
    {
        return JsonConvert.DeserializeObject<AuthenticationTicket>(serializedTicket);
    }
}
