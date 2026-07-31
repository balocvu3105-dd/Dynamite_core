// src/Dynamite.Application/Interfaces/IBackupService.cs
namespace Dynamite.Application.Interfaces;

using System.Threading.Tasks;

public interface IBackupService
{
    Task<string> CreateBackupAsync();
    Task<(bool Success, string Message)> RestoreBackupAsync();
}
