using SmartFAQAssistantApi.Models;

namespace SmartFAQAssistantApi.Services.IServices;

public interface IChatService
{
    Task<ChatResponse> ProcessChatAsync(ChatRequest request);
}
