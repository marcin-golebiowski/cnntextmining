using TextMining.Model;

namespace TextMining.Crawling
{
    public interface IAction
    {
        void Do(CNNPage page);
    }
}