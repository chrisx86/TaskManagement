using TodoApp.Core.Models;
using TodoApp.Core.Services;

namespace TodoApp.Core.DTOs;

public record FilterData(
    TodoStatus? Status,
    UserTaskFilter UserRelation,
    int? AssignedToUser,
    string? SearchKeyword
);
