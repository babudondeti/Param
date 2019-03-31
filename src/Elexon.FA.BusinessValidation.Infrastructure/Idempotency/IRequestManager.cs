﻿using System;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Infrastructure.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}