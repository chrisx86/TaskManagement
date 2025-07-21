3. 專案：TodoApp.WinForms (表現層)

目標： 此專案是使用者直接互動的介面層。它負責呈現資料和接收使用者輸入。

需建立的資料夾結構：

Generated code
TodoApp.WinForms/
├── Controls/
├── Forms/
└── Utils/ (或 Helpers/)
IGNORE_WHEN_COPYING_START
content_copy
download
Use code with caution.
IGNORE_WHEN_COPYING_END

Controls/ 資料夾

用途： 存放可重用的自訂使用者控制項 (User Controls)。如果應用程式中有一些需要重複使用的複雜 UI 組件（例如一個帶有特定驗證功能的文字方塊），可以將其建立為自訂控制項並放在這裡。

將會包含的檔案範例：

(此專案初期可能為空，但建立此資料夾是良好實踐) ClearableDateTimePicker.cs

Forms/ 資料夾

用途： 存放應用程式中所有的視窗 (Forms) 和對話框 (Dialogs)。這是 WinForms 專案中最主要的資料夾。

將會包含的檔案範例：

LoginForm.cs

MainForm.cs (可能是由 Form1.cs 重命名而來)

TaskDialog.cs

UserManagementDialog.cs

AdminDashboardForm.cs

Utils/ (或 Helpers/) 資料夾

用途： 存放一些 UI 層專用的輔助工具類別，這些類別不屬於任何特定視窗，但可以被多個視窗共用。

將會包含的檔案範例：

MessageBoxHelper.cs: 一個封裝了標準化 MessageBox.Show 呼叫的類別，以確保應用程式中的提示框風格一致。

AppContext.cs: 用於儲存全域狀態（如當前登入使用者）的靜態類別。

最終結構預覽

完成後，您在 Visual Studio 的方案總管中看到的結構應該會像這樣：

Generated code
Solution 'TodoApp'
├── TodoApp.Core
│   ├── Models/
│   └── Services/
├── TodoApp.Infrastructure
│   ├── Data/
│   ├── Security/
│   └── Services/
└── TodoApp.WinForms
    ├── Controls/
    ├── Forms/
    ├── Utils/
    └── Program.cs
IGNORE_WHEN_COPYING_START
content_copy
download
Use code with caution.
IGNORE_WHEN_COPYING_END

遵循這套資料夾結構，可以讓您的專案從一開始就保持高度的組織性和清晰度，為後續的開發工作打下堅實的基礎。


好的，這個錯誤訊息比之前的更深入，它標誌著我們已經進入了依賴注入 (Dependency Injection - DI) 的設定環節。這是現代 .NET 應用開發中非常核心的一部分。

讓我們來解讀這個複雜的錯誤訊息。

錯誤訊息分析

這個錯誤實際上包含兩個部分：

The entry point exited without ever building an IHost.

白話文： "我 (EF Core 工具) 嘗試運行你的 Program.cs 來理解你的服務設定，但我還沒來得及建立好應用程式的主機 (Host)，你的 Main 方法就結束了。"

原因： 預設的 WinForms Program.cs 檔案裡沒有設定通用的主機 (IHost)，它只是直接 Application.Run(new Form1())。EF Core 的設計時工具不認識這種傳統的啟動方式。

Unable to resolve service for type 'DbContextOptions<...>' while attempting to activate 'AppDbContext'.

白話文： "我找不到一個叫做 DbContextOptions<AppDbContext> 的服務，所以我沒辦法建立一個 AppDbContext 的實例。"

原因： 我們的 AppDbContext 的建構函式需要一個 DbContextOptions 參數 (public AppDbContext(DbContextOptions<AppDbContext> options))。在正常的應用程式執行中，這個參數是由 DI 容器提供的。但現在，EF Core 工具在設計階段找不到任何關於如何建立這個 options 物件的設定資訊。

總結來說，EF Core 工具需要知道如何建立 AppDbContext 的實例，而建立 AppDbContext 又需要 DI 容器提供 DbContextOptions，但我們的 Program.cs 還沒有設定好這個 DI 容器。

解決方案

我們需要修改 Program.cs，將其從傳統的 WinForms 啟動方式，改造成現代 .NET 通用主機 (Generic Host) 的模式。這不僅能解決目前的問題，也是我們後續實作依賴注入的必經之路。

步驟：修改 TodoApp.WinForms/Program.cs

請將 TodoApp.WinForms 專案中的 Program.cs 檔案的全部內容替換為以下程式碼。

Generated csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoApp.Infrastructure.Data;

namespace TodoApp.WinForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // This is the modern .NET way to bootstrap an application.
            // It sets up dependency injection, configuration, logging, and more.
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            ApplicationConfiguration.Initialize();
            Application.Run(ServiceProvider.GetRequiredService<Form1>()); // We will change Form1 later
        }

        // A property to hold the service provider.
        public static IServiceProvider? ServiceProvider { get; private set; }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    // This is where we register our services for Dependency Injection.
                    
                    // 1. Register DbContext
                    // We need to configure the path to our SQLite database.
                    // For now, let's put it in the user's local app data folder.
                    var dbPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "TodoApp");
                    
                    // Ensure the directory exists.
                    Directory.CreateDirectory(dbPath);
                    var dbFullPath = Path.Combine(dbPath, "todo_app.db");

                    services.AddDbContext<AppDbContext>(options =>
                        // Here we tell EF Core to use SQLite and provide the connection string.
                        options.UseSqlite($"Data Source={dbFullPath}"));
                    
                    // 2. Register Forms
                    // We register our forms so we can request them from the DI container.
                    services.AddTransient<Form1>(); // TODO: This will be replaced by LoginForm/MainForm
                    
                    // 3. Register Services (we will add them in the next milestone)
                    // services.AddTransient<IUserService, UserService>();
                    // services.AddTransient<ITaskService, TaskService>();
                });
        }
    }
}

程式碼說明

CreateHostBuilder(): 這是新的核心。我們使用 Host.CreateDefaultBuilder() 來建立一個標準的應用程式主機，它內建了 DI、設定、日誌等功能。

.ConfigureServices(...): 在這個方法裡，我們告訴 DI 容器如何建立我們需要的服務。

services.AddDbContext<AppDbContext>(...): 這是解決問題的關鍵程式碼。

我們告訴 DI 容器：「當有任何地方需要 AppDbContext 時，請這樣建立它」。

options.UseSqlite(...): 我們進一步告訴它，DbContextOptions 應該使用 SQLite，並且資料庫檔案的路徑 (connection string) 是 Data Source=...。

我們選擇將資料庫檔案放在使用者的 LocalAppData 資料夾下，這是一個標準做法，可以避免將資料寫入需要管理員權限的程式安裝目錄。

ServiceProvider = host.Services;: 我們將設定好的 DI 容器保存到一個靜態屬性中，方便應用程式的其他地方存取。

Application.Run(ServiceProvider.GetRequiredService<Form1>()): 我們不再是 new Form1()，而是向 DI 容器「請求」一個 Form1 的實例。DI 容器會自動幫我們建立它。

重新執行指令

在完成對 Program.cs 的修改並儲存後：

建置 (Build) 一次您的解決方案 (按 Ctrl+Shift+B)，確保沒有編譯錯誤。

回到 套件管理器主控台 (Package Manager Console)。

確認「預設專案」是 TodoApp.Infrastructure。

再次執行指令：

Generated powershell
Add-Migration InitialCreate
IGNORE_WHEN_COPYING_START
content_copy
download
Use code with caution.
Powershell
IGNORE_WHEN_COPYING_END

這次，EF Core 的設計時工具將能夠：

成功運行 Program.cs 並建立 IHost。

從主機的服務提供者 (ServiceProvider) 中，找到我們註冊的 DbContextOptions<AppDbContext>。

使用這些 options 成功地建立一個 AppDbContext 的實例。

最終，順利地產生遷移檔案。

這個步驟是從傳統 WinForms 開發邁向現代 .NET 應用開發的一個重要里程碑。


好的，我們來執行里程碑 3 的最後一個任務，這一步是將我們後端的服務與前端的應用程式串接起來的關鍵。

任務 3.4：設定依賴注入 (Dependency Injection - DI)

這個任務的目標是修改 TodoApp.WinForms 專案中的 Program.cs 檔案。我們需要告訴應用程式的「通用主機 (Generic Host)」如何建立我們剛剛實作的 UserService。具體來說，就是當應用程式的任何地方請求一個 IUserService 時，DI 容器應該提供一個 UserService 的實例給它。

操作步驟

在 Visual Studio 的 方案總管 (Solution Explorer) 中，找到 TodoApp.WinForms 專案。

打開 Program.cs 檔案。

找到 CreateHostBuilder() 方法中的 .ConfigureServices(...) 區塊。

在裡面加入註冊服務的程式碼。

修改後的 Program.cs

以下是修改後的完整 Program.cs 檔案。我用註解標示出了本次新增的程式碼。

檔案路徑： TodoApp.WinForms/Program.cs

Generated csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoApp.Core.Services; // Add this using directive for interfaces
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Services; // Add this using directive for implementations

namespace TodoApp.WinForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            ApplicationConfiguration.Initialize();
            // We'll change this to LoginForm later
            Application.Run(ServiceProvider.GetRequiredService<Form1>());
        }

        public static IServiceProvider? ServiceProvider { get; private set; }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    // This is where we register our services for Dependency Injection.
                    
                    // 1. Register DbContext
                    var dbPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "TodoApp");
                    Directory.CreateDirectory(dbPath);
                    var dbFullPath = Path.Combine(dbPath, "todo_app.db");

                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlite($"Data Source={dbFullPath}"));
                    
                    // --- NEW CODE STARTS HERE ---

                    // 2. Register Services
                    // We map the interface to its concrete implementation.
                    // When a part of the app asks for an 'IUserService', the DI container will provide a 'UserService'.
                    services.AddTransient<IUserService, UserService>();
                    
                    // We do the same for ITaskService, even though its implementation is not yet complete.
                    // This prepares our application for future development.
                    // services.AddTransient<ITaskService, TaskService>(); // We will uncomment this when TaskService is created.
                    
                    // 3. Register Forms
                    // We register our forms so we can request them from the DI container.
                    services.AddTransient<Form1>(); // TODO: This will be replaced by LoginForm/MainForm
                    
                    // --- NEW CODE ENDS HERE ---
                });
        }
    }
}

程式碼說明

using TodoApp.Core.Services; 和 using TodoApp.Infrastructure.Services;: 我們需要引入這兩個命名空間，以便在 Program.cs 中能夠存取到 IUserService (介面) 和 UserService (實現類別)。

services.AddTransient<IUserService, UserService>();: 這是本次修改的核心。

services.AddTransient: 這是註冊服務的方法之一。Transient (暫時的) 生命週期意味著每次當有程式碼請求 IUserService 時，DI 容器都會建立一個全新的 UserService 實例。

<IUserService, UserService>: 這是一個泛型參數，它建立了一個映射關係。它告訴 DI 容器：「當請求的型別是 IUserService (第一個參數) 時，你應該建立一個 UserService (第二個參數) 的實例來滿足這個請求。」

為什麼使用 AddTransient？

DI 容器通常提供三種主要的服務生命週期：

Singleton (單例): 在應用程式的整個生命週期中，只會建立一個實例。所有對該服務的請求都會得到同一個實例。適用於無狀態的、執行緒安全的服務。

Scoped (範圍): 在一個特定的「工作單元」範圍內，只會建立一個實例。在 Web 應用中，一個 HTTP 請求就是一個範圍。在桌面應用中，我們通常需要手動建立範圍。DbContext 預設就是以 Scoped 註冊的。

Transient (暫時的): 每次請求都會建立一個新的實例。這對於輕量級的、有狀態的服務（或者為了確保狀態隔離）是個安全的選擇。

對於我們的 UserService，它依賴於 DbContext (Scoped)。雖然將 UserService 設為 Scoped 也是可行的，但 Transient 是一個更簡單、更安全的起點，它能確保每個操作單元（例如一個表單）獲取到的服務都是乾淨的，不會互相干擾。

里程碑 3 總結

恭喜！我們已經完成了里程碑 3：核心服務與認證邏輯的所有工作。

我們從定義服務的「契約」(Interfaces)，到實現了核心的密碼安全工具，再到編寫了完整的 UserService 業務邏輯，最後成功地將這個服務註冊到了應用程式的依賴注入容器中。

現在，我們的應用程式已經具備了處理使用者登入、註冊等後端能力。下一步，我們將開始建立真正的使用者介面 (LoginForm)，並讓它來「消費」我們剛剛建立的 IUserService。


程式碼說明 (MainForm.cs)
依賴注入: MainForm 現在透過其建構函式接收 ITaskService 和 IUserService 的實例。
MainForm_Load:
這是我們的主要入口點。它現在是 async void，因為我們需要呼叫非同步的 LoadTasksAsync 方法。
它依序呼叫了三個輔助方法：SetupDataGridView (設定UI)、ConfigureUIAccess (設定權限)、LoadTasksAsync (載入資料)。
SetupDataGridView:
dgvTasks.AutoGenerateColumns = false;：這是非常關鍵的一步。我們不希望 DataGridView 自動根據 TodoItem 的所有屬性（包括 Id, CreatorId 等）產生欄位。
我們以程式化的方式手動新增所需的欄位，並透過 DataPropertyName 屬性將每個欄位繫結到 TodoItem 物件的特定屬性上。
繫結到導覽屬性: 注意 AssignedTo.Username 和 Creator.Username。因為我們在 TaskService 中使用了 .Include()，所以 DataGridView 可以直接存取到關聯的 User 物件的 Username 屬性，非常方便。
ConfigureUIAccess:
我們從 ApplicationState.CurrentUser 讀取當前使用者的角色。
根據使用者是否為管理員，來動態設定管理員專用按鈕的 Visible 屬性。
LoadTasksAsync:
這是一個非同步方法，負責資料的載入。
它顯示了等待狀態，呼叫 _taskService.GetAllTasksAsync()。
dgvTasks.DataSource = tasks;: 這是資料繫結的核心。我們將從服務獲取到的 List<TodoItem> 直接設定為 DataGridView 的資料來源。DataGridView 會自動根據我們在 SetupDataGridView 中設定的 DataPropertyName 來填充每一列的儲存格。
使用 try...catch...finally 來處理可能的錯誤並確保 UI 狀態最終恢復正常。


dotnet publish E:\Project\VisualStudio\TodoApp\TodoApp\TodoApp.WinForms\TodoApp.WinForms.csproj -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -c Release







