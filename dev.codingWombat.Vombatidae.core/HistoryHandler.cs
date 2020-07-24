using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.business;
using dev.codingWombat.Vombatidae.store;

namespace dev.codingWombat.Vombatidae.core
{
    public interface IHistoryHandler
    {
        RequestResponseHistory Load(Guid id);
        void AppendRequest(Guid id, RequestResponse requestResponse);
        Task SaveOptionsAsync();
    }

    public class HistoryHandler : IHistoryHandler
    {
        private ConcurrentDictionary<string,RequestResponseHistory> _historyCache;
        private readonly ICacheRepository _repository;

        public HistoryHandler(ICacheRepository repository)
        {
            _repository = repository;
            _historyCache = new ConcurrentDictionary<string, RequestResponseHistory>(repository.ReadHistory());
        }

        public RequestResponseHistory Load(Guid id)
        {
            return _historyCache[id.ToString()];
        }

        public void AppendRequest(Guid id, RequestResponse requestResponse)
        {
            if (!_historyCache.ContainsKey(id.ToString()))
            {
               _historyCache[id.ToString()] = new RequestResponseHistory{History = new ConcurrentQueue<RequestResponse>(), Id = id};
            }
            
            _historyCache[id.ToString()].Append(requestResponse);
        }

        public async Task SaveOptionsAsync()
        {
            await _repository.WriteHistoryAsync(_historyCache.ToDictionary(pair => pair.Key, pair => pair.Value));
        }
    }
}