using Microsoft.AspNetCore.Components;
using Markdig;

namespace flashcard.Components.Components;

public partial class MarkdownRenderer : ComponentBase
{
    [Parameter]
    public string? Content { get; set; }

    private ElementReference contentElement;
    private string renderedContent = "";

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(Content))
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
            renderedContent = Markdown.ToHtml(Content, pipeline);

            await InvokeAsync(StateHasChanged);
        }
    }
}