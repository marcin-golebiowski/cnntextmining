
namespace TextMining.TextTools
{
    public interface IComparator
    {
        // im większe tym podobieństwo większe
        double Compare(Vector x, Vector y);
    }
}
