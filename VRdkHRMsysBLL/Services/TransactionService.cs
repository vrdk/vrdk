﻿using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapHelper _mapHelper;

        public TransactionService(ITransactionRepository transactionRepository,
                                  IMapHelper mapHelper)
        {
            _transactionRepository = transactionRepository;
            _mapHelper = mapHelper;
        }

        public async Task CreateAsync(TransactionDTO transaction, bool writeChanges = false)
        {
            var entity = _mapHelper.Map<TransactionDTO, Transaction>(transaction);
            await _transactionRepository.CreateAsync(entity, writeChanges);
        }
    }
}
