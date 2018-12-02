﻿using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Transaction;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface ITransactionService
    {
        Task CreateAsync(TransactionDTO transaction);
        Task<TransactionTypeDTO[]> GetTransactionTypesAsync();
    }
}