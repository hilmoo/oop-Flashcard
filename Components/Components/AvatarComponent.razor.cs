using flashcard.model;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Components
{
    public partial class AvatarComponent : ComponentBase
    {
        [Parameter] public UserProfile? Profile { get; set; }
    }
}