using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Transaction;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly IMapHelper _mapHelper;

        public TransactionService(ITransactionRepository transactionRepository,
                                  ITransactionTypeRepository transactionTypeRepository,
                                  IMapHelper mapHelper)
        {
            _transactionRepository = transactionRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _mapHelper = mapHelper;
        }
        
        public async Task<TransactionTypeDTO[]> GetTransactionTypesAsync()
        {
            var types = await _transactionTypeRepository.GetAsync();
            return _mapHelper.MapCollection<TransactionType, TransactionTypeDTO>(types);
        }

        public async Task CreateAsync(TransactionDTO transaction)
        {
            var entity = _mapHelper.Map<TransactionDTO, Transaction>(transaction);
            await _transactionRepository.CreateAsync(entity);
        }
    }
}
