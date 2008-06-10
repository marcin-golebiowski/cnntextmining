
namespace TextMining.TextTools
{
    public interface INewsComparator
    {
        // im większe tym podobieństwo większe
        double Compare(Vector x, Vector y);
    }
}
