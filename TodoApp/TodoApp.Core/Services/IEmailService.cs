#nullable enable
using System.Threading.Tasks;

namespace TodoApp.Core.Services;

// --- FIXED: Add the 'public' keyword to make this interface accessible from other projects. ---
// 修正：加入 'public' 關鍵字，讓這個介面可以被其他專案存取。
public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}