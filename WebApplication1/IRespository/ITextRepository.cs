using static Zenex.Registration.Models.Registeration;

namespace Zenex.Registration.IRespository
{
    public interface ITextRepository
    {
        List<BPText> GetAllTexts();
        Task<BPText> CreateText(BPText Text);
        Task<BPText> UpdateText(BPText Text);
        Task<BPText> DeleteText(BPText Text);

    }
}
