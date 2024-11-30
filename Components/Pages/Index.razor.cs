using flashcard.model;
using flashcard.z_dummydata;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Pages
{
	public partial class Index : ComponentBase
	{
		private string searchText = string.Empty;
		private string selectedCategory = string.Empty;
		private List<FlashCard> flashCards = [];
		private List<FlashCard> filteredFlashCards = [];

		protected override void OnInitialized()
		{
			flashCards = DummyDataCardBasic.GetFlashCards();
			filteredFlashCards = flashCards;
		}

		private void ApplyFilters()
		{
			var searchTextLower = searchText.ToLowerInvariant();
			var selectedCategoryLower = selectedCategory.ToLowerInvariant();

			filteredFlashCards = flashCards
			.Where(fc => (string.IsNullOrEmpty(searchText) || fc.Title.Contains(searchTextLower, StringComparison.InvariantCultureIgnoreCase)) &&
			(selectedCategory == "all" || string.IsNullOrEmpty(selectedCategory) || fc.Category.ToLowerInvariant().Equals(selectedCategoryLower)))
			.ToList();
		}

		private void HandleSearch(ChangeEventArgs e)
		{
			searchText = e.Value?.ToString() ?? string.Empty;
			ApplyFilters();
		}

		private void SelectCategory(ChangeEventArgs e)
		{
			selectedCategory = e.Value?.ToString() ?? string.Empty;
			ApplyFilters();
		}
	}
}
