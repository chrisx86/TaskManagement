2. 專案：TodoApp.Infrastructure (基礎設施層)

目標： 此專案是「實現」層。它提供了 TodoApp.Core 中定義的介面的具體實現，並處理所有與外部系統（如資料庫）互動的細節。

需建立的資料夾結構：

Generated code
TodoApp.Infrastructure/
├── Data/
├── Security/
└── Services/
IGNORE_WHEN_COPYING_START
content_copy
download
Use code with caution.
IGNORE_WHEN_COPYING_END

Data/ 資料夾

用途： 存放所有與資料存取層（DAL）相關的程式碼，主要是 Entity Framework Core 的部分。

將會包含的檔案範例：

AppDbContext.cs: EF Core 的資料庫上下文類別，是與資料庫溝通的橋樑。

Migrations/: 這個資料夾不需要手動建立。當您執行第一個 EF Core 遷移指令時，EF Core 工具會自動產生它，用來存放資料庫結構的變更歷史。

Security/ 資料夾

用途： 存放與安全性相關的輔助類別。將安全性邏輯獨立出來，有助於審計和維護。

將會包含的檔案範例：

PasswordHasher.cs: 負責將使用者密碼進行雜湊加鹽處理及驗證的類別。

Services/ 資料夾

用途： 存放業務邏輯服務的「具體實現類別」。這些類別會實現 TodoApp.Core 中定義的服務介面。

將會包含的檔案範例：

UserService.cs: 實現 IUserService 介面，包含使用者驗證、查詢等實際邏輯。

TaskService.cs: 實現 ITaskService 介面，包含處理任務 CRUD、權限檢查等實際邏輯。




好的，我們來執行里程碑 3 的核心任務。

任務 3.3：實作使用者服務 (UserService)

這個任務的目標是在 TodoApp.Infrastructure 專案中建立 UserService.cs，並在其中具體實現我們在 IUserService 介面中定義的所有方法。這個類別將封裝所有與使用者相關的業務邏輯，並直接與 AppDbContext 和 PasswordHasher 互動。

操作步驟

在 Visual Studio 的 方案總管 (Solution Explorer) 中，找到 TodoApp.Infrastructure 專案。

在專案的 Services 資料夾上按右鍵，選擇 加入 (Add) > 類別 (Class...)，並將其命名為 UserService.cs。

將以下程式碼複製到 UserService.cs 檔案中。

UserService.cs

檔案路徑： TodoApp.Infrastructure/Services/UserService.cs

Generated csharp
// We need access to DbContext, models, interfaces, and security tools.
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Security;

// The namespace should match the project and folder structure.
namespace TodoApp.Infrastructure.Services;

/// <summary>
/// Implements the IUserService interface, providing concrete business logic for user management.
/// </summary>
public class UserService : IUserService
{
    // A private field to hold the database context, injected via the constructor.
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor for UserService. It receives the AppDbContext via dependency injection.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Authenticates a user based on their username and password.
    /// </summary>
    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        // 1. Find the user by username. The search is case-insensitive for better usability.
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

        // 2. If user is not found, authentication fails.
        if (user == null)
        {
            return null;
        }

        // 3. Verify the provided password against the stored hash.
        if (!PasswordHasher.VerifyPassword(password, user.HashedPassword))
        {
            // Password does not match.
            return null;
        }

        // 4. Authentication successful. Return the user object.
        return user;
    }

    /// <summary>
    /// Retrieves a list of all users in the system.
    /// </summary>
    public async Task<List<User>> GetAllUsersAsync()
    {
        // Simply query the Users table and return all entries as a list.
        // AsNoTracking() is a performance optimization for read-only queries.
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    public async Task<User> CreateUserAsync(string username, string password, UserRole role)
    {
        // 1. Check if a user with the same username already exists.
        if (await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
        {
            // Throw an exception if the username is taken. The UI layer will have to catch this.
            throw new InvalidOperationException($"Username '{username}' already exists.");
        }

        // 2. Hash the new user's password.
        var hashedPassword = PasswordHasher.HashPassword(password);

        // 3. Create a new User entity.
        var newUser = new User
        {
            Username = username,
            HashedPassword = hashedPassword,
            Role = role
        };

        // 4. Add the new user to the context and save changes to the database.
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        // 5. Return the newly created user object (which now has an Id).
        return newUser;
    }

    /// <summary>
    /// Deletes a user from the system.
    /// </summary>
    public async Task DeleteUserAsync(int userId)
    {
        // 1. Find the user to be deleted.
        var user = await _context.Users.FindAsync(userId);

        // 2. If user exists, remove them.
        if (user != null)
        {
            // Note: OnDelete behavior in DbContext will handle related TodoItems.
            // Tasks created by this user will cause a restrict error.
            // Tasks assigned to this user will have their AssignedToId set to null.
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        // If user is not found, we do nothing.
    }

    /// <summary>
    /// Resets a user's password.
    /// </summary>
    public async Task ResetPasswordAsync(int userId, string newPassword)
    {
        // 1. Find the user.
        var user = await _context.Users.FindAsync(userId);

        // 2. If user exists, update their password.
        if (user != null)
        {
            // 3. Hash the new password.
            user.HashedPassword = PasswordHasher.HashPassword(newPassword);
            
            // 4. Mark the user entity as modified and save changes.
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        // If user is not found, an exception could be thrown, but for now we do nothing.
    }
}

程式碼說明

依賴注入 (Constructor Injection):

UserService 透過其建構函式接收一個 AppDbContext 的實例。它不自己 new AppDbContext()，而是依賴外部的 DI 容器來提供。這是實現鬆散耦合的關鍵。

AuthenticateAsync:

使用 FirstOrDefaultAsync 根據使用者名稱（不區分大小寫）查詢使用者。

如果找不到使用者，或 PasswordHasher.VerifyPassword 回傳 false，則認證失敗，回傳 null。

只有在使用者存在且密碼驗證成功時，才回傳 User 物件。

GetAllUsersAsync:

這是一個簡單的讀取操作。

.AsNoTracking(): 這是一個重要的效能優化。它告訴 EF Core：「我只是要讀取這些資料，不會對它們做任何修改。」這樣 EF Core 就不會花費額外的資源來追蹤這些實體的狀態，從而提升查詢速度。

CreateUserAsync:

業務規則檢查: 在寫入資料庫之前，先檢查使用者名稱是否已被使用。如果已被使用，拋出一個 InvalidOperationException。UI 層需要捕獲並處理這個異常，向使用者顯示友善的提示。

呼叫 PasswordHasher.HashPassword 來處理密碼。

使用 _context.Users.Add() 將新的使用者實體加入到 EF Core 的追蹤器中。

await _context.SaveChangesAsync(): 這是將所有掛起的變更（在這個案例中是 Add 操作）一次性寫入資料庫的指令。只有執行了這個方法，資料才會真正儲存。

DeleteUserAsync 和 ResetPasswordAsync:

這兩個方法都遵循「查詢 -> 修改 -> 儲存」的模式。

FindAsync(userId) 是根據主鍵查詢實體的最高效方法。

在 DeleteUserAsync 中，我們依賴於在 AppDbContext 中設定的 OnDelete 行為來處理與 TodoItem 的關聯。

在 ResetPasswordAsync 中，我們修改了 user.HashedPassword 屬性，然後呼叫 _context.Users.Update(user) 來告知 EF Core 這個實體已被修改（雖然 EF Core 通常能自動檢測到，但明確呼叫是個好習慣），最後呼叫 SaveChangesAsync 來儲存變更。

完成這個檔案後，我們就擁有了一個功能完整的、可被測試的、與資料庫互動的使用者管理服務。下一步就是將這個服務註冊到 DI 容器中，以便我們的 WinForms 應用程式可以使用它。








