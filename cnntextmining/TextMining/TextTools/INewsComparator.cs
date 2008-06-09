using TextMining.Model;

namespace TextMining.TextTools
{
    public interface INewsComparator
    {
        // im większe tym podobieństwo większe
        double Compare(News x, News y);
    }
}
