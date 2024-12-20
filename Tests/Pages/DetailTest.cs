using System.Security.Claims;
using AngleSharp.Dom;
using flashcard.Components.Components;
using flashcard.model;
using flashcard.Tests.Mock;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Detail = flashcard.Components.Pages.Detail;
using System.ComponentModel;

namespace flashcard.Tests.Pages;

public class DetailTest : TestContext
{
	[Fact]
	public void ComponentRendersCorrectlyBeforeStart()
	{
		AuthState.SetupAuthenticationState(Services);
		var flashCardsData = DummyDataTest.CardData.GetFlashCardProblems();

		var detailComponent = RenderComponent<Detail>();

		var startButton = detailComponent.Find("button.btn.btn-active");

		Assert.NotNull(startButton);
		Assert.Contains("Start", startButton.TextContent);
		Assert.DoesNotContain(flashCardsData[0].Question, detailComponent.Markup);
	}

	[Fact]
	public void ComponentStartsAndDisplaysFirstProblem()
	{
		AuthState.SetupAuthenticationState(Services);
		var flashCardsData = DummyDataTest.CardData.GetFlashCardProblems();

		var detailComponent = RenderComponent<Detail>();


		var startButton = detailComponent.Find("button.btn.btn-active");
		startButton.Click();
		var firstQuestion = detailComponent.Find(".flipcard-front");

		Assert.NotNull(firstQuestion);
		Assert.Contains(flashCardsData[0].Question, firstQuestion.TextContent);
	}

	[Fact]
	public void CanNavigateToNextProblem()
	{
		AuthState.SetupAuthenticationState(Services);
		var flashCardsData = DummyDataTest.CardData.GetFlashCardProblems();

		var detailComponent = RenderComponent<Detail>();

		var startButton = detailComponent.Find("button.btn.btn-active");
		startButton.Click();
		var nextButton = detailComponent.Find("#next-button");
		nextButton.Click();
		var secondQuestion = detailComponent.Find(".flipcard-front");

		Assert.NotNull(secondQuestion);
		Assert.Contains(flashCardsData[1].Question, secondQuestion.TextContent);
	}

	[Fact]
	public void CanNavigateBackToPreviousProblem()
	{
		AuthState.SetupAuthenticationState(Services);
		var flashCardsData = DummyDataTest.CardData.GetFlashCardProblems();

		var detailComponent = RenderComponent<Detail>();

		var startButton = detailComponent.Find("button.btn.btn-active");
		startButton.Click();
		var nextButton = detailComponent.Find("#next-button");
		nextButton.Click();
		var prevButton = detailComponent.Find("#prev-button");
		prevButton.Click();
		var firstQuestion = detailComponent.Find(".flipcard-front");

		Assert.NotNull(firstQuestion);
		Assert.Contains(flashCardsData[0].Question, firstQuestion.TextContent);
	}

	[Fact]
	public void CannotNavigatePastFirstOrLastProblem()
	{
		AuthState.SetupAuthenticationState(Services);
		var flashCardsData = DummyDataTest.CardData.GetFlashCardProblems();

		var detailComponent = RenderComponent<Detail>();

		var startButton = detailComponent.Find("button.btn.btn-active");
		startButton.Click();
		var prevButton = detailComponent.Find("#prev-button");
		var nextButton = detailComponent.Find("#next-button");

		// Assert - Cannot navigate backward initially
		Assert.True(prevButton.HasAttribute("disabled"));

		// Navigate to the last problem
		for (int i = 0; i < flashCardsData.Count - 1; i++)
		{
			nextButton.Click();
		}

		// Assert - Cannot navigate forward at the last problem
		Assert.True(nextButton.HasAttribute("disabled"));
	}

	[Fact]
	public void CurrentIndex_Should_Update_When_NavigatingFlashcards()
	{
		AuthState.SetupAuthenticationState(Services);
		var flashCardsData = DummyDataTest.CardData.GetFlashCardProblems();

		var detailComponent = RenderComponent<Detail>();

		var startButton = detailComponent.Find("button.btn.btn-active");
		startButton.Click();

		var initialIndicator = detailComponent.Find("#progress-indicator");
		Assert.Equal("1 / " + flashCardsData.Count, initialIndicator.TextContent);

		var nextButton = detailComponent.Find("#next-button");
		nextButton.Click();

		var nextIndicator = detailComponent.Find("#progress-indicator");
		Assert.Equal("2 / " + flashCardsData.Count, nextIndicator.TextContent);

		var prevButton = detailComponent.Find("#prev-button");
		prevButton.Click();

		var prevIndicator = detailComponent.Find("#progress-indicator");
		Assert.Equal("1 / " + flashCardsData.Count, prevIndicator.TextContent);
	}

}
