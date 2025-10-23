using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using SmartFAQAssistantApi.Models;
using SmartFAQAssistantApi.Services.IServices;
using SmartFAQAssistantApi.Settings;

namespace SmartFAQAssistantApi.Services;


public class ChatService : IChatService
{
    private readonly ChatClient _chatClient;
    private readonly ILogger<ChatService> _logger;

    public ChatService(OpenAIClient openAIClient, ILogger<ChatService> logger, IOptions<OpenAISettings> openAiSettings)
    {
        _chatClient = openAIClient.GetChatClient(openAiSettings.Value.Model);
        _logger = logger;
    }

    // Helper to build messages
    private List<ChatMessage> BuildMessages(string userQuestion)
    {

        string systemPrompt = @"
You are SmartFAQ — an intelligent assistant that helps users find accurate answers based on company documents and uploaded FAQs.

Your core rules:
1. Use only the information provided in the user's retrieved document context.
2. If the answer is not clearly in the context, say politely: 
   “I'm sorry, but the uploaded FAQ documents don't mention that specifically.”
3. Never make up information or give opinions.
4. Keep your answers clear, structured, and concise (2–4 sentences max).
5. When possible, mention the section or source (e.g., “According to the FAQ document…”).
6. Respond in a friendly and professional tone.";

            string assistantIntro = "Hello 👋 I'm SmartFAQ, your document-based assistant. You can ask me anything about your uploaded FAQs or documents.";

        return new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new AssistantChatMessage(assistantIntro),
            new UserChatMessage(userQuestion)
        };
    }

    public async Task<ChatResponse> ProcessChatAsync(ChatRequest request)
    {
        var messages = BuildMessages(request.Question);
        ChatCompletion completion = await _chatClient.CompleteChatAsync(messages);
        return new ChatResponse { Answer = completion.Content[0].Text };
    }

}