using flashcard.model;

namespace flashcard.services
{
    public class FlashCardService
    {
        public List<FlashCardProblem> FlashCardProblems { get; private set; } = new List<FlashCardProblem>();

        public void AddFlashCardProblem(string question, string answer)
        {
            FlashCardProblems.Add(new FlashCardProblem { Question = question, Answer = answer });
        }

        public void DeleteFlashCardProblem(int index)
        {
            FlashCardProblems.RemoveAt(index);
        }
    }
}