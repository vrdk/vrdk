using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface ITransactionService
    {
        Task CreateAsync(TransactionDTO transaction, bool writeChanges = false);
    }
}