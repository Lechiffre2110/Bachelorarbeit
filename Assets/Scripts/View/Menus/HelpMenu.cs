using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HelpMenu : MonoBehaviour, IMenu
{
    [SerializeField] private TMP_InputField _questionInputField;
    [SerializeField] private Transform _chatPanel;
    [SerializeField] private GameObject _userChatMessagePrefab;
    [SerializeField] private GameObject _assistantChatMessagePrefab;
    [SerializeField] private GameObject _inputField; //There is an input field inside here
    // private const string _gptAssistantID = "asst_6njlLgpATyxucc7nnm6PVmkB"; //change assistant to use gpt-4 
    private const string _apiKey = "";
    private HttpClient _client;

    private string _threadID;
    private string _runID;

    private const string welcomeMessage = "Hallo ich bin Milo, Dein persönlicher Assistent für die Nutzung der App. Stelle mir Fragen zur Benutzung der App oder zur Thematik.";


    async void Start()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.openai.com/v1/");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        _client.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
        CreateChatMessage(_assistantChatMessagePrefab, welcomeMessage);
        _threadID = await CreateThreadAsync();
    }

    public async void SubmitQuestionToOpenAI()
    {
        if (string.IsNullOrEmpty(_questionInputField.text))
        {
            return;
        }

        MoveInputFieldToOrigialPosition();

        string userMessage = _questionInputField.text;
        CreateChatMessage(_userChatMessagePrefab, userMessage);
        _questionInputField.text = "";

        GameObject loadingMessageInstance = CreateChatMessage(_assistantChatMessagePrefab, "Milo denkt nach…");
        string response = await ProcessMessageAndGetResponse(userMessage);
        Destroy(loadingMessageInstance);

        CreateChatMessage(_assistantChatMessagePrefab, response);
    }

    public async Task<string> ProcessMessageAndGetResponse(string userMessage)
    {
        // Create a new message in the thread
        await CreateMessageAsync(_threadID, userMessage);

        // Create a run with the specific assistant
        _runID = await CreateRunAsync(_threadID, _gptAssistantID);

        // Wait for the run to complete
        bool isCompleted = false;
        while (!isCompleted)
        {
            isCompleted = await IsRunCompletedAsync(_threadID, _runID);
            await Task.Delay(1000); // Wait a bit before checking again to avoid spamming the server with requests
        }

        // Get the response from the first message in the thread
        var response = await GetFirstMessageContentAsync(_threadID);
        Debug.Log(response);
        return response;
    }


    private async Task<string> CreateThreadAsync()
    {
        var body = new { };
        var jsonBody = JsonSerializer.Serialize(body);
        var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("threads", httpContent);

        var content = await response.Content.ReadAsStringAsync();
        Debug.Log(content);

        var threadResponse = JsonSerializer.Deserialize<ThreadResponse>(content);

        if (threadResponse != null)
        {
            Debug.Log($"Thread ID: {threadResponse.id}");
        }
        else
        {
            Debug.Log("Failed to deserialize the response.");
        }

        return threadResponse?.id;
    }

    public async Task<string> CreateMessageAsync(string threadId, string message)
    {
        var messageData = new
        {
            role = "user",
            content = message
        };

        var messageJson = JsonSerializer.Serialize(messageData);
        var httpContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"threads/{threadId}/messages", httpContent);
        //response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        Debug.Log(responseContent);

        var messageResponse = JsonSerializer.Deserialize<MessageResponse>(responseContent);
        Debug.Log("Message ID: " + messageResponse.id);
        return messageResponse?.id;
    }

    public async Task<string> CreateRunAsync(string threadId, string assistantId)
    {
        var runData = new
        {
            assistant_id = assistantId
        };

        var runJson = JsonSerializer.Serialize(runData);
        var httpContent = new StringContent(runJson, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"threads/{threadId}/runs", httpContent);
        //response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        Debug.Log(responseContent);

        var runResponse = JsonSerializer.Deserialize<RunResponse>(responseContent);
        Debug.Log("Run ID: " + runResponse.id);
        return runResponse?.id;
    }

    public async Task<string> GetFirstMessageContentAsync(string threadId)
    {
        var response = await _client.GetAsync($"threads/{threadId}/messages");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Debug.Log(content);

        var messagesResponse = JsonSerializer.Deserialize<MessagesListResponse>(content);
        if (messagesResponse.data.Count > 0 && messagesResponse.data[0].content.Count > 0)
        {
            Debug.Log("Message Content: " + messagesResponse.data[0].content[0].text.value);
            return messagesResponse.data[0].content[0].text.value;
        }

        return "No messages found.";
    }

    public async Task<bool> IsRunCompletedAsync(string threadId, string runId)
    {
        var response = await _client.GetAsync($"threads/{threadId}/runs/{runId}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var runResponse = JsonSerializer.Deserialize<RunResponse>(content);
        Debug.Log("Run Status: " + runResponse.status);
        return runResponse.status == "completed";
    }

    private GameObject CreateChatMessage(GameObject prefab, string message)
    {
        if (prefab == _assistantChatMessagePrefab)
        {
            GameObject chatMessage = Instantiate(prefab, _chatPanel);
            chatMessage.transform.Find("AI Message").GetComponent<TextMeshProUGUI>().text = message;
            StartCoroutine(AdjustHeightAfterRender(chatMessage));
            return chatMessage;
        }
        else
        {
            GameObject chatMessage = Instantiate(prefab, _chatPanel);
            chatMessage.transform.Find("AI Message").GetComponent<TextMeshProUGUI>().text = message;
            StartCoroutine(AdjustHeightAfterRender(chatMessage));
            return chatMessage;
        }
    }

    private IEnumerator AdjustHeightAfterRender(GameObject chatMessage)
    {
        yield return new WaitForEndOfFrame();

        TextMeshProUGUI textComponent = chatMessage.transform.Find("AI Message").GetComponent<TextMeshProUGUI>();
        string messageContent = textComponent.text;
        int charCount = messageContent.Length;

        float estimatedHeight = EstimateTextHeightBasedOnCharactersPerLine(charCount);
        Debug.Log("Estimated height: " + estimatedHeight);

        RectTransform messageRectTransform = chatMessage.GetComponent<RectTransform>();
        messageRectTransform.sizeDelta = new Vector2(messageRectTransform.sizeDelta.x, estimatedHeight);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_chatPanel.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases(); 

        yield return new WaitForSeconds(0.1f); // Adjust this delay as needed
        _chatPanel.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0f;

    }

    private float EstimateTextHeightBasedOnCharactersPerLine(int charCount, int charactersPerLine = 33)
    {
        int lineCount = Mathf.CeilToInt(charCount / (float)charactersPerLine);
        float lineSpacing = 65;
        float totalTextHeight = lineCount * lineSpacing;
        float padding = 70f;

        return totalTextHeight + padding;
    }

    public void MoveInputFieldAboveKeyboard()
    {
        _inputField.transform.DOLocalMoveY(0, 0.1f);
    }

    public void MoveInputFieldToOrigialPosition()
    {
        _inputField.transform.DOLocalMoveY(-410, 0.075f);
    }


    public void ToggleMenu()
    {
        if (IsOpen())
        {
            CloseMenu();
        }
        else
        {
            transform.DOLocalMoveY(-205, 0.25f);
        }
    }

    public void CloseMenu()
    {
        transform.DOLocalMoveY(-1750, 0.25f);
    }

    public bool IsOpen()
    {
        return transform.localPosition.y >= -1000;
    }
}

[Serializable]
public class ThreadResponse
{
    public string id { get; set; }
}

[Serializable]
public class MessageResponse
{
    public string id { get; set; }
}

[Serializable]
public class RunResponse
{
    public string id { get; set; }
    public string status { get; set; }
}

[Serializable]
public class MessagesListResponse
{
    public List<MessageContent> data { get; set; }
}

[Serializable]
public class MessageContent
{
    public List<Content> content { get; set; }
}

[Serializable]
public class Content
{
    public TextContent text { get; set; }
}

[Serializable]
public class TextContent
{
    public string value { get; set; }
}
