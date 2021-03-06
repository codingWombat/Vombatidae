﻿using System;
using System.Collections.Concurrent;

namespace dev.codingWombat.Vombatidae.business
{
    public class RequestResponseHistory
    {
        public Guid Id { get; set; }
        public ConcurrentQueue<RequestResponse> History { get; set; }
        
        public void Append(RequestResponse requestResponse)
        {
            History.Enqueue(requestResponse);
        }
    }
}